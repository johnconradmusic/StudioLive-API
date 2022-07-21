using Presonus.StudioLive32.Api.Models;
using Presonus.StudioLive32.Api.Models.Auxes;
using Presonus.UC.Api.Devices;
using Presonus.UC.Api.Models.Global;
using System.ComponentModel;

namespace Presonus.Studio1824C.Wpf
{
    public class MainViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler? PropertyChanged;

        public UC.Api.Devices.Studio1824C Device { get; }

        public MainViewModel(Presonus.UC.Api.Devices.Studio1824C device)
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
