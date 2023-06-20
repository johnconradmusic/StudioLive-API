
using Presonus.UCNet.Api.Attributes;
using Presonus.UCNet.Api.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Presonus.UCNet.Api.Models.Channels
{
    public class OutputDACBus : RoutableChannel
    {
        public OutputDACBus(ChannelTypes channelType, int index, MixerStateService mixerStateService) : base(channelType, index, mixerStateService)
        {
        }

        //public float auxpremode { get => GetValue(); set => SetValue(value); }
        //public float busmode { get => GetValue(); set => SetValue(value); }
        //public float busdelay { get => GetValue(); set => SetValue(value); }
        [ParameterPath("filter/hpf")] public float hpf { get => GetValue(); set => SetValue(value); }

        public static List<string> auxpremode_values = new() { "Pre", "Pre2", "Post" };

        public static List<string> busmode_values = new() { "Aux", "Subgroup", "Matrix" };

        public static List<string> bussource_values = new() { "Mixer", "Network" };
        public bool dawpostdsp { get => GetBoolean(); set => SetBoolean(value); }

        public bool lr_assign { get => GetBoolean(); set => SetBoolean(value); }

        public float bussrc { get => GetValue(); set => SetValue(value); }

        #region Link Options

        [ParameterPath("linkoptions/ch_gain")] public bool link_ch_gain { get => GetBoolean(); set => SetBoolean(value); }
        [ParameterPath("linkoptions/pan")] public bool link_pan { get => GetBoolean(); set => SetBoolean(value); }
        [ParameterPath("linkoptions/fader")] public bool link_fader { get => GetBoolean(); set => SetBoolean(value); }
        [ParameterPath("linkoptions/dyn")] public bool link_dyn { get => GetBoolean(); set => SetBoolean(value); }
        [ParameterPath("linkoptions/ch_name")] public bool link_ch_name { get => GetBoolean(); set => SetBoolean(value); }
        [ParameterPath("linkoptions/ins_fx")] public bool link_ins_fx { get => GetBoolean(); set => SetBoolean(value); }


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

        [ParameterPath("eq/eqgain5")]
        public float eq_gain5 { get => GetValue(); set => SetValue(value); }

        [ParameterPath("eq/eqq5")]
        public float eq_q5 { get => GetValue(); set => SetValue(value); }

        [ParameterPath("eq/eqfreq5")]
        public float eq_freq5 { get => GetValue(); set => SetValue(value); }

        [ParameterPath("eq/eqbandon5")]
        public bool eq_bandon5 { get => GetBoolean(); set => SetBoolean(value); }


        [ParameterPath("eq/eqgain6")]
        public float eq_gain6 { get => GetValue(); set => SetValue(value); }

        [ParameterPath("eq/eqq6")]
        public float eq_q6 { get => GetValue(); set => SetValue(value); }

        [ParameterPath("eq/eqfreq6")]
        public float eq_freq6 { get => GetValue(); set => SetValue(value); }

        [ParameterPath("eq/eqbandon6")]
        public bool eq_bandon6 { get => GetBoolean(); set => SetBoolean(value); }

        [ParameterPath("eq/eqbandop6")]
        public bool eq_bandop6 { get => GetBoolean(); set => SetBoolean(value); }

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
        [ParameterPath("comp/keylisten")] public bool comp_keylisten { get => GetBoolean(); set => SetBoolean(value); }



        [ParameterPath("limit/limiteron")] public bool limiter_on { get => GetBoolean(); set => SetBoolean(value); }
        [ParameterPath("limit/threshold")] public float limiter_threshold { get => GetValue(); set => SetValue(value); }

        #endregion

    }
}
