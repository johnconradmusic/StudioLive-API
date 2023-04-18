using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Presonus.UCNet.Api.Models;
using System.Diagnostics;
using System.Text.Json;
using Presonus.UCNet.Api.Services;
using System.Windows.Markup;

namespace Presonus.UCNet.Api.Helpers
{

	public class MixerStateTraverser
	{
		private void TraverseObject(JsonElement objectElement, string path, MixerStateService mixerState)
		{
			var properties = objectElement.EnumerateObject();
			foreach (var property in properties)
			{
				switch (property.Name)
				{
					case "children":
						if (!Mixer.Counted)
						{
							if (path.StartsWith("line/"))
								Mixer.ChannelCounts[ChannelTypes.LINE]++;
							else if (path.StartsWith("return/"))
								Mixer.ChannelCounts[ChannelTypes.RETURN]++;
							else if (path.StartsWith("fxreturn/"))
								Mixer.ChannelCounts[ChannelTypes.FXRETURN]++;
							else if (path.StartsWith("aux/"))
								Mixer.ChannelCounts[ChannelTypes.AUX]++;
							else if (path.StartsWith("fx/"))
								Mixer.ChannelCounts[ChannelTypes.FX]++;
						}
						Traverse(property.Value, CreatePath(path), mixerState);
						continue;
					case "values":
						Traverse(property.Value, CreatePath(path), mixerState);
						continue;
					case "ranges":
						Traverse(property.Value, CreatePath(path), mixerState);
						continue;
					case "strings":
						Traverse(property.Value, CreatePath(path), mixerState);
						continue;
					default:
						Traverse(property.Value, CreatePath(path, property.Name), mixerState);
						continue;
				}
			}
		}

		private string CreatePath(string path, string propertyName = null)
		{
			if (string.IsNullOrEmpty(path))
			{
				return propertyName ?? string.Empty;
			}

			if (path.Last() == '/')
			{
				return propertyName != null ? $"{path}{propertyName}" : path;
			}

			return propertyName != null ? $"{path}/{propertyName}" : path;
		}
		

		public void Traverse(JsonElement element, string path, MixerStateService mixerState)
		{
			try
			{
				switch (element.ValueKind)
				{
					case JsonValueKind.Number:
						if (path.Contains("color"))
						{
							var data = element.GetInt64();
							byte[] buffer = new byte[8];

							if (data is long longData)
							{
								byte[] longBytes = BitConverter.GetBytes(longData);
								//Array.Reverse(longBytes); // Reverse the byte order
								Buffer.BlockCopy(longBytes, 0, buffer, 0, 8); // Copy the bytes to the buffer
							}
							else
							{
								if (data != 0)
								buffer = BitConverter.GetBytes(data);
							}

							Array.Reverse(buffer); // Reverse the byte order
							string hexString = BitConverter.ToString(buffer, 0, 4).Replace("-", "");
							//Console.WriteLine(hexString);
							mixerState.SetString(path, hexString);
						}
						else
						{
							mixerState.SetValue(path, element.GetSingle(), false);
						}
						break;
					case JsonValueKind.String:
						mixerState.SetString(path, element.GetString() ?? string.Empty, false);
						break;
					case JsonValueKind.Array:
						mixerState.SetStrings(path, element.EnumerateArray()
							.Select(item => item.GetString() ?? string.Empty)
							.Where(str => str != string.Empty)
							.ToArray(), false);
						break;
					case JsonValueKind.Object:
						TraverseObject(element, path, mixerState);
						break;
					default:
						Debug.WriteLine($"Unknown JsonValueKind '{element.ValueKind}' at path '{path}'");
						break;
				}
			}
			catch (Exception ex)
			{
				Console.WriteLine(ex.Message);
			}
		}
	}

}
