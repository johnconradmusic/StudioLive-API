using MixingStation.Api.Models;
using Serilog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;

namespace MixingStation.Api.Services
{
    /// <summary>
    /// Polls the Mixing Station REST API for available mixers and opens a websocket connection.
    /// </summary>
    public class BroadcastService : IDisposable
    {
        private readonly CommunicationService _communicationService;
        private readonly HttpClient _httpClient;
        private readonly CancellationTokenSource _cts = new();

        private Task? _task;

        public BroadcastService(CommunicationService communicationService)
        {
            _communicationService = communicationService;
            _httpClient = new HttpClient();
        }

        public void StartReceive()
        {
            _task = Task.Run(() => ListenerAsync(_cts.Token), _cts.Token);
        }

        private async Task ListenerAsync(CancellationToken cancellationToken)
        {
            while (!cancellationToken.IsCancellationRequested)
            {
                try
                {
                    if (!_communicationService.IsConnected)
                    {
                        var mixers = await DiscoverMixersAsync(cancellationToken);
                        var first = mixers.FirstOrDefault();
                        if (first != null)
                        {
                            await _communicationService.ConnectAsync(first, cancellationToken);
                        }
                    }
                }
                catch (Exception exception)
                {
                    Log.Error("[{className}] {exception}", nameof(BroadcastService), exception);
                }

                await Task.Delay(TimeSpan.FromSeconds(3), cancellationToken);
            }
        }

        private async Task<List<MixingStationMixerEndpoint>> DiscoverMixersAsync(CancellationToken cancellationToken)
        {
            var endpoints = new List<MixingStationMixerEndpoint>();
            foreach (var host in GetDiscoveryHosts())
            {
                foreach (var restPort in GetDiscoveryPorts())
                {
                    var endpoint = await TryDiscoverEndpointAsync(host, restPort, cancellationToken);
                    if (endpoint != null)
                    {
                        endpoints.Add(endpoint);
                    }
                }
            }

            return endpoints
                .GroupBy(e => $"{e.Host}:{e.RestPort}:{e.WebSocketPort}")
                .Select(group => group.First())
                .ToList();
        }

        private async Task<MixingStationMixerEndpoint?> TryDiscoverEndpointAsync(string host, int restPort, CancellationToken cancellationToken)
        {
            var baseUri = new Uri($"http://{host}:{restPort}");

            // Preferred: discovery endpoint with explicit mixer data.
            var discoveryUri = new Uri(baseUri, "/api/discovery");
            try
            {
                var response = await _httpClient.GetStringAsync(discoveryUri, cancellationToken);
                var mixers = ParseDiscoveryResponse(host, restPort, response);
                if (mixers.Count > 0)
                    return mixers[0];
            }
            catch
            {
                // Fall through to direct probe.
            }

            // Fallback: probe channel-info to confirm this host is a valid Mixing Station API.
            var channelInfoUri = new Uri(baseUri, "/api/channel-info");
            try
            {
                var payload = await _httpClient.GetStringAsync(channelInfoUri, cancellationToken);
                var channelInfo = JsonSerializer.Deserialize<MixingStationChannelInfo>(payload);
                if (channelInfo?.ChannelTypes?.Count > 0)
                {
                    return new MixingStationMixerEndpoint
                    {
                        Id = $"{host}:{restPort}",
                        Name = $"Mixing Station ({host})",
                        Host = host,
                        RestPort = restPort,
                        WebSocketPort = restPort
                    };
                }
            }
            catch
            {
                // Not reachable/compatible.
            }

            return null;
        }

        private static List<MixingStationMixerEndpoint> ParseDiscoveryResponse(string defaultHost, int defaultRestPort, string json)
        {
            var results = new List<MixingStationMixerEndpoint>();
            using var doc = JsonDocument.Parse(json);

            if (doc.RootElement.ValueKind == JsonValueKind.Array)
            {
                foreach (var item in doc.RootElement.EnumerateArray())
                {
                    var mixer = ParseMixerObject(defaultHost, defaultRestPort, item);
                    if (mixer != null)
                        results.Add(mixer);
                }
            }
            else if (doc.RootElement.ValueKind == JsonValueKind.Object)
            {
                var mixer = ParseMixerObject(defaultHost, defaultRestPort, doc.RootElement);
                if (mixer != null)
                    results.Add(mixer);
            }

            return results;
        }

        private static MixingStationMixerEndpoint? ParseMixerObject(string defaultHost, int defaultRestPort, JsonElement element)
        {
            if (element.ValueKind != JsonValueKind.Object)
                return null;

            string host = defaultHost;
            int restPort = defaultRestPort;
            int wsPort = defaultRestPort;
            string name = "Mixing Station";
            string id = string.Empty;

            if (element.TryGetProperty("host", out var hostProperty) && hostProperty.ValueKind == JsonValueKind.String)
                host = hostProperty.GetString() ?? defaultHost;

            if (element.TryGetProperty("restPort", out var restPortProperty) && restPortProperty.TryGetInt32(out var parsedRestPort))
                restPort = parsedRestPort;

            if (element.TryGetProperty("wsPort", out var wsPortProperty) && wsPortProperty.TryGetInt32(out var parsedWsPort))
                wsPort = parsedWsPort;

            if (element.TryGetProperty("name", out var nameProperty) && nameProperty.ValueKind == JsonValueKind.String)
                name = nameProperty.GetString() ?? name;

            if (element.TryGetProperty("id", out var idProperty) && idProperty.ValueKind == JsonValueKind.String)
                id = idProperty.GetString() ?? string.Empty;

            return new MixingStationMixerEndpoint
            {
                Id = string.IsNullOrWhiteSpace(id) ? $"{host}:{restPort}" : id,
                Name = name,
                Host = host,
                RestPort = restPort,
                WebSocketPort = wsPort
            };
        }

        private static IEnumerable<string> GetDiscoveryHosts()
        {
            var envHosts = Environment.GetEnvironmentVariable("MIXING_STATION_DISCOVERY_HOSTS");
            if (!string.IsNullOrWhiteSpace(envHosts))
            {
                return envHosts
                    .Split(',', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries)
                    .Distinct(StringComparer.OrdinalIgnoreCase);
            }

            return new[] { "127.0.0.1", "localhost" };
        }

        private static IEnumerable<int> GetDiscoveryPorts()
        {
            var envPorts = Environment.GetEnvironmentVariable("MIXING_STATION_DISCOVERY_PORTS");
            if (!string.IsNullOrWhiteSpace(envPorts))
            {
                return envPorts
                    .Split(',', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries)
                    .Select(port => int.TryParse(port, out var parsed) ? parsed : -1)
                    .Where(port => port > 0)
                    .Distinct();
            }

            return new[] { 8080, 8081, 9000 };
        }

        public void Dispose()
        {
            _cts.Cancel();
            try
            {
                _task?.GetAwaiter().GetResult();
            }
            catch
            {
                // ignored
            }

            _cts.Dispose();
            _httpClient.Dispose();
        }
    }
}
