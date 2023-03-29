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
		public static Dictionary<string, object> DeserializeDecompressedBuffer(byte[] buffer)
		{
			using var stream = new MemoryStream(buffer);
			using var reader = new BinaryReader(stream);

			if (reader.ReadByte() != 0x7B)
			{
				Console.WriteLine("Failed to find opening curly brace at position {0}", stream.Position);
				return null;
			}

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
					{
						Console.WriteLine("(ZB) Failed to find delimiter 1, found {0} instead at position {1}", controlCharacter, stream.Position);
						return null;
					}

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
							{
								(workingSet[0] as List<object>).Add(leaf);
							}
							else
							{
								(workingSet[0] as Dictionary<string, object>).Add(Encoding.ASCII.GetString(keyData), leaf);
							}

							workingSet.Insert(0, leaf);
							continue;
						}

					case 0x5B: // New leaf array
						{
							var leaf = new List<object>();

							if (workingSet[0] is List<object>)
							{
								(workingSet[0] as List<object>).Add(leaf);
							}
							else
							{
								(workingSet[0] as Dictionary<string, object>).Add(Encoding.ASCII.GetString(keyData), leaf);
							}

							workingSet.Insert(0, leaf);
							continue;
						}

					case 0x53: // string
						{
							if (reader.ReadByte() != 0x69)
							{
								Console.WriteLine("(ZB) Failed to find delimiter 2 at position {0}", stream.Position);
								return null;
							}

							length = reader.ReadByte();
							break;
						}

					case 0x64: // float32
						{
							length = 4;
							break;
						}

					case 0x69: // int8
						{
							length = 1;
							break;
						}

					case 0x6c: // int32
						{
							length = 4;
							break;
						}

					case 0x4c: // int64
						{
							length = 8;
							break;
						}

					default:
						{
							Console.WriteLine("Unknown type {0} at position {1}", type, stream.Position);
							return null;
						}
				}

				var valueData = reader.ReadBytes(length);
				object value;

				switch (type)
				{
					case 0x53: // string
						{
							//Console.WriteLine("string");
							value = Encoding.ASCII.GetString(valueData);
							break;
						}

					case 0x64: // float32
						{
							if (BitConverter.IsLittleEndian)
							{
								Array.Reverse(valueData); // Reverse the bytes if the system is using little-endian byte order
							}
							value = BitConverter.ToSingle(valueData, 0);
							break;
						}

				

					case 0x69: // int8
						{
							//Console.WriteLine("short");
							value = (sbyte)valueData[0];
							break;
						}

					case 0x6c: // int32
						{
							//Console.WriteLine("int");
							int intValue = BitConverter.ToInt32(valueData, 0);

							checked // check for overflow and underflow errors
							{
								value = intValue;
							}

							break;
						}

					case 0x4c: // int64
						{
							//Console.WriteLine("long");

							long longValue = BitConverter.ToInt64(valueData, 0);

							checked // check for overflow and underflow errors
							{
								value = longValue;
							}

							break;
						}

					default:
						{
							value = Encoding.ASCII.GetString(valueData);
							break;
						}
				}

				if (workingSet[0] is List<object>)
				{
					(workingSet[0] as List<object>).Add(value);
				}
				else
				{
					(workingSet[0] as Dictionary<string, object>).Add(Encoding.ASCII.GetString(keyData), value);
				}

				// Log the deserialized value
				string valueString = value is string ? value.ToString() : "";
				//Console.WriteLine($"Type: {type}, Length: {length}, Value: {valueString}");


			}
			return rootTree;
		}
	}
}

