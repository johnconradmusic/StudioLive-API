using Revelator.io24.Api.Attributes;
using Revelator.io24.Api.Scene;
using System;
using System.ComponentModel;

namespace Revelator.io24.Api.Models.Inputs
{
	public class LineChannel : DeviceRoutingBase
	{
		public event PropertyChangedEventHandler PropertyChanged;

		protected override void OnPropertyChanged(PropertyChangedEventArgs eventArgs)
			=> PropertyChanged?.Invoke(this, eventArgs);

		private readonly RawService _rawService;
		public LineChannel(string routingPrefix, RawService rawService)
			: base(routingPrefix, rawService)
		{
			_rawService = rawService;
		}

		public string AutomationName => username;

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
		public int solo
		{
			get => (int)GetValue();
			set => SetValue(value);
		}

		[RouteValue("dca/volume")]
		public double level
		{
			get => GetValue();
			set => SetValue(value);
		}

		public double volume
		{
			get { return GetValue(); }
			set { SetValue(value); }
		}
		public int mute
		{
			get => (int)GetValue();
			set => SetValue(value);
		}
		public double pan { get => GetValue(); set => SetValue(value); }
		public double stereopan { get => GetValue(); set => SetValue(value); }
		public int link
		{
			get => (int)GetValue();
			set => SetValue(value);
		}

		public int linkmaster
		{
			get => (int)GetValue();
			set => SetValue(value);
		}
		public int dawpostdsp
		{
			get => (int)GetValue();
			set => SetValue(value);
		}
		public int memab { get; set; }
		public string iconid { get; set; }
		public long aux_asn_flags { get; set; }
		public double aux1 { get => GetValue(); set => SetValue(value); }
		public double aux2 { get => GetValue(); set => SetValue(value); }
		public double aux3 { get => GetValue(); set => SetValue(value); }
		public double aux4 { get => GetValue(); set => SetValue(value); }
		public double aux5 { get => GetValue(); set => SetValue(value); }
		public double aux6 { get; set; }
		public double aux7 { get; set; }
		public double aux8 { get; set; }
		public double aux9 { get; set; }
		public double aux10 { get; set; }
		public double aux11 { get; set; }
		public double aux12 { get; set; }
		public double aux13 { get; set; }
		public double aux14 { get; set; }
		public double aux15 { get; set; }
		public double aux16 { get; set; }
		public double aux17 { get; set; }
		public double aux18 { get; set; }
		public double aux19 { get; set; }
		public double aux20 { get; set; }
		public double aux21 { get; set; }
		public double aux22 { get; set; }
		public double aux23 { get; set; }
		public double aux24 { get; set; }
		public double aux25 { get; set; }
		public double aux26 { get; set; }
		public double aux27 { get; set; }
		public double aux28 { get; set; }
		public double aux29 { get; set; }
		public double aux30 { get; set; }
		public double aux31 { get; set; }
		public double aux32 { get; set; }
		public double aux12_pan { get; set; }
		public double aux34_pan { get; set; }
		public double aux56_pan { get; set; }
		public double aux78_pan { get; set; }
		public double aux910_pan { get; set; }
		public double aux1112_pan { get; set; }
		public double aux1314_pan { get; set; }
		public double aux1516_pan { get; set; }
		public double aux1718_pan { get; set; }
		public double aux1920_pan { get; set; }
		public double aux2122_pan { get; set; }
		public double aux2324_pan { get; set; }
		public double aux2526_pan { get; set; }
		public double aux2728_pan { get; set; }
		public double aux2930_pan { get; set; }
		public double aux3132_pan { get; set; }
		public double aux12_stpan { get; set; }
		public double aux34_stpan { get; set; }
		public double aux56_stpan { get; set; }
		public double aux78_stpan { get; set; }
		public double aux910_stpan { get; set; }
		public double aux1112_stpan { get; set; }
		public double aux1314_stpan { get; set; }
		public double aux1516_stpan { get; set; }
		public double aux1718_stpan { get; set; }
		public double aux1920_stpan { get; set; }
		public double aux2122_stpan { get; set; }
		public double aux2324_stpan { get; set; }
		public double aux2526_stpan { get; set; }
		public double aux2728_stpan { get; set; }
		public double aux2930_stpan { get; set; }
		public double aux3132_stpan { get; set; }
		public int mono
		{
			get => (int)GetValue();
			set => SetValue(value);
		}
		public double monolevel { get; set; }
		public double centerdiv { get; set; }
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
		public int lr
		{
			get => (int)GetValue();
			set => SetValue(value);
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
		public double FXA { get; set; }
		public double FXB { get; set; }
		public double FXC { get; set; }
		public double FXD { get; set; }
		public double FXE { get; set; }
		public double FXF { get; set; }
		public double FXG { get; set; }
		public double FXH { get; set; }
		public int inputsrc
		{
			get => (int)GetValue();
			set => SetValue(value);
		}
		public double delay { get => GetValue(); set => SetValue(value); }
		public int flexassignflags { get; set; }

		[RouteValue("48v")]
		public int phantom
		{
			get => (int)GetValue();
			set => SetValue(value);
		}
		public int polarity
		{
			get => (int)GetValue();
			set => SetValue(value);
		}
		public double preampgain { get => GetValue(); set => SetValue(value); }
		public double digitalgain { get => GetValue(); set => SetValue(value); }
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
		[RouteValue("filter/hpf")]
		public double hipass { get => GetValue(); set => SetValue(value); }
		public double trim { get => GetValue(); set => SetValue(value); }
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
		public double busdelay { get => GetValue(); set => SetValue(value); }
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
		public double fx1 { get; set; }
		public double fx2 { get; set; }
		public double fx3 { get; set; }
		public double fx4 { get; set; }
		public double fx5 { get; set; }
		public double fx6 { get; set; }
		public double fx7 { get; set; }
		public double fx8 { get; set; }
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

		[RouteValue("comp/threshold")]
		public double comp_threshold
		{
			get => GetValue();
			set => SetValue(value);
		}
		[RouteValue("comp/ratio")]
		public double comp_ratio
		{
			get => GetValue();
			set => SetValue(value);
		}
		[RouteValue("comp/attack")]
		public double comp_attack
		{
			get => GetValue();
			set => SetValue(value);
		}
		[RouteValue("comp/release")]
		public double comp_release
		{
			get => GetValue();
			set => SetValue(value);
		}
		[RouteValue("comp/gain")]
		public double comp_gain
		{
			get => GetValue();
			set => SetValue(value);
		}
		#endregion

		#region gate
		[RouteValue("gate/threshold")]
		public double gate_threshold
		{
			get => GetValue();
			set => SetValue(value);
		}
		[RouteValue("gate/range")]
		public double gate_range
		{
			get => GetValue();
			set => SetValue(value);
		}
		[RouteValue("gate/attack")]
		public double gate_attack
		{
			get => GetValue();
			set => SetValue(value);
		}
		[RouteValue("gate/release")]
		public double gate_release
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
		public double eq_gain1
		{
			get => GetValue();
			set => SetValue(value);
		}
		[RouteValue("eq/eqgain2")]
		public double eq_gain2
		{
			get => GetValue();
			set => SetValue(value);
		}
		[RouteValue("eq/eqgain3")]
		public double eq_gain3
		{
			get => GetValue();
			set => SetValue(value);
		}
		[RouteValue("eq/eqgain4")]
		public double eq_gain4
		{
			get => GetValue();
			set => SetValue(value);
		}
		[RouteValue("eq/eqq1")]
		public double eq_q1
		{
			get => GetValue();
			set => SetValue(value);
		}
		[RouteValue("eq/eqq2")]
		public double eq_q2
		{
			get => GetValue();
			set => SetValue(value);
		}
		[RouteValue("eq/eqq3")]
		public double eq_q3
		{
			get => GetValue();
			set => SetValue(value);
		}
		[RouteValue("eq/eqq4")]
		public double eq_q4
		{
			get => GetValue();
			set => SetValue(value);
		}
		[RouteValue("eq/eqfreq1")]
		public double eq_freq1
		{
			get => GetValue();
			set => SetValue(value);
		}
		[RouteValue("eq/eqfreq2")]
		public double eq_freq2
		{
			get => GetValue();
			set => SetValue(value);
		}
		[RouteValue("eq/eqfreq3")]
		public double eq_freq3
		{
			get => GetValue();
			set => SetValue(value);
		}
		[RouteValue("eq/eqfreq4")]
		public double eq_freq4
		{
			get => GetValue();
			set => SetValue(value);
		}
		#endregion
		[RouteValue("limit/threshold")]
		public double limiter_threshold
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
