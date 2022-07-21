using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Presonus.UC.Api.Components.Parameters
{
	public class Channel : ParameterBase
	{
		public Param chnum;
		public Param name;
		public Param username;
		public Param color;
		public Param select;
		public Param solo;
		public Param volume;
		public Param mute;
		public Param pan;
		public Param stereopan;
		public Param panlinkstate;
		public Param link;
		public Param linkmaster;
		public Param dawpostdsp;
		public Param memab;
		public Param iconid;
		public Param meter;
		public Param meterpeak; 
		public Param meter2;
		public Param meter2peak;
		public Param rta_active;
		public Param rta_pre;

		public Channel(string path) : base(path)
		{
			chnum = new(this, "Number", ParamType.STRING);
			name = new(this, "Name", ParamType.STRING);
			username = new(this, "User Name", ParamType.STRING);
			color = new(this, "Color", ParamType.COLOR);
			select = new(this, "Select", ParamType.TOGGLE);
			solo = new(this, "Solo", ParamType.TOGGLE);
			volume = new(this, "Volume", ParamType.FLOAT, def: -84, units: ParamUnits.GAIN, min: -84, max: 10, mid: -9, curve: ParamCurve.FADER);
			mute = new(this, "Mute", ParamType.TOGGLE);
			pan = new(this, "Pan", ParamType.FLOAT, def: 0.5f, units: ParamUnits.PAN, min: 0, max: 1);
			stereopan = new(this, "Stereo Width", ParamType.FLOAT, def: 1, units: ParamUnits.PERCENT, min: 0, max: 1);
			panlinkstate = new(this, "Pan Linked", ParamType.TOGGLE);
			link = new(this, "Link", ParamType.TOGGLE);
			linkmaster = new(this, "Link Master", ParamType.TOGGLE);
			dawpostdsp = new(this, "DAW Send Post DSP", ParamType.LIST, def: 0, units: ParamUnits.PREPOSTLIST);
			memab = new(this, "Alt A/B", ParamType.TOGGLE);
			iconid = new(this, "Channel Icon ID", ParamType.STRING);
			meter = new(this, "Meter", ParamType.FLOAT, units: ParamUnits.CH_GAIN, min: 0, max: 1);
			meterpeak = new(this, "Meter Peak", ParamType.FLOAT, units: ParamUnits.CH_GAIN, min: 0, max: 1);
			meter2 = new(this, "Meter 2", ParamType.FLOAT, units: ParamUnits.CH_GAIN, min: 0, max: 1);
			meter2peak = new(this, "Meter 2 Peak", ParamType.FLOAT, units: ParamUnits.CH_GAIN, min: 0, max: 1);
			rta_active = new(this, "RTA Active", ParamType.TOGGLE);
			rta_pre = new(this, "RTA Pre", ParamType.TOGGLE, def: 1);
		}
	}
}
