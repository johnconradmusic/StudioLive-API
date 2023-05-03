using Presonus.UCNet.Api.Models;
using Presonus.UCNet.Api.Models.Channels;
using Presonus.UCNet.Api.Services;
using System.Collections.ObjectModel;
using System.ComponentModel;

namespace Presonus.UCNet.Api;

public class BlindViewModel : INotifyPropertyChanged
{
    private MixerStateService _mixerStateService;
    private MeterService _meterService;

    public BlindViewModel(MixerStateService mixerStateService, MeterService meterService)
    {
        _mixerStateService = mixerStateService;
        BuildMixer(mixerStateService);
    }

    public event PropertyChangedEventHandler? PropertyChanged;

    public Channel SelectedChannel { get; set; }
    public MeterData MeterData { get; set; }

    public ReductionMeterData ReductionMeterData { get; set; }

    public ObservableCollection<MicLineInput> MicLineInputs { get; } = new ObservableCollection<MicLineInput>();

    public ObservableCollection<StereoLineInput> StereoLineInputs { get; } = new ObservableCollection<StereoLineInput>();

    public ObservableCollection<InputChannel> FXReturns { get; } = new ObservableCollection<InputChannel>();

    public ObservableCollection<MicLineInput> Talkback { get; } = new ObservableCollection<MicLineInput>();

    public ObservableCollection<OutputDACBus> Main { get; } = new ObservableCollection<OutputDACBus>();

    public ObservableCollection<Channel> AllChannels { get; } = new ObservableCollection<Channel>();

    public Presets Presets { get; set; }

    public Global Global { get; set; }

    private void BuildMixer(MixerStateService mixerStateService)
    {
        Presets = new Presets(mixerStateService);
        Presets.PropertyChanged += (sender, args) => OnPropertyChanged(nameof(Presets));

        Global = new Global(mixerStateService);
        Global.PropertyChanged += (sender, args) => OnPropertyChanged(nameof(Global));

        for (int i = 0; i < Mixer.ChannelCounts[ChannelTypes.LINE]; i++)
        {
            var chan = new MicLineInput(ChannelTypes.LINE, i + 1, _mixerStateService);
            chan.PropertyChanged += (sender, args) => OnPropertyChanged(nameof(MicLineInputs));
            MicLineInputs.Add(chan);
            AllChannels.Add(chan);
        }

        for (int i = 0; i < Mixer.ChannelCounts[ChannelTypes.FXRETURN]; i++)
        {
            var chan = new InputChannel(ChannelTypes.FXRETURN, i + 1, _mixerStateService);
            chan.PropertyChanged += (sender, args) => OnPropertyChanged(nameof(FXReturns));
            FXReturns.Add(chan);
            AllChannels.Add(chan);
        }

        for (int i = 0; i < Mixer.ChannelCounts[ChannelTypes.RETURN]; i++)
        {
            var chan = new StereoLineInput(i + 1, _mixerStateService);
            chan.PropertyChanged += (sender, args) => OnPropertyChanged(nameof(StereoLineInputs));
            StereoLineInputs.Add(chan);
            AllChannels.Add(chan);
        }

        for (int i = 0; i < Mixer.ChannelCounts[ChannelTypes.TALKBACK]; i++)
        {
            var chan = new MicLineInput(ChannelTypes.TALKBACK, i + 1, _mixerStateService);
            chan.PropertyChanged += (sender, args) => OnPropertyChanged(nameof(Talkback));
            Talkback.Add(chan);
            AllChannels.Add(chan);
        }

        for (int i = 0; i < Mixer.ChannelCounts[ChannelTypes.MAIN]; i++)
        {
            var chan = new OutputDACBus(ChannelTypes.MAIN, i + 1, _mixerStateService);
            chan.PropertyChanged += (sender, args) => OnPropertyChanged(nameof(Main));
            Main.Add(chan);
            AllChannels.Add(chan);
        }
    }

    protected virtual void OnPropertyChanged(string propertyName)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        //if (propertyName == "MeterData") Console.WriteLine(MeterData.Input[0]);
    }

    public void Test()
    {
        _mixerStateService.RequestProjects();
        // _mixerStateService.RecallSceneFile("02.test.proj", "02.testing.scn");
    }
}