using Revelator.io24.Api.Attributes;
using Revelator.io24.Api.Scene;
using System;
using System.ComponentModel;

namespace Revelator.io24.Api.Models.Inputs
{
	public class LineChannel : ChannelBase
	{


		public LineChannel(string routingPrefix, RawService rawService)
			: base(routingPrefix, rawService)
		{

		}

		public bool sub1 { get => GetBoolean(); set => SetBoolean(value); }
		public bool sub2 { get => GetBoolean(); set => SetBoolean(value); }
		public bool sub3 { get => GetBoolean(); set => SetBoolean(value); }
		public bool sub4 { get => GetBoolean(); set => SetBoolean(value); }







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
		[RouteValueRange(-84, 10, Enums.Unit.db)] public float FXA { get => GetValue(); set => SetValue(value); }
		[RouteValueRange(-84, 10, Enums.Unit.db)] public float FXB { get => GetValue(); set => SetValue(value); }
		[RouteValueRange(-84, 10, Enums.Unit.db)] public float FXC { get => GetValue(); set => SetValue(value); }
		[RouteValueRange(-84, 10, Enums.Unit.db)] public float FXD { get => GetValue(); set => SetValue(value); }
		[RouteValueRange(-84, 10, Enums.Unit.db)] public float FXE { get => GetValue(); set => SetValue(value); }
		[RouteValueRange(-84, 10, Enums.Unit.db)] public float FXF { get => GetValue(); set => SetValue(value); }
		[RouteValueRange(-84, 10, Enums.Unit.db)] public float FXG { get => GetValue(); set => SetValue(value); }
		[RouteValueRange(-84, 10, Enums.Unit.db)] public float FXH { get => GetValue(); set => SetValue(value); }
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
