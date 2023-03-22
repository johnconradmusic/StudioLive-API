using Presonus.StudioLive32.Api.Attributes;
using Presonus.UCNet.Api.Services;
using System.ComponentModel;

namespace Presonus.StudioLive32.Api.Models.Outputs
{
    public class Main : DeviceRoutingBase
    {
        public override event PropertyChangedEventHandler PropertyChanged;

        protected override void OnPropertyChanged(PropertyChangedEventArgs eventArgs)
            => PropertyChanged?.Invoke(this, eventArgs);
        public Main(MixerStateService rawService)
            : base("main/ch1", rawService)
        {
            //
        }
    }
}
