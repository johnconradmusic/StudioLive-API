using Presonus.StudioLive32.Api.Attributes;
using Presonus.UCNet.Api.Devices;
using Presonus.UCNet.Api.Enums;
using Presonus.UCNet.Api.Services;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Presonus.StudioLive32.Api.Models.Auxes
{
    public class BusChannel : ChannelBase
    {
        public BusSource bussrc { get => (BusSource)GetValue(); set => SetValue((int)value); }
        public DawPostDsp dawpostdsp { get => (DawPostDsp)GetValue(); set => SetValue((int)value); }
        public BusChannel(string routePrefix, MixerStateService rawService, Device device) : base(routePrefix, rawService, device) { }
        public BusChannelRoutingMode auxpremode { get => (BusChannelRoutingMode)(GetValue() * 2); set => SetValue((float)value / 2); }
        public int busmode { get => (int)GetValue(); set => SetValue(value); }
        public float busdelay { get => GetValue(); set => SetValue(value); }
        [RouteValueRange(0, 1000, Enums.Unit.hz)][RouteValue("filter/hpf")] public float hipass { get => GetValue(); set => SetValue(value); }
        public bool lr_assign { get => GetBoolean(); set => SetBoolean(value); }

        #region EQ
        [RouteValue("eq/eqallon")] public bool eq_on { get { return GetBoolean(); } set { SetBoolean(value); } }
        [RouteValue("eq/eqbandon1")] public bool eq_bandon1 { get { return GetBoolean(); } set { SetBoolean(value); } }
        [RouteValue("eq/eqbandon2")] public bool eq_bandon2 { get { return GetBoolean(); } set { SetBoolean(value); } }
        [RouteValue("eq/eqbandon3")] public bool eq_bandon3 { get { return GetBoolean(); } set { SetBoolean(value); } }
        [RouteValue("eq/eqbandon4")] public bool eq_bandon4 { get { return GetBoolean(); } set { SetBoolean(value); } }
        [RouteValue("eq/eqbandon5")] public bool eq_bandon5 { get { return GetBoolean(); } set { SetBoolean(value); } }
        [RouteValue("eq/eqbandon6")] public bool eq_bandon6 { get { return GetBoolean(); } set { SetBoolean(value); } }
        [RouteValue("eq/eqbandop1")] public bool eq_bandop1 { get { return GetBoolean(); } set { SetBoolean(value); } }
        [RouteValue("eq/eqbandop4")] public bool eq_bandop6 { get { return GetBoolean(); } set { SetBoolean(value); } }
        [RouteValue("eq/eqgain1")] public float eq_gain1 { get => GetValue(); set => SetValue(value); }
        [RouteValue("eq/eqgain2")] public float eq_gain2 { get => GetValue(); set => SetValue(value); }
        [RouteValue("eq/eqgain3")] public float eq_gain3 { get => GetValue(); set => SetValue(value); }
        [RouteValue("eq/eqgain4")] public float eq_gain4 { get => GetValue(); set => SetValue(value); }
        [RouteValue("eq/eqgain5")] public float eq_gain5 { get => GetValue(); set => SetValue(value); }
        [RouteValue("eq/eqgain6")] public float eq_gain6 { get => GetValue(); set => SetValue(value); }
        [RouteValue("eq/eqq1")] public float eq_q1 { get => GetValue(); set => SetValue(value); }
        [RouteValue("eq/eqq2")] public float eq_q2 { get => GetValue(); set => SetValue(value); }
        [RouteValue("eq/eqq3")] public float eq_q3 { get => GetValue(); set => SetValue(value); }
        [RouteValue("eq/eqq4")] public float eq_q4 { get => GetValue(); set => SetValue(value); }
        [RouteValue("eq/eqq5")] public float eq_q5 { get => GetValue(); set => SetValue(value); }
        [RouteValue("eq/eqq6")] public float eq_q6 { get => GetValue(); set => SetValue(value); }
        [RouteValue("eq/eqfreq1")] public float eq_freq1 { get => GetValue(); set => SetValue(value); }
        [RouteValue("eq/eqfreq2")] public float eq_freq2 { get => GetValue(); set => SetValue(value); }
        [RouteValue("eq/eqfreq3")] public float eq_freq3 { get => GetValue(); set => SetValue(value); }
        [RouteValue("eq/eqfreq4")] public float eq_freq4 { get => GetValue(); set => SetValue(value); }
        [RouteValue("eq/eqfreq5")] public float eq_freq5 { get => GetValue(); set => SetValue(value); }
        [RouteValue("eq/eqfreq6")] public float eq_freq6 { get => GetValue(); set => SetValue(value); }

        #endregion

    }
}
