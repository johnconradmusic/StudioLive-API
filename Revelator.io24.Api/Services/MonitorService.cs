//------------------------------------------------------------------------------
// The Assistant - Copyright (c) 2016-2023, John Conrad
//------------------------------------------------------------------------------
using Presonus.StudioLive32.Api.Extensions;
using Presonus.StudioLive32.Api.Helpers;
using Serilog;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics.Metrics;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;

namespace Presonus.StudioLive32.Api.Services
{
	public class MonitorService : IDisposable
	{
		private readonly UdpClient _udpClient;
		private readonly Thread _thread;

		private readonly RawService _rawService;

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

		public ushort Port { get; }

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

					Thread.Sleep(20);
				}
				catch (Exception exception)
				{
					Log.Error("[{className}] {exception}", nameof(MonitorService), exception);
				}
			}
		}

		private void Analyze(byte[] data)
		{
			var msg = Encoding.ASCII.GetString(data.Range(12, 16));
			//Console.WriteLine(data.Length);
			switch (msg)
			{
				case "levl":
					DoMetering(data);
					break;
			}
			return;
		}



		private void DoMetering(byte[] data)
		{
			data = data.Skip(20).ToArray();

			List<float> readValues(int count, int skipBytes = 0)
			{
				List<float> values = new List<float>();
				for (int i = 0; i < count; i++)
				{
					float val = BitConverter.ToUInt16(data, (skipBytes + i) * 2);

					values.Add(val/(ushort.MaxValue/2));
				}
				data = data.Skip((skipBytes + count) * 2).ToArray();
				return values;
			}

			var Inputs = readValues(16);


			for (int i = 0; i < Inputs.Count; i++)
			{
				var meter = Inputs[i];
				_rawService.SetValue("line/ch" + (i + 1).ToString() + "/meter", meter);
			}


			var InputStrips = new Dictionary<string, List<float>>
			{
				{ "pregate", readValues(16, 3) },
				{ "postgate", readValues(16) },
				{ "postcomp", readValues(16) },
				{ "posteq", readValues(16) },
				{ "postlimiter", readValues(16) }
			};

			for (int i = 0; i < Inputs.Count; i++)
			{
				float chan = Inputs[i];

				var gateReduction = InputStrips["pregate"][i] - InputStrips["postgate"][i];

				_rawService.SetValue("line/ch" + (i + 1).ToString() + "/gate/reduction", gateReduction);

				var compReduction = InputStrips["postgate"][i] - InputStrips["postcomp"][i];

				_rawService.SetValue("line/ch" + (i + 1).ToString() + "/comp/reduction", compReduction);

				var limiterReduction = InputStrips["postcomp"][i] - InputStrips["postlimiter"][i];

				_rawService.SetValue("line/ch" + (i + 1).ToString() + "/limiter/reduction", limiterReduction);

			}
			return;

			var Faders = readValues(16);

			var FXReturnStrips = new Dictionary<string, List<float>>
			{
				{ "input", readValues(2 * 2, 8) },
				{ "stripA", readValues(2 * 2) },
				{ "stripB", readValues(2 * 2) },
				{ "stripC", readValues(2 * 2) }
			};

			var Auxes = readValues(6);

			var AuxStrips = new Dictionary<string, List<float>>
			{
				{ "stripA", readValues(6) },
				{ "stripB", readValues(6) },
				{ "stripC", readValues(6) },
				{ "stripD", readValues(6) }
			};

			var FXStrips = new Dictionary<string, List<float>>
			{
				{ "inputs", readValues(2) },
				{ "stripA", readValues(2) },
				{ "stripB", readValues(2) },
				{ "stripC", readValues(2) }
			};

			var Main = readValues(1 * 2);

			var MainStrips = new Dictionary<string, List<float>>
			{
				{ "stageA", readValues(1 * 2) },
				{ "stageB", readValues(1 * 2) },
				{ "stageC", readValues(1 * 2) },
				{ "stageD", readValues(1 * 2) }
			};
		}

		public void Dispose()
		{
			_udpClient.Dispose();
		}
	}
}