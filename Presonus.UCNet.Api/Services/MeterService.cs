using Presonus.UCNet.Api.Extensions;
using Presonus.UCNet.Api.Helpers;
using Presonus.UCNet.Api.Messages;
using Presonus.UCNet.Api.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace Presonus.UCNet.Api.Services
{
	public class MeterService : IDisposable
	{
		public event EventHandler<MeterDataEventArgs> MeterDataReceived;
		public event EventHandler<ReductionDataEventArgs> ReductionDataReceived;

		private readonly UdpClient _udpClient;
		private bool _disposed;

		private MeterData meterData = new();
		private ReductionMeterData reductionMeterData = new();

		public MeterService()
		{
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
				int delayMilliseconds = 1000;
				await Task.Delay(delayMilliseconds);
			}
			while (!_disposed)
			{
				var result = await _udpClient.ReceiveAsync();
				var data = result.Buffer;
				var isUcNetPackage = PackageHelper.IsUcNetPackage(data);
				if (!isUcNetPackage) continue;

				var messageType = PackageHelper.GetMessageType(data);
				if (messageType != MessageCode.MeterSingles)
				{
					Console.WriteLine($"meter service message {messageType}");
					continue;
				}
				ProcessMeteringData(data);
				await Task.Delay(20);

			}
		}

		private void ProcessMeteringData(byte[] data)
		{
			var msg = Encoding.ASCII.GetString(data.Range(12, 16));
			if (msg == MessageCode.Meter)
			{
				data = data.Skip(20).ToArray();
				ReadMeterValues(data);
				MeterDataReceived?.Invoke(this, new(meterData));
			}
			else if (msg == MessageCode.Reduction)
			{
				data = data.Skip(20).ToArray();
				ReadReductionValues(data);
				ReductionDataReceived?.Invoke(this, new(reductionMeterData));
			}
		}

		private float[] ReadValues(byte[] data, int count, int skipBytes = 0, int bytesPerValue = 2)
		{
			float[] values = new float[count];
			int offset = skipBytes * bytesPerValue;

			for (int i = 0; i < count; i++)
			{
				float val = BitConverter.ToUInt16(data, offset);
				values[i] = val / 65535f;
				offset += bytesPerValue;
			}

			return values;
		}
		private void ReadReductionValues(byte[] data)
		{
			reductionMeterData.InputGateReduction = ReadValues(data, Mixer.ChannelCounts[ChannelTypes.LINE]);
			reductionMeterData.InputCompReduction = ReadValues(data, Mixer.ChannelCounts[ChannelTypes.LINE]);
			reductionMeterData.InputLimitReduction = ReadValues(data, Mixer.ChannelCounts[ChannelTypes.LINE]);
		}
		private void ReadMeterValues(byte[] data)
		{
			meterData.InputInput = ReadValues(data, Mixer.ChannelCounts[ChannelTypes.LINE]);
			meterData.InputPreGate = ReadValues(data, Mixer.ChannelCounts[ChannelTypes.LINE], 3);
			meterData.InputPostGate = ReadValues(data, Mixer.ChannelCounts[ChannelTypes.LINE]);
			meterData.InputPostComp = ReadValues(data, Mixer.ChannelCounts[ChannelTypes.LINE]);
			meterData.InputPostEQ = ReadValues(data, Mixer.ChannelCounts[ChannelTypes.LINE]);
			meterData.InputPostLimit = ReadValues(data, Mixer.ChannelCounts[ChannelTypes.LINE]);
			meterData.InputPostFader = ReadValues(data, Mixer.ChannelCounts[ChannelTypes.LINE]);
		}
			//meterData.FxReturnStrip["input"] = ReadValues(Mixer.ChannelCounts[ChannelTypes.RETURN] * 2, 8);
			//meterData.FxReturnStrip["stripA"] = ReadValues(Mixer.ChannelCounts[ChannelTypes.RETURN] * 2);
			//meterData.FxReturnStrip["stripB"] = ReadValues(Mixer.ChannelCounts[ChannelTypes.RETURN] * 2);
			//meterData.FxReturnStrip["stripC"] = ReadValues(Mixer.ChannelCounts[ChannelTypes.RETURN] * 2);

			//meterData.AuxMetering = ReadValues(Mixer.ChannelCounts[ChannelTypes.AUX]);

			//meterData.AuxChStrip["stripA"] = ReadValues(Mixer.ChannelCounts[ChannelTypes.AUX]);
			//meterData.AuxChStrip["stripB"] = ReadValues(Mixer.ChannelCounts[ChannelTypes.AUX]);
			//meterData.AuxChStrip["stripC"] = ReadValues(Mixer.ChannelCounts[ChannelTypes.AUX]);
			//meterData.AuxChStrip["stripD"] = ReadValues(Mixer.ChannelCounts[ChannelTypes.AUX]);


			//meterData.FxChStrip["inputs"] = ReadValues(Mixer.ChannelCounts[ChannelTypes.FX]);
			//meterData.FxChStrip["stripA"] = ReadValues(Mixer.ChannelCounts[ChannelTypes.FX]);
			//meterData.FxChStrip["stripB"] = ReadValues(Mixer.ChannelCounts[ChannelTypes.FX]);
			//meterData.FxChStrip["stripC"] = ReadValues(Mixer.ChannelCounts[ChannelTypes.FX]);

			//meterData.Main = ReadValues(Mixer.ChannelCounts[ChannelTypes.MAIN] * 2);

			//meterData.MainChStrip["stageA"] = ReadValues(Mixer.ChannelCounts[ChannelTypes.MAIN] * 2);
			//meterData.MainChStrip["stageB"] = ReadValues(Mixer.ChannelCounts[ChannelTypes.MAIN] * 2);
			//meterData.MainChStrip["stageC"] = ReadValues(Mixer.ChannelCounts[ChannelTypes.MAIN] * 2);
			//meterData.MainChStrip["stageD"] = ReadValues(Mixer.ChannelCounts[ChannelTypes.MAIN] * 2);

		

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
	}
	public class MeterDataEventArgs : EventArgs
	{
		public MeterData MeterData { get; }

		public MeterDataEventArgs(MeterData meterData)
		{
			MeterData = meterData;
		}
	}

	public class ReductionDataEventArgs : EventArgs
	{
		public ReductionMeterData ReductionData { get; }

		public ReductionDataEventArgs(ReductionMeterData reductionData)
		{
			ReductionData = reductionData;
		}
	}
}