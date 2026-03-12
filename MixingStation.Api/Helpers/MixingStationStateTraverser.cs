using MixingStation.Api.Models;
using MixingStation.Api.Services;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;

namespace MixingStation.Api.Helpers;

public class MixingStationStateTraverser
{
    public void Traverse(JsonElement element, MixerStateService mixerState)
    {
        IEnumerable<MixingStationNode> nodes = MixingStationNodeParser.Flatten(element);
        foreach (var node in nodes)
        {
            mixerState.SetNode(node);

            if (!string.IsNullOrWhiteSpace(node.Name))
                mixerState.SetString(Join(node.Path, "name"), node.Name, false);

           foreach(var key in node.Keys)
                mixerState.SetString(Join(node.Path, key), string.Empty, false);

        }
    }

    private static string Join(string path, string next)
    {
        if (string.IsNullOrWhiteSpace(path))
            return next;

        return $"{path}.{next}";
    }
}
