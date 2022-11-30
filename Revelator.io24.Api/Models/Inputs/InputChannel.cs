using Newtonsoft.Json;
using Presonus.StudioLive32.Api.Attributes;
using Presonus.UC.Api.Devices;
using Presonus.UC.Api.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Presonus.StudioLive32.Api.Models.Inputs
{
    public class InputChannel : ChannelBase, INotifyPropertyChanged
    {

        public InputChannel(string routingPrefix, RawService rawService, Device device) : base(routingPrefix, rawService, device) { }

        public DawPostDsp dawpostdsp { get => GetEnumValue<DawPostDsp>(); set => SetEnumValue(value); }
        public USBSource usb_src
        {
            get => GetEnumValue<USBSource>(); set
            {
                if (LinkMaster && device.Channels[device.Channels.IndexOf(this) + 1] is InputChannel next)
                    next.usb_src = value + 1;
                SetEnumValue(value); usb_src2 = value + 1;
            }
        }
        public USBSource usb_src2 { get => GetEnumValue<USBSource>(); set => SetEnumValue(value); }

        public int sd_src { get => (int)GetValue(); set => SetValue(value); }
        #region EQ
        [RouteValue("eq/eqallon")] public bool eq_on { get { return GetBoolean(); } set { SetBoolean(value); } }
        [RouteValue("eq/eqbandon1")] public bool eq_bandon1 { get { return GetBoolean(); } set { SetBoolean(value); } }
        [RouteValue("eq/eqbandon2")] public bool eq_bandon2 { get { return GetBoolean(); } set { SetBoolean(value); } }
        [RouteValue("eq/eqbandon3")] public bool eq_bandon3 { get { return GetBoolean(); } set { SetBoolean(value); } }
        [RouteValue("eq/eqbandon4")] public bool eq_bandon4 { get { return GetBoolean(); } set { SetBoolean(value); } }
        [RouteValue("eq/eqbandop1")] public bool eq_bandop1 { get { return GetBoolean(); } set { SetBoolean(value); } }
        [RouteValue("eq/eqbandop4")] public bool eq_bandop4 { get { return GetBoolean(); } set { SetBoolean(value); } }
        [RouteValueRange(-15, 15, Enums.Unit.db)][RouteValue("eq/eqgain1")] public float eq_gain1 { get => GetValue(); set => SetValue(value); }
        [RouteValueRange(-15, 15, Enums.Unit.db)][RouteValue("eq/eqgain2")] public float eq_gain2 { get => GetValue(); set => SetValue(value); }
        [RouteValueRange(-15, 15, Enums.Unit.db)][RouteValue("eq/eqgain3")] public float eq_gain3 { get => GetValue(); set => SetValue(value); }
        [RouteValueRange(-15, 15, Enums.Unit.db)][RouteValue("eq/eqgain4")] public float eq_gain4 { get => GetValue(); set => SetValue(value); }
        [RouteValueRange(0.1f, 10, Enums.Unit.octave)][RouteValue("eq/eqq1")] public float eq_q1 { get => GetValue(); set => SetValue(value); }
        [RouteValueRange(0.1f, 10, Enums.Unit.octave)][RouteValue("eq/eqq2")] public float eq_q2 { get => GetValue(); set => SetValue(value); }
        [RouteValueRange(0.1f, 10, Enums.Unit.octave)][RouteValue("eq/eqq3")] public float eq_q3 { get => GetValue(); set => SetValue(value); }
        [RouteValueRange(0.1f, 10, Enums.Unit.octave)][RouteValue("eq/eqq4")] public float eq_q4 { get => GetValue(); set => SetValue(value); }
        [RouteValueRange(36, 18000, Enums.Unit.hz)][RouteValue("eq/eqfreq1")] public float eq_freq1 { get => GetFrequency(); set => SetFrequency(value); }
        [RouteValueRange(36, 18000, Enums.Unit.hz)][RouteValue("eq/eqfreq2")] public float eq_freq2 { get => GetFrequency(); set => SetFrequency(value); }
        [RouteValueRange(36, 18000, Enums.Unit.hz)][RouteValue("eq/eqfreq3")] public float eq_freq3 { get => GetFrequency(); set => SetFrequency(value); }
        [RouteValueRange(36, 18000, Enums.Unit.hz)][RouteValue("eq/eqfreq4")] public float eq_freq4 { get => GetFrequency(); set => SetFrequency(value); }
        #endregion
    }
}
