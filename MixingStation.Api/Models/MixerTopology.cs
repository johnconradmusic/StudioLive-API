using System.Collections.Generic;

namespace MixingStation.Api.Models;

public sealed class MixerTopology
{
    public bool IsSynchronized { get; set; }
    public bool HasChannelInfo { get; set; }
    public int TotalChannels { get; set; }

    public Dictionary<ChannelTypes, int> ChannelCounts { get; } = new()
    {
        { ChannelTypes.LINE, 0 },
        { ChannelTypes.RETURN, 0 },
        { ChannelTypes.FXRETURN, 0 },
        { ChannelTypes.TALKBACK, 0 },
        { ChannelTypes.AUX, 0 },
        { ChannelTypes.FX, 0 },
        { ChannelTypes.MAIN, 0 },
        { ChannelTypes.SUB, 0 },
        { ChannelTypes.GEQ, 0 }
    };

    public Dictionary<ChannelTypes, MixerChannelLayout> ChannelLayouts { get; } = new();

    public void ApplyChannelInfo(MixingStationChannelInfo info)
    {
        TotalChannels = info.TotalChannels;
        ChannelLayouts.Clear();

        foreach (var key in new List<ChannelTypes>(ChannelCounts.Keys))
            ChannelCounts[key] = 0;

        foreach (var definition in info.ChannelTypes)
        {
            var mappedType = definition.Type switch
            {
                0 => ChannelTypes.LINE,
                1 => ChannelTypes.RETURN,
                2 => ChannelTypes.FX,
                3 => ChannelTypes.FXRETURN,
                4 => ChannelTypes.AUX,
                6 => ChannelTypes.MAIN,
                8 => ChannelTypes.SUB,
                9 => ChannelTypes.TALKBACK,
                _ => ChannelTypes.NONE
            };

            if (mappedType == ChannelTypes.NONE)
                continue;

            ChannelCounts[mappedType] = definition.Count;
            ChannelLayouts[mappedType] = new MixerChannelLayout
            {
                Offset = definition.Offset,
                Count = definition.Count,
                Stereo = definition.Stereo,
                Name = definition.Name,
                ShortName = definition.ShortName
            };
        }

        HasChannelInfo = true;
    }
}
