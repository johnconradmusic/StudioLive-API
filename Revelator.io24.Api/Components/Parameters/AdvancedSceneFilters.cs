using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Presonus.UC.Api.Components.Parameters
{
	public class AdvancedSceneFilters : ParameterBase
	{
		public Param fltr_channel_info;
		public Param fltr_preamp;
		public Param fltr_channelstrip;
		public Param fltr_input_fatch;
		public Param fltr_output_fatch;
		public Param fltr_channel_delay;
		public Param fltr_mutes;
		public Param fltr_main_mix_level;
		public Param fltr_main_mix_assigns;
		public Param fltr_subgroup_assigns;
		public Param fltr_aux_matrix_mixes;
		public Param fltr_fx_mixes;
		public Param fltr_fx_type;
		public Param fltr_dca_groups;
		public Param fltr_mute_groups;

		public AdvancedSceneFilters(string path) : base(path)
		{
		}
	}
}
