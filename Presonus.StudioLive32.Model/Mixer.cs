using Presonus.StudioLive32.Api;
using Presonus.StudioLive32.Api.Models;
using System.ComponentModel;

namespace Presonus.StudioLive32.Model
{
	public class Mixer : DeviceRoutingBase
	{
		public Mixer(string routePrefix, RawService rawService) : base(routePrefix, rawService)
		{

		}

		public event PropertyChangedEventHandler PropertyChanged;
		protected override void OnPropertyChanged(PropertyChangedEventArgs eventArgs) { PropertyChanged?.Invoke(this, eventArgs); }
	}
}