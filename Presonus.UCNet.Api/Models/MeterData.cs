using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Presonus.UCNet.Api.Models
{
	public class MeterData
	{
		public float[] Input { get; set; }
		public Dictionary<string, float[]> ChannelStrip { get; set; }
		public float[] AuxMetering { get; set; }
		public Dictionary<string, float[]> AuxChStrip { get; set; }
		public float[] MainMixFaders { get; set; }
		public Dictionary<string, float[]> MainChStrip { get; set; }
		public float[] Main { get; set; }
		public Dictionary<string, float[]> FxChStrip { get; set; }
		public Dictionary<string, float[]> FxReturnStrip { get; set; }
	}

}
