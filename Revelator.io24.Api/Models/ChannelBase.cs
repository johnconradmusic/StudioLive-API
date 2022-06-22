using Revelator.io24.Api.Attributes;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Revelator.io24.Api.Models
{
    public class ChannelBase : DeviceRoutingBase
    {
        public event PropertyChangedEventHandler PropertyChanged;
        protected override void OnPropertyChanged(PropertyChangedEventArgs eventArgs) { PropertyChanged?.Invoke(this, eventArgs); }
        public ChannelBase(string routingPrefix, RawService rawService) : base(routingPrefix, rawService) { }

        public string AutomationName => username;
        public string AutomationId => _routePrefix + username;
        public bool LinkSlave => !(link && !linkmaster);
        public string username { get => GetString(); set => SetString(value); }
        public bool solo { get => GetBoolean(); set => SetBoolean(value); }
        public int color { get => (int)GetValue(); set => SetValue(value); }
        [RouteValueRange(-84, 10, Enums.Unit.db)] public float volume { get { return GetValue(); } set { SetValue(value); } }
        public bool mute { get => GetBoolean(); set => SetBoolean(value); }
        [RouteValueRange(-100, 100, Enums.Unit.none)] public float pan { get => GetValue(); set => SetValue(value); }
        [RouteValueRange(-100, 100, Enums.Unit.none)] public float stereopan { get => GetValue(); set => SetValue(value); }
        public bool link { get => GetBoolean(); set => SetBoolean(value); }
        public bool linkmaster { get => GetBoolean(); set => SetBoolean(value); }
        public int dawpostdsp { get => (int)GetValue(); set => SetValue(value); }
        public int memab { get => (int)GetValue(); set => SetValue(value); }
        public string iconid { get => GetString(); set => SetString(value); }
        public long aux_asn_flags { get => (long)GetValue(); set => SetValue(value); }
        public bool clip { get => GetBoolean(); }
        public string name { get => GetString(); set => SetString(value); }


        #region aux
        [RouteValueRange(-84, 10, Enums.Unit.db)] public float aux1 { get => GetValue(); set => SetValue(value); }
        [RouteValueRange(-84, 10, Enums.Unit.db)] public float aux2 { get => GetValue(); set => SetValue(value); }
        [RouteValueRange(-84, 10, Enums.Unit.db)] public float aux3 { get => GetValue(); set => SetValue(value); }
        [RouteValueRange(-84, 10, Enums.Unit.db)] public float aux4 { get => GetValue(); set => SetValue(value); }
        [RouteValueRange(-84, 10, Enums.Unit.db)] public float aux5 { get => GetValue(); set => SetValue(value); }
        [RouteValueRange(-84, 10, Enums.Unit.db)] public float aux6 { get => GetValue(); set => SetValue(value); }
        [RouteValueRange(-84, 10, Enums.Unit.db)] public float aux7 { get => GetValue(); set => SetValue(value); }
        [RouteValueRange(-84, 10, Enums.Unit.db)] public float aux8 { get => GetValue(); set => SetValue(value); }
        [RouteValueRange(-84, 10, Enums.Unit.db)] public float aux9 { get => GetValue(); set => SetValue(value); }
        [RouteValueRange(-84, 10, Enums.Unit.db)] public float aux10 { get => GetValue(); set => SetValue(value); }
        [RouteValueRange(-84, 10, Enums.Unit.db)] public float aux11 { get => GetValue(); set => SetValue(value); }
        [RouteValueRange(-84, 10, Enums.Unit.db)] public float aux12 { get => GetValue(); set => SetValue(value); }
        [RouteValueRange(-84, 10, Enums.Unit.db)] public float aux13 { get => GetValue(); set => SetValue(value); }
        [RouteValueRange(-84, 10, Enums.Unit.db)] public float aux14 { get => GetValue(); set => SetValue(value); }
        [RouteValueRange(-84, 10, Enums.Unit.db)] public float aux15 { get => GetValue(); set => SetValue(value); }
        [RouteValueRange(-84, 10, Enums.Unit.db)] public float aux16 { get => GetValue(); set => SetValue(value); }
        [RouteValueRange(-84, 10, Enums.Unit.db)] public float aux17 { get => GetValue(); set => SetValue(value); }
        [RouteValueRange(-84, 10, Enums.Unit.db)] public float aux18 { get => GetValue(); set => SetValue(value); }
        [RouteValueRange(-84, 10, Enums.Unit.db)] public float aux19 { get => GetValue(); set => SetValue(value); }
        [RouteValueRange(-84, 10, Enums.Unit.db)] public float aux20 { get => GetValue(); set => SetValue(value); }
        [RouteValueRange(-84, 10, Enums.Unit.db)] public float aux21 { get => GetValue(); set => SetValue(value); }
        [RouteValueRange(-84, 10, Enums.Unit.db)] public float aux22 { get => GetValue(); set => SetValue(value); }
        [RouteValueRange(-84, 10, Enums.Unit.db)] public float aux23 { get => GetValue(); set => SetValue(value); }
        [RouteValueRange(-84, 10, Enums.Unit.db)] public float aux24 { get => GetValue(); set => SetValue(value); }
        [RouteValueRange(-84, 10, Enums.Unit.db)] public float aux25 { get => GetValue(); set => SetValue(value); }
        [RouteValueRange(-84, 10, Enums.Unit.db)] public float aux26 { get => GetValue(); set => SetValue(value); }
        [RouteValueRange(-84, 10, Enums.Unit.db)] public float aux27 { get => GetValue(); set => SetValue(value); }
        [RouteValueRange(-84, 10, Enums.Unit.db)] public float aux28 { get => GetValue(); set => SetValue(value); }
        [RouteValueRange(-84, 10, Enums.Unit.db)] public float aux29 { get => GetValue(); set => SetValue(value); }
        [RouteValueRange(-84, 10, Enums.Unit.db)] public float aux30 { get => GetValue(); set => SetValue(value); }
        [RouteValueRange(-84, 10, Enums.Unit.db)] public float aux31 { get => GetValue(); set => SetValue(value); }
        [RouteValueRange(-84, 10, Enums.Unit.db)] public float aux32 { get => GetValue(); set => SetValue(value); }
        [RouteValueRange(-100, 100, Enums.Unit.none)] public float aux12_pan { get => GetValue(); set => SetValue(value); }
        [RouteValueRange(-100, 100, Enums.Unit.none)] public float aux34_pan { get => GetValue(); set => SetValue(value); }
        [RouteValueRange(-100, 100, Enums.Unit.none)] public float aux56_pan { get => GetValue(); set => SetValue(value); }
        [RouteValueRange(-100, 100, Enums.Unit.none)] public float aux78_pan { get => GetValue(); set => SetValue(value); }
        [RouteValueRange(-100, 100, Enums.Unit.none)] public float aux910_pan { get => GetValue(); set => SetValue(value); }
        [RouteValueRange(-100, 100, Enums.Unit.none)] public float aux1112_pan { get => GetValue(); set => SetValue(value); }
        [RouteValueRange(-100, 100, Enums.Unit.none)] public float aux1314_pan { get => GetValue(); set => SetValue(value); }
        [RouteValueRange(-100, 100, Enums.Unit.none)] public float aux1516_pan { get => GetValue(); set => SetValue(value); }
        [RouteValueRange(-100, 100, Enums.Unit.none)] public float aux1718_pan { get => GetValue(); set => SetValue(value); }
        [RouteValueRange(-100, 100, Enums.Unit.none)] public float aux1920_pan { get => GetValue(); set => SetValue(value); }
        [RouteValueRange(-100, 100, Enums.Unit.none)] public float aux2122_pan { get => GetValue(); set => SetValue(value); }
        [RouteValueRange(-100, 100, Enums.Unit.none)] public float aux2324_pan { get => GetValue(); set => SetValue(value); }
        [RouteValueRange(-100, 100, Enums.Unit.none)] public float aux2526_pan { get => GetValue(); set => SetValue(value); }
        [RouteValueRange(-100, 100, Enums.Unit.none)] public float aux2728_pan { get => GetValue(); set => SetValue(value); }
        [RouteValueRange(-100, 100, Enums.Unit.none)] public float aux2930_pan { get => GetValue(); set => SetValue(value); }
        [RouteValueRange(-100, 100, Enums.Unit.none)] public float aux3132_pan { get => GetValue(); set => SetValue(value); }
        [RouteValueRange(-100, 100, Enums.Unit.none)] public float aux12_stpan { get => GetValue(); set => SetValue(value); }
        [RouteValueRange(-100, 100, Enums.Unit.none)] public float aux34_stpan { get => GetValue(); set => SetValue(value); }
        [RouteValueRange(-100, 100, Enums.Unit.none)] public float aux56_stpan { get => GetValue(); set => SetValue(value); }
        [RouteValueRange(-100, 100, Enums.Unit.none)] public float aux78_stpan { get => GetValue(); set => SetValue(value); }
        [RouteValueRange(-100, 100, Enums.Unit.none)] public float aux910_stpan { get => GetValue(); set => SetValue(value); }
        [RouteValueRange(-100, 100, Enums.Unit.none)] public float aux1112_stpan { get => GetValue(); set => SetValue(value); }
        [RouteValueRange(-100, 100, Enums.Unit.none)] public float aux1314_stpan { get => GetValue(); set => SetValue(value); }
        [RouteValueRange(-100, 100, Enums.Unit.none)] public float aux1516_stpan { get => GetValue(); set => SetValue(value); }
        [RouteValueRange(-100, 100, Enums.Unit.none)] public float aux1718_stpan { get => GetValue(); set => SetValue(value); }
        [RouteValueRange(-100, 100, Enums.Unit.none)] public float aux1920_stpan { get => GetValue(); set => SetValue(value); }
        [RouteValueRange(-100, 100, Enums.Unit.none)] public float aux2122_stpan { get => GetValue(); set => SetValue(value); }
        [RouteValueRange(-100, 100, Enums.Unit.none)] public float aux2324_stpan { get => GetValue(); set => SetValue(value); }
        [RouteValueRange(-100, 100, Enums.Unit.none)] public float aux2526_stpan { get => GetValue(); set => SetValue(value); }
        [RouteValueRange(-100, 100, Enums.Unit.none)] public float aux2728_stpan { get => GetValue(); set => SetValue(value); }
        [RouteValueRange(-100, 100, Enums.Unit.none)] public float aux2930_stpan { get => GetValue(); set => SetValue(value); }
        [RouteValueRange(-100, 100, Enums.Unit.none)] public float aux3132_stpan { get => GetValue(); set => SetValue(value); }

        #endregion
        public bool mono { get => GetBoolean(); set => SetBoolean(value); }
        [RouteValueRange(-84, 10, Enums.Unit.db)] public float monolevel { get => GetValue(); set => SetValue(value); }
        public float centerdiv { get => GetValue(); set => SetValue(value); }
        public int insertslot { get => (int)GetValue(); set => SetValue(value); }
        public int insertprepost { get => (int)GetValue(); set => SetValue(value); }
        public int adc_src { get => (int)GetValue(); set => SetValue(value); }
        public int avb_src { get => (int)GetValue(); set => SetValue(value); }
        public int usb_src { get => (int)GetValue(); set => SetValue(value); }
        public int sd_src { get => (int)GetValue(); set => SetValue(value); }
        public int adc_src2 { get => (int)GetValue(); set => SetValue(value); }
        public int avb_src2 { get => (int)GetValue(); set => SetValue(value); }
        public int usb_src2 { get => (int)GetValue(); set => SetValue(value); }
        public int sd_src2 { get => (int)GetValue(); set => SetValue(value); }

        #region Compressor
        [RouteValue("comp/on")] public bool comp_on { get => GetBoolean(); set => SetBoolean(value); }
        [RouteValue("comp/automode")] public bool comp_automode { get => GetBoolean(); set => SetBoolean(value); }
        [RouteValue("comp/softknee")] public bool comp_softknee { get => GetBoolean(); set => SetBoolean(value); }
        [RouteValueRange(-56, 0, Enums.Unit.db)][RouteValue("comp/threshold")] public float comp_threshold { get => GetValue(); set => SetValue(value); }
        [RouteValue("comp/ratio")] public float comp_ratio { get => GetValue(); set => SetValue(value); }
        [RouteValue("comp/attack")] public float comp_attack { get => GetValue(); set => SetValue(value); }
        [RouteValue("comp/release")] public float comp_release { get => GetValue(); set => SetValue(value); }
        [RouteValue("comp/gain")] public float comp_gain { get => GetValue(); set => SetValue(value); }
        #endregion

        #region Limiter
        [RouteValue("limit/threshold")] public float limiter_threshold { get => GetValue(); set => SetValue(value); }
        [RouteValue("limit/limiteron")] public bool limiter_on { get => GetBoolean(); set => SetBoolean(value); }
        #endregion
    }
}
