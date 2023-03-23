using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Presonus.UCNet.Api.Models;
using System.Diagnostics;
using System.Text.Json;

namespace Presonus.UCNet.Api.Helpers
{

	public class MixerStateTraverser
	{
		private void TraverseObject(JsonElement objectElement, string path, MixerState mixerState)
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
								Mixer.ChannelCounts["LINE"]++;
							else if (path.StartsWith("return/"))
								Mixer.ChannelCounts["RETURN"]++;
							else if (path.StartsWith("fxreturn/"))
								Mixer.ChannelCounts["FXRETURN"]++;
							else if (path.StartsWith("aux/"))
								Mixer.ChannelCounts["AUX"]++;
							else if (path.StartsWith("fx/"))
								Mixer.ChannelCounts["FX"]++;
						}
						Traverse(property.Value, CreatePath(path), mixerState);
						continue;
					case "values":
					case "ranges":
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


		public void Traverse(JsonElement element, string path, MixerState mixerState)
		{
			try
			{
				switch (element.ValueKind)
				{
					case JsonValueKind.Number:
						mixerState.SetValue(path, element.GetSingle());
						break;
					case JsonValueKind.String:
						mixerState.SetString(path, element.GetString() ?? string.Empty);
						break;
					case JsonValueKind.Array:
						mixerState.SetStrings(path, element.EnumerateArray()
							.Select(item => item.GetString() ?? string.Empty)
							.Where(str => str != string.Empty)
							.ToArray());
						break;
					case JsonValueKind.Object:
						TraverseObject(element, path, mixerState);
						break;
					default:
						Debug.WriteLine($"Unknown JsonValueKind '{element.ValueKind}' at path '{path}'");
						break;
				}
			}
			catch(Exception ex)
			{
				Console.WriteLine(ex.Message);
			}
		}
	}

}
