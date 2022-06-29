using Presonus.StudioLive32.Api.Attributes;
using System.ComponentModel;

namespace Presonus.StudioLive32.Api.Models.Effects
{
    public class ReverbEffects : DeviceRoutingBase, INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        protected override void OnPropertyChanged(PropertyChangedEventArgs eventArgs)
            => PropertyChanged?.Invoke(this, eventArgs);

        public ReverbEffects(string routePrefix, RawService rawService)
            : base(routePrefix, rawService)
        {
            //
        }

        [RouteValue("size")]
        public int Size
        {
            get => GetVolume();
            set => SetVolume(value);
        }

        [RouteValue("hp_freq")]
        public int HighPassFreq
        {
            get => GetVolume();
            set => SetVolume(value);
        }

        [RouteValue("predelay")]
        public int PreDelay
        {
            get => GetVolume();
            set => SetVolume(value);
        }
    }
}
