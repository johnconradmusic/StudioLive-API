using Presonus.UCNet.Api.Services;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Presonus.UCNet.Api.Models
{
	public class ProjectFilters : ParameterRouter
	{
		public ProjectFilters(MixerStateService mixerStateService) : base("projectfilters", -1, mixerStateService)
		{
		}

		public bool fltr_input_source { get => GetBoolean(); set => SetBoolean(value); }
		public bool fltr_flexmixmode { get => GetBoolean(); set => SetBoolean(value); }
		public bool fltr_flexmixprepostmode { get => GetBoolean(); set => SetBoolean(value); }
		public bool fltr_fxmixpreposmode { get => GetBoolean(); set => SetBoolean(value); }
		public bool fltr_talkbackassigns { get => GetBoolean(); set => SetBoolean(value); }
		public bool fltr_solosettings { get => GetBoolean(); set => SetBoolean(value); }
		public bool fltr_generalsettings { get => GetBoolean(); set => SetBoolean(value); }
		public bool fltr_avbstreamrouting { get => GetBoolean(); set => SetBoolean(value); }
		public bool fltr_inputpatching { get => GetBoolean(); set => SetBoolean(value); }
		public bool fltr_outputpatching { get => GetBoolean(); set => SetBoolean(value); }
		public bool fltr_avbpatching { get => GetBoolean(); set => SetBoolean(value); }
		public bool fltr_sdpatching { get => GetBoolean(); set => SetBoolean(value); }
		public bool fltr_usbpatching { get => GetBoolean(); set => SetBoolean(value); }
		public bool fltr_geq { get => GetBoolean(); set => SetBoolean(value); }
		public bool fltr_user_functions { get => GetBoolean(); set => SetBoolean(value); }

		public override event PropertyChangedEventHandler PropertyChanged;

		public override void OnPropertyChanged(PropertyChangedEventArgs eventArgs)
		{
			PropertyChanged?.Invoke(this, eventArgs);
		}
	}
}
