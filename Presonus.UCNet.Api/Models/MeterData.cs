using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Presonus.UCNet.Api.Models
{
	public class ReductionMeterData
	{
		public float[] InputGateReduction { get; set; }
		public float[] InputCompReduction { get; set; }
		public float[] InputLimitReduction { get; set; }

	}
	public class MeterData
	{

		public float GetData(ChannelSelector channelSelector)
		{
			return channelSelector.Type switch
			{
				ChannelTypes.LINE => InputInput[channelSelector.Channel],
				ChannelTypes.MAIN => Main[channelSelector.Channel],
				ChannelTypes.AUX => AuxMetering[channelSelector.Channel],
				ChannelTypes.FX => 0f,
				ChannelTypes.FXRETURN => 0f,
				ChannelTypes.RETURN => 0f,
				ChannelTypes.NONE => 0f,
				_ => 0,
			};
		}


		public float[] InputInput { get; set; }
		public float[] InputPreGate { get; set; }
		public float[] InputPostGate { get; set; }
		public float[] InputPostComp { get; set; }
		public float[] InputPostEQ { get; set; }
		public float[] InputPostLimit { get; set; }
		public float[] InputPostFader { get; set; }

		public float[] AuxMetering { get; set; }
		public Dictionary<string, float[]> AuxChStrip { get; set; } = new();

		public float[] Main { get; set; }
		public Dictionary<string, float[]> MainChStrip { get; set; } = new();

		public Dictionary<string, float[]> FxChStrip { get; set; } = new();
		public Dictionary<string, float[]> FxReturnStrip { get; set; } = new();


		public event PropertyChangedEventHandler PropertyChanged;
	}
}
