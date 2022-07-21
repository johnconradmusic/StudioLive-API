using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Presonus.UC.Api.Components.Parameters
{
	public class MicLineInput : InputChannel
	{
		public Param preampactive = new("Local Preamp Active", ParamType.TOGGLE, def: 1);
		public Param remotepreactive = new("Remote Preamp Active", ParamType.TOGGLE);
		public Param remotepreperm = new("Remote Preamp Permission", ParamType.TOGGLE);
		public Param diggainactive = new("Digital Gain Active", ParamType.TOGGLE);
		public Param gaincompavail = new("Gain Compensation Available", ParamType.TOGGLE);
		public Param _48v = new ("48V", ParamType.LIST, units: ParamUnits.ON_OFF);
		public Param _48v_hwctrl = new ("48V hardware control", ParamType.LIST, units: ParamUnits.ON_OFF);
		public Param polarity = new("Polarity", ParamType.LIST, units: ParamUnits.ON_OFF);
		public Param preampgain = new("Preamp Gain", ParamType.FLOAT, units: ParamUnits.GAIN, min: 0, max: 60, curve: ParamCurve.LINEAR);
		public Param digitalgain = new("Digital Gain", ParamType.FLOAT, def: 0, units: ParamUnits.GAIN, min: -20, max: 20, curve: ParamCurve.LINEAR, steps: 400);
		public Param preampmode = new("Preamp Mode", ParamType.LIST, units: ParamUnits.PREAMPMODELIST);
		public Param linesense = new("hardware line sense state", ParamType.LIST, units: ParamUnits.PREAMPMODELIST);
		public Param _10db_boost = new ("10 dB boost", ParamType.LIST, units: ParamUnits.ON_OFF);
		public Param clip = new("Meter Clip", ParamType.TOGGLE);
		public Param gatekeysrc = new("Gate Key Source", ParamType.LIST, units: ParamUnits.EMPTYPARAMLIST);
		public Param compkeysrc = new("Comp Key Source", ParamType.LIST, units: ParamUnits.EMPTYPARAMLIST);
		public Param digsendsrc = new("Digital Send Source", ParamType.LIST, units: ParamUnits.DIGITALSENDSOURCELIST);
		public Param gaincomp = new("Remote Gain Compensation", ParamType.TOGGLE);


		public MicLineInput(string path) : base(path)
		{
			this.path = path;
		}
	}
}
