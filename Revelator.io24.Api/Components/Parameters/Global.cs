using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Presonus.UC.Api.Components.Parameters
{
	public class Global : ParameterBase
	{
		public Param identify = new("Identify", ParamType.TOGGLE);
		public Param stagebox_mode;
		public Param dcamode = new("DCA Mode", ParamType.TOGGLE);
		public Param panmode;
		public Param mixer_name = new("Mixer Name", ParamType.STRING);
		public Param devicename = new("Mixer Network name", ParamType.STRING);
		public Param mixer_version = new("Mixer Version", ParamType.STRING);
		public Param mixer_version_date = new("Mixer Version Build Date", ParamType.STRING);
		public Param mixer_serial = new("Mixer Serial Number", ParamType.STRING);
		public Param registered_user = new("Registered Username", ParamType.STRING);
		public Param progress_text1 = new("Progress Text", ParamType.STRING);
		public Param progress_text2 = new("Progress Text", ParamType.STRING);
		public Param progress_percent;
		public Param samplerate;
		public Param showPeakHold;
		public Param ledbrightness;
		public Param scribblebrightness;
		public Param lcdbrightness;
		public Param auxmutemode;
		public Param mastermixlock = new("Master Mix Lock", ParamType.TOGGLE);
		public Param rta_active = new("RTA Active", ParamType.TOGGLE);
		public Param rta_pre;
		public Param sd_assignable_source;
		public Param usb_assignable_source;
		public Param fltrname;
		public Param fltrmute;
		public Param fltrfx;
		public Param fltreqdynins;
		public Param fltreqdynouts;
		public Param fltraux;
		public Param fltrassign;
		public Param fltrpreamps;
		public Param fltrfader;
		public Param fltrgeq;
		public Param fltrdcagrp;
		public Param fltr48v;
		public Param fltrmutegroups;
		public Param fltruser;
		public Param fltrpatch;
		public Param scene_safe_ins_1_32;
		public Param scene_safe_ins_33_64;
		public Param soft_power_logout = new("Soft Power Logout", ParamType.TOGGLE);
		public Param last_logged_in_profile_index;
		public Param bus_level_limit;
		public Param if_mode = new("Interface Mode", ParamType.TOGGLE);


		public Global(string path) : base(path)
		{
		}
	}
}
