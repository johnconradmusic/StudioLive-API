using Presonus.UCNet.Api.Services;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Presonus.UCNet.Api.Models
{
	public class AdvancedSceneFilters : ParameterRouter
	{
		public AdvancedSceneFilters(MixerStateService mixerStateService) : base("advancedscenefilters", -1, mixerStateService)
		{

		}

		public bool fltr_channel_info
		{
			get => GetBoolean();
			set => SetBoolean(value);
		}
		public bool fltr_preamp { get => GetBoolean(); set => SetBoolean(value); }
		public bool fltr_channelstrip { get => GetBoolean(); set => SetBoolean(value); }
		public bool fltr_input_fatch { get => GetBoolean(); set => SetBoolean(value); }
		public bool fltr_output_fatch { get => GetBoolean(); set => SetBoolean(value); }
		public bool fltr_channel_delay { get => GetBoolean(); set => SetBoolean(value); }
		public bool fltr_mutes { get => GetBoolean(); set => SetBoolean(value); }
		public bool fltr_main_mix_level { get => GetBoolean(); set => SetBoolean(value); }
		public bool fltr_main_mix_assigns { get => GetBoolean(); set => SetBoolean(value); }
		public bool fltr_subgroup_assigns { get => GetBoolean(); set => SetBoolean(value); }
		public bool fltr_aux_matrix_mixes { get => GetBoolean(); set => SetBoolean(value); }
		public bool fltr_fx_mixes { get => GetBoolean(); set => SetBoolean(value); }
		public bool fltr_fx_type { get => GetBoolean(); set => SetBoolean(value); }
		public bool fltr_dca_groups { get => GetBoolean(); set => SetBoolean(value); }
		public bool fltr_mute_groups { get => GetBoolean(); set => SetBoolean(value); }

		public override event PropertyChangedEventHandler PropertyChanged;

		public override void OnPropertyChanged(PropertyChangedEventArgs eventArgs)
		{
			PropertyChanged?.Invoke(this, eventArgs);
		}
	}
}
