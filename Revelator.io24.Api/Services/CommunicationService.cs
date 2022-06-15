﻿using Revelator.io24.Api.Extensions;
using Revelator.io24.Api.Helpers;
using Revelator.io24.Api.Messages;
using Revelator.io24.Api.Messages.Readers;
using Revelator.io24.Api.Scene;
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

namespace Revelator.io24.Api.Services
{
    public class CommunicationService : IDisposable
    {
        private readonly MonitorService _monitorService;
        private readonly RawService _rawService;

        private TcpClient _tcpClient;
        private Thread _listeningThread;
        private Thread _writingThread;

        private ushort _deviceId;

        public bool IsConnected => _tcpClient?.Connected ?? false;

        public CommunicationService(
            MonitorService monitorService,
            RawService rawService)
        {
            _monitorService = monitorService;
            _rawService = rawService;

            _rawService.SetValueMethod = SetRouteValue;
            _rawService.SetStringMethod = SetStringValue;

            _listeningThread = new Thread(Listener) { IsBackground = true };
            _listeningThread.Start();

            _writingThread = new Thread(KeepAlive) { IsBackground = true };
            _writingThread.Start();
        }

        public void Connect(ushort deviceId, int tcpPort)
        {
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
            var welcomeMessage = tcpMessageWriter.CreateWelcomeMessage(_monitorService.Port);
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

                    var bytesReceived = networkStream.Read(receiveBytes, 0, receiveBytes.Length);
                    var data = receiveBytes.Range(0, bytesReceived);

                    //Multiple messages can be in one package:
                    var chunks = PackageHelper.ChuncksByIndicator(data).ToArray();
                    foreach (var chunck in chunks)
                    {
                        var messageType = PackageHelper.GetMessageType(chunck);
                        Log.Information(messageType);
                        switch (messageType)
                        {
                            case "BO":
                                BO(chunck);
                                break;
                            case "CK":
                                var chks = PackageHelper.ChuncksByIndicator(chunck).ToArray();
                                foreach (var c in chks)
                                {
                                    var mt = PackageHelper.GetMessageType(c);
                                    Log.Information("CK? :" + mt);
                                    //CK(c);
                                }
                                break;
                            case "PL":
                                //PL List:
                                PL(chunck);
                                break;
                            case "PR":

                                break;
                            case "PV":
                                //PV Settings packet
                                PV(chunck);
                                break;
                            case "JM":
                                var jm = JM.GetJsonMessage(chunck);
                                Json(jm);
                                break;
                            case "ZM":
                                var zm = ZM.GetJsonMessage(chunck);
                                Json(zm);
                                break;
                            case "PS":
                                PS(chunck);
                                break;
                            case "MS":
                                break;
                            default:
                                break;
                        }
                    }
                }
                catch (Exception exception)
                {
                    Log.Error("[{className}] {exception}", nameof(CommunicationService), exception);
                }
            }
        }
        void CK(byte[] data)
        {
            var header = data.Range(0, 4);
            var messageLength = data.Range(4, 6);
            var messageType = data.Range(6, 8);
            var from = data.Range(8, 10);
            var to = data.Range(10, 12);

            var str = Encoding.ASCII.GetString(data.Range(12));
            Log.Information("CK: " + str);
        }


        private void Json(string json)
        {

            Log.Information("JSON: " + json);
            var jsonElement = JsonSerializer.Deserialize<JsonElement>(json);
            if (!jsonElement.TryGetProperty("id", out var idProperty))
                return;

            var id = idProperty.GetString();

            switch (id)
            {
                case "SynchronizePart":
                    //Happens when lining and unlinking mic channels.
                    //If linked, volume and gain reduction (bug in UC Control?) is bound to Right Channel.
                    //Fatchannel is bound to "both", ex. toggle toggles both to same state.
                    return;
                case "Synchronize":
                    _rawService.Syncronize(json);
                    return;
                case "SubscriptionReply":
                    //We now have communication.
                    return;
                case "SubscriptionLost":
                    RequestCommunicationMessage();
                    return;
                case "UserLoggedIn":
                    //logged in
                    return;
                default:
                    Log.Warning("[{className}] Unknown json id {messageType}", nameof(CommunicationService), id);
                    return;
            }
        }

        private void BO(byte[] data)
        {
            var header = data.Range(0, 4);
            var messageLength = data.Range(4, 6);
            var messageType = data.Range(6, 8);
            var from = data.Range(8, 10);
            var to = data.Range(10, 12);

            var str = Encoding.ASCII.GetString(data.Range(12));
            //Log.Information("BO: " + str);
        }

        /// <summary>
        /// Updates list value
        /// </summary>
        /// <param name="data"></param>
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

            //0x0A (\n): List delimiter
            //Last char is a 0x00 (\0)
            var list = Encoding.ASCII.GetString(data.Range((i + 7), -1)).Split('\n');
            //if (!route.EndsWith("/presets/preset"))
            //{
            //    Log.Warning("[{className}] PL unknown list on route {route}", nameof(CommunicationService), route);
            //    return;
            //}
            _rawService.UpdateStringsState(route, list);
        }

        /// <summary>
        /// Updates float value.
        /// Happens ex. on turning thing on and off, ex. EQ
        /// </summary>
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

            _rawService.UpdateValueState(route, state);
        }

        /// <summary>
        /// Updates string value.
        /// Changing profile, changing names...
        /// </summary>
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

            _rawService.UpdateStringState(route, value);
        }

        public void SetStringValue(string route, string value)
        {
            var writer = new TcpMessageWriter(_deviceId);
            var data = writer.CreateRouteStringUpdate(route, value);

            SendMessage(data);
        }

        public void SetRouteValue(string route, float value)
        {
            var writer = new TcpMessageWriter(_deviceId);
            var data = writer.CreateRouteValueUpdate(route, value);

            Serilog.Log.Information("set value: " + route + " - " + value.ToString());


            SendMessage(data);
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

                Log.Information(route + " - " + state.ToString());
                var networkStream = GetNetworkStream();
                if (networkStream is null)
                    return false;

                networkStream.Write(message, 0, message.Length);
                Log.Information("success");

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