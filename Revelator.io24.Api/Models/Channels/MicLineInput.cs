using Presonus.StudioLive32.Api.Attributes;
using Presonus.UCNet.Api.NewDataModel;
using Presonus.UCNet.Api.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Presonus.UCNet.Api.Models.Channels
{
	public class MicLineInput : InputChannel
	{
		public MicLineInput(ChannelTypes channelType, int index, MixerStateService mixerStateService) : base(channelType, index, mixerStateService)
		{
		}

		[ParameterPath("48v")]
		public float phantom { get => GetValue(); set => SetValue(value); }
		public bool polarity { get => GetBoolean(); set => SetBoolean(value); }
		public float preampgain { get => GetValue(); set => SetValue(value); }
		public float digitalgain { get => GetValue(); set => SetValue(value); }
		public bool clip { get => GetBoolean(); set => SetBoolean(value); }
		public bool remotepreperm { get => GetBoolean(); set => SetBoolean(value); }
		public bool pream { get => GetBoolean(); set => SetBoolean(value); }


		[ParameterPath("eq/eqfreq1")] public float eqfreq1 { get => GetValue(); set => SetValue(value); }

		[ParameterPath("comp/reduction")] public float comp_reduction { get => GetValue(); }


	}
}
