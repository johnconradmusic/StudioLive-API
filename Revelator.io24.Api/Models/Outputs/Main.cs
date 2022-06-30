using Presonus.StudioLive32.Api.Attributes;
using System.ComponentModel;

namespace Presonus.StudioLive32.Api.Models.Outputs
{
    public class Main : DeviceRoutingBase
    {
        public override event PropertyChangedEventHandler PropertyChanged;

        protected override void OnPropertyChanged(PropertyChangedEventArgs eventArgs)
            => PropertyChanged?.Invoke(this, eventArgs);
        public Main(RawService rawService)
            : base("main/ch1", rawService)
        {
            //
        }


    }
}
