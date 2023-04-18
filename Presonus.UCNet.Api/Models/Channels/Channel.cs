using Presonus.UCNet.Api.Services;
using System.ComponentModel;

namespace Presonus.UCNet.Api.Models
{
	public class Channel : ParameterRouter, INotifyPropertyChanged
	{

		public Channel(ChannelTypes channelType, int index, MixerStateService mixerStateService) : base(channelType, index, mixerStateService)
		{		}

		public override event PropertyChangedEventHandler PropertyChanged;

		public bool clip { get => GetBoolean(); set => SetBoolean(value); }

		public bool linkable => !(_channelIndex % 2 == 0);

		public string chnum { get => GetString(); set => SetString(value); }

		public string name { get => GetString(); set => SetString(value); }

		public string username { get => GetString(); set => SetString(value); }

		public string color { get => GetString(); set => SetString(value); }

		public bool select { get => GetBoolean(); set => SetBoolean(value); }

		public bool solo { get => GetBoolean(); set => SetBoolean(value); }

		public float volume { get => GetValue(); set => SetValue(value); }

		public bool mute { get => GetBoolean(); set => SetBoolean(value); }

		public float pan { get => GetValue(); set => SetValue(value); }

		public float stereopan { get => GetValue(); set => SetValue(value); }

		public bool panlinkstate { get => GetBoolean(); set => SetBoolean(value); }

		public bool link
		{ get { return GetBoolean(); } set { SetBoolean(value); OnPropertyChanged(new("linkslave")); } }

		public bool linkslave => link && !linkmaster;

		public bool linkmaster { get => GetBoolean(); set => SetBoolean(value); }

		public string iconid { get => GetString(); set => SetString(value); }

		public float meterpeak { get => GetValue(); set => SetValue(value); }

		public float meter2 { get => GetValue(); set => SetValue(value); }

		public float meter2peak { get => GetValue(); set => SetValue(value); }

		public bool rta_active { get => GetBoolean(); set => SetBoolean(value); }

		public bool rta_pre { get => GetBoolean(); set => SetBoolean(value); }

		public override void OnPropertyChanged(PropertyChangedEventArgs eventArgs)
		{
			PropertyChanged?.Invoke(this, eventArgs);
		}
	}
}