//using MixingStation.Api.Models;
//using Serilog;
//using System;
//using System.Collections.Generic;
//using System.Net.Http;
//using System.Net.WebSockets;
//using System.Text;
//using System.Text.Json;
//using System.Threading;
//using System.Threading.Tasks;

//namespace MixingStation.Api.Services
//{
//    public class CommunicationService : IDisposable
//    {
//        private readonly MixerStateService _mixerStateService;
//        private readonly HttpClient _httpClient;

//        private ClientWebSocket? _webSocket;
//        private CancellationTokenSource? _cts;
//        private Task? _receiveLoop;
//        private MixingStationMixerEndpoint? _currentEndpoint;

//        public CommunicationService(MeterService meterService, MixerStateService mixerStateService)
//        {
//            _mixerStateService = mixerStateService;
//            _httpClient = new HttpClient();

//            _mixerStateService.SendValueMethod = SendValue;
//            _mixerStateService.SendStringMethod = SendString;

//            _mixerStateService.FileOperationMethod = (_, _, _, _) => { };
//            _mixerStateService.GetProjects = () => Task.FromResult(new List<GenericListItem>());
//            _mixerStateService.GetScenes = _ => Task.FromResult(new List<GenericListItem>());
//            _mixerStateService.GetPresets = () => Task.FromResult(new List<GenericListItem>());
//            _mixerStateService.ChannelResetMethod = (_, _) => { };
//            _mixerStateService.ChannelCopyPaste = (_, _) => { };
//            _mixerStateService.AssignMutes = _ => { };
//        }

//        public bool IsConnected => _webSocket?.State == WebSocketState.Open;

//        public async Task<bool> ConnectAsync(MixingStationMixerEndpoint endpoint, CancellationToken cancellationToken = default)
//        {
//            try
//            {
//                await DisconnectAsync();

//                _currentEndpoint = endpoint;
//                _cts = CancellationTokenSource.CreateLinkedTokenSource(cancellationToken);
//                _webSocket = new ClientWebSocket();
//                await _webSocket.ConnectAsync(endpoint.WebSocketUri, _cts.Token);

//                await InitialStateSyncAsync(_cts.Token);

//                _receiveLoop = Task.Run(() => ReceiveLoop(_cts.Token), _cts.Token);
//                Log.Information("[{className}] Connected to mixer {name} at {host}:{port}", nameof(CommunicationService), endpoint.Name, endpoint.Host, endpoint.WebSocketPort);
//                return true;
//            }
//            catch (Exception exception)
//            {
//                Log.Error("[{className}] failed to connect: {exception}", nameof(CommunicationService), exception);
//                await DisconnectAsync();
//                return false;
//            }
//        }

//        private async Task InitialStateSyncAsync(CancellationToken cancellationToken)
//        {
//            if (_currentEndpoint == null)
//                return;

//            await FetchAndSynchronizeJsonAsync("/api/channel-info", cancellationToken);
//            await FetchAndSynchronizeJsonAsync("/api/state", cancellationToken);

//            if (_webSocket?.State == WebSocketState.Open)
//            {
//                await SendWsPayloadAsync(new
//                {
//                    type = "subscribe",
//                    topics = new[] { "state", "channel-info" }
//                }, cancellationToken);
//            }
//        }

//        private async Task FetchAndSynchronizeJsonAsync(string path, CancellationToken cancellationToken)
//        {
//            if (_currentEndpoint == null)
//                return;

//            var uri = new Uri(_currentEndpoint.RestBaseUri, path);
//            var payload = await _httpClient.GetStringAsync(uri, cancellationToken);
//            ProcessJson(payload);
//        }

//        private async Task ReceiveLoop(CancellationToken cancellationToken)
//        {
//            if (_webSocket == null)
//                return;

//            var buffer = new byte[64 * 1024];
//            while (!cancellationToken.IsCancellationRequested && _webSocket.State == WebSocketState.Open)
//            {
//                var result = await _webSocket.ReceiveAsync(buffer, cancellationToken);
//                if (result.MessageType == WebSocketMessageType.Close)
//                    break;

//                var message = Encoding.UTF8.GetString(buffer, 0, result.Count);
//                ProcessJson(message);
//            }
//        }

//        private void ProcessJson(string json)
//        {
//            try
//            {
//                var jsonElement = JsonSerializer.Deserialize<JsonElement>(json);
//                if (jsonElement.ValueKind != JsonValueKind.Object)
//                    return;

//                if (jsonElement.TryGetProperty("channelTypes", out _) || jsonElement.TryGetProperty("child", out _))
//                {
//                    _mixerStateService.Synchronize(json);
//                    return;
//                }

//                if (jsonElement.TryGetProperty("path", out var pathElement) &&
//                    pathElement.ValueKind == JsonValueKind.String &&
//                    jsonElement.TryGetProperty("value", out var valueElement))
//                {
//                    var path = pathElement.GetString();
//                    if (string.IsNullOrWhiteSpace(path))
//                        return;

//                    switch (valueElement.ValueKind)
//                    {
//                        case JsonValueKind.Number when valueElement.TryGetSingle(out var numeric):
//                            _mixerStateService.SetValue(path, numeric, false);
//                            break;
//                        case JsonValueKind.True:
//                        case JsonValueKind.False:
//                            _mixerStateService.SetValue(path, valueElement.GetBoolean() ? 1f : 0f, false);
//                            break;
//                        case JsonValueKind.String:
//                            _mixerStateService.SetString(path, valueElement.GetString() ?? string.Empty, false);
//                            break;
//                    }
//                }
//            }
//            catch (Exception ex)
//            {
//                Log.Warning("[{className}] Failed to process json payload: {message}", nameof(CommunicationService), ex.Message);
//            }
//        }

//        public void SendString(string route, string value)
//        {
//            _ = SendWsPayloadAsync(new { type = "set", path = route, value });
//        }

//        public void SendValue(string route, float value)
//        {
//            _ = SendWsPayloadAsync(new { type = "set", path = route, value });
//        }

//        private async Task SendWsPayloadAsync(object payload, CancellationToken cancellationToken = default)
//        {
//            if (_webSocket?.State != WebSocketState.Open)
//                return;

//            var json = JsonSerializer.Serialize(payload);
//            var bytes = Encoding.UTF8.GetBytes(json);
//            await _webSocket.SendAsync(bytes, WebSocketMessageType.Text, true, cancellationToken);
//        }

//        public async Task DisconnectAsync()
//        {
//            try
//            {
//                _cts?.Cancel();
//                if (_webSocket?.State == WebSocketState.Open)
//                {
//                    await _webSocket.CloseAsync(WebSocketCloseStatus.NormalClosure, "shutdown", CancellationToken.None);
//                }
//            }
//            catch
//            {
//                // ignored
//            }
//            finally
//            {
//                _webSocket?.Dispose();
//                _webSocket = null;
//                _cts?.Dispose();
//                _cts = null;
//                _receiveLoop = null;
//            }
//        }

//        public void Dispose()
//        {
//            DisconnectAsync().GetAwaiter().GetResult();
//            _httpClient.Dispose();
//        }
//    }
//}
