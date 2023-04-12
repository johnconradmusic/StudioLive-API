﻿//------------------------------------------------------------------------------
// The Assistant - Copyright (c) 2016-2023, John Conrad
//------------------------------------------------------------------------------
using Presonus.UCNet.Api.Helpers;
using Presonus.UCNet.Api.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Windows.Input;
using System.Windows.Markup;

namespace Presonus.UCNet.Api.Messages
{
	public class TcpMessageWriter
	{
		private readonly ushort _deviceId;

		public TcpMessageWriter(ushort deviceId)
		{
			_deviceId = deviceId;
		}

		private static byte[] Create(List<byte> data, string messageType)
		{
			//Length [4..6]:
			var length = (ushort)(data.Count - 6);
			var lengthBytes = BitConverter.GetBytes(length);
			data[4] = lengthBytes[0];
			data[5] = lengthBytes[1];

			//MessageType [6..8]:
			var messageTypeBytes = Encoding.ASCII.GetBytes(messageType);
			if (messageTypeBytes.Length != 2)
				throw new InvalidOperationException("Messagetype must be two bytes.");

			data[6] = messageTypeBytes[0];
			data[7] = messageTypeBytes[1];

			return data.ToArray();
		}

		private static List<byte> CreateHeader(ushort deviceId)
		{
			var data = new List<byte>();

			//Header [0..4]:
			data.AddRange(Encoding.ASCII.GetBytes("UC"));
			data.AddRange(BitConverter.GetBytes((ushort)256));

			//Length [4..6] (placeholder):
			data.AddRange(new byte[] { 0x00, 0x00 });

			//MessageType [6..8] (placeholder):
			data.AddRange(new byte[] { 0x00, 0x00 });

			//From [8..10]:
			data.AddRange(BitConverter.GetBytes((ushort)104));

			//To [10..12]:
			data.AddRange(BitConverter.GetBytes(deviceId));

			return data;
		}
		public const string CHANNEL_PRESETS = "presets/channel";
		public const string PROJECTS = "presets/proj";

		public byte[] CreateProjectsRequest()
		{
			List<byte> data = CreateHeader(_deviceId);

			var key = PROJECTS;

			ushort id = (ushort)UniqueRandom.Get(16).Request();
			byte[] idBuffer = BitConverter.GetBytes(id);
			Array.Reverse(idBuffer); // Convert to big-endian format

			byte[] keyBuffer = Encoding.ASCII.GetBytes("List" + key.ToString());

			byte[] nullBuffer = new byte[] { 0x00, 0x00 };

			List<byte> packetBuffer = new List<byte>();
			packetBuffer.AddRange(idBuffer);
			packetBuffer.AddRange(keyBuffer);
			packetBuffer.AddRange(nullBuffer);

			return Create(packetBuffer, MessageCode.FileRequest);

		}

		public byte[] CreateSceneRecall(string projFile, string sceneFile)
		{
			var data = CreateHeader(_deviceId);

			string json = JsonSerializer.Serialize(new
			{
				id = "StorePreset",
				url = "presets",
				presetTarget = "",
				presetTargetSlave = 0,
				presetFile = $"presets/proj/{projFile}/{sceneFile}"
			});

			//JsonLength [12..16]:
			data.AddRange(BitConverter.GetBytes(json.Length));

			//Json [16..]
			data.AddRange(Encoding.ASCII.GetBytes(json));

			return Create(data, MessageCode.JSON);
		}

		public byte[] CreateProjectRecall(string projFile)
		{
			var data = CreateHeader(_deviceId);

			string json = JsonSerializer.Serialize(new
			{
				id = "RestorePreset",
				url = "presets",
				presetTarget = "",
				presetTargetSlave = 0,
				presetFile = "presets/proj/" + projFile
			});
			//JsonLength [12..16]:
			data.AddRange(BitConverter.GetBytes(json.Length));

			//Json [16..]
			data.AddRange(Encoding.ASCII.GetBytes(json));

			return Create(data, MessageCode.JSON);

		}
		public byte[] CreateClientInfoMessage()
		{
			var data = CreateHeader(_deviceId);

			var json = JsonSerializer.Serialize(new
			{
				id = "Subscribe",
				clientName = "Universal Control",
				clientInternalName = "ucapp",
				clientType = "CustomAPI",
				clientDescription = "CustomAPI for StudioLive",
				clientIdentifier = "661b1ece-b4d3-44b3-913c-d12964456f0b",
				clientOptions = "perm users levl redu rtan",
				clientEncoding = 23106
			});

			//JsonLength [12..16]:
			data.AddRange(BitConverter.GetBytes(json.Length));

			//Json [16..]
			data.AddRange(Encoding.ASCII.GetBytes(json));

			return Create(data, "JM");
		}

		public byte[] CreateKeepAliveMessage()
		{
			var data = CreateHeader(_deviceId);

			return Create(data, "KA");
		}

		public byte[] CreateWelcomeMessage(ushort monitorPort)
		{
			var data = CreateHeader(_deviceId);

			//Port [12..14]:
			data.AddRange(BitConverter.GetBytes(monitorPort));

			return Create(data, "UM");
		}

		public byte[] CreateRouteStringUpdate(string route, string value)
		{
			var data = CreateHeader(_deviceId);

			//Text [12..x]:
			data.AddRange(Encoding.ASCII.GetBytes(route));

			//Empty [0..3]:
			data.AddRange(new byte[] { 0x00, 0x00, 0x00 });

			//State [x+3..]:
			data.AddRange(Encoding.ASCII.GetBytes(value + "\0"));

			return Create(data, "PS");
		}

		public byte[] CreateRouteValueUpdate(string route, float value)
		{
			var data = CreateHeader(_deviceId);

			//Text [12..x]:
			data.AddRange(Encoding.ASCII.GetBytes(route));

			//Empty [0..3]:
			data.AddRange(new byte[] { 0x00, 0x00, 0x00 });

			//State [x+3..x+7]:
			data.AddRange(BitConverter.GetBytes(value));

			return Create(data, "PV");
		}
	}
}