using Presonus.StudioLive32.Api.Services;
using Presonus.UCNet.Api.Models;
using Presonus.UCNet.Api.Models.Channels;
using Presonus.UCNet.Api.NewDataModel;
using Presonus.UCNet.Api.Services;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Threading.Tasks;

namespace Presonus.UCNet.Api;
public class MainViewModel : INotifyPropertyChanged
{
	private MixerStateService _mixerStateService;

	public event PropertyChangedEventHandler? PropertyChanged;

	protected virtual void OnPropertyChanged(string propertyName)
	{
		PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
	}

	public ObservableCollection<MicLineInput> MicLineInputs { get; } = new ObservableCollection<MicLineInput>();
	public ObservableCollection<StereoLineInput> StereoLineInputs { get; } = new ObservableCollection<StereoLineInput>();
	public ObservableCollection<InputChannel> FXReturns { get; } = new ObservableCollection<InputChannel>();
	public ObservableCollection<MicLineInput> Talkback { get; } = new ObservableCollection<MicLineInput>();
	public ObservableCollection<OutputDACBus> Main { get; } = new ObservableCollection<OutputDACBus>();


	public MainViewModel(MixerStateService mixerStateService)
	{
		_mixerStateService = mixerStateService;

		for (int i = 0; i < Mixer.ChannelCounts[ChannelTypes.LINE.ToString()]; i++)
		{
			var chan = new MicLineInput(ChannelTypes.LINE,i + 1, _mixerStateService);
			chan.PropertyChanged += (sender, args) => OnPropertyChanged(nameof(MicLineInputs));
			MicLineInputs.Add(chan);
		}

		for (int i = 0; i < Mixer.ChannelCounts[ChannelTypes.FXRETURN.ToString()]; i++)
		{
			var chan = new InputChannel(ChannelTypes.FXRETURN, i + 1, _mixerStateService);
			chan.PropertyChanged += (sender, args) => OnPropertyChanged(nameof(FXReturns));
			FXReturns.Add(chan);
		}

		for (int i = 0; i < Mixer.ChannelCounts[ChannelTypes.RETURN.ToString()]; i++)
		{
			var chan = new StereoLineInput(i + 1, _mixerStateService);
			chan.PropertyChanged += (sender, args) => OnPropertyChanged(nameof(StereoLineInputs));
			StereoLineInputs.Add(chan);
		}
		for (int i = 0; i < Mixer.ChannelCounts[ChannelTypes.TALKBACK.ToString()]; i++)
		{
			var chan = new MicLineInput(ChannelTypes.TALKBACK, i + 1, _mixerStateService);
			chan.PropertyChanged += (sender, args) => OnPropertyChanged(nameof(Talkback));
			Talkback.Add(chan);
		}
		for (int i = 0; i < Mixer.ChannelCounts[ChannelTypes.MAIN.ToString()]; i++)
		{
			var chan = new OutputDACBus(ChannelTypes.MAIN, i + 1, _mixerStateService);
			chan.PropertyChanged += (sender, args) => OnPropertyChanged(nameof(Main));
			Main.Add(chan);
		}
	}
}
