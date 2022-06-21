﻿using Revelator.io24.Api.Attributes;
using Revelator.io24.Api.Scene;
using System;
using System.ComponentModel;

namespace Revelator.io24.Api.Models.Inputs
{
    public class LineChannel : DeviceRoutingBase
    {
        public event PropertyChangedEventHandler PropertyChanged;

        protected override void OnPropertyChanged(PropertyChangedEventArgs eventArgs)
        {
            PropertyChanged?.Invoke(this, eventArgs);
        }

        public LineChannel(string routingPrefix, RawService rawService)
            : base(routingPrefix, rawService)
        {

        }

        public string AutomationName => username;
        public string AutomationId => _routePrefix + username;
        public bool LinkSlave => !(link && !linkmaster);

        [RouteValue("clip")]
        public bool Clip
        {
            get => GetBoolean();
        }
        public string username
        {
            get => GetString();
            set => SetString(value);
        }
        public int color
        {
            get => (int)GetValue();
            set => SetValue(value);
        }
        public bool solo
        {
            get => GetBoolean();
            set => SetBoolean(value);
        }

        [RouteValue("dca/volume")]
        [RouteValueRange(-84, 10, Enums.Unit.db)]
        public float level
        {
            get => GetValue();
            set => SetValue(value);
        }

        [RouteValueRange(-84, 10, Enums.Unit.db)]
        public float volume
        {
            get { return GetValue(); }
            set { SetValue(value); }
        }
        public bool mute
        {
            get => GetBoolean();
            set => SetBoolean(value);
        }
        [RouteValueRange(-100, 100, Enums.Unit.none)]
        public float pan { get => GetValue(); set => SetValue(value); }
        [RouteValueRange(-100, 100, Enums.Unit.none)]
        public float stereopan { get => GetValue(); set => SetValue(value); }
        public bool link
        {
            get => GetBoolean();
            set => SetBoolean(value);
        }

        [RouteValue("sub1")]
        public bool A
        {
            get => GetBoolean();
            set => SetBoolean(value);
        }
        [RouteValue("sub2")]
        public bool B
        {
            get => GetBoolean();
            set => SetBoolean(value);
        }
        [RouteValue("sub3")]
        public bool C
        {
            get => GetBoolean();
            set => SetBoolean(value);
        }
        [RouteValue("sub4")]
        public bool D
        {
            get => GetBoolean();
            set => SetBoolean(value);
        }

        public bool linkmaster
        {
            get => GetBoolean();
            set => SetBoolean(value);
        }

        public int dawpostdsp
        {
            get => (int)GetValue();
            set => SetValue(value);
        }
        //public int memab { get; set; }
        //public string iconid { get; set; }
        //public long aux_asn_flags { get; set; }

        #region aux sends
        [RouteValueRange(-84, 10, Enums.Unit.db)]
        public float aux1 { get => GetValue(); set => SetValue(value); }
        [RouteValueRange(-84, 10, Enums.Unit.db)]
        public float aux2 { get => GetValue(); set => SetValue(value); }
        [RouteValueRange(-84, 10, Enums.Unit.db)]
        public float aux3 { get => GetValue(); set => SetValue(value); }
        [RouteValueRange(-84, 10, Enums.Unit.db)]
        public float aux4 { get => GetValue(); set => SetValue(value); }
        [RouteValueRange(-84, 10, Enums.Unit.db)]
        public float aux5 { get => GetValue(); set => SetValue(value); }
        [RouteValueRange(-84, 10, Enums.Unit.db)]
        public float aux6 { get => GetValue(); set => SetValue(value); }
        [RouteValueRange(-84, 10, Enums.Unit.db)]
        public float aux7 { get => GetValue(); set => SetValue(value); }
        [RouteValueRange(-84, 10, Enums.Unit.db)]
        public float aux8 { get => GetValue(); set => SetValue(value); }
        [RouteValueRange(-84, 10, Enums.Unit.db)]
        public float aux9 { get => GetValue(); set => SetValue(value); }
        [RouteValueRange(-84, 10, Enums.Unit.db)]
        public float aux10 { get => GetValue(); set => SetValue(value); }
        [RouteValueRange(-84, 10, Enums.Unit.db)]
        public float aux11{ get => GetValue(); set => SetValue(value); }
        [RouteValueRange(-84, 10, Enums.Unit.db)]
        public float aux12 { get => GetValue(); set => SetValue(value); }
        [RouteValueRange(-84, 10, Enums.Unit.db)]
        public float aux13 { get => GetValue(); set => SetValue(value); }
        [RouteValueRange(-84, 10, Enums.Unit.db)]
        public float aux14 { get => GetValue(); set => SetValue(value); }
        [RouteValueRange(-84, 10, Enums.Unit.db)]
        public float aux15 { get => GetValue(); set => SetValue(value); }
        [RouteValueRange(-84, 10, Enums.Unit.db)]
        public float aux16 { get => GetValue(); set => SetValue(value); }
        [RouteValueRange(-84, 10, Enums.Unit.db)]
        public float aux17 { get => GetValue(); set => SetValue(value); }
        [RouteValueRange(-84, 10, Enums.Unit.db)]
        public float aux18 { get => GetValue(); set => SetValue(value); }
        [RouteValueRange(-84, 10, Enums.Unit.db)]
        public float aux19 { get => GetValue(); set => SetValue(value); }
        [RouteValueRange(-84, 10, Enums.Unit.db)]
        public float aux20 { get => GetValue(); set => SetValue(value); }
        [RouteValueRange(-84, 10, Enums.Unit.db)]
        public float aux21 { get => GetValue(); set => SetValue(value); }
        [RouteValueRange(-84, 10, Enums.Unit.db)]
        public float aux22 { get => GetValue(); set => SetValue(value); }
        [RouteValueRange(-84, 10, Enums.Unit.db)]
        public float aux23 { get => GetValue(); set => SetValue(value); }
        [RouteValueRange(-84, 10, Enums.Unit.db)]
        public float aux24 { get => GetValue(); set => SetValue(value); }
        [RouteValueRange(-84, 10, Enums.Unit.db)]
        public float aux25 { get => GetValue(); set => SetValue(value); }
        [RouteValueRange(-84, 10, Enums.Unit.db)]
        public float aux26 { get => GetValue(); set => SetValue(value); }
        [RouteValueRange(-84, 10, Enums.Unit.db)]
        public float aux27 { get => GetValue(); set => SetValue(value); }
        [RouteValueRange(-84, 10, Enums.Unit.db)]
        public float aux28 { get => GetValue(); set => SetValue(value); }
        [RouteValueRange(-84, 10, Enums.Unit.db)]
        public float aux29 { get => GetValue(); set => SetValue(value); }
        [RouteValueRange(-84, 10, Enums.Unit.db)]
        public float aux30 { get => GetValue(); set => SetValue(value); }
        [RouteValueRange(-84, 10, Enums.Unit.db)]
        public float aux31 { get => GetValue(); set => SetValue(value); }
        [RouteValueRange(-84, 10, Enums.Unit.db)]
        public float aux32 { get => GetValue(); set => SetValue(value); }
        [RouteValueRange(-100, 100, Enums.Unit.none)]
        public float aux12_pan { get => GetValue(); set => SetValue(value); }
        [RouteValueRange(-100, 100, Enums.Unit.none)]
        public float aux34_pan { get => GetValue(); set => SetValue(value); }
        [RouteValueRange(-100, 100, Enums.Unit.none)]
        public float aux56_pan { get => GetValue(); set => SetValue(value); }
        [RouteValueRange(-100, 100, Enums.Unit.none)]
        public float aux78_pan { get => GetValue(); set => SetValue(value); }
        [RouteValueRange(-100, 100, Enums.Unit.none)]
        public float aux910_pan { get => GetValue(); set => SetValue(value); }
        [RouteValueRange(-100, 100, Enums.Unit.none)]
        public float aux1112_pan { get => GetValue(); set => SetValue(value); }
        [RouteValueRange(-100, 100, Enums.Unit.none)]
        public float aux1314_pan { get => GetValue(); set => SetValue(value); }
        [RouteValueRange(-100, 100, Enums.Unit.none)]
        public float aux1516_pan { get => GetValue(); set => SetValue(value); }
        [RouteValueRange(-100, 100, Enums.Unit.none)]
        public float aux1718_pan { get => GetValue(); set => SetValue(value); }
        [RouteValueRange(-100, 100, Enums.Unit.none)]
        public float aux1920_pan { get => GetValue(); set => SetValue(value); }
        [RouteValueRange(-100, 100, Enums.Unit.none)]
        public float aux2122_pan { get => GetValue(); set => SetValue(value); }
        [RouteValueRange(-100, 100, Enums.Unit.none)]
        public float aux2324_pan { get => GetValue(); set => SetValue(value); }
        [RouteValueRange(-100, 100, Enums.Unit.none)]
        public float aux2526_pan { get => GetValue(); set => SetValue(value); }
        [RouteValueRange(-100, 100, Enums.Unit.none)]
        public float aux2728_pan { get => GetValue(); set => SetValue(value); }
        [RouteValueRange(-100, 100, Enums.Unit.none)]
        public float aux2930_pan { get => GetValue(); set => SetValue(value); }
        [RouteValueRange(-100, 100, Enums.Unit.none)]
        public float aux3132_pan { get => GetValue(); set => SetValue(value); }
        [RouteValueRange(-100, 100, Enums.Unit.none)]
        public float aux12_stpan { get => GetValue(); set => SetValue(value); }
        [RouteValueRange(-100, 100, Enums.Unit.none)]
        public float aux34_stpan { get => GetValue(); set => SetValue(value); }
        [RouteValueRange(-100, 100, Enums.Unit.none)]
        public float aux56_stpan { get => GetValue(); set => SetValue(value); }
        [RouteValueRange(-100, 100, Enums.Unit.none)]
        public float aux78_stpan { get => GetValue(); set => SetValue(value); }
        [RouteValueRange(-100, 100, Enums.Unit.none)]
        public float aux910_stpan { get => GetValue(); set => SetValue(value); }
        [RouteValueRange(-100, 100, Enums.Unit.none)]
        public float aux1112_stpan { get => GetValue(); set => SetValue(value); }
        [RouteValueRange(-100, 100, Enums.Unit.none)]
        public float aux1314_stpan { get => GetValue(); set => SetValue(value); }
        [RouteValueRange(-100, 100, Enums.Unit.none)]
        public float aux1516_stpan { get => GetValue(); set => SetValue(value); }
        [RouteValueRange(-100, 100, Enums.Unit.none)]
        public float aux1718_stpan { get => GetValue(); set => SetValue(value); }
        [RouteValueRange(-100, 100, Enums.Unit.none)]
        public float aux1920_stpan { get => GetValue(); set => SetValue(value); }
        [RouteValueRange(-100, 100, Enums.Unit.none)]
        public float aux2122_stpan { get => GetValue(); set => SetValue(value); }
        [RouteValueRange(-100, 100, Enums.Unit.none)]
        public float aux2324_stpan { get => GetValue(); set => SetValue(value); }
        [RouteValueRange(-100, 100, Enums.Unit.none)]
        public float aux2526_stpan { get => GetValue(); set => SetValue(value); }
        [RouteValueRange(-100, 100, Enums.Unit.none)]
        public float aux2728_stpan { get => GetValue(); set => SetValue(value); }
        [RouteValueRange(-100, 100, Enums.Unit.none)]
        public float aux2930_stpan { get => GetValue(); set => SetValue(value); }
        [RouteValueRange(-100, 100, Enums.Unit.none)]
        public float aux3132_stpan { get => GetValue(); set => SetValue(value); }

        #endregion


        public int mono
        {
            get => (int)GetValue();
            set => SetValue(value);
        }
        public float monolevel { get; set; }
        public float centerdiv { get; set; }
        public int insertslot
        {
            get => (int)GetValue();
            set => SetValue(value);
        }
        public int insertprepost
        {
            get => (int)GetValue();
            set => SetValue(value);
        }
        public int adc_src
        {
            get => (int)GetValue();
            set => SetValue(value);
        }
        public int avb_src
        {
            get => (int)GetValue();
            set => SetValue(value);
        }
        public int usb_src
        {
            get => (int)GetValue();
            set => SetValue(value);
        }
        public int sd_src
        {
            get => (int)GetValue();
            set => SetValue(value);
        }
        public int adc_src2
        {
            get => (int)GetValue();
            set => SetValue(value);
        }
        public int avb_src2
        {
            get => (int)GetValue();
            set => SetValue(value);
        }
        public int usb_src2
        {
            get => (int)GetValue();
            set => SetValue(value);
        }
        public int sd_src2
        {
            get => (int)GetValue();
            set => SetValue(value);
        }
        public bool lr
        {
            get => GetBoolean();
            set => SetBoolean(value);
        }

        public int sub_asn_flags
        {
            get => (int)GetValue();
            set => SetValue(value);
        }
        public int fx_asn_flags
        {
            get => (int)GetValue();
            set => SetValue(value);
        }
        public float FXA { get; set; }
        public float FXB { get; set; }
        public float FXC { get; set; }
        public float FXD { get; set; }
        public float FXE { get; set; }
        public float FXF { get; set; }
        public float FXG { get; set; }
        public float FXH { get; set; }
        public int inputsrc
        {
            get => (int)GetValue();
            set => SetValue(value);
        }
        public float delay { get => GetValue(); set => SetValue(value); }
        public int flexassignflags { get; set; }

        [RouteValue("48v")]
        public bool phantom
        {
            get => GetBoolean();
            set => SetBoolean(value);
        }

        public bool polarity
        {
            get => GetBoolean();
            set => SetBoolean(value);
        }

        [RouteValueRange(0, 60, Enums.Unit.db)]
        public float preampgain
        {
            get => GetValue();
            set => SetValue(value);
        }
        public float digitalgain { get => GetValue(); set => SetValue(value); }
        public int _10db_boost
        {
            get => (int)GetValue();
            set => SetValue(value);
        }
        public int gatekeysrc
        {
            get => (int)GetValue();
            set => SetValue(value);
        }
        public int compkeysrc
        {
            get => (int)GetValue();
            set => SetValue(value);
        }
        public int digsendsrc
        {
            get => (int)GetValue();
            set => SetValue(value);
        }
        public int gaincomp
        {
            get => (int)GetValue();
            set => SetValue(value);
        }
        [RouteValueRange(0, 1000, Enums.Unit.hz)]
        [RouteValue("filter/hpf")]
        public float hipass { get => GetValue(); set => SetValue(value); }
        public int auxpremode
        {
            get => (int)GetValue();
            set => SetValue(value);
        }
        public int busmode
        {
            get => (int)GetValue();
            set => SetValue(value);
        }
        public float busdelay { get => GetValue(); set => SetValue(value); }
        public bool lr_assign
        {
            get => GetBoolean();
            set => SetBoolean(value);
        }
        public int bussrc
        {
            get => (int)GetValue();
            set => SetValue(value);
        }
        public string name { get => GetString(); set => SetString(value); }
        public float fx1 { get; set; }
        public float fx2 { get; set; }
        public float fx3 { get; set; }
        public float fx4 { get; set; }
        public float fx5 { get; set; }
        public float fx6 { get; set; }
        public float fx7 { get; set; }
        public float fx8 { get; set; }
        public int asn_flags_1_32
        {
            get => (int)GetValue();
            set => SetValue(value);
        }
        public int asn_flags_33_64
        {
            get => (int)GetValue();
            set => SetValue(value);
        }
        public int asn_flags_fxret_ret
        {
            get => (int)GetValue();
            set => SetValue(value);
        }

        #region Compressor
        [RouteValue("comp/on")]
        public bool comp_on
        {
            get => GetBoolean();
            set => SetBoolean(value);
        }

        [RouteValue("comp/automode")]
        public bool comp_automode
        {
            get => GetBoolean();
            set => SetBoolean(value);
        }

        [RouteValue("comp/softknee")]
        public bool comp_softknee
        {
            get => GetBoolean();
            set => SetBoolean(value);
        }

        [RouteValueRange(-56, 0, Enums.Unit.db)]
        [RouteValue("comp/threshold")]
        public float comp_threshold
        {
            get => GetValue();
            set => SetValue(value);
        }

        [RouteValue("comp/ratio")]
        public float comp_ratio
        {
            get => GetValue();
            set => SetValue(value);
        }
        [RouteValue("comp/attack")]
        public float comp_attack
        {
            get => GetValue();
            set => SetValue(value);
        }
        [RouteValue("comp/release")]
        public float comp_release
        {
            get => GetValue();
            set => SetValue(value);
        }
        [RouteValue("comp/gain")]
        public float comp_gain
        {
            get => GetValue();
            set => SetValue(value);
        }
        #endregion

        #region gate
        [RouteValue("gate/threshold")]
        public float gate_threshold
        {
            get => GetValue();
            set => SetValue(value);
        }
        [RouteValue("gate/range")]
        public float gate_range
        {
            get => GetValue();
            set => SetValue(value);
        }
        [RouteValue("gate/attack")]
        public float gate_attack
        {
            get => GetValue();
            set => SetValue(value);
        }
        [RouteValue("gate/release")]
        public float gate_release
        {
            get => GetValue();
            set => SetValue(value);
        }
        [RouteValue("gate/on")]
        public bool gate_on
        {
            get => GetBoolean();
            set => SetBoolean(value);
        }
        [RouteValue("gate/expander")]
        public bool gate_expander
        {
            get => GetBoolean();
            set => SetBoolean(value);
        }

        #endregion

        #region EQ
        [RouteValue("eq/eqallon")]
        public bool eq_on
        {
            get { return GetBoolean(); }
            set { SetBoolean(value); }
        }
        [RouteValue("eq/eqbandon1")]
        public bool eq_bandon1
        {
            get { return GetBoolean(); }
            set { SetBoolean(value); }
        }
        [RouteValue("eq/eqbandon2")]
        public bool eq_bandon2
        {
            get { return GetBoolean(); }
            set { SetBoolean(value); }
        }
        [RouteValue("eq/eqbandon3")]
        public bool eq_bandon3
        {
            get { return GetBoolean(); }
            set { SetBoolean(value); }
        }
        [RouteValue("eq/eqbandon4")]
        public bool eq_bandon4
        {
            get { return GetBoolean(); }
            set { SetBoolean(value); }
        }
        [RouteValue("eq/eqbandop1")]
        public bool eq_bandop1
        {
            get { return GetBoolean(); }
            set { SetBoolean(value); }
        }
        [RouteValue("eq/eqbandop4")]
        public bool eq_bandop4
        {
            get { return GetBoolean(); }
            set { SetBoolean(value); }
        }
        [RouteValue("eq/eqgain1")]
        public float eq_gain1
        {
            get => GetValue();
            set => SetValue(value);
        }
        [RouteValue("eq/eqgain2")]
        public float eq_gain2
        {
            get => GetValue();
            set => SetValue(value);
        }
        [RouteValue("eq/eqgain3")]
        public float eq_gain3
        {
            get => GetValue();
            set => SetValue(value);
        }
        [RouteValue("eq/eqgain4")]
        public float eq_gain4
        {
            get => GetValue();
            set => SetValue(value);
        }
        [RouteValue("eq/eqq1")]
        public float eq_q1
        {
            get => GetValue();
            set => SetValue(value);
        }
        [RouteValue("eq/eqq2")]
        public float eq_q2
        {
            get => GetValue();
            set => SetValue(value);
        }
        [RouteValue("eq/eqq3")]
        public float eq_q3
        {
            get => GetValue();
            set => SetValue(value);
        }
        [RouteValue("eq/eqq4")]
        public float eq_q4
        {
            get => GetValue();
            set => SetValue(value);
        }
        [RouteValue("eq/eqfreq1")]
        public float eq_freq1
        {
            get => GetValue();
            set => SetValue(value);
        }
        [RouteValue("eq/eqfreq2")]
        public float eq_freq2
        {
            get => GetValue();
            set => SetValue(value);
        }
        [RouteValue("eq/eqfreq3")]
        public float eq_freq3
        {
            get => GetValue();
            set => SetValue(value);
        }
        [RouteValue("eq/eqfreq4")]
        public float eq_freq4
        {
            get => GetValue();
            set => SetValue(value);
        }
        #endregion
        [RouteValue("limit/threshold")]
        public float limiter_threshold
        {
            get => GetValue();
            set => SetValue(value);
        }
        [RouteValue("limit/limiteron")]
        public bool limiter_on
        {
            get => GetBoolean();
            set => SetBoolean(value);
        }
    }
}
