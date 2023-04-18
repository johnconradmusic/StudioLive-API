using Presonus.UCNet.Api.Attributes;
using Presonus.UCNet.Api.Helpers;

using Presonus.UCNet.Api.Services;

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

		[ParameterPath("filter/hpf")] public float hpf { get => GetValue(); set => SetValue(value); }
		[ParameterPath("opt/swapcompeq")] public bool swapcompeq { get => GetBoolean(); set => SetBoolean(value); }

		#region Link Options

		[ParameterPath("linkoptions/ch_gain")] public bool link_ch_gain { get => GetBoolean(); set => SetBoolean(value); }
		[ParameterPath("linkoptions/pan")] public bool link_pan { get => GetBoolean(); set => SetBoolean(value); }
		[ParameterPath("linkoptions/fader")] public bool link_fader { get => GetBoolean(); set => SetBoolean(value); }
		[ParameterPath("linkoptions/dyn")] public bool link_dyn { get => GetBoolean(); set => SetBoolean(value); }
		[ParameterPath("linkoptions/ch_name")] public bool link_ch_name { get => GetBoolean(); set => SetBoolean(value); }
		[ParameterPath("linkoptions/ins_fx")] public bool link_ins_fx { get => GetBoolean(); set => SetBoolean(value); }


		#endregion

		#region Gate
		[ParameterPath("gate/on")] public bool gate_on { get => GetBoolean(); set => SetBoolean(value); }
		[ParameterPath("gate/threshold")] public float gate_threshold { get => GetValue(); set => SetValue(value); }
		[ParameterPath("gate/range")] public float gate_range { get => GetValue(); set => SetValue(value); }
		[ParameterPath("gate/attack")] public float gate_attack { get => GetValue(); set => SetValue(value); }
		[ParameterPath("gate/release")] public float gate_release { get => GetValue(); set => SetValue(value); }
		[ParameterPath("gate/keyfilter")] public float gate_keyfilter { get => GetValue(); set => SetValue(value); }
		[ParameterPath("gate/expander")] public bool gate_expander { get => GetBoolean(); set => SetBoolean(value); }
		[ParameterPath("gate/keylisten")] public bool gate_keylisten { get => GetBoolean(); set => SetBoolean(value); }

		#endregion

		#region EQ

		[ParameterPath("eq/eqallon")]
		public bool eq_on { get => GetBoolean(); set => SetBoolean(value); }

		[ParameterPath("eq/eqgain1")]
		public float eq_gain1 { get => GetValue(); set => SetValue(value); }

		[ParameterPath("eq/eqq1")]
		public float eq_q1 { get => GetValue(); set => SetValue(value); }

		[ParameterPath("eq/eqfreq1")]
		public float eq_freq1 { get => GetValue(); set => SetValue(value); }

		[ParameterPath("eq/eqbandon1")]
		public bool eq_bandon1 { get => GetBoolean(); set => SetBoolean(value); }

		[ParameterPath("eq/eqbandop1")]
		public bool eq_bandop1 { get => GetBoolean(); set => SetBoolean(value); }

		[ParameterPath("eq/eqgain2")]
		public float eq_gain2 { get => GetValue(); set => SetValue(value); }

		[ParameterPath("eq/eqq2")]
		public float eq_q2 { get => GetValue(); set => SetValue(value); }

		[ParameterPath("eq/eqfreq2")]
		public float eq_freq2 { get => GetValue(); set => SetValue(value); }

		[ParameterPath("eq/eqbandon2")]
		public bool eq_bandon2 { get => GetBoolean(); set => SetBoolean(value); }

		[ParameterPath("eq/eqgain3")]
		public float eq_gain3 { get => GetValue(); set => SetValue(value); }

		[ParameterPath("eq/eqq3")]
		public float eq_q3 { get => GetValue(); set => SetValue(value); }

		[ParameterPath("eq/eqfreq3")]
		public float eq_freq3 { get => GetValue(); set => SetValue(value); }

		[ParameterPath("eq/eqbandon3")]
		public bool eq_bandon3 { get => GetBoolean(); set => SetBoolean(value); }

		[ParameterPath("eq/eqgain4")]
		public float eq_gain4 { get => GetValue(); set => SetValue(value); }

		[ParameterPath("eq/eqq4")]
		public float eq_q4 { get => GetValue(); set => SetValue(value); }

		[ParameterPath("eq/eqfreq4")]
		public float eq_freq4 { get => GetValue(); set => SetValue(value); }

		[ParameterPath("eq/eqbandon4")]
		public bool eq_bandon4 { get => GetBoolean(); set => SetBoolean(value); }

		[ParameterPath("eq/eqbandop4")]
		public bool eq_bandop4 { get => GetBoolean(); set => SetBoolean(value); }

		#endregion

		#region Compressor/Limiter

		[ParameterPath("comp/on")] public bool comp_on { get => GetBoolean(); set => SetBoolean(value); }
		[ParameterPath("comp/softknee")] public bool comp_softknee { get => GetBoolean(); set => SetBoolean(value); }
		[ParameterPath("comp/automode")] public bool comp_automode { get => GetBoolean(); set => SetBoolean(value); }
		[ParameterPath("comp/threshold")] public float comp_threshold { get => GetValue(); set => SetValue(value); }
		[ParameterPath("comp/ratio")] public float comp_ratio { get => GetValue(); set => SetValue(value); }
		[ParameterPath("comp/attack")] public float comp_attack { get => GetValue(); set => SetValue(value); }
		[ParameterPath("comp/release")] public float comp_release { get => GetValue(); set => SetValue(value); }
		[ParameterPath("comp/gain")] public float comp_gain { get => GetValue(); set => SetValue(value); }
		[ParameterPath("comp/keyfilter")] public float comp_keyfilter { get => GetValue(); set => SetValue(value); }


		[ParameterPath("limt/limiteron")] public bool limiter_on { get => GetBoolean(); set => SetBoolean(value); }
		[ParameterPath("limt/threshold")] public float limiter_threshold { get => GetValue(); set => SetValue(value); }

		#endregion

	}
}