using Presonus.UCNet.Api.Extensions;
using Presonus.UCNet.Api.Helpers;
using Serilog;
using System;
using System.Diagnostics;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;

namespace Presonus.UCNet.Api.Services
{
    /// <summary>
    /// Service for listening to broadcast messages from the console.
    /// All messages are broadcasted on port 47809.
    /// The mixer sends a broadcast message every three seconds.
    /// </summary>
    public class BroadcastService : IDisposable
    {
        private readonly UdpClient _udpClient;
        private readonly Thread _thread;
        private readonly CommunicationService _communicationService;

        public BroadcastService(
            CommunicationService communicationService)
        {
            _communicationService = communicationService;

            _udpClient = new UdpClient();
            _udpClient.Client.SetSocketOption(SocketOptionLevel.Socket, SocketOptionName.ReuseAddress, true);
            _udpClient.Client.Bind(GetIpEndpoint());
            

            _thread = new Thread(Listener) { IsBackground = true };
        }

        private IPEndPoint GetIpEndpoint()
        {
            var platform = Environment.OSVersion.Platform;
            switch (platform)
            {
                //Mac OS X:
                case PlatformID.MacOSX:
                case PlatformID.Unix:
                    return new IPEndPoint(IPAddress.Any, 47809);

                //Windows:
                case PlatformID.Win32S:
                case PlatformID.Win32Windows:
                case PlatformID.Win32NT:
                case PlatformID.WinCE:
                case PlatformID.Xbox:
                default:
                    return new IPEndPoint(IPAddress.Any, 47809);
            }
        }

        public void StartReceive()
        {
            _thread.Start();
        }

        private void Listener()
        {
            while (true)
            {
                try
                {
                    IPEndPoint endPoint = null;
                    var data = _udpClient.Receive(ref endPoint);
                    var isUcNetPackage = PackageHelper.IsUcNetPackage(data);
                    if (!isUcNetPackage)
                        continue;

                    var messageType = PackageHelper.GetMessageType(data);
                    //if the message is not a device announcement, ignore it
                    if (messageType != "DA")
                    {
                        
                        if (messageType != "NO")
                        {
                            Log.Information("[{className}] {messageType} not DA", nameof(BroadcastService), messageType);
                        }
                        continue;
                    }

                    if (!_communicationService.IsConnected)
                    {
                        var deviceId = BitConverter.ToUInt16(data.Range(8, 10), 0);
                        var tcpPort = BitConverter.ToUInt16(data.Range(4, 6), 0);
                        _communicationService.Connect(deviceId, tcpPort);
                    }

                }
                catch (Exception exception)
                {
                    Log.Error("[{className}] {exception}", nameof(BroadcastService), exception);
                }
            }
        }

        public void Dispose()
        {
            _udpClient.Dispose();
        }
    }
}
