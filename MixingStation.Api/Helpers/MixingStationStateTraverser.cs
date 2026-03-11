using MixingStation.Api.Models;
using MixingStation.Api.Services;
using System.Linq;
using System.Text.Json;

namespace MixingStation.Api.Helpers;

public class MixingStationStateTraverser
{
    public void Traverse(JsonElement element, MixerStateService mixerState)
    {
        foreach (var node in MixingStationNodeParser.Flatten(element))
        {
            mixerState.SetNode(node);

            if (!string.IsNullOrWhiteSpace(node.Name))
                mixerState.SetString(Join(node.Path, "$name"), node.Name, false);

            if (node.Keys.Count > 0)
                mixerState.SetStrings(Join(node.Path, "$val"), node.Keys.ToArray(), false);
        }
    }

    private static string Join(string path, string next)
    {
        if (string.IsNullOrWhiteSpace(path))
            return next;

        return $"{path}/{next}";
    }
}
