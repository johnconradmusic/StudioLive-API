using System.Collections.Generic;

namespace MixingStation.Api.Models
{
	public class Mixer
	{
		public static bool Synchronized { get; set; } = false;

		public static Dictionary<ChannelTypes, int> ChannelCounts { get; set; } = new()
		{
			{ChannelTypes.LINE, 0 },
			{ChannelTypes.RETURN, 0 },
			{ChannelTypes.FXRETURN,0 },
			{ChannelTypes.TALKBACK,0 },
			{ChannelTypes.AUX,0 },
			{ChannelTypes.FX, 0 },
			{ChannelTypes.MAIN,0 },
			{ChannelTypes.SUB,0 },
			{ChannelTypes.GEQ, 0 }
		};

		public static bool Counted { get; internal set; }

		public static int TotalChannels { get; private set; }

		public static Dictionary<ChannelTypes, MixerChannelLayout> ChannelLayouts { get; } = new();

		public static void ApplyMixingStationChannelInfo(MixingStationChannelInfo info)
		{
			ChannelLayouts.Clear();
			TotalChannels = info.TotalChannels;

			foreach (var key in new List<ChannelTypes>(ChannelCounts.Keys))
			{
				ChannelCounts[key] = 0;
			}

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

			Counted = true;
		}

		public static Dictionary<GenericListItem,List<GenericListItem>> Scenes { get; set; } = new();
		public static List<GenericListItem> ChannelPresets { get; set; } = new();
		public static List<GenericListItem> Projects { get; set; } = new();
	}
}
