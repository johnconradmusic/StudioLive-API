using ICSharpCode.SharpZipLib.Zip.Compression.Streams;
using Newtonsoft.Json;
using Presonus.UCNet.Api.Extensions;
using Presonus.UCNet.Api.Helpers;
using Presonus.UCNet.Api.Messages;
using Presonus.UCNet.Api.Messages.Readers;
using Presonus.UCNet.Api.Messages;
using Presonus.UCNet.Api.Messages.Readers;
using Presonus.UCNet.Api.Models;

using Presonus.UCNet.Api.Services;
using Serilog;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Text.Json;
using System.Threading;
using Newtonsoft.Json.Linq;

namespace Presonus.UCNet.Api.Services
{
	public class CommunicationService : IDisposable
	{
		private readonly MeterService _meterService;
		private readonly MixerStateService _mixerStateService;

		private TcpClient _tcpClient;
		private Thread _listeningThread;
		private Thread _writingThread;

		private ushort _deviceId;

		private List<byte[]> chunkBuffer = new List<byte[]>();

		public CommunicationService(MeterService meterService, MixerStateService mixerStateService)
		{
			_meterService = meterService;
			_mixerStateService = mixerStateService;

			_mixerStateService.SendValueMethod = SendValue;
			_mixerStateService.SendStringMethod = SendString;
			_mixerStateService.RecallProject = RecallProject;
			_mixerStateService.RecallScene = RecallScene;
			_mixerStateService.GetProjects = RequestProjectList;
			_listeningThread = new Thread(Listener) { IsBackground = true };
			_writingThread = new Thread(KeepAlive) { IsBackground = true };
		}

		private void RequestProjectList()
		{
			var writer = new TcpMessageWriter(_deviceId);
			var data = writer.CreateProjectsRequest();

			SendMessage(data);
		}

		public static bool ConnectionEstablished { get; set; }
		public bool IsConnected => _tcpClient?.Connected ?? false;

		private static List<float> ReadValues(BinaryReader reader, int count)
		{
			var values = new List<float>();
			for (int i = 0; i < count; i++)
			{
				values.Add(reader.ReadUInt16() / 65535f);
			}
			return values;
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
				case MessageCode.FileData:
					Console.WriteLine("FILE DATA");
					break;
				case MessageCode.Unknown1:
					break;

				case MessageCode.Chunk:
					CK(chunk);
					break;

				case MessageCode.ParamStrList:
					PL(chunk);
					break;

				case MessageCode.ParamColor:
					ProcessColorData(chunk);
					break;

				case "PR":
					break;

				case MessageCode.ParamValue:
					ProcessValue(chunk);
					break;

				case MessageCode.JSON:
					var jm = JM.GetJsonMessage(chunk);
					ProcessJson(jm);
					break;

				case MessageCode.CompressedJSON:
					var zm = ZM.GetJsonMessage(chunk);
					ProcessJson(zm);
					break;

				case MessageCode.ParamString:
					PS(chunk);
					break;

				case MessageCode.FaderPosition:
					ParseFaderData(chunk);
					break;

				case MessageCode.ZLIB:
					var data = chunk.Skip(16).ToArray();
					HandleZlib(data);
					break;

				default:
					break;
			}
		}

		private void ProcessColorData(byte[] data)
		{
			data = data.Skip(12).ToArray();

			int idx = Array.IndexOf(data, (byte)0x00); // Find the NULL terminator of the key string

			string route = Encoding.ASCII.GetString(data, 0, idx); // Convert the bytes to a string

			//Console.WriteLine($"PC ({route}) {idx}");
			byte[] newArray = new byte[data.Length - idx - 3]; // Create a new array to hold the extracted bytes
			Array.Copy(data, idx + 3, newArray, 0, newArray.Length); // Copy the bytes to the new array

			var val = BitConverter.ToString(newArray);

			var valString = val.Replace("-", "");

			_mixerStateService.SetString(route, valString, false);
		}

		private void HandleZlib(byte[] chunk)
		{
			Console.WriteLine("Handling ZLib");
			var info = DeserializeZlibBuffer(chunk);

			var jsonString = JsonConvert.SerializeObject(info, Formatting.Indented);
			//File.WriteAllText("C:\\Dev\\jsondump1.json", jsonString);
			_mixerStateService.Synchronize(jsonString);
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

		private void ProcessJson(string json)
		{
			var jsonElement = System.Text.Json.JsonSerializer.Deserialize<JsonElement>(json);
			if (!jsonElement.TryGetProperty("id", out var idProperty))
				return;

			var id = idProperty.GetString();

			switch (id)
			{
				case "SynchronizePart":
					HandleSynchronizePart();
					break;

				case "Synchronize":
					_mixerStateService.Synchronize(json);
					break;

				case "SubscriptionReply":
					HandleSubscriptionReply();
					break;

				case "SubscriptionLost":
					RequestCommunicationMessage();
					break;

				case "UserLoggedIn":
					HandleUserLoggedIn();
					break;

				case "RecalledPreset":
					HandleRecalledPreset();
					break;
				case "StoredPreset":
					HandleStoredPreset();
					break;

				default:
					Log.Warning("[{className}] Unknown json id {messageType}", nameof(CommunicationService), id);
					break;
			}
		}

		private void HandleStoredPreset()
		{
			Console.WriteLine("Stored a Preset");
		}

		private void HandleSynchronizePart()
		{
			Console.WriteLine("HandleSynchronizePart");
		}

		private void HandleSubscriptionReply()
		{
			Console.WriteLine("Connection established.");
			ConnectionEstablished = true;
			foreach (var kvp in Mixer.ChannelCounts)
			{
				Console.WriteLine($"{kvp.Key} - {kvp.Value}");
			}
		}

		private void HandleUserLoggedIn()
		{
			Console.WriteLine("User logged in.");
		}

		private void HandleRecalledPreset()
		{
			Console.WriteLine("Preset recalled.");
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

		private void ProcessValue(byte[] data)
		{
			var header = data.Range(0, 4);
			var messageLength = data.Range(4, 6);
			var messageType = data.Range(6, 8);
			var from = data.Range(8, 10);
			var to = data.Range(10, 12);

			var route = Encoding.ASCII.GetString(data.Range(12, -7));
			var emptyBytes = data.Range(-7, -4);
			var value = BitConverter.ToSingle(data.Range(-4), 0);
			Console.WriteLine($"Parameter changed: {route} ({value})");
			if (route.Contains("/dca/")) return;
			_mixerStateService.SetValue(route, value, false);
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

		public static Dictionary<string, object> DeserializeZlibBuffer(byte[] buffer)
		{
			using var inputStream = new MemoryStream(buffer, 0, buffer.Length);
			using var zlibStream = new InflaterInputStream(inputStream);
			using var outputStream = new MemoryStream();

			zlibStream.CopyTo(outputStream);

			byte[] decompressedData = outputStream.ToArray();
			return ZlibPayloadDeserializer.DeserializeDecompressedBuffer(decompressedData);
		}

		public void ParseFaderData(byte[] data)
		{
			data = data.Skip(20).ToArray();

			var order = new ChannelTypes[] 
			{ ChannelTypes.LINE, ChannelTypes.RETURN, ChannelTypes.FXRETURN, ChannelTypes.TALKBACK, ChannelTypes.AUX, ChannelTypes.FX, ChannelTypes.MAIN };

			var values = new Dictionary<ChannelTypes, List<float>>();

			using (var stream = new MemoryStream(data))
			using (var reader = new BinaryReader(stream))
			{
				for (int i = 0; i < order.Length; i++)
				{
					ChannelTypes type = order[i]; //"LINE", "RETURN", etc..
					values[type] = ReadValues(reader, Mixer.ChannelCounts[type]);
					for (int j = 0; j < values[type].Count; j++)
					{
						var count = values[type][j];
						var chanString = ChannelUtil.GetChannelString(new(type, j + 1, null, null)) + "/volume";
						_mixerStateService.SetValue(chanString, count, false);
					}
				}
			}
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
		public void RecallScene(string projFile, string sceneFile)
		{
			var writer = new TcpMessageWriter(_deviceId);
			var data = writer.CreateSceneRecall(projFile, sceneFile);

			SendMessage(data);
		}
		public void RecallProject(string projFile)
		{
			var writer = new TcpMessageWriter(_deviceId);
			var data = writer.CreateProjectRecall(projFile);

			SendMessage(data);
		}

		public void SendString(string route, string value)
		{
			var writer = new TcpMessageWriter(_deviceId);
			var data = writer.CreateRouteStringUpdate(route, value);

			SendMessage(data);
		}

		public void SendValue(string route, float value)
		{
			Console.WriteLine($"Sending value update: {route} {value}");
			var writer = new TcpMessageWriter(_deviceId);
			var data = writer.CreateRouteValueUpdate(route, value);

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

		public struct SettingType
		{
			public char[] name;
			public object value;
		}
	}
}