using MixingStation.Api.Helpers;
using MixingStation.Api.Models;
using MixingStation.Api.Services;
using System.Linq;
using System.Text.Json;

namespace MixingStation.Api;
public class MixerStateSynchronizer
{
    private readonly MixingStationStateTraverser _mixingStationTraverser;

    public MixerStateSynchronizer(MixingStationStateTraverser mixingStationTraverser)
    {
        _mixingStationTraverser = mixingStationTraverser;
    }

    public void Synchronize(string json, MixerStateService mixerState)
    {
        var doc = JsonSerializer.Deserialize<JsonDocument>(json);
        if (doc == null)
            return;

        if (doc.RootElement.TryGetProperty("channelTypes", out _))
        {
            var channelInfo = JsonSerializer.Deserialize<MixingStationChannelInfo>(json);
            if (channelInfo != null)
            {
                Mixer.ApplyMixingStationChannelInfo(channelInfo);
                mixerState.SetValue("meta/totalChannels", channelInfo.TotalChannels, false);
            }
            return;
        }

        if (!doc.RootElement.TryGetProperty("child", out _))
            return;

        _mixingStationTraverser.Traverse(doc.RootElement, mixerState);

        if (doc.RootElement.TryGetProperty("child", out var rootChild) &&
            rootChild.TryGetProperty("ch", out var channels) &&
            channels.TryGetProperty("child", out var channelChildren) &&
            channelChildren.ValueKind == JsonValueKind.Object)
        {
            var count = channelChildren.EnumerateObject().Count();
            if (count > 0)
            {
                mixerState.SetValue("meta/totalChannels", count, false);
                Mixer.Synchronized = true;
            }
        }
    }
}
