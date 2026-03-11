using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace MixingStation.Api.Models;

public class MixingStationChannelInfo
{
    [JsonPropertyName("totalChannels")]
    public int TotalChannels { get; set; }

    [JsonPropertyName("channelTypes")]
    public List<MixingStationChannelType> ChannelTypes { get; set; } = new();
}

public class MixingStationChannelType
{
    [JsonPropertyName("offset")]
    public int Offset { get; set; }

    [JsonPropertyName("count")]
    public int Count { get; set; }

    [JsonPropertyName("stereo")]
    public bool Stereo { get; set; }

    [JsonPropertyName("name")]
    public string Name { get; set; } = string.Empty;

    [JsonPropertyName("shortName")]
    public string ShortName { get; set; } = string.Empty;

    [JsonPropertyName("type")]
    public int Type { get; set; }
}

public class MixerChannelLayout
{
    public int Offset { get; set; }
    public int Count { get; set; }
    public bool Stereo { get; set; }
    public string Name { get; set; } = string.Empty;
    public string ShortName { get; set; } = string.Empty;
}

public class MixingStationNode
{
    public string Path { get; set; } = string.Empty;
    public string Name { get; set; } = string.Empty;
    public IReadOnlyList<string> Keys { get; set; } = new List<string>();
}

public static class MixingStationNodeParser
{
    public static IEnumerable<MixingStationNode> Flatten(JsonElement root)
    {
        if (root.ValueKind != JsonValueKind.Object)
            return Enumerable.Empty<MixingStationNode>();

        var nodes = new List<MixingStationNode>();
        Walk(root, string.Empty, nodes);
        return nodes;
    }

    private static void Walk(JsonElement element, string path, ICollection<MixingStationNode> nodes)
    {
        var node = new MixingStationNode { Path = path };

        if (element.TryGetProperty("name", out var nameElement) && nameElement.ValueKind == JsonValueKind.String)
            node.Name = nameElement.GetString() ?? string.Empty;

        if (element.TryGetProperty("val", out var valElement) && valElement.ValueKind == JsonValueKind.Array)
        {
            node.Keys = valElement.EnumerateArray()
                .Select(v => v.GetString())
                .Where(v => !string.IsNullOrWhiteSpace(v))
                .Cast<string>()
                .ToArray();
        }

        nodes.Add(node);

        if (!element.TryGetProperty("child", out var childElement) || childElement.ValueKind != JsonValueKind.Object)
            return;

        foreach (var child in childElement.EnumerateObject())
        {
            var childPath = string.IsNullOrWhiteSpace(path) ? child.Name : $"{path}/{child.Name}";
            Walk(child.Value, childPath, nodes);
        }
    }
}
