using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Presonus.UC.Api.Components.Parameters
{
	public class ChannelFilters : ParameterBase
	{
		public Param preset_eq;
		public Param preset_gate;
		public Param preset_comp;
		public Param preset_preamp;
		public Param preset_polarity;
		public Param preset_channel_type;
		public Param preset_alt_ab;
		public Param preset_aux_fxsend_pan;
		public Param preset_select_colors;
		public Param preset_48v;
		public Param preset_pan;
		public Param preset_channel_names;
		public Param preset_bus_assigns;
		public Param preset_group_assigns;
		public Param preset_mutes;
		public Param preset_faders;
		public Param paste_preamp;
		public Param paste_polarity;
		public Param paste_channel_type;
		public Param paste_alt_ab;
		public Param paste_aux_fxsend_pan;
		public Param paste_select_colors;
		public Param paste_48v;
		public Param paste_pan;
		public Param paste_channel_names;
		public Param paste_bus_assigns;
		public Param paste_group_assigns;
		public Param paste_mutes;
		public Param paste_faders;
		public Param do_load;
		public Param dont_load;

		public ChannelFilters(string path) : base(path)
		{
		}
	}
}
