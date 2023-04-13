using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Presonus.UCNet.Api.Models
{
	public class ReductionData
	{
		public float[] InputGateReduction { get; set; }
		public float[] InputCompReduction { get; set; }
		public float[] InputLimitReduction { get; set; }
	}
	public class MeterData
	{
		public float[] Input { get; set; }
		public Dictionary<string, float[]> ChannelStrip { get; set; } = new();
		public float[] AuxMetering { get; set; }
		public Dictionary<string, float[]> AuxChStrip { get; set; } = new();
		public float[] MainMixFaders { get; set; }
		public Dictionary<string, float[]> MainChStrip { get; set; } = new();
		public float[] Main { get; set; }
		public Dictionary<string, float[]> FxChStrip { get; set; } = new();
		public Dictionary<string, float[]> FxReturnStrip { get; set; } = new();

	}

}
