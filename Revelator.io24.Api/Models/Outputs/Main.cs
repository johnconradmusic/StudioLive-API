using Revelator.io24.Api.Attributes;
using System.ComponentModel;

namespace Revelator.io24.Api.Models.Outputs
{
    public class Main : DeviceRoutingBase
    {
        public event PropertyChangedEventHandler PropertyChanged;

        protected override void OnPropertyChanged(PropertyChangedEventArgs eventArgs)
            => PropertyChanged?.Invoke(this, eventArgs);
        public Main(RawService rawService)
            : base("main/ch1", rawService)
        {
            //
        }

        [RouteValue("hardwareMute")]
        public bool HardwareMute
        {
            get => GetBoolean();
            //set => SetBoolean(value);
        }
    }
}
