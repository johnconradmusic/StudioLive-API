using Presonus.StudioLive32.Api.Services;
using Presonus.UCNet.Api.Models;
using Presonus.UCNet.Api.Services;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Threading.Tasks;

namespace Presonus.UCNet.Api;
public class MainViewModel : INotifyPropertyChanged
{
	private MixerStateService _mixerStateService;

	public event PropertyChangedEventHandler PropertyChanged;

	protected virtual void OnPropertyChanged(string propertyName)
	{
		PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
	}

	public MainViewModel(MixerStateService mixerStateService)
	{
		_mixerStateService = mixerStateService;

		channel1 = new(NewDataModel.ChannelTypes.LINE, 1, mixerStateService);
	}

	public CompleteChannel channel1 { get; set; }


}
