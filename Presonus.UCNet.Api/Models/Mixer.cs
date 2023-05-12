using System.Collections.Generic;

namespace Presonus.UCNet.Api.Models
{
	public class Mixer
	{
		public static bool Synchronized { get; set; } = false;

		public static Dictionary<ChannelTypes, int> ChannelCounts { get; set; } = new()
		{
			{ChannelTypes.LINE, 0 },
			{ChannelTypes.RETURN, 0 },
			{ChannelTypes.FXRETURN,0 },
			{ChannelTypes.TALKBACK,1 },
			{ChannelTypes.AUX,0 },
			{ChannelTypes.FX, 0 },
			{ChannelTypes.MAIN,1 }
		};

		public static bool Counted { get; internal set; }

		public static Dictionary<GenericListItem,List<GenericListItem>> Scenes { get; set; } = new();
		public static List<GenericListItem> ChannelPresets { get; set; } = new();
		public static List<GenericListItem> Projects { get; set; } = new();
	}
}