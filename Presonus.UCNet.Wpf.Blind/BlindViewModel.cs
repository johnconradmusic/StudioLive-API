using Presonus.UCNet.Api.Models;
using Presonus.UCNet.Api.Models.Channels;
using Presonus.UCNet.Api.Services;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Threading.Tasks;

namespace Presonus.UCNet.Api;

public class BlindViewModel : INotifyPropertyChanged
{
	private MixerStateService mixerStateService;
	private MeterService _meterService;

	public BlindViewModel(MixerStateService mixerStateService, MeterService meterService)
	{
		this.mixerStateService = mixerStateService;
		BuildMixer();
	}

	public event PropertyChangedEventHandler? PropertyChanged;

	public Channel SelectedChannel { get; set; }
	public MeterData MeterData { get; set; }

	public ReductionMeterData ReductionMeterData { get; set; }

	public ObservableCollection<MicLineInput> MicLineInputs { get; } = new ObservableCollection<MicLineInput>();

	public ObservableCollection<StereoLineInput> StereoLineInputs { get; } = new ObservableCollection<StereoLineInput>();

	public ObservableCollection<InputChannel> FXReturns { get; } = new ObservableCollection<InputChannel>();

	public ObservableCollection<MicLineInput> Talkback { get; } = new ObservableCollection<MicLineInput>();

	public ObservableCollection<OutputDACBus> Auxes { get; } = new ObservableCollection<OutputDACBus>();

	public ObservableCollection<OutputDACBus> Main { get; } = new ObservableCollection<OutputDACBus>();

	public ObservableCollection<Channel> AllChannels { get; } = new ObservableCollection<Channel>();

	public ObservableCollection<FX> FX { get; } = new ObservableCollection<FX>();

	public ObservableCollection<GEQ> GEQ { get; } = new ObservableCollection<GEQ>();

	public Presets Presets { get; set; }

	public Global Global { get; set; }

	private void BuildMixer()
	{
		Presets = new Presets(mixerStateService);
		Presets.PropertyChanged += (sender, args) => OnPropertyChanged(nameof(Presets));

		Global = new Global(mixerStateService);
		Global.PropertyChanged += (sender, args) => OnPropertyChanged(nameof(Global));

		for (int i = 0; i < Mixer.ChannelCounts[ChannelTypes.LINE]; i++)
		{
			var chan = new MicLineInput(ChannelTypes.LINE, i + 1, mixerStateService);
			chan.PropertyChanged += (sender, args) => OnPropertyChanged(nameof(MicLineInputs));
			MicLineInputs.Add(chan);
			AllChannels.Add(chan);
		}

		for (int i = 0; i < Mixer.ChannelCounts[ChannelTypes.FXRETURN]; i++)
		{
			var chan = new InputChannel(ChannelTypes.FXRETURN, i + 1, mixerStateService);
			chan.PropertyChanged += (sender, args) => OnPropertyChanged(nameof(FXReturns));
			FXReturns.Add(chan);
			AllChannels.Add(chan);
		}

		for (int i = 0; i < Mixer.ChannelCounts[ChannelTypes.RETURN]; i++)
		{
			var chan = new StereoLineInput(i + 1, mixerStateService);
			chan.PropertyChanged += (sender, args) => OnPropertyChanged(nameof(StereoLineInputs));
			StereoLineInputs.Add(chan);
			AllChannels.Add(chan);
		}

		for (int i = 0; i < Mixer.ChannelCounts[ChannelTypes.TALKBACK]; i++)
		{
			var chan = new MicLineInput(ChannelTypes.TALKBACK, i + 1, mixerStateService);
			chan.PropertyChanged += (sender, args) => OnPropertyChanged(nameof(Talkback));
			Talkback.Add(chan);
			AllChannels.Add(chan);
		}
		for (int i = 0; i < Mixer.ChannelCounts[ChannelTypes.FX]; i++)
		{
			var fx = new FX(i + 1, mixerStateService);
			fx.PropertyChanged += (sender, args) => OnPropertyChanged(nameof(FX));
			FX.Add(fx);
		}
		for (int i = 0; i < Mixer.ChannelCounts[ChannelTypes.AUX]; i++)
		{
			var chan = new OutputDACBus(ChannelTypes.AUX, i + 1, mixerStateService);
			chan.PropertyChanged += (sender, args) => OnPropertyChanged(nameof(Auxes));
			Auxes.Add(chan);
			AllChannels.Add(chan);
		}
		for (int i = 0; i < Mixer.ChannelCounts[ChannelTypes.MAIN]; i++)
		{
			var chan = new OutputDACBus(ChannelTypes.MAIN, i + 1, mixerStateService);
			chan.PropertyChanged += (sender, args) => OnPropertyChanged(nameof(Main));
			Main.Add(chan);
			AllChannels.Add(chan);
		}
		for (int i = 0; i < Mixer.ChannelCounts[ChannelTypes.GEQ]; i++)
		{
			var geq = new GEQ(i + 1, mixerStateService);
			geq.PropertyChanged += (sender, args) => OnPropertyChanged(nameof(GEQ));
			GEQ.Add(geq);
		}
	}

	protected virtual void OnPropertyChanged(string propertyName)
	{
		PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
	}
}