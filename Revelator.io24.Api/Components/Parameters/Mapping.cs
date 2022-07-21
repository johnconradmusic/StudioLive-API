using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Presonus.UC.Api.Components.Parameters
{
	public class Mapping : ParameterBase
	{
		public Param dawMode = new("DAW Mode", ParamType.TOGGLE);
		public Param fatch_view = new("Fat Channel View", ParamType.TOGGLE);
		public Param shift = new("Shift", ParamType.TOGGLE);
		public Param bank_input;
		public Param bank_user = new("User Layer", ParamType.TOGGLE);
		public Param bank_auxinputs = new("Aux Inputs", ParamType.TOGGLE);
		public Param bank_dca_groups = new("DCA Groups", ParamType.TOGGLE);
		public Param bank_mix_fx = new("Mix/FX Master", ParamType.TOGGLE);
		public Param bank_prev = new("Bank Previous", ParamType.TOGGLE);
		public Param bank_next = new("Bank Next", ParamType.TOGGLE);
		public Param input_prev = new("Input Page Previous", ParamType.TOGGLE);
		public Param input_next = new("Input Page Next", ParamType.TOGGLE);
		public Param fc_page_prev = new("Page Previous", ParamType.TOGGLE);
		public Param fc_page_next = new("Page Next", ParamType.TOGGLE);
		public Param mixselect_1 = new("Mix 1", ParamType.TOGGLE);
		public Param mixselect_2 = new("Mix 2", ParamType.TOGGLE);
		public Param mixselect_3 = new("Mix 3", ParamType.TOGGLE);
		public Param mixselect_4 = new("Mix 4", ParamType.TOGGLE);
		public Param mixselect_5 = new("Mix 5", ParamType.TOGGLE);
		public Param mixselect_6 = new("Mix 6", ParamType.TOGGLE);
		public Param mixselect_7 = new("Mix 7", ParamType.TOGGLE);
		public Param mixselect_8 = new("Mix 8", ParamType.TOGGLE);
		public Param mixselect_9 = new("Mix 9", ParamType.TOGGLE);
		public Param mixselect_10 = new("Mix 10", ParamType.TOGGLE);
		public Param mixselect_11 = new("Mix 11", ParamType.TOGGLE);
		public Param mixselect_12 = new("Mix 12", ParamType.TOGGLE);
		public Param mixselect_13 = new("Mix 13", ParamType.TOGGLE);
		public Param mixselect_14 = new("Mix 14", ParamType.TOGGLE);
		public Param mixselect_15 = new("Mix 15", ParamType.TOGGLE);
		public Param mixselect_16 = new("Mix 16", ParamType.TOGGLE);
		public Param mixselect_fx_a = new("Mix FX A", ParamType.TOGGLE);
		public Param mixselect_fx_b = new("Mix FX B", ParamType.TOGGLE);
		public Param mixselect_fx_c = new("Mix FX C", ParamType.TOGGLE);
		public Param mixselect_fx_d = new("Mix FX D", ParamType.TOGGLE);
		public Param mixselect_main;

		public Mapping(string path) : base(path)
		{
		}
	}
}
