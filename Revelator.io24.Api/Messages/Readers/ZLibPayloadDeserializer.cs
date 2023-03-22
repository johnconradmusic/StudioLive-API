using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Presonus.UCNet.Api.Messages.Readers
{
	public class ZlibPayloadDeserializer
	{
		public static Dictionary<string, object> DeserializeZlibBuffer(byte[] buffer)
		{
			Console.WriteLine("Calling DSZLIB");
			using var stream = new MemoryStream(buffer);
			using var reader = new BinaryReader(stream);

			if (reader.ReadByte() != 0x7B)
				return null;

			var rootTree = new Dictionary<string, object>();
			var workingSet = new List<object> { rootTree };

			while (stream.Position != buffer.Length)
			{
				byte[] keyData = null;
				if (workingSet[0] is List<object>)
				{
					int nextChar = reader.PeekChar();
					if (nextChar == 0x5D)
					{
						reader.ReadByte();
						workingSet.RemoveAt(0);
						continue;
					}
				}
				else
				{
					var controlCharacter = reader.ReadByte();
					if (controlCharacter == 0x7D)
					{
						workingSet.RemoveAt(0);
						continue;
					}

					if (controlCharacter != 0x69)
						throw new Exception($"(ZB) Failed to find delimiter 1, found {controlCharacter} instead at position {stream.Position}");

					var length2 = reader.ReadByte();
					keyData = reader.ReadBytes(length2);
				}

				var type = reader.ReadByte();
				int length = 0;
				switch (type)
				{
					case 0x7B: // New leaf dictionary
						{
							var leaf = new Dictionary<string, object>();

							if (workingSet[0] is List<object>)
								(workingSet[0] as List<object>).Add(leaf);
							else
								(workingSet[0] as Dictionary<string, object>).Add(Encoding.ASCII.GetString(keyData), leaf);

							workingSet.Insert(0, leaf);
							continue;
						}

					case 0x5B: // New leaf array
						{
							var leaf = new List<object>();

							if (workingSet[0] is List<object>)
								(workingSet[0] as List<object>).Add(leaf);
							else
								(workingSet[0] as Dictionary<string, object>).Add(Encoding.ASCII.GetString(keyData), leaf);

							workingSet.Insert(0, leaf);
							continue;
						}

					case 0x53: // string
						{
							if (reader.ReadByte() != 0x69)
								throw new Exception("(ZB) Failed to find delimiter 2");

							length = reader.ReadByte();
							break;
						}

					case 0x64: // float32
						length = 4;
						break;

					case 0x69: // int8
						length = 1;
						break;

					case 0x6c: // int32
						length = 4;
						break;

					case 0x4c: // int64
						length = 8;
						break;

					default:
						throw new Exception($"Unknown type {type} at position {stream.Position}");
				}

				var valueData = reader.ReadBytes(length);

				object value;

				switch (type)
				{
					case 0x53: // string
						value = Encoding.ASCII.GetString(valueData);
						break;

					case 0x64: // float32
						value = BitConverter.ToSingle(valueData, 0);
						break;

					case 0x69: // int8
						value = (sbyte)valueData[0];
						break;
					case 0x6c: // int32
						value = BitConverter.ToInt32(valueData, 0);
						break;

					case 0x4c: // int64
						value = BitConverter.ToInt64(valueData, 0);
						break;

					default:
						value = Encoding.ASCII.GetString(valueData);
						break;
				}
				

				if (workingSet[0] is List<object>)
				{
					(workingSet[0] as List<object>).Add(value);
				}
				else
				{
					(workingSet[0] as Dictionary<string, object>).Add(Encoding.ASCII.GetString(keyData), value);
				}
			}

			return rootTree;
		}
	}
}


