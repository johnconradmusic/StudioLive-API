using Presonus.UCNet.Api.Attributes;
using Presonus.UCNet.Api.Services;
using System.Collections.Generic;
using System.ComponentModel;

namespace Presonus.UCNet.Api.Models.Channels
{
    public class FX : OutputBus
    {
        public static Dictionary<string, string> FXNames = new Dictionary<string, string>()
        {
            {FXClassIDs.DigitalXLReverb, "Digital XL Reverb"},
            {FXClassIDs.PAE16Reverb, "PAE-16 Reverb"},
            {FXClassIDs.Digital335Reverb, "335 Digital Reverb"},
            {FXClassIDs.VintagePlateReverb, "Vintage Plate Reverb"},
            {FXClassIDs.MonoDelay, "Mono Delay"},
            {FXClassIDs.StereoDelay, "Stereo Delay"},
            {FXClassIDs.PingPongDelay, "Ping Pong Delay"},
            {FXClassIDs.Chorus, "Chorus"},
            {FXClassIDs.Flanger, "Flanger"}
        };

        public static List<string> PAE16TypeList { get; } = new List<string>()
    {
        "Rock Snare Hall",
        "Studio Live Room",
        "Large Guitar Hall",
        "Drum AMS 1",
        "Vocal Double",
        "Small Hall",
        "Darker Room",
        "Slap Room"
    };

        public static List<string> PAE335TypeList { get; } = new List<string>()
    {
        "Concert Hall",
        "Drum Large Hall",
        "Drum Small Hall 1",
        "Large Church",
        "Small Room",
        "Lively Studio",
        "Parking Garage",
        "Drum Tight Room"
    };

        public static List<string> VintageReverbTypeList { get; } = new List<string>()
    {
        "Med Vocal Plate",
        "Short Vocal Plate",
        "Long Vocal Plate",
        "Early Reflection 1",
        "Gtr Ambient Sm",
        "Gtr Ambient Md",
        "Gtr Ambient Lg"
    };

        public static List<string> DigitalXLReverbTypeList { get; } = new List<string>()
    {
        "Van",
        "Guitar Closet",
        "Tight Room",
        "Tiny Room",
        "Drum Small Room 1",
        "Large Room",
        "Long Small Room",
        "Drum Garage"
    };
        public FX(int index, MixerStateService mixerStateService) : base(ChannelTypes.FX, index, mixerStateService)
        {
        }

        public override event PropertyChangedEventHandler PropertyChanged;

        public float type { get => GetValue(); set => SetValue(value); }

        [ParameterPath("plugin/classId")]
        public string classId { get => GetString(); set => SetString(value); }

        public string Name => classId != null ? FXNames[classId] : "Unknown";

        [ParameterPath("plugin/delay")]
        public float delay { get => GetValue(); set => SetValue(value); }

        [ParameterPath("plugin/feedback")]
        public float feedback { get => GetValue(); set => SetValue(value); }

        [ParameterPath("plugin/lpf")]
        public float lpf { get => GetValue(); set => SetValue(value); }

        [ParameterPath("plugin/hpf")]
        public float hpf { get => GetValue(); set => SetValue(value); }

        [ParameterPath("plugin/fb_lpf")]
        public float fb_lpf { get => GetValue(); set => SetValue(value); }

        [ParameterPath("plugin/fb_hpf")]
        public float fb_hpf { get => GetValue(); set => SetValue(value); }

        [ParameterPath("plugin/delay_l")]
        public float delay_l { get => GetValue(); set => SetValue(value); }

        [ParameterPath("plugin/delay_r")]
        public float delay_r { get => GetValue(); set => SetValue(value); }

        [ParameterPath("plugin/fb_l")]
        public float fb_l { get => GetValue(); set => SetValue(value); }

        [ParameterPath("plugin/fb_r")]
        public float fb_r { get => GetValue(); set => SetValue(value); }

        [ParameterPath("plugin/spread")]
        public float spread { get => GetValue(); set => SetValue(value); }

        [ParameterPath("plugin/fb_l_lpf")]
        public float fb_l_lpf { get => GetValue(); set => SetValue(value); }

        [ParameterPath("plugin/fb_l_hpf")]
        public float fb_l_hpf { get => GetValue(); set => SetValue(value); }

        [ParameterPath("plugin/fb_r_lpf")]
        public float fb_r_lpf { get => GetValue(); set => SetValue(value); }

        [ParameterPath("plugin/fb_r_hpf")]
        public float fb_r_hpf { get => GetValue(); set => SetValue(value); }

        [ParameterPath("plugin/dly_offset_l")]
        public float dly_offset_l { get => GetValue(); set => SetValue(value); }

        [ParameterPath("plugin/dly_offset_r")]
        public float dly_offset_r { get => GetValue(); set => SetValue(value); }

        [ParameterPath("plugin/reflection")]
        public float reflection { get => GetValue(); set => SetValue(value); }

        [ParameterPath("plugin/diffusion")]
        public float diffusion { get => GetValue(); set => SetValue(value); }

        [ParameterPath("plugin/size")]
        public float size { get => GetValue(); set => SetValue(value); }

        [ParameterPath("plugin/ratio")]
        public float ratio { get => GetValue(); set => SetValue(value); }

        [ParameterPath("plugin/lfdamp_freq")]
        public float lfdamp_freq { get => GetValue(); set => SetValue(value); }

        [ParameterPath("plugin/lfdamp_gain")]
        public float lfdamp_gain { get => GetValue(); set => SetValue(value); }

        [ParameterPath("plugin/hfdamp_freq")]
        public float hfdamp_freq { get => GetValue(); set => SetValue(value); }

        [ParameterPath("plugin/hfdamp_gain")]
        public float hfdamp_gain { get => GetValue(); set => SetValue(value); }

        [ParameterPath("plugin/predelay")]
        public float predelay { get => GetValue(); set => SetValue(value); }

        [ParameterPath("plugin/rate")]
        public float rate { get => GetValue(); set => SetValue(value); }

        [ParameterPath("plugin/depth")]
        public float depth { get => GetValue(); set => SetValue(value); }

        [ParameterPath("plugin/offset")]
        public float offset { get => GetValue(); set => SetValue(value); }

        [ParameterPath("plugin/width")]
        public float width { get => GetValue(); set => SetValue(value); }

        [ParameterPath("plugin/shape")]
        public float shape { get => GetValue(); set => SetValue(value); }

        [ParameterPath("plugin/polarity")]
        public bool polarity { get => GetBoolean(); set => SetBoolean(value); }

        [ParameterPath("plugin/range")]
        public float range { get => GetValue(); set => SetValue(value); }

        [ParameterPath("plugin/type")]
        public float verbType { get => GetValue(); set => SetValue(value); }

        public override void OnPropertyChanged(PropertyChangedEventArgs eventArgs)
        {
            PropertyChanged?.Invoke(this, eventArgs);
        }
    }
}