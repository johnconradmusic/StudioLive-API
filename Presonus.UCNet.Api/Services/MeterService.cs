using Presonus.UCNet.Api.Extensions;
using Presonus.UCNet.Api.Helpers;
using Presonus.UCNet.Api;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using Presonus.UCNet.Api.Models;
using Microsoft.VisualBasic;
using System.Threading;
using Presonus.UCNet.Api.Services;
using Presonus.UCNet.Api.NewDataModel;

namespace Presonus.UCNet.Api.Services
{
	public class MeterService : IDisposable
	{
		private readonly UdpClient _udpClient;
		private readonly MixerStateService _mixerStateService;
		private readonly MeterDataStorage _meterDataStorage;
		private bool _disposed;

		public MeterService(MixerStateService mixerStateService, MeterDataStorage meterDataStorage)
		{
			this._meterDataStorage = meterDataStorage;
			this._mixerStateService = mixerStateService;
			_udpClient = new UdpClient(0);
			var ipEndpoint = _udpClient.Client.LocalEndPoint as IPEndPoint;
			if (ipEndpoint is null)
				throw new InvalidOperationException("Failed to start UDP server.");

			Port = (ushort)ipEndpoint.Port;

			StartListening();
		}

		public ushort Port { get; }

		private async void StartListening()
		{
			while (!Mixer.Counted)
			{
				int delayMilliseconds = 10;
				await Task.Delay(delayMilliseconds);
			}
			while (!_disposed)
			{
				var result = await _udpClient.ReceiveAsync();
				var data = result.Buffer;
				var isUcNetPackage = PackageHelper.IsUcNetPackage(data);
				if (!isUcNetPackage) continue;

				var messageType = PackageHelper.GetMessageType(data);
				if (messageType != "MS")
				{
					Console.WriteLine($"meter server message {messageType}");
					continue;
				}
				AnalyzeMeterData(data);

				// Add the delay here

			}
		}

		private void AnalyzeMeterData(byte[] data)
		{
			var msg = Encoding.ASCII.GetString(data.Range(12, 16));
			ProcessMeteringData(data, msg);
		}
	
		private void ProcessMeteringData(byte[] data, string msg)
		{
			//Console.WriteLine(msg);
			if (msg == "levl")
			{
				data = data.Skip(20).ToArray();
				var meterData = ReadLevl(data);
				_meterDataStorage.UpdateMeterData(meterData);

				//ReadStripValues(data, new[] { "pregate", "postgate", "postcomp", "posteq", "postlimiter" }, Mixer.ChannelCounts["LINE"], inputStrips, out data);

				//ReadValues(data, Mixer.ChannelCounts["LINE"], faders, out data);
				//ReadStripValues(data, new[] { "input", "stripA", "stripB", "stripC" }, 2 * 2, fxReturnStrips, out data, 8);
				//ReadValues(data, 6, auxes, out data);
				//ReadStripValues(data, new[] { "stripA", "stripB", "stripC", "stripD" }, 6, auxStrips, out data);
				//ReadStripValues(data, new[] { "inputs", "stripA", "stripB", "stripC" }, 2, fxStrips, out data);
				//ReadValues(data, 1 * 2, main, out data);
				//ReadStripValues(data, new[] { "stageA", "stageB", "stageC", "stageD" }, 1 * 2, mainStrips, out data);
			}
			else if (msg == "redu")
			{
				//data = data.Skip(20).ToArray();

				//ReadValues(data, Mixer.ChannelCounts["LINE"], gateReduction, out data);
				//ReadValues(data, 16, compReduction, out data);
				//ReadValues(data, 16, limitReduction, out data);
			}
		}

		private MeterData ReadLevl(byte[] data)
		{
			int offset = 0;

			float[] ReadValues(int count, int skipBytes = 0)
			{
				float[] values = new float[count];
				offset += skipBytes * 2;

				for (int i = 0; i < count; i++)
				{
					float val = BitConverter.ToUInt16(data, offset);
					values[i] = val / 65535f;
					offset += 2;
				}

				return values;
			}

			var meterData = new MeterData
			{
				Input = ReadValues(Mixer.ChannelCounts[ChannelTypes.LINE.ToString()]),
				ChannelStrip = new Dictionary<string, float[]>
				{
					["stripA"] = ReadValues(Mixer.ChannelCounts[ChannelTypes.LINE.ToString()], 3),
					["stripB"] = ReadValues(Mixer.ChannelCounts[ChannelTypes.LINE.ToString()]),
					["stripC"] = ReadValues(Mixer.ChannelCounts[ChannelTypes.LINE.ToString()]),
					["stripD"] = ReadValues(Mixer.ChannelCounts[ChannelTypes.LINE.ToString()]),
					["stripE"] = ReadValues(Mixer.ChannelCounts[ChannelTypes.LINE.ToString()])
				},
				MainMixFaders = ReadValues(Mixer.ChannelCounts[ChannelTypes.LINE.ToString()]),
				FxReturnStrip = new Dictionary<string, float[]>
				{
					["input"] = ReadValues(Mixer.ChannelCounts[ChannelTypes.RETURN.ToString()] * 2, 8),
					["stripA"] = ReadValues(Mixer.ChannelCounts[ChannelTypes.RETURN.ToString()] * 2),
					["stripB"] = ReadValues(Mixer.ChannelCounts[ChannelTypes.RETURN.ToString()] * 2),
					["stripC"] = ReadValues(Mixer.ChannelCounts[ChannelTypes.RETURN.ToString()] * 2)
				},
				AuxMetering = ReadValues(Mixer.ChannelCounts[ChannelTypes.AUX.ToString()]),
				AuxChStrip = new Dictionary<string, float[]>
				{
					["stripA"] = ReadValues(Mixer.ChannelCounts[ChannelTypes.AUX.ToString()]),
					["stripB"] = ReadValues(Mixer.ChannelCounts[ChannelTypes.AUX.ToString()]),
					["stripC"] = ReadValues(Mixer.ChannelCounts[ChannelTypes.AUX.ToString()]),
					["stripD"] = ReadValues(Mixer.ChannelCounts[ChannelTypes.AUX.ToString()])
				},
				FxChStrip = new Dictionary<string, float[]>
				{
					["inputs"] = ReadValues(Mixer.ChannelCounts[ChannelTypes.FX.ToString()]),
					["stripA"] = ReadValues(Mixer.ChannelCounts[ChannelTypes.FX.ToString()]),
					["stripB"] = ReadValues(Mixer.ChannelCounts[ChannelTypes.FX.ToString()]),
					["stripC"] = ReadValues(Mixer.ChannelCounts[ChannelTypes.FX.ToString()])
				},
				Main = ReadValues(Mixer.ChannelCounts[ChannelTypes.MAIN.ToString()] * 2),
				MainChStrip = new Dictionary<string, float[]>
				{
					["stageA"] = ReadValues(Mixer.ChannelCounts[ChannelTypes.MAIN.ToString()] * 2),
					["stageB"] = ReadValues(Mixer.ChannelCounts[ChannelTypes.MAIN.ToString()] * 2),
					["stageC"] = ReadValues(Mixer.ChannelCounts[ChannelTypes.MAIN.ToString()] * 2),
					["stageD"] = ReadValues(Mixer.ChannelCounts[ChannelTypes.MAIN.ToString()] * 2)
				}
			};

			return meterData;
		}




		#region IDisposable Support
		protected virtual void Dispose(bool disposing)
		{
			if (!_disposed)
			{
				if (disposing)
				{
					// Dispose managed state (managed objects)
					_udpClient.Dispose();
				}

				_disposed = true;
			}
		}

		public void Dispose()
		{
			Dispose(true);
		}

		#endregion
	}
}