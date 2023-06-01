using Presonus.UCNet.Api.Attributes;

using Presonus.UCNet.Api.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Presonus.UCNet.Api.Models.Channels
{
    public class StereoLineInput : InputChannel
    {
        public StereoLineInput(int index, MixerStateService mixerStateService) : base(ChannelTypes.RETURN, index, mixerStateService)
        {
        }

        public new List<string> inputsrc_values = new List<string>() { "Network", "USB", "SD Card" };


        [ParameterPath("opt/swapcompeq")] public bool swapcompeq { get => GetBoolean(); set => SetBoolean(value); }

        public float trim { get => GetValue(); set => SetValue(value); }

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

        [ParameterPath("comp/on")] public bool comp_on { get => GetBoolean(); set => SetBoolean(value); }
        [ParameterPath("comp/softknee")] public bool comp_softknee { get => GetBoolean(); set => SetBoolean(value); }
        [ParameterPath("comp/automode")] public bool comp_automode { get => GetBoolean(); set => SetBoolean(value); }
        [ParameterPath("comp/threshold")] public float comp_threshold { get => GetValue(); set => SetValue(value); }
        [ParameterPath("comp/ratio")] public float comp_ratio { get => GetValue(); set => SetValue(value); }
        [ParameterPath("comp/attack")] public float comp_attack { get => GetValue(); set => SetValue(value); }
        [ParameterPath("comp/release")] public float comp_release { get => GetValue(); set => SetValue(value); }
        [ParameterPath("comp/gain")] public float comp_gain { get => GetValue(); set => SetValue(value); }

        [ParameterPath("limt/limiteron")] public bool limiter_on { get => GetBoolean(); set => SetBoolean(value); }
        [ParameterPath("limt/threshold")] public float limiter_threshold { get => GetValue(); set => SetValue(value); }
    }
}
