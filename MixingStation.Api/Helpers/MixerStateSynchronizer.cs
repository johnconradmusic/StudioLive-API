using MixingStation.Api.Helpers;
using MixingStation.Api.Models;
using MixingStation.Api.Services;
using System.Linq;
using System.Text.Json;

namespace MixingStation.Api;

public sealed class MixerStateSynchronizer
{
    private readonly MixingStationStateTraverser _mixingStationTraverser;

    public MixerStateSynchronizer(MixingStationStateTraverser mixingStationTraverser)
    {
        _mixingStationTraverser = mixingStationTraverser;
    }

    public void Synchronize(string json, MixerStateService mixerStateService)
    {
        using var doc = JsonDocument.Parse(json);
        var root = doc.RootElement;

        if (root.ValueKind != JsonValueKind.Object)
            return;

        if (root.TryGetProperty("channelTypes", out _))
        {
            SynchronizeChannelInfo(root, mixerStateService);
            return;
        }

        if (!root.TryGetProperty("child", out _))
            return;

        _mixingStationTraverser.Traverse(root, mixerStateService);

        UpdateDerivedMetadata(root, mixerStateService);
    }

    private static void SynchronizeChannelInfo(JsonElement root, MixerStateService mixerStateService)
    {
        var channelInfo = JsonSerializer.Deserialize<MixingStationChannelInfo>(root.GetRawText());
        if (channelInfo == null)
            return;

        mixerStateService.Topology.ApplyChannelInfo(channelInfo);
        mixerStateService.SetValue("meta.totalChannels", channelInfo.TotalChannels, false);
    }

    private static void UpdateDerivedMetadata(JsonElement root, MixerStateService mixerStateService)
    {
        if (root.TryGetProperty("child", out var rootChild) &&
            rootChild.TryGetProperty("ch", out var channels) &&
            channels.TryGetProperty("child", out var channelChildren) &&
            channelChildren.ValueKind == JsonValueKind.Object)
        {
            var count = channelChildren.EnumerateObject().Count();
            if (count > 0)
            {
                mixerStateService.SetValue("meta.totalChannels", count, false);
                mixerStateService.Topology.IsSynchronized = true;
            }
        }
    }
}
