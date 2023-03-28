using Presonus.StudioLive32.Api.Services;
using Presonus.UCNet.Api.Models;
using Presonus.UCNet.Api.Models.Channels;
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

	public ObservableCollection<MicLineInput> Channels { get; } = new ObservableCollection<MicLineInput>();

	public MainViewModel(MixerStateService mixerStateService)
	{
		_mixerStateService = mixerStateService;

		for (int i = 0; i < 16; i++)
		{
			var chan = new MicLineInput(NewDataModel.ChannelTypes.LINE, i + 1, _mixerStateService);
			chan.PropertyChanged += (sender, args) => OnPropertyChanged(nameof(Channels));
			Channels.Add(chan);
		}
	}
}
