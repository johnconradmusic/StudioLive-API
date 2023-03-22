//------------------------------------------------------------------------------
// The Assistant - Copyright (c) 2016-2023, John Conrad
//------------------------------------------------------------------------------
using ICSharpCode.SharpZipLib.Zip.Compression.Streams;
using Newtonsoft.Json;
using Presonus.StudioLive32.Api.Extensions;
using Presonus.StudioLive32.Api.Helpers;
using Presonus.StudioLive32.Api.Messages;
using Presonus.StudioLive32.Api.Messages.Readers;
using Presonus.UCNet.Api.Messages;
using Presonus.UCNet.Api.Messages.Readers;
using Presonus.UCNet.Api.Services;
using Serilog;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Text.Json;
using System.Threading;

namespace Presonus.StudioLive32.Api.Services
{
    public class CommunicationService : IDisposable
    {
        private readonly MeterService _meterService;
        private readonly MixerStateService _mixerStateService;
        //private readonly RawService _rawService;

        private TcpClient _tcpClient;
        private Thread _listeningThread;
        private Thread _writingThread;

        private ushort _deviceId;

        private List<byte[]> chunkBuffer = new List<byte[]>();


        public CommunicationService(MeterService meterService, MixerStateService mixerStateService)
        {
            _meterService = meterService;
            _mixerStateService = mixerStateService;

            // Update the methods to use the new MixerStateService
            _mixerStateService.SetValueMethod = SetRouteValue;
            _mixerStateService.SetStringMethod = SetStringValue;

            _listeningThread = new Thread(Listener) { IsBackground = true };
            _writingThread = new Thread(KeepAlive) { IsBackground = true };
        }

        public bool IsConnected => _tcpClient?.Connected ?? false;

        public static bool ConnectionEstablished { get; set; }

        private void RequestCommunicationMessage()
        {
            var networkStream = GetNetworkStream();
            if (networkStream is null)
                return;

            var welcomeMessage = CreateWelcomeMessage();
            networkStream.Write(welcomeMessage, 0, welcomeMessage.Length);
        }

        private byte[] CreateWelcomeMessage()
        {
            var list = new List<byte>();

            var tcpMessageWriter = new TcpMessageWriter(_deviceId);
            var welcomeMessage = tcpMessageWriter.CreateWelcomeMessage(_meterService.Port);
            list.AddRange(welcomeMessage);

            var jsonMessage = tcpMessageWriter.CreateClientInfoMessage();
            list.AddRange(jsonMessage);

            return list.ToArray();
        }

        private void KeepAlive()
        {
            while (true)
            {
                try
                {
                    var networkStream = GetNetworkStream();
                    if (networkStream is null)
                        continue;

                    var tcpMessageWriter = new TcpMessageWriter(_deviceId);
                    var keepAliveMessage = tcpMessageWriter.CreateKeepAliveMessage();
                    networkStream.Write(keepAliveMessage, 0, keepAliveMessage.Length);
                }
                catch (Exception exception)
                {
                    Log.Error("[{className}] {exception}", nameof(CommunicationService), exception);
                }
                finally
                {
                    Thread.Sleep(TimeSpan.FromSeconds(1));
                }
            }
        }

        private void Listener()
        {
            var receiveBytes = new byte[65536];
            while (true)
            {
                try
                {
                    var networkStream = GetNetworkStream();
                    if (networkStream is null)
                    {
                        Thread.Sleep(TimeSpan.FromMilliseconds(100));
                        continue;
                    }

                    var numberOfBytes = networkStream.Read(receiveBytes, 0, receiveBytes.Length);
                    var data = receiveBytes.Range(0, numberOfBytes);

                    //Multiple messages can be in one package:
                    var chunks = PackageHelper.ChuncksByIndicator(data).ToArray();
                    foreach (var chunk in chunks)
                    {
                        var messageType = PackageHelper.GetMessageType(chunk);
                        ProcessMessages(chunk);
                    }
                }
                catch (Exception exception)
                {
                    //Console.WriteLine(exception.Message);
                }
            }
        }

        private void ProcessMessages(byte[] chunk)
        {
            var messageType = PackageHelper.GetMessageType(chunk);

            Console.WriteLine(messageType);
            switch (messageType)
            {
                case MessageCode.Unknown1:
                    break;

                case MessageCode.Chunk:
                    CK(chunk);
                    break;

                case MessageCode.ParamStrList:
                    PL(chunk);
                    break;

                case "PR":
                    break;

                case MessageCode.ParamValue:
                    PV(chunk);
                    break;

                case MessageCode.JSON:
                    var jm = JM.GetJsonMessage(chunk);
                    Json(jm);
                    break;

                case MessageCode.CompressedJSON:
                    var zm = ZM.GetJsonMessage(chunk);
                    Json(zm);
                    break;

                case MessageCode.ParamString:
                    PS(chunk);
                    break;

                case MessageCode.FaderPosition:
                    Log.Information("ms");
                    break;

                case MessageCode.ZLIB:
                    var data = chunk.Skip(16).ToArray();
                    HandleZlib(data);
                    break;

                default:
                    break;
            }
        }

        private void HandleZlib(byte[] chunk)
        {
            try
            {
                Console.WriteLine("Handling ZLib");
                var info = DeserializeZlibBuffer(chunk);

                var jsonString = JsonConvert.SerializeObject(info, Formatting.Indented);
                Console.WriteLine(jsonString);
                _mixerStateService.Synchronize(jsonString);
            }
            catch (Exception e)
            {
                Console.WriteLine($"Error {e.Message}");
            }
        }

        private void CK(byte[] data)
        {
            Console.WriteLine("Entering CK method");

            data = data.Skip(16).ToArray();

            int chunkOffset = BitConverter.ToInt32(data, 0);
            int totalSize = BitConverter.ToInt32(data, 4);
            int chunkSize = BitConverter.ToInt32(data, 8);

            Console.WriteLine($"chunkOffset: {chunkOffset}, totalSize: {totalSize}, chunkSize: {chunkSize}");

            byte[] chunkData = data.Skip(12).ToArray();
            chunkBuffer.Add(chunkData);

            Console.WriteLine($"Added chunkData to chunkBuffer. Current chunkBuffer count: {chunkBuffer.Count}");

            if (chunkOffset + chunkSize == totalSize)
            {
                Console.WriteLine("chunkOffset + chunkSize == totalSize condition met");

                List<byte[]> fullBuffer = chunkBuffer;
                chunkBuffer = new List<byte[]>();
                HandleZlib(Concatenate(fullBuffer));
            }
            else
            {
                Console.WriteLine("chunkOffset + chunkSize != totalSize condition not met");
            }

            Console.WriteLine("Exiting CK method");
        }


        private byte[] Concatenate(List<byte[]> buffers)
        {
            int totalLength = 0;
            foreach (byte[] buffer in buffers)
            {
                totalLength += buffer.Length;
            }

            byte[] result = new byte[totalLength];
            int position = 0;

            foreach (byte[] buffer in buffers)
            {
                Buffer.BlockCopy(buffer, 0, result, position, buffer.Length);
                position += buffer.Length;
            }

            return result;
        }

        private void Json(string json)
        {
            var jsonElement = System.Text.Json.JsonSerializer.Deserialize<JsonElement>(json);
            if (!jsonElement.TryGetProperty("id", out var idProperty))
                return;

            var id = idProperty.GetString();

            switch (id)
            {
                case "SynchronizePart":
                    //Happens when linking and unlinking mic channels.
                    //If linked, volume and gain reduction (bug in UC Control?) is bound to Right Channel.
                    //Fatchannel is bound to "both", ex. toggle toggles both to same state.
                    return;

                case "Synchronize":
                    _mixerStateService.Synchronize(json);
                    return;

                case "SubscriptionReply":
                    Serilog.Log.Information("Connection established.");
                    ConnectionEstablished = true;
                    return;

                case "SubscriptionLost":
                    RequestCommunicationMessage();
                    return;

                case "UserLoggedIn":
                    //logged in
                    return;

                case "RecalledPreset":
                    //Console.WriteLine(jsonElement.ToString());
                    return;

                default:
                    Log.Warning("[{className}] Unknown json id {messageType}", nameof(CommunicationService), id);
                    return;
            }
        }

        private void PL(byte[] data)
        {
            var header = data.Range(0, 4);
            var messageLength = data.Range(4, 6);
            var messageType = data.Range(6, 8);
            var from = data.Range(8, 10);
            var to = data.Range(10, 12);

            var i = Array.IndexOf<byte>(data, 0x00, 12);
            var route = Encoding.ASCII.GetString(data.Range(12, i));

            var selectedPreset = BitConverter.ToSingle(data.Range((i + 3), (i + 7)), 0);

            var list = Encoding.ASCII.GetString(data.Range((i + 7), -1)).Split('\n');

            _mixerStateService.SetStrings(route, list, false);
        }

        private void PV(byte[] data)
        {
            var header = data.Range(0, 4);
            var messageLength = data.Range(4, 6);
            var messageType = data.Range(6, 8);
            var from = data.Range(8, 10);
            var to = data.Range(10, 12);

            var route = Encoding.ASCII.GetString(data.Range(12, -7));
            var emptyBytes = data.Range(-7, -4);
            var state = BitConverter.ToSingle(data.Range(-4), 0);
            _mixerStateService.SetValue(route, state, false);
        }

        private void PS(byte[] data)
        {
            var header = data.Range(0, 4);
            var messageLength = data.Range(4, 6);
            var messageType = Encoding.ASCII.GetString(data.Range(6, 8));
            var from = data.Range(8, 10);
            var to = data.Range(10, 12);

            //Ex. "line/ch1/preset_name\0\0\0Slap Echo\0"
            var str = Encoding.ASCII.GetString(data.Range(12));
            var split = str.Split('\0');
            var route = split[0];
            var value = split[3];

            _mixerStateService.SetString(route, value, false);
        }

        public static ZLibPayload ConvertToZlibPayload(Dictionary<string, object> deserializedData)
        {
            try
            {
                // Serialize the deserialized data to a JSON string
                var jsonString = JsonConvert.SerializeObject(deserializedData);

                // Deserialize the JSON string to a ZLibPayload object
                var result = JsonConvert.DeserializeObject<ZLibPayload>(jsonString);

                return result;
            }
            catch (Exception ex)
            {
                Console.WriteLine("An error occurred during the conversion:");
                Console.WriteLine(ex.Message);
                throw;
            }
        }

        public static Dictionary<string, object> DeserializeZlibBuffer(byte[] buffer)
        {

            // Skip the first 16 bytes
            using var inputStream = new MemoryStream(buffer, 0, buffer.Length);
            using var zlibStream = new InflaterInputStream(inputStream);
            using var outputStream = new MemoryStream();

            zlibStream.CopyTo(outputStream);

            byte[] decompressedData = outputStream.ToArray();
            return ZlibPayloadDeserializer.DeserializeZlibBuffer(decompressedData);
        }

        public void Connect(ushort deviceId, int tcpPort)
        {
            _listeningThread.Start();
            _writingThread.Start();
            _tcpClient?.Dispose();

            _deviceId = deviceId;

            _tcpClient = new TcpClient();
            _tcpClient.Connect(IPAddress.Loopback, tcpPort);

            RequestCommunicationMessage();
        }

        public NetworkStream GetNetworkStream()
        {
            return _tcpClient?.Connected is true
                ? _tcpClient.GetStream()
                : null;
        }

        public void SetStringValue(string route, string value, bool broadcast)
        {
            if (broadcast)
            {
                var writer = new TcpMessageWriter(_deviceId);
                var data = writer.CreateRouteStringUpdate(route, value);

                SendMessage(data);
            }
            //_rawService.UpdateStringState(route, value);
        }

        public void SetRouteValue(string route, float value, bool broadcast = true)
        {
            if (broadcast)
            {
                var writer = new TcpMessageWriter(_deviceId);
                var data = writer.CreateRouteValueUpdate(route, value);

                SendMessage(data);
            }
            //_rawService.UpdateValueState(route, value);
        }

        public bool SendMessage(byte[] message)
        {
            try
            {
                var header = message.Range(0, 4);
                var messageLength = message.Range(4, 6);
                var messageType = message.Range(6, 8);
                var from = message.Range(8, 10);
                var to = message.Range(10, 12);
                var route = Encoding.ASCII.GetString(message.Range(12, -7));
                var emptyBytes = message.Range(-7, -4);
                var state = BitConverter.ToSingle(message.Range(-4), 0);

                var networkStream = GetNetworkStream();
                if (networkStream is null)
                    return false;

                networkStream.Write(message, 0, message.Length);

                return true;
            }
            catch
            {
                return false;
            }
        }

        public void Dispose()
        {
            _tcpClient?.Dispose();
        }
    }
}