using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Presonus.UC.Api.Components.Parameters
{
	public class StageboxSetup : ParameterBase
	{
		public Param mixerlist;
		public Param selected_name;
		public Param stagebox_mode;
		public Param preamp_control;
		public Param avb_src;
		public Param avb_src2;
		public Param auto_route;
		public Param identify;
		public Param apply;
		public Param preamp_lock;
		public Param stagebox_type;
		public Param connect_status;
		public Param latency_menu;
		public Param latency;
		public Param apply_latency;
		public Param avb_src_1_8;
		public Param avb_src_9_16;
		public Param avb_src_17_24;
		public Param avb_src_25_32;
		public Param avb_src_33_40;
		public Param avb_src_41_48;
		public Param avb_src_49_56;
		public Param avb_src_57_64;
		public Param input_1_8_entity;
		public Param input_9_16_entity;
		public Param input_17_24_entity;
		public Param input_25_32_entity;
		public Param input_33_40_entity;
		public Param input_41_48_entity;
		public Param input_49_56_entity;
		public Param input_57_64_entity;
		public Param input_1_8_stream;
		public Param input_9_16_stream;
		public Param input_17_24_stream;
		public Param input_25_32_stream;
		public Param input_33_40_stream;
		public Param input_41_48_stream;
		public Param input_49_56_stream;
		public Param input_57_64_stream;
		public Param avb_clocksrc;
		public Param avb_clockstatus;

		public StageboxSetup(string path) : base(path)
		{
		}
	}
}
