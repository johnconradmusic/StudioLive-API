using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Revelator.io24.Api.Scene
{
    //public class Altmem
    //{
    //    public Opt opt { get; set; }
    //    public Filter filter { get; set; }
    //    public Gate gate { get; set; }
    //    public Comp comp { get; set; }
    //    public Eq eq { get; set; }
    //    public Limit limit { get; set; }
    //    public Linkoptions linkoptions { get; set; }
    //    public Dca dca { get; set; }
    //}

  
    //public class Comp
    //{
    //    public int on { get; set; }
    //    public int softknee { get; set; }
    //    public int automode { get; set; }
    //    public double threshold { get; set; }
    //    public double ratio { get; set; }
    //    public double attack { get; set; }
    //    public double release { get; set; }
    //    public double gain { get; set; }
    //    public double keyfilter { get; set; }
    //    public int keylisten { get; set; }
    //    public string __classid { get; set; }
    //}

    //public class Dca
    //{
    //    public double volume { get; set; }
    //    public double aux1 { get; set; }
    //    public double aux2 { get; set; }
    //    public double aux3 { get; set; }
    //    public double aux4 { get; set; }
    //    public double aux5 { get; set; }
    //    public double aux6 { get; set; }
    //    public double aux7 { get; set; }
    //    public double aux8 { get; set; }
    //    public double aux9 { get; set; }
    //    public double aux10 { get; set; }
    //    public double aux11 { get; set; }
    //    public double aux12 { get; set; }
    //    public double aux13 { get; set; }
    //    public double aux14 { get; set; }
    //    public double aux15 { get; set; }
    //    public double aux16 { get; set; }
    //    public double aux17 { get; set; }
    //    public double aux18 { get; set; }
    //    public double aux19 { get; set; }
    //    public double aux20 { get; set; }
    //    public double aux21 { get; set; }
    //    public double aux22 { get; set; }
    //    public double aux23 { get; set; }
    //    public double aux24 { get; set; }
    //    public double aux25 { get; set; }
    //    public double aux26 { get; set; }
    //    public double aux27 { get; set; }
    //    public double aux28 { get; set; }
    //    public double aux29 { get; set; }
    //    public double aux30 { get; set; }
    //    public double aux31 { get; set; }
    //    public double aux32 { get; set; }
    //    public double fx1 { get; set; }
    //    public double fx2 { get; set; }
    //    public double fx3 { get; set; }
    //    public double fx4 { get; set; }
    //    public double fx5 { get; set; }
    //    public double fx6 { get; set; }
    //    public double fx7 { get; set; }
    //    public double fx8 { get; set; }
    //}

    //public class Eq
    //{
    //    public int eqallon { get; set; }
    //    public double eqgain1 { get; set; }
    //    public double eqq1 { get; set; }
    //    public double eqfreq1 { get; set; }
    //    public int eqbandon1 { get; set; }
    //    public int eqbandop1 { get; set; }
    //    public double eqgain2 { get; set; }
    //    public double eqq2 { get; set; }
    //    public double eqfreq2 { get; set; }
    //    public int eqbandon2 { get; set; }
    //    public double eqgain3 { get; set; }
    //    public double eqq3 { get; set; }
    //    public double eqfreq3 { get; set; }
    //    public int eqbandon3 { get; set; }
    //    public double eqgain4 { get; set; }
    //    public double eqq4 { get; set; }
    //    public double eqfreq4 { get; set; }
    //    public int eqbandon4 { get; set; }
    //    public int eqbandop4 { get; set; }
    //    public string __classid { get; set; }
    //    public double eqgain5 { get; set; }
    //    public double eqq5 { get; set; }
    //    public double eqfreq5 { get; set; }
    //    public int eqbandon5 { get; set; }
    //    public double eqgain6 { get; set; }
    //    public double eqq6 { get; set; }
    //    public double eqfreq6 { get; set; }
    //    public int eqbandon6 { get; set; }
    //    public int eqbandop6 { get; set; }
    //}

    //public class Filter
    //{
    //    public double hpf { get; set; }
    //}

    //public class Fxbus
    //{
    //    public Ch1 ch1 { get; set; }
    //    public Ch2 ch2 { get; set; }
    //    public Ch3 ch3 { get; set; }
    //    public Ch4 ch4 { get; set; }
    //}

    //public class Fxreturn
    //{
    //    public Ch1 ch1 { get; set; }
    //    public Ch2 ch2 { get; set; }
    //    public Ch3 ch3 { get; set; }
    //    public Ch4 ch4 { get; set; }
    //}

    //public class Gate
    //{
    //    public int on { get; set; }
    //    public int keylisten { get; set; }
    //    public int expander { get; set; }
    //    public double keyfilter { get; set; }
    //    public double threshold { get; set; }
    //    public double range { get; set; }
    //    public double attack { get; set; }
    //    public double release { get; set; }
    //}

    //public class Limit
    //{
    //    public int limiteron { get; set; }
    //    public double threshold { get; set; }
    //}

    //public class Line
    //{
    //    public Ch1 ch1 { get; set; }
    //    public Ch2 ch2 { get; set; }
    //    public Ch3 ch3 { get; set; }
    //    public Ch4 ch4 { get; set; }
    //    public Ch5 ch5 { get; set; }
    //    public Ch6 ch6 { get; set; }
    //    public Ch7 ch7 { get; set; }
    //    public Ch8 ch8 { get; set; }
    //    public Ch9 ch9 { get; set; }
    //    public Ch10 ch10 { get; set; }
    //    public Ch11 ch11 { get; set; }
    //    public Ch12 ch12 { get; set; }
    //    public Ch13 ch13 { get; set; }
    //    public Ch14 ch14 { get; set; }
    //    public Ch15 ch15 { get; set; }
    //    public Ch16 ch16 { get; set; }
    //    public Ch17 ch17 { get; set; }
    //    public Ch18 ch18 { get; set; }
    //    public Ch19 ch19 { get; set; }
    //    public Ch20 ch20 { get; set; }
    //    public Ch21 ch21 { get; set; }
    //    public Ch22 ch22 { get; set; }
    //    public Ch23 ch23 { get; set; }
    //    public Ch24 ch24 { get; set; }
    //    public Ch25 ch25 { get; set; }
    //    public Ch26 ch26 { get; set; }
    //    public Ch27 ch27 { get; set; }
    //    public Ch28 ch28 { get; set; }
    //    public Ch29 ch29 { get; set; }
    //    public Ch30 ch30 { get; set; }
    //    public Ch31 ch31 { get; set; }
    //    public Ch32 ch32 { get; set; }
    //}

    //public class Linkoptions
    //{
    //    public int ch_gain { get; set; }
    //    public int pan { get; set; }
    //    public int fader { get; set; }
    //    public int dyn { get; set; }
    //    public int ch_name { get; set; }
    //    public int ins_fx { get; set; }
    //}

    //public class Main
    //{
    //    public Ch1 ch1 { get; set; }
    //}

    //public class Mutegroup
    //{
    //    public int allon { get; set; }
    //    public int mutegroup1 { get; set; }
    //    public int mutegroup2 { get; set; }
    //    public int mutegroup3 { get; set; }
    //    public int mutegroup4 { get; set; }
    //    public int mutegroup5 { get; set; }
    //    public int mutegroup6 { get; set; }
    //    public int mutegroup7 { get; set; }
    //    public int mutegroup8 { get; set; }
    //    public string mutegroup1username { get; set; }
    //    public string mutegroup2username { get; set; }
    //    public string mutegroup3username { get; set; }
    //    public string mutegroup4username { get; set; }
    //    public string mutegroup5username { get; set; }
    //    public string mutegroup6username { get; set; }
    //    public string mutegroup7username { get; set; }
    //    public string mutegroup8username { get; set; }
    //    public string mutegroup1mutes { get; set; }
    //    public string mutegroup2mutes { get; set; }
    //    public string mutegroup3mutes { get; set; }
    //    public string mutegroup4mutes { get; set; }
    //    public string mutegroup5mutes { get; set; }
    //    public string mutegroup6mutes { get; set; }
    //    public string mutegroup7mutes { get; set; }
    //    public string mutegroup8mutes { get; set; }
    //    public string safe_mutes { get; set; }
    //}

    //public class Opt
    //{
    //    public int swapcompeq { get; set; }
    //}

    //public class Plugin
    //{
    //    public int type { get; set; }
    //    public double predelay { get; set; }
    //    public double reflection { get; set; }
    //    public double size { get; set; }
    //    public double lpf { get; set; }
    //    public double lfdamp_freq { get; set; }
    //    public double lfdamp_gain { get; set; }
    //    public string __classid { get; set; }
    //    public double delay_l { get; set; }
    //    public double delay_r { get; set; }
    //    public double fb_l { get; set; }
    //    public double fb_r { get; set; }
    //    public double spread { get; set; }
    //    public double hpf { get; set; }
    //    public double fb_l_lpf { get; set; }
    //    public double fb_l_hpf { get; set; }
    //    public double fb_r_lpf { get; set; }
    //    public double fb_r_hpf { get; set; }
    //}

    //public class Return
    //{
    //    public Ch1 ch1 { get; set; }
    //    public Ch2 ch2 { get; set; }
    //    public Ch3 ch3 { get; set; }
    //}

    //public class Root
    //{
    //    public Mutegroup mutegroup { get; set; }
    //    public Line line { get; set; }
    //    public Return @return { get; set; }
    //    public Talkback talkback { get; set; }
    //    public Signalgen signalgen { get; set; }
    //    public Aux aux { get; set; }
    //    public Sub sub { get; set; }
    //    public Fxbus fxbus { get; set; }
    //    public Fxreturn fxreturn { get; set; }
    //    public Main main { get; set; }
    //    public Filtergroup filtergroup { get; set; }
    //    public Autofiltergroup autofiltergroup { get; set; }
    //    public Fx fx { get; set; }
    //}

    //public class Signalgen
    //{
    //    public int type { get; set; }
    //    public double freq { get; set; }
    //    public double level { get; set; }
    //}

    //public class Sub
    //{
    //    public Ch1 ch1 { get; set; }
    //    public Ch2 ch2 { get; set; }
    //    public Ch3 ch3 { get; set; }
    //    public Ch4 ch4 { get; set; }
    //}

    //public class Talkback
    //{
    //    public Ch1 ch1 { get; set; }
    //}

}
