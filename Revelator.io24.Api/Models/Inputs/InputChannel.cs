using Revelator.io24.Api.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Revelator.io24.Api.Models.Inputs
{
    public class InputChannel : ChannelBase
    {
        public InputChannel(string routingPrefix, RawService rawService) : base(routingPrefix, rawService) { }

        #region EQ
        [RouteValue("eq/eqallon")] public bool eq_on { get { return GetBoolean(); } set { SetBoolean(value); } }
        [RouteValue("eq/eqbandon1")] public bool eq_bandon1 { get { return GetBoolean(); } set { SetBoolean(value); } }
        [RouteValue("eq/eqbandon2")] public bool eq_bandon2 { get { return GetBoolean(); } set { SetBoolean(value); } }
        [RouteValue("eq/eqbandon3")] public bool eq_bandon3 { get { return GetBoolean(); } set { SetBoolean(value); } }
        [RouteValue("eq/eqbandon4")] public bool eq_bandon4 { get { return GetBoolean(); } set { SetBoolean(value); } }
        [RouteValue("eq/eqbandop1")] public bool eq_bandop1 { get { return GetBoolean(); } set { SetBoolean(value); } }
        [RouteValue("eq/eqbandop4")] public bool eq_bandop4 { get { return GetBoolean(); } set { SetBoolean(value); } }
        [RouteValue("eq/eqgain1")] public float eq_gain1 { get => GetValue(); set => SetValue(value); }
        [RouteValue("eq/eqgain2")] public float eq_gain2 { get => GetValue(); set => SetValue(value); }
        [RouteValue("eq/eqgain3")] public float eq_gain3 { get => GetValue(); set => SetValue(value); }
        [RouteValue("eq/eqgain4")] public float eq_gain4 { get => GetValue(); set => SetValue(value); }
        [RouteValue("eq/eqq1")] public float eq_q1 { get => GetValue(); set => SetValue(value); }
        [RouteValue("eq/eqq2")] public float eq_q2 { get => GetValue(); set => SetValue(value); }
        [RouteValue("eq/eqq3")] public float eq_q3 { get => GetValue(); set => SetValue(value); }
        [RouteValue("eq/eqq4")] public float eq_q4 { get => GetValue(); set => SetValue(value); }
        [RouteValue("eq/eqfreq1")] public float eq_freq1 { get => GetValue(); set => SetValue(value); }
        [RouteValue("eq/eqfreq2")] public float eq_freq2 { get => GetValue(); set => SetValue(value); }
        [RouteValue("eq/eqfreq3")] public float eq_freq3 { get => GetValue(); set => SetValue(value); }
        [RouteValue("eq/eqfreq4")] public float eq_freq4 { get => GetValue(); set => SetValue(value); }
        #endregion
    }
}
