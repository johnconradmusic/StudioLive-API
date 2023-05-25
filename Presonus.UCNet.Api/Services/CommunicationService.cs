using ICSharpCode.SharpZipLib.Zip.Compression.Streams;
using Newtonsoft.Json;
using Presonus.UCNet.Api.Extensions;
using Presonus.UCNet.Api.Helpers;
using Presonus.UCNet.Api.Messages;
using Presonus.UCNet.Api.Messages.Readers;
using Presonus.UCNet.Api.Models;
using Serilog;
using Serilog.Core;
using System;
using System.Buffers.Binary;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Shapes;
using System.Xml.Linq;

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

			_mixerStateService.FileOperationMethod = FileOperation;

			_mixerStateService.GetProjects = RequestProjectList;
			_mixerStateService.GetScenes = RequestSceneList;
			_mixerStateService.GetPresets = RequestChannelPresets;

			_mixerStateService.ChannelResetMethod = SendChannelReset;

			_mixerStateService.ChannelCopyPaste = ChannelCopyOrPaste;

			_listeningThread = new Thread(Listener) { IsBackground = true };
			_writingThread = new Thread(KeepAlive) { IsBackground = true };
		}
		public void ChannelCopyOrPaste(ChannelSelector channel, bool paste)
		{
			var writer = new TcpMessageWriter(_deviceId);
			var data = writer.CreateChannelCopyOrPaste(channel, paste);
			SendMessage(data);
		}


		public static bool ConnectionEstablished { get; set; }

		public bool IsConnected => _tcpClient?.Connected ?? false;

		public void SendChannelReset(ChannelTypes channelType, int channelIndex)
		{
			var writer = new TcpMessageWriter(_deviceId);
			var data = writer.CreateResetChannelMessage(channelType, channelIndex);
			SendMessage(data);
		}

		private static List<float> ReadValues(BinaryReader reader, int count)
		{
			var values = new List<float>();
			for (int i = 0; i < count; i++)
			{
				values.Add(reader.ReadUInt16() / 65535f);
			}
			return values;
		}
		public const string CHANNEL_PRESETS = "presets/channel";
		public const string PROJECTS = "presets/proj";
		List<GenericListItem> requestedItems;
		private async Task<List<GenericListItem>> RequestProjectList()
		{
			requestedItems = new();
			requestCompleted = false;

			var writer = new TcpMessageWriter(_deviceId);
			var data = writer.CreateFileRequest(PROJECTS);
			SendMessage(data);

			while (!requestCompleted)
			{
				await Task.Delay(10);
			}
			Console.WriteLine("request complete.");
			return requestedItems;
		}
		private async Task<List<GenericListItem>> RequestChannelPresets()
		{
			requestedItems = new();
			requestCompleted = false;

			var writer = new TcpMessageWriter(_deviceId);
			var data = writer.CreateFileRequest(CHANNEL_PRESETS);
			SendMessage(data);

			while (!requestCompleted)
			{
				await Task.Delay(10);
			}
			Console.WriteLine("request complete.");
			return requestedItems;
		}
		private async Task<List<GenericListItem>> RequestSceneList(string proj)
		{
			requestedItems = new();
			requestCompleted = false;
			string key = "presets/" + proj;
			var writer = new TcpMessageWriter(_deviceId);
			var data = writer.CreateFileRequest(key);
			Console.WriteLine($"requesting {key}");
			SendMessage(data);
			while (!requestCompleted)
			{
				await Task.Delay(10);
			}
			Console.WriteLine("request complete.");
			return requestedItems;
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
			//Console.WriteLine("msg");
			var messageType = PackageHelper.GetMessageType(chunk);

			Console.WriteLine(messageType);
			switch (messageType)
			{
				case MessageCode.FileData:
					HandleFileData(chunk);

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
					var jmsg = JM.GetJsonMessage(chunk);
					ProcessJson(jmsg);
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
		Dictionary<int, ChunkSet> files = new Dictionary<int, ChunkSet>();

		private void HandleFileData(byte[] data)
		{
			ChunkData ret;

			var ids = UniqueRandom.Get(16);
			var chunk = ParseChunk(data);
			if (!ids.Active.Contains(chunk.id))
			{
				Console.WriteLine("Invalid FD Chunk");
			}
			if (chunk.totalSize == chunk.payloadSize)
			{
				ret = new ChunkData()
				{
					id = chunk.id,
					data = chunk.data
				};
				ids.Release(ret.id);
				var jm = Encoding.ASCII.GetString(ret.data);
				File.WriteAllText("C:\\Dev\\jsonTest2.json", jm);
				ProcessReceivedFileData(jm);
			}
			else
			{
				ChunkSet currChunkSet;
				if (!files.TryGetValue(chunk.id, out currChunkSet))
				{
					// Create new entry for the chunk set
					currChunkSet = new ChunkSet()
					{
						max = chunk.totalSize,
						data = new byte[0]
					};
					files.Add(chunk.id, currChunkSet);
				}
				// Ensure the data buffer matches the chunk size
				if (chunk.payloadSize != chunk.data.Length)
				{
					Console.WriteLine($"FD chunk with ID {chunk.id} was inconsistent in size (expected {chunk.payloadSize}, got {chunk.data.Length})");
					return;
				}

				// Ensure consistent state
				if (chunk.totalSize != currChunkSet.max || chunk.bytesRead != currChunkSet.data.Length)
				{
					Console.WriteLine($"FD chunk with ID {chunk.id} was not consistent with the chunk set information");
					return;
				}

				currChunkSet.data = currChunkSet.data.Concat(chunk.data).ToArray();
				if (currChunkSet.max == currChunkSet.data.Length)
				{
					ret = new ChunkData()
					{
						id = chunk.id,
						data = currChunkSet.data
					};
					ids.Release(ret.id);
					var jm = Encoding.ASCII.GetString(ret.data);
					File.WriteAllText("C:\\Dev\\jsonTest2.json", jm);
					ProcessReceivedFileData(jm);
				}
			}
		}
		bool requestCompleted = false;
		private void ProcessReceivedFileData(string jm)
		{
			Console.WriteLine("start processing");
			var jsonElement = System.Text.Json.JsonSerializer.Deserialize<JsonElement>(jm);

			if (jsonElement.TryGetProperty("files", out var files))
			{
				var array = files.EnumerateArray().ToArray();
				foreach (var file in array)
				{
					file.TryGetProperty("name", out var name);
					file.TryGetProperty("title", out var title);
					if (!name.GetString().EndsWith(".cnfg") && !name.GetString().EndsWith(".lock"))
					{
						Console.WriteLine("added an item");
						requestedItems.Add(new() { Name = name.GetString(), Title = title.GetString() });
					}
				}
			}
			Console.WriteLine("completed processing");
			requestCompleted = true;
		}

		public class ChunkData
		{
			/**
			 * Chunk ID
			 */
			public ushort id;

			/**
			 * Data buffer
			 */
			public byte[] data;

			/**
			 * Current position / Number of previous bytes in the chunk set
			 */
			public ushort bytesRead;

			/**
			 * Total size of the chunk set
			 */
			public ushort totalSize;

			/**
			 * Size of the current chunk
			 **/
			public ushort payloadSize;
		}

		public static ChunkData ParseChunk(byte[] data)
		{
			data = data.Skip(12).ToArray();
			byte[] header = new byte[14];
			Array.Copy(data, 0, header, 0, 14);

			ChunkData chunkData = new ChunkData();

			chunkData.id = ReadUInt16BE(header, 0);
			header = header[2..];

			chunkData.bytesRead = ReadUInt16LE(header, 0);
			header = header[2..];

			byte[] unknown1 = header[0..2];
			header = header[2..];

			chunkData.totalSize = ReadUInt16LE(header, 0);
			header = header[2..];

			byte[] unknown2 = header[0..4];
			header = header[4..];

			chunkData.payloadSize = ReadUInt16LE(header, 0);

			chunkData.data = data[14..];

			return chunkData;
		}

		public static ushort ReadUInt16BE(byte[] buffer, int offset)
		{
			byte[] reversedBytes = new byte[2] { buffer[offset + 1], buffer[offset] };
			return BitConverter.ToUInt16(reversedBytes, 0);
		}

		public static ushort ReadUInt16LE(byte[] buffer, int offset)
		{
			return BitConverter.ToUInt16(buffer, offset);
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
			//File.WriteAllText("C:\\Dev\\jsondump2.json", jsonString);
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
			Console.WriteLine($"String changed: {route} ({value})");

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

		public void FileOperation(Presets.OperationType operation, string projFile, string sceneFile = null, ChannelSelector selector = null)
		{
			var writer = new TcpMessageWriter(_deviceId);
			byte[] data = writer.CreatePresetMessage(operation, projFile, sceneFile, selector);
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
	}
}
