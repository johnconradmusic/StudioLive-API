using Presonus.UCNet.Api.Services;
using System.ComponentModel;
using System.Windows;

namespace Presonus.UCNet.Api.Models
{
	public class Channel : ParameterRouter, INotifyPropertyChanged
	{
		
		public Channel(ChannelTypes channelType, int index, MixerStateService mixerStateService) : base(channelType, index, mixerStateService)
		{
		}

		protected MeterData _meterData;

		public override event PropertyChangedEventHandler PropertyChanged;

		public bool clipProtection { get; set; } = true;
		public bool clip { get => GetBoolean(); set => SetBoolean(value); }

		public bool linkable => !(ChannelIndex % 2 == 0);

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
		{ get { return GetBoolean(); } set { SetBoolean(value); OnPropertyChanged(new PropertyChangedEventArgs(nameof(linked_visibility))); } }

		public Visibility linked_visibility
		{
			get
			{
				if (linkmaster) return Visibility.Visible;
				if (link && !linkmaster) return Visibility.Collapsed;
				else return Visibility.Visible;
			}
		}
		public bool linkmaster
		{ get { return GetBoolean(); } set { SetBoolean(value); OnPropertyChanged(new PropertyChangedEventArgs(nameof(linked_visibility))); } }

		public bool linkslave => link && !linkmaster;

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