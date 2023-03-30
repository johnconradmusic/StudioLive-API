using System.Collections.Generic;

namespace Presonus.UCNet.Api.Models
{
	public class Mixer
	{
		public static bool Synchronized { get; set; } = false;
		public static Dictionary<string, int> ChannelCounts { get; set; } = new()
		{
			{"LINE", 0 },
			{"RETURN", 0 },
			{"FXRETURN",0 },
			{"TALKBACK",1 },
			{"AUX",0 },
			{"FX", 0 },
			{"MAIN",1 }
		};
		public static bool Counted { get; internal set; }
	}
}