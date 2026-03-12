using MixingStation.Api.Models;
using MixingStation.Api.Schema;
using Serilog;
using System;
using System.Buffers;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Net.WebSockets;
using System.Text;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;

namespace MixingStation.Api.Services;

public sealed class MixingStationSessionService : IDisposable
{
    private static readonly JsonSerializerOptions JsonOptions = new(JsonSerializerDefaults.Web)
    {
        PropertyNameCaseInsensitive = true
    };

    private readonly MixerStateService _mixerStateService;
    private readonly HttpClient _httpClient;

    private ClientWebSocket? _webSocket;
    private CancellationTokenSource? _cts;
    private Task? _receiveLoop;

    public MixingStationSessionService(MeterService meterService, MixerStateService mixerStateService)
    {
        _mixerStateService = mixerStateService;

        _httpClient = new HttpClient
        {
            BaseAddress = BuildHttpBaseUri(),
            Timeout = TimeSpan.FromSeconds(10)
        };

        _mixerStateService.SendMethod = SendValue;

        _ = meterService;
    }

    public bool IsConnected => _webSocket?.State == WebSocketState.Open;

    public async Task<bool> ConnectAsync(CancellationToken cancellationToken = default)
    {
        try
        {
            _cts = CancellationTokenSource.CreateLinkedTokenSource(cancellationToken);
            var appState = await GetAppStateAsync(_cts.Token).ConfigureAwait(false);

            //await DisconnectAsync().ConfigureAwait(false);


            Log.Debug("[{ClassName}] App state before connect: {AppState}", nameof(MixingStationSessionService), appState);

            await EnsureMixerConnectedAsync(_cts.Token).ConfigureAwait(false);

            _webSocket = new ClientWebSocket();
            await _webSocket.ConnectAsync(BuildWebSocketUri(), _cts.Token).ConfigureAwait(false);

            await InitialStateSyncAsync(_cts.Token).ConfigureAwait(false);

            _receiveLoop = Task.Run(() => ReceiveLoopAsync(_cts.Token), _cts.Token);

            Log.Information(
                "[{ClassName}] Connected to local Mixing Station API at {BaseAddress}",
                nameof(MixingStationSessionService),
                _httpClient.BaseAddress);

            return true;
        }
        catch (Exception ex)
        {
            Log.Error(ex, "[{ClassName}] Failed to establish session", nameof(MixingStationSessionService));
            await DisconnectAsync().ConfigureAwait(false);
            return false;
        }
    }

    public async Task DisconnectAsync()
    {
        try
        {
            _cts?.Cancel();

            try
            {
                using var response = await _httpClient.PostAsync("app/mixers/disconnect", content: null).ConfigureAwait(false);
            }
            catch
            {
                // ignore REST disconnect failures during shutdown
            }

            if (_webSocket?.State == WebSocketState.Open)
            {
                await _webSocket.CloseAsync(
                    WebSocketCloseStatus.NormalClosure,
                    "shutdown",
                    CancellationToken.None).ConfigureAwait(false);
            }
        }
        catch
        {
            // ignored
        }

        try
        {
            if (_receiveLoop != null)
                await _receiveLoop.ConfigureAwait(false);
        }
        catch
        {
            // ignored
        }
        finally
        {
            _webSocket?.Dispose();
            _webSocket = null;

            _cts?.Dispose();
            _cts = null;

            _receiveLoop = null;
        }
    }
    public class AppState
    {
        public MixingStationState State;
        public int Progress;

        public override string ToString()
        {
            return $"State={State}, Progress={Progress}%";
        }
    }
    public enum MixingStationState
    {
        Idle,
        Connecting,
        Connected,
        Syncing,
        Searching,
        Unknown
    }

    private async Task<AppState> GetAppStateAsync(CancellationToken cancellationToken)
    {
        try
        {
            using var response = await _httpClient.GetAsync("app/state", cancellationToken).ConfigureAwait(false);
            response.EnsureSuccessStatusCode();

            var json = await response.Content.ReadAsStringAsync(cancellationToken).ConfigureAwait(false);

            using var doc = JsonDocument.Parse(json);
            var root = doc.RootElement;

            Log.Debug(
                "[{ClassName}] Retrieved app state: {AppStateJson}",
                nameof(MixingStationSessionService),
                root.GetRawText());

            if (root.TryGetProperty("state", out var stateElement) &&
                stateElement.ValueKind == JsonValueKind.String &&
                Enum.TryParse<MixingStationState>(stateElement.GetString(), ignoreCase: true, out var state))
            {
                root.TryGetProperty("progress", out var progressElement);
                return new AppState
                {
                    State = state,
                    Progress = progressElement.GetInt32()
                };
            }

            return null;
        }
        catch (Exception ex)
        {
            Log.Warning(ex, "[{ClassName}] Failed to get app state", nameof(MixingStationSessionService));
            return null;
        }
    }

    private async Task EnsureMixerConnectedAsync(CancellationToken cancellationToken)
    {
        var current = await GetCurrentMixerAsync(cancellationToken).ConfigureAwait(false);
        if (IsMeaningfulJson(current))
        {
            Log.Debug(
                "[{ClassName}] Mixing Station already has an active mixer: {Mixer}",
                nameof(MixingStationSessionService),
                current.Value.GetRawText());

            return;
        }

        await StartMixerSearchAsync(cancellationToken).ConfigureAwait(false);
        await Task.Delay(GetSearchDelay(), cancellationToken).ConfigureAwait(false);

        var results = await GetMixerSearchResultsAsync(cancellationToken).ConfigureAwait(false);
        Log.Debug("[{ClassName}] Mixer search returned {Count} results", nameof(MixingStationSessionService), results.Count);

        var selected = SelectMixerResult(results);
        if (selected.ValueKind == JsonValueKind.Undefined)
            throw new InvalidOperationException("Mixing Station search returned no matching mixers.");
        await Task.Delay(TimeSpan.FromMilliseconds(750), cancellationToken).ConfigureAwait(false);

        await ConnectMixerAsync(selected, cancellationToken).ConfigureAwait(false);
        var state = await GetAppStateAsync(cancellationToken).ConfigureAwait(false);
        while (state.State != MixingStationState.Connected)
        {
            Log.Debug(
                "[{ClassName}] Waiting for mixer connection... {State}%",
                nameof(MixingStationSessionService),
                state);
            await Task.Delay(500, cancellationToken).ConfigureAwait(false);
            state = await GetAppStateAsync(cancellationToken).ConfigureAwait(false);
        }

        Log.Debug("[{ClassName}] App state after connect attempt: {AppState}", nameof(MixingStationSessionService), state);
        await Task.Delay(TimeSpan.FromMilliseconds(3000), cancellationToken).ConfigureAwait(false);

        var connected = await GetCurrentMixerAsync(cancellationToken).ConfigureAwait(false);
        if (!IsMeaningfulJson(connected))
            throw new InvalidOperationException("Mixer connect request completed, but no current mixer is active.");
    }

    public async Task<UiNode> GetUINodes()
    {
        var paths = _mixerStateService.GetAllPaths();
        var service = new MixingStationUiSchemaService(new(_httpClient), new());
        var nodes = await service.BuildUiTreeAsync(paths).ConfigureAwait(false);

        return nodes;
    }

    private async Task InitialStateSyncAsync(CancellationToken cancellationToken)
    {
        await FetchAndProcessRestJsonAsync("console/information", cancellationToken).ConfigureAwait(false);
        await FetchAndProcessRestJsonAsync("console/data/paths", cancellationToken).ConfigureAwait(false);


        // Log.Debug(nodes.DumpTree());

        await SubscribeConsoleDataAsync(cancellationToken).ConfigureAwait(false);

        Log.Debug(
            "[{ClassName}] Initial state sync complete. TotalChannels={TotalChannels}",
            nameof(MixingStationSessionService),
            _mixerStateService.Topology.TotalChannels);
    }

    private async Task FetchAndProcessRestJsonAsync(string relativePath, CancellationToken cancellationToken)
    {
        using var response = await _httpClient.GetAsync(relativePath, cancellationToken).ConfigureAwait(false);
        response.EnsureSuccessStatusCode();

        var json = await response.Content.ReadAsStringAsync(cancellationToken).ConfigureAwait(false);
        if (!string.IsNullOrWhiteSpace(json))
            ProcessRestPayload(json, null);
    }

    private async Task<JsonElement?> GetCurrentMixerAsync(CancellationToken cancellationToken)
    {
        try
        {
            using var response = await _httpClient.GetAsync("app/mixers/current", cancellationToken).ConfigureAwait(false);
            if (!response.IsSuccessStatusCode)
                return null;

            var json = await response.Content.ReadAsStringAsync(cancellationToken).ConfigureAwait(false);
            if (string.IsNullOrWhiteSpace(json))
                return null;

            using var doc = JsonDocument.Parse(json);
            return doc.RootElement.Clone();
        }
        catch
        {
            return null;
        }
    }

    private async Task StartMixerSearchAsync(CancellationToken cancellationToken)
    {
        var body = BuildSearchRequestBody();
        using var content = new StringContent(body, Encoding.UTF8, "application/json");
        using var response = await _httpClient.PostAsync("app/mixers/search", content, cancellationToken).ConfigureAwait(false);
        response.EnsureSuccessStatusCode();
    }

    private async Task<List<JsonElement>> GetMixerSearchResultsAsync(CancellationToken cancellationToken)
    {
        using var response = await _httpClient.GetAsync("app/mixers/searchResults", cancellationToken).ConfigureAwait(false);
        response.EnsureSuccessStatusCode();

        var json = await response.Content.ReadAsStringAsync(cancellationToken).ConfigureAwait(false);
        if (string.IsNullOrWhiteSpace(json))
            return new List<JsonElement>();

        using var doc = JsonDocument.Parse(json);
        var root = doc.RootElement;
        var results = new List<JsonElement>();

        if (root.ValueKind == JsonValueKind.Array)
        {
            foreach (var item in root.EnumerateArray())
                results.Add(item.Clone());

            return results;
        }

        if (root.ValueKind == JsonValueKind.Object)
        {
            if (TryGetArrayProperty(root, "results", out var array))
            {
                foreach (var item in array.EnumerateArray())
                    results.Add(item.Clone());

                return results;
            }

            results.Add(root.Clone());
        }

        return results;
    }

    private async Task ConnectMixerAsync(JsonElement mixerSearchResult, CancellationToken cancellationToken)
    {
        var mixerName =
            mixerSearchResult.TryGetProperty("name", out var name) && name.ValueKind == JsonValueKind.String
                ? name.GetString()
                : "?";

        Log.Information(
            "[{ClassName}] Attempting to connect to mixer {MixerName}",
            nameof(MixingStationSessionService),
            mixerName);

        var body = BuildConnectRequestBody(mixerSearchResult);

        using var content = new StringContent(body, Encoding.UTF8, "application/json");
        using var response = await _httpClient.PostAsync("app/mixers/connect", content, cancellationToken).ConfigureAwait(false);

        var responseText = await response.Content.ReadAsStringAsync(cancellationToken).ConfigureAwait(false);
        if (!response.IsSuccessStatusCode)
        {
            throw new HttpRequestException(
                $"POST app/mixers/connect failed with {(int)response.StatusCode}: {responseText}");
        }
    }

    private async Task SubscribeConsoleDataAsync(CancellationToken cancellationToken)
    {
        var subscription = new
        {
            path = "/console/data/subscribe",
            method = "POST",
            body = new
            {
                path = "ch.0.mix.lvl",           // or more specific e.g. "ch.*.mix.lvl" for main LR faders only
                format = new[] { "val" } // or "norm" / "number" may work in some versions, but "val" is standard
                                         // format = "number"     // ← this key is usually NOT used; see notes below
            }
        };

        var json = JsonSerializer.Serialize(subscription);

        await _webSocket.SendAsync(
            new ArraySegment<byte>(Encoding.UTF8.GetBytes(json)),
            WebSocketMessageType.Text,
            true,
            cancellationToken).ConfigureAwait(false);
    }

    private async Task ReceiveLoopAsync(CancellationToken cancellationToken)
    {
        if (_webSocket == null)
            return;

        while (!cancellationToken.IsCancellationRequested && _webSocket.State == WebSocketState.Open)
        {
            try
            {
                var message = await ReceiveFullTextMessageAsync(_webSocket, cancellationToken).ConfigureAwait(false);
                if (message == null)
                    break;

                ProcessWebSocketMessage(message);
            }
            catch (OperationCanceledException) when (cancellationToken.IsCancellationRequested)
            {
                break;
            }
            catch (WebSocketException ex)
            {
                Log.Warning(ex, "[{ClassName}] WebSocket receive failed", nameof(MixingStationSessionService));
                break;
            }
            catch (Exception ex)
            {
                Log.Warning(ex, "[{ClassName}] Unexpected websocket processing failure", nameof(MixingStationSessionService));
            }
        }
    }

    private void ProcessWebSocketMessage(string json)
    {
        try
        {
            using var doc = JsonDocument.Parse(json);
            var root = doc.RootElement;

            if (root.ValueKind != JsonValueKind.Object)
                return;

            var path = root.GetProperty("path").GetString(); // quick check to skip irrelevant messages without "path"

            if (root.TryGetProperty("body", out var body))
            {
                if (root.TryGetProperty("error", out var error) &&
                    error.ValueKind != JsonValueKind.Null &&
                    error.ValueKind != JsonValueKind.Undefined)
                {
                    Log.Warning(
                        "[{ClassName}] WebSocket API error for path {Path}: {Error}",
                        nameof(MixingStationSessionService),
                        path,
                        error.ToString());
                }

                ProcessBodyElement(body, path);
                return;
            }

            ProcessBodyElement(root, path);
        }
        catch (Exception ex)
        {
            Log.Warning(ex, "[{ClassName}] Failed to parse websocket payload", nameof(MixingStationSessionService));
        }
    }

    private void ProcessRestPayload(string json, string? path)
    {
        try
        {
            using var doc = JsonDocument.Parse(json);
            ProcessBodyElement(doc.RootElement, path);
        }
        catch (Exception ex)
        {
            Log.Warning(ex, "[{ClassName}] Failed to parse REST payload", nameof(MixingStationSessionService));
        }
    }

    private void ProcessBodyElement(JsonElement element, string? path)
    {
        if (element.ValueKind == JsonValueKind.Object)
        {
            if (LooksLikeFullStatePayload(element))
            {
                _mixerStateService.Synchronize(element.GetRawText());
                return;
            }

            if (TryApplySingleValueUpdate(element, path))
                return;
        }

        if (element.ValueKind == JsonValueKind.Array)
        {
            foreach (var item in element.EnumerateArray())
                ProcessBodyElement(item, path);
        }
    }

    private bool TryApplySingleValueUpdate(JsonElement element, string? path)
    {
        Log.Debug(
            "[{ClassName}] Attempting to apply single value update from payload: {Payload}",
            nameof(MixingStationSessionService),
            element.GetRawText());

        if (!element.TryGetProperty("value", out var valueElement))
        {
            return false;
        }

        if (string.IsNullOrWhiteSpace(path))
            return false;

        path = path.Replace("/console/data/get/", "");

        switch (valueElement.ValueKind)
        {
            case JsonValueKind.Number when valueElement.TryGetDouble(out var number):
                _mixerStateService.SetValue(path, (float)number, false);
                return true;

            case JsonValueKind.True:
            case JsonValueKind.False:
                _mixerStateService.SetValue(path, valueElement.GetBoolean(), false);
                return true;

            case JsonValueKind.String:
                _mixerStateService.SetValue(path, valueElement.GetString(), false);
                return true;

            default:
                return false;
        }
    }

    private static bool LooksLikeFullStatePayload(JsonElement element)
    {
        if (element.ValueKind != JsonValueKind.Object)
            return false;

        return element.TryGetProperty("channelTypes", out _) ||
               element.TryGetProperty("child", out _) ||
               element.TryGetProperty("channels", out _) ||
               element.TryGetProperty("bus", out _) ||
               element.TryGetProperty("matrix", out _) ||
               element.TryGetProperty("dca", out _);
    }

    public void SendValue(string path, object? value)
    {
        _ = SendConsoleSetAsync(path, value);
    }

    private async Task SendConsoleSetAsync(string path, object? value, CancellationToken cancellationToken = default)
    {
        if (_webSocket?.State != WebSocketState.Open)
            return;

        var request = new
        {
            path = $"/console/data/set/{path}/val",
            method = "POST",
            body = new
            {
                format = ChooseFormat(path, value),
                value
            }
        };

        await SendWebSocketEnvelopeAsync(request, cancellationToken).ConfigureAwait(false);
    }

    private static string ChooseFormat(string path, object? value)
    {
        return value switch
        {
            float => "number",
            double => "number",
            int => "number",
            bool => "boolean",
            string => "string",
            _ => "string"

        };
    }

    private async Task SendWebSocketEnvelopeAsync(object envelope, CancellationToken cancellationToken)
    {
        if (_webSocket?.State != WebSocketState.Open)
            return;

        var json = JsonSerializer.Serialize(envelope, JsonOptions);
        var bytes = Encoding.UTF8.GetBytes(json);

        await _webSocket.SendAsync(
            new ArraySegment<byte>(bytes),
            WebSocketMessageType.Text,
            true,
            cancellationToken).ConfigureAwait(false);
    }

    private static async Task<string?> ReceiveFullTextMessageAsync(ClientWebSocket webSocket, CancellationToken cancellationToken)
    {
        var buffer = ArrayPool<byte>.Shared.Rent(16 * 1024);

        try
        {
            using var stream = new MemoryStream();

            while (true)
            {
                var result = await webSocket.ReceiveAsync(new ArraySegment<byte>(buffer), cancellationToken).ConfigureAwait(false);

                if (result.MessageType == WebSocketMessageType.Close)
                    return null;

                if (result.Count > 0)
                    stream.Write(buffer, 0, result.Count);

                if (result.EndOfMessage)
                    break;
            }

            return Encoding.UTF8.GetString(stream.ToArray());
        }
        finally
        {
            ArrayPool<byte>.Shared.Return(buffer);
        }
    }

    private static string BuildSearchRequestBody()
    {
        return JsonSerializer.Serialize(new
        {
            consoleId = GetConsoleId()
        });
    }

    private static string BuildConnectRequestBody(JsonElement mixerSearchResult)
    {
        return JsonSerializer.Serialize(new
        {
            consoleId = GetConsoleId(),
            ip = mixerSearchResult.TryGetProperty("ip", out var ip) && ip.ValueKind == JsonValueKind.String
                ? ip.GetString()
                : null
        });
    }

    private static JsonElement SelectMixerResult(List<JsonElement> results)
    {
        var model = 0;

        foreach (var result in results)
        {
            if (result.TryGetProperty("modelId", out var modelId) &&
                modelId.ValueKind == JsonValueKind.Number &&
                modelId.TryGetInt32(out var parsedModelId) &&
                parsedModelId == model)
            {
                return result;
            }
        }

        return results.FirstOrDefault();
    }

    private static int GetConsoleId()
    {
        return GetIntEnv("MIXING_STATION_CONSOLE_ID") ?? 2;
    }

    private static bool TryGetArrayProperty(JsonElement element, string name, out JsonElement array)
    {
        if (element.TryGetProperty(name, out array) && array.ValueKind == JsonValueKind.Array)
            return true;

        array = default;
        return false;
    }

    private static bool IsMeaningfulJson(JsonElement? element)
    {
        if (!element.HasValue)
            return false;

        var value = element.Value;

        if (value.ValueKind == JsonValueKind.Null || value.ValueKind == JsonValueKind.Undefined)
            return false;

        if (value.ValueKind == JsonValueKind.Object)
            return value.EnumerateObject().Any();

        if (value.ValueKind == JsonValueKind.Array)
            return value.GetArrayLength() > 0;

        return true;
    }

    private static TimeSpan GetSearchDelay()
    {
        var ms = GetIntEnv("MIXING_STATION_SEARCH_DELAY_MS");
        return TimeSpan.FromMilliseconds(ms.GetValueOrDefault(1500));
    }

    private static int? GetIntEnv(string name)
    {
        var value = Environment.GetEnvironmentVariable(name);
        return int.TryParse(value, out var parsed) ? parsed : null;
    }

    private static Uri BuildHttpBaseUri()
    {
        var host = Environment.GetEnvironmentVariable("MIXING_STATION_API_HOST");
        if (string.IsNullOrWhiteSpace(host))
            host = "localhost";

        var port = GetIntEnv("MIXING_STATION_API_PORT") ?? 8080;
        return new Uri($"http://{host}:{port}/");
    }

    private static Uri BuildWebSocketUri()
    {
        var host = Environment.GetEnvironmentVariable("MIXING_STATION_API_HOST");
        if (string.IsNullOrWhiteSpace(host))
            host = "localhost";

        var port = GetIntEnv("MIXING_STATION_API_PORT") ?? 8080;
        return new Uri($"ws://{host}:{port}/");
    }

    public void Dispose()
    {
        DisconnectAsync().GetAwaiter().GetResult();
        _httpClient.Dispose();
    }
}
