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
		public float[] InputInput { get; set; }
		public float[] InputPreGate { get; set; }
		public float[] InputPostGate { get; set; }
		public float[] InputPostComp { get; set; }
		public float[] InputPostEQ { get; set; }
		public float[] InputPostLimit { get; set; }
		public float[] AuxMetering { get; set; }
		public Dictionary<string, float[]> AuxChStrip { get; set; } = new();
		public float[] InputPostFader { get; set; }
		public Dictionary<string, float[]> MainChStrip { get; set; } = new();
		public float[] Main { get; set; }
		public Dictionary<string, float[]> FxChStrip { get; set; } = new();
		public Dictionary<string, float[]> FxReturnStrip { get; set; } = new();


		public event PropertyChangedEventHandler PropertyChanged;
	}
}
