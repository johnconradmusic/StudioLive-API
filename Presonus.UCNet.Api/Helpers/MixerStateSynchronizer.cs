using Presonus.UCNet.Api.Helpers;
using Presonus.UCNet.Api.Models;
using Presonus.UCNet.Api.Services;
using System;
using System.IO;
using System.Text.Json;
using System.Windows.Shapes;

namespace Presonus.UCNet.Api;
public class MixerStateSynchronizer
{
    private readonly MixerStateTraverser _traverser;

    public MixerStateSynchronizer(MixerStateTraverser traverser)
    {
        _traverser = traverser;
    }

    public void Synchronize(string json, MixerStateService mixerState)
    {
        var doc = JsonSerializer.Deserialize<JsonDocument>(json);

        File.WriteAllText("C:\\Dev\\jsonDump.json", json);

        if (doc == null) return;

        if (doc.RootElement.TryGetProperty("id", out var id))
        {
            if (id is JsonElement ID)
            {
                var jsonID = ID.GetString();
                if (jsonID == "SynchronizePart")
                {
                    var part = doc.RootElement.GetProperty("part").GetString() + "/";
                    var classId = doc.RootElement.GetProperty("classId").GetString();
                    mixerState.SetString(part + "classId", classId, false);
                    var values = doc.RootElement.GetProperty("values");

                    _traverser.Traverse(values, part, mixerState);
                }
            }
        }

        if (doc.RootElement.TryGetProperty("children", out var children))
        {
            _traverser.Traverse(children, string.Empty, mixerState);
            Mixer.Counted = true;
        }
    }
}
