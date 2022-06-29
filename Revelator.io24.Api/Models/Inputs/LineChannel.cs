using Presonus.StudioLive32.Api.Attributes;
using Presonus.StudioLive32.Api.Scene;
using System;
using System.ComponentModel;

namespace Presonus.StudioLive32.Api.Models.Inputs
{
    public class LineChannel : InputChannel
    {
        public LineChannel(string routingPrefix, RawService rawService) : base(routingPrefix, rawService)
        {
        }

        public bool sub1 { get => GetBoolean(); set => SetBoolean(value); }
        public bool sub2 { get => GetBoolean(); set => SetBoolean(value); }
        public bool sub3 { get => GetBoolean(); set => SetBoolean(value); }
        public bool sub4 { get => GetBoolean(); set => SetBoolean(value); }

        public bool lr { get => GetBoolean(); set => SetBoolean(value); }

        public int sub_asn_flags { get => (int)GetValue(); set => SetValue(value); }
        public int fx_asn_flags { get => (int)GetValue(); set => SetValue(value); }
        [RouteValueRange(-84, 10, Enums.Unit.db)] public float FXA { get => GetValue(); set => SetValue(value); }
        [RouteValueRange(-84, 10, Enums.Unit.db)] public float FXB { get => GetValue(); set => SetValue(value); }
        [RouteValueRange(-84, 10, Enums.Unit.db)] public float FXC { get => GetValue(); set => SetValue(value); }
        [RouteValueRange(-84, 10, Enums.Unit.db)] public float FXD { get => GetValue(); set => SetValue(value); }
        [RouteValueRange(-84, 10, Enums.Unit.db)] public float FXE { get => GetValue(); set => SetValue(value); }
        [RouteValueRange(-84, 10, Enums.Unit.db)] public float FXF { get => GetValue(); set => SetValue(value); }
        [RouteValueRange(-84, 10, Enums.Unit.db)] public float FXG { get => GetValue(); set => SetValue(value); }
        [RouteValueRange(-84, 10, Enums.Unit.db)] public float FXH { get => GetValue(); set => SetValue(value); }
        public int inputsrc { get => (int)GetValue(); set => SetValue(value); }
        public float delay { get => GetValue(); set => SetValue(value); }
        public int flexassignflags { get; set; }
        [RouteValue("48v")] public bool phantom { get => GetBoolean(); set => SetBoolean(value); }
        public bool polarity { get => GetBoolean(); set => SetBoolean(value); }
        [RouteValueRange(0, 60, Enums.Unit.db)] public float preampgain { get => GetValue(); set => SetValue(value); }
        public float digitalgain { get => GetValue(); set => SetValue(value); }
        public int _10db_boost { get => (int)GetValue(); set => SetValue(value); }
        public int gatekeysrc { get => (int)GetValue(); set => SetValue(value); }
        public int compkeysrc { get => (int)GetValue(); set => SetValue(value); }
        public int digsendsrc { get => (int)GetValue(); set => SetValue(value); }
        public int gaincomp { get => (int)GetValue(); set => SetValue(value); }
        [RouteValueRange(0, 1000, Enums.Unit.hz)][RouteValue("filter/hpf")] public float hipass { get => GetValue(); set => SetValue(value); }

        public float fx1 { get; set; }
        public float fx2 { get; set; }
        public float fx3 { get; set; }
        public float fx4 { get; set; }
        public float fx5 { get; set; }
        public float fx6 { get; set; }
        public float fx7 { get; set; }
        public float fx8 { get; set; }
        public int asn_flags_1_32 { get => (int)GetValue(); set => SetValue(value); }
        public int asn_flags_33_64 { get => (int)GetValue(); set => SetValue(value); }
        public int asn_flags_fxret_ret { get => (int)GetValue(); set => SetValue(value); }

        #region Gate
        [RouteValue("gate/threshold")] public float gate_threshold { get => GetValue(); set => SetValue(value); }
        [RouteValue("gate/range")] public float gate_range { get => GetValue(); set => SetValue(value); }
        [RouteValue("gate/attack")] public float gate_attack { get => GetValue(); set => SetValue(value); }
        [RouteValue("gate/release")] public float gate_release { get => GetValue(); set => SetValue(value); }
        [RouteValue("gate/on")] public bool gate_on { get => GetBoolean(); set => SetBoolean(value); }
        [RouteValue("gate/expander")] public bool gate_expander { get => GetBoolean(); set => SetBoolean(value); }
        #endregion

    }
}
