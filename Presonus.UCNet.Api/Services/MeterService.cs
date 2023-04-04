using Presonus.StudioLive32.Api.Extensions;
using Presonus.StudioLive32.Api.Helpers;
using Presonus.StudioLive32.Api;
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

namespace Presonus.StudioLive32.Api.Services
{
	public class MeterService : IDisposable
	{
		private readonly UdpClient _udpClient;
		private readonly MixerStateService _mixerStateService;
		private bool _disposed;

		public MeterService(MixerStateService mixerStateService)
		{
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

				if (messageType != "MS") continue;

				AnalyzeMeterData(data);

				// Add the delay here

			}
		}

		private void AnalyzeMeterData(byte[] data)
		{
			var msg = Encoding.ASCII.GetString(data.Range(12, 16));
			ProcessMeteringData(data, msg);
		}
		List<float> inputs = new();
		Dictionary<string, List<float>> inputStrips = new();
		List<float> faders = new();
		Dictionary<string, List<float>> fxReturnStrips = new();
		List<float> auxes = new();
		Dictionary<string, List<float>> auxStrips = new();
		Dictionary<string, List<float>> fxStrips = new();
		List<float> main = new();
		Dictionary<string, List<float>> mainStrips = new();

		List<float> gateReduction = new();
		List<float> compReduction = new();
		List<float> limitReduction = new();

		private void ProcessMeteringData(byte[] data, string msg)
		{
			if (msg == "levl")
			{
				data = data.Skip(20).ToArray();

				ReadValues(data, Mixer.ChannelCounts["LINE"], inputs, out data);
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

			WriteMeterValues();
		}

		private void WriteMeterValues()
		{
			for (int i = 0; i < inputs.Count; i++)
			{
				var meter = inputs[i];
				//Console.WriteLine($"meter {meter}");
				_mixerStateService.SetValue("line/ch" + (i + 1).ToString() + "/meter", meter, false);
			}


		}

		private void ReadValues(byte[] data, int count, List<float> values, out byte[] remainingData, int skipBytes = 0)
		{
			values.Clear();
			for (int i = 0; i < count; i++)
			{
				float val = BitConverter.ToUInt16(data, (skipBytes + i) * 2);
				values.Add(val / 65535);
			}
			remainingData = data.Skip((skipBytes + count) * 2).ToArray();
		}

		private void ReadStripValues(byte[] data, string[] stripNames, int count, Dictionary<string, List<float>> strips, out byte[] remainingData, int skipBytes = 0)
		{
			strips.Clear();
			List<float> tempList = new List<float>();

			foreach (var stripName in stripNames)
			{
				ReadValues(data, count, tempList, out data, skipBytes);
				strips.Add(stripName, new List<float>(tempList));
				skipBytes = 0; // Only skip bytes for the first strip
			}

			remainingData = data;
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