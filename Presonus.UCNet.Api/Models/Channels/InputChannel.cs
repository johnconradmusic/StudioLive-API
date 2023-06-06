using Presonus.UCNet.Api.Enums;
using Presonus.UCNet.Api.Services;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Presonus.UCNet.Api.Models.Channels
{
	public class InputChannel : RoutableChannel
	{
		public InputChannel(ChannelTypes channelType, int index, MixerStateService mixerStateService)
			: base(channelType, index, mixerStateService)
		{
		}

		public bool lr { get => GetBoolean(); set => SetBoolean(value); }

		public string sub_asn_flags { get => GetString(); set => SetString(value); }
		public bool sub1 { get => GetBoolean(); set => SetBoolean(value); }
		public bool sub2 { get => GetBoolean(); set => SetBoolean(value); }
		public bool sub3 { get => GetBoolean(); set => SetBoolean(value); }
		public bool sub4 { get => GetBoolean(); set => SetBoolean(value); }

		public bool assign_fx1 { get => GetBoolean(); set => SetBoolean(value); }
		public bool assign_fx2 { get => GetBoolean(); set => SetBoolean(value); }
		public bool assign_fx3 { get => GetBoolean(); set => SetBoolean(value); }
		public bool assign_fx4 { get => GetBoolean(); set => SetBoolean(value); }
		public bool assign_fx5 { get => GetBoolean(); set => SetBoolean(value); }
		public bool assign_fx6 { get => GetBoolean(); set => SetBoolean(value); }
		public bool assign_fx7 { get => GetBoolean(); set => SetBoolean(value); }
		public bool assign_fx8 { get => GetBoolean(); set => SetBoolean(value); }

		public bool dawpostdsp { get => GetBoolean(); set => SetBoolean(value); }


		public float FXA { get => GetValue(); set => SetValue(value); }
		public float FXB { get => GetValue(); set => SetValue(value); }
		public float FXC { get => GetValue(); set => SetValue(value); }
		public float FXD { get => GetValue(); set => SetValue(value); }
		public float FXE { get => GetValue(); set => SetValue(value); }
		public float FXF { get => GetValue(); set => SetValue(value); }
		public float FXG { get => GetValue(); set => SetValue(value); }
		public float FXH { get => GetValue(); set => SetValue(value); }

		public float inputsrc { get => GetValue(); set => SetValue(value); }
		public List<string> inputsrc_values => new List<string>(Enum.GetNames(typeof(LineInputSource)).ToList());
		public float inputsrc_preview { get => GetValue(); set => SetValue(value); }
		public float delay { get => GetValue(); set => SetValue(value); }
	}
}