using Presonus.StudioLive32.Api.Models;
using Presonus.StudioLive32.Api.Models.Auxes;
using Presonus.UC.Api.Devices;
using System.ComponentModel;

namespace Presonus.StudioLive32.Wpf
{
	public class MainViewModel : INotifyPropertyChanged
	{
		public event PropertyChangedEventHandler? PropertyChanged;

		public Device Device { get; }
		public ChannelBase SelectedChannel { get; set; }

		public MainViewModel(StudioLive32R device)
		{
			Device = device;
			device.PropertyChanged += (sender, args) => OnPropertyChanged(nameof(Device));
		}

		public MainViewModel() { }

		public void OnPropertyChanged(string? name = null)
		{
			PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
		}
	}
}
