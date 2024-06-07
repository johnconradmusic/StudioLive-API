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
		public event EventHandler<RTADataEventArgs> RTADataReceived;

		private readonly UdpClient _udpClient;
		private bool _disposed;

		public MeterData MeterData = new();
		public ReductionMeterData ReductionMeterData = new();
		public RTAData RTAData = new RTAData();

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
				await Task.Delay(10);

			}
		}

		private void ProcessMeteringData(byte[] data)
		{
			var msg = Encoding.ASCII.GetString(data.Range(12, 16));
			if (msg == MessageCode.Meter)
			{
				data = data.Skip(20).ToArray();
				ReadMeterValues(data);
				MeterDataReceived?.Invoke(this, new(MeterData));
			}
			else if (msg == MessageCode.Reduction)
			{
				data = data.Skip(20).ToArray();
				ReadReductionValues(data);
				ReductionDataReceived?.Invoke(this, new(ReductionMeterData));
			}
			else if (msg == MessageCode.RTA)
			{
				data = data.Skip(20).ToArray();
				ReadRTAValues(data);
				RTADataReceived?.Invoke(this, new(RTAData));
			}
		}

		private void ReadRTAValues(byte[] data)
		{
			var val = ReadValues(data, 99, divisor: short.MaxValue);
			RTAData.Data = val;
		}



		private float[] ReadValues(byte[] data, int count, int skipBytes = 0, int bytesPerValue = 2, float divisor = 65535f)
		{
			float[] values = new float[count];
			int offset = skipBytes * bytesPerValue;

			for (int i = 0; i < count; i++)
			{
				float val = BitConverter.ToUInt16(data, offset);
				values[i] = val / divisor;
				offset += bytesPerValue;
			}

			return values;
		}
		private void ReadReductionValues(byte[] data)
		{
			ReductionMeterData.InputGateReduction = ReadValues(data, Mixer.ChannelCounts[ChannelTypes.LINE]);
			ReductionMeterData.InputCompReduction = ReadValues(data, Mixer.ChannelCounts[ChannelTypes.LINE]);
			ReductionMeterData.InputLimitReduction = ReadValues(data, Mixer.ChannelCounts[ChannelTypes.LINE]);
		}
		private void ReadMeterValues(byte[] data)
		{
			MeterData.InputInput = ReadValues(data, Mixer.ChannelCounts[ChannelTypes.LINE]);
			MeterData.InputPreGate = ReadValues(data, Mixer.ChannelCounts[ChannelTypes.LINE], 3);
			MeterData.InputPostGate = ReadValues(data, Mixer.ChannelCounts[ChannelTypes.LINE]);
			MeterData.InputPostComp = ReadValues(data, Mixer.ChannelCounts[ChannelTypes.LINE]);
			MeterData.InputPostEQ = ReadValues(data, Mixer.ChannelCounts[ChannelTypes.LINE]);
			MeterData.InputPostLimit = ReadValues(data, Mixer.ChannelCounts[ChannelTypes.LINE]);
			MeterData.InputPostFader = ReadValues(data, Mixer.ChannelCounts[ChannelTypes.LINE]);
			MeterData.FxReturnStrip["input"] = ReadValues(data, Mixer.ChannelCounts[ChannelTypes.RETURN] * 2, 8);
			MeterData.FxReturnStrip["stripA"] = ReadValues(data, Mixer.ChannelCounts[ChannelTypes.RETURN] * 2);
			MeterData.FxReturnStrip["stripB"] = ReadValues(data, Mixer.ChannelCounts[ChannelTypes.RETURN] * 2);
			MeterData.FxReturnStrip["stripC"] = ReadValues(data, Mixer.ChannelCounts[ChannelTypes.RETURN] * 2);

			MeterData.AuxMetering = ReadValues(data, Mixer.ChannelCounts[ChannelTypes.AUX]);

			MeterData.AuxChStrip["stripA"] = ReadValues(data, Mixer.ChannelCounts[ChannelTypes.AUX]);
			MeterData.AuxChStrip["stripB"] = ReadValues(data, Mixer.ChannelCounts[ChannelTypes.AUX]);
			MeterData.AuxChStrip["stripC"] = ReadValues(data, Mixer.ChannelCounts[ChannelTypes.AUX]);
			MeterData.AuxChStrip["stripD"] = ReadValues(data, Mixer.ChannelCounts[ChannelTypes.AUX]);


			MeterData.FxChStrip["inputs"] = ReadValues(data, Mixer.ChannelCounts[ChannelTypes.FX]);
			MeterData.FxChStrip["stripA"] = ReadValues(data, Mixer.ChannelCounts[ChannelTypes.FX]);
			MeterData.FxChStrip["stripB"] = ReadValues(data, Mixer.ChannelCounts[ChannelTypes.FX]);
			MeterData.FxChStrip["stripC"] = ReadValues(data, Mixer.ChannelCounts[ChannelTypes.FX]);

			MeterData.Main = ReadValues(data, Mixer.ChannelCounts[ChannelTypes.MAIN] * 2);

			MeterData.MainChStrip["stageA"] = ReadValues(data, Mixer.ChannelCounts[ChannelTypes.MAIN] * 2);
			MeterData.MainChStrip["stageB"] = ReadValues(data, Mixer.ChannelCounts[ChannelTypes.MAIN] * 2);
			MeterData.MainChStrip["stageC"] = ReadValues(data, Mixer.ChannelCounts[ChannelTypes.MAIN] * 2);
			MeterData.MainChStrip["stageD"] = ReadValues(data, Mixer.ChannelCounts[ChannelTypes.MAIN] * 2);

		}


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

	public class RTADataEventArgs
	{
		public RTAData RTAData { get; }

		public RTADataEventArgs(RTAData rtaData)
		{
			RTAData = rtaData;
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