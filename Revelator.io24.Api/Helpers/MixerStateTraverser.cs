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
					case "values":
					case "strings":
					case "ranges":
						Traverse(property.Value, CreatePath(path), mixerState);						
						continue;
					default:
						var pathName = CreatePath(path, property.Name);
						Traverse(property.Value, pathName, mixerState);
						continue;
				}
			}
		}

		private string CreatePath(string path, string propertyName = null)
		{
			if (path is null || path.Length < 1)
				return $"{path}{propertyName}";

			if (path.Last() == '/')
				return $"{path}{propertyName}";

			return $"{path}/{propertyName}";
		}

		public void Traverse(JsonElement element, string path, MixerState mixerState)
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
	}

}
