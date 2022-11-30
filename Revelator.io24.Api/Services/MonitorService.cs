using Presonus.StudioLive32.Api.Extensions;
using Presonus.StudioLive32.Api.Helpers;
using Serilog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;

namespace Presonus.StudioLive32.Api.Services
{
    /// <summary>
    /// This service is used to receive UDP packages containing monitorin data.
    /// Examples of this is the mic/volume meters, and FX meters.
    /// </summary>
    public class MonitorService : IDisposable
    {
        private readonly UdpClient _udpClient;
        private readonly Thread _thread;

        private readonly RawService _rawService;
        public ushort Port { get; }

        public MonitorService(RawService rawService)
        {
            this._rawService = rawService;
            _udpClient = new UdpClient(0);
            var ipEndpoint = _udpClient.Client.LocalEndPoint as IPEndPoint;
            if (ipEndpoint is null)
                throw new InvalidOperationException("Failed to start UDP server.");

            Port = (ushort)ipEndpoint.Port;

            _thread = new Thread(Listener) { IsBackground = true };
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
                    //Log.Information("received on udp");
                    var isUcNetPackage = PackageHelper.IsUcNetPackage(data);
                    if (!isUcNetPackage)
                        continue;

                    var messageType = PackageHelper.GetMessageType(data);
                    //Log.Information("[{className}] {messageType}", nameof(MonitorService), messageType);
                    if (messageType != "MS")
                    {
                        Console.WriteLine("Monitor: NOT MS " + messageType);
                        //Log.Information("[{className}] {messageType} not MS", nameof(MonitorService), messageType);
                        continue;
                    }

                    Analyze(data);
                }
                catch (Exception exception)
                {
                    Log.Error("[{className}] {exception}", nameof(MonitorService), exception);
                }
            }
        }

        private Dictionary<string, int> _count = new Dictionary<string, int>();

        /// <summary>
        /// This Package type is used for real time monitoring.
        /// </summary>
        /// <param name="data"></param>
        private void Analyze(byte[] data)
        {
            var header = Encoding.ASCII.GetString(data.Range(0, 4)); //UC01
            var unknownValue = BitConverter.ToUInt16(data.Range(4, 6), 0); //always: 0x6C, 0xDB : 108, 219: 56172 (27867 inversed)
            var type = Encoding.ASCII.GetString(data.Range(6, 8)); //MS: Meter Status?
            var from = data.Range(8, 10);
            var to = data.Range(10, 12);
            var msg = Encoding.ASCII.GetString(data.Range(12, 16));
            //Console.WriteLine(msg);
            switch (msg)
            {
                case "levl":
                    for (int i = 0; i < 32; i++)
                    {
                        var val = BitConverter.ToUInt16(data, 20 + (i * 2));
                        float normalizedValue = (float)val / (float)ushort.MaxValue;
                        //_values.Line[i] = normalizedValue;
                        _rawService.SetValue("line/ch" + (i + 1).ToString() + "/meter", normalizedValue);
                    }
                    //_values.RaiseModelUpdated();
                    break;
                case "redu":

                    break;
            }
            return;
        }

        public void Dispose()
        {
            _udpClient.Dispose();
        }
    }
}
