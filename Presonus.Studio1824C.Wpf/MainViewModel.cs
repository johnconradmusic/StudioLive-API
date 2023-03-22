using Presonus.StudioLive32.Api.Models;
using Presonus.StudioLive32.Api.Models.Auxes;
using Presonus.UCNet.Api.Devices;
using Presonus.UCNet.Api.Models.Global;
using System.ComponentModel;

namespace Presonus.Studio1824C.Wpf
{
    public class MainViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler? PropertyChanged;

        public UCNet.Api.Devices.Studio1824C Device { get; }

        public MainViewModel(Presonus.UCNet.Api.Devices.Studio1824C device)
        {
            System.Console.WriteLine("created viewmodel");
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
