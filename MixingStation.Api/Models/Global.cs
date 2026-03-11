using MixingStation.Api.Attributes;
using MixingStation.Api.Services;
using System.ComponentModel;

namespace MixingStation.Api.Models
{
	public class Global : ParameterRouter, INotifyPropertyChanged
	{
		public Global(MixerStateService mixerStateService) : base("global", -1, mixerStateService)
		{
		}

		public override event PropertyChangedEventHandler PropertyChanged;

		public string devicename { get => GetString(); set => SetString(value); }

		public override void OnPropertyChanged(PropertyChangedEventArgs eventArgs)
		{
			PropertyChanged?.Invoke(this, eventArgs);
		}
	}
}