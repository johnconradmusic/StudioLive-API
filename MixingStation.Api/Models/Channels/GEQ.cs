using MixingStation.Api.Attributes;
using MixingStation.Api.Services;
using System.ComponentModel;

namespace MixingStation.Api.Models.Channels
{
	public class GEQ : ParameterRouter
	{
		public GEQ(int index, MixerStateService mixerStateService) : base("geq/ch", index, mixerStateService)
		{
		}


		public override event PropertyChangedEventHandler PropertyChanged;

		public float gain1 { get => GetFloat(); set => SetValue(value); }

		public float gain2 { get => GetFloat(); set => SetValue(value); }

		public float gain3 { get => GetFloat(); set => SetValue(value); }

		public float gain4 { get => GetFloat(); set => SetValue(value); }

		public float gain5 { get => GetFloat(); set => SetValue(value); }

		public float gain6 { get => GetFloat(); set => SetValue(value); }

		public float gain7 { get => GetFloat(); set => SetValue(value); }

		public float gain8 { get => GetFloat(); set => SetValue(value); }

		public float gain9 { get => GetFloat(); set => SetValue(value); }

		public float gain10 { get => GetFloat(); set => SetValue(value); }

		public float gain11 { get => GetFloat(); set => SetValue(value); }

		public float gain12 { get => GetFloat(); set => SetValue(value); }

		public float gain13 { get => GetFloat(); set => SetValue(value); }

		public float gain14 { get => GetFloat(); set => SetValue(value); }

		public float gain15 { get => GetFloat(); set => SetValue(value); }

		public float gain16 { get => GetFloat(); set => SetValue(value); }

		public float gain17 { get => GetFloat(); set => SetValue(value); }

		public float gain18 { get => GetFloat(); set => SetValue(value); }

		public float gain19 { get => GetFloat(); set => SetValue(value); }

		public float gain20 { get => GetFloat(); set => SetValue(value); }

		public float gain21 { get => GetFloat(); set => SetValue(value); }

		public float gain22 { get => GetFloat(); set => SetValue(value); }

		public float gain23 { get => GetFloat(); set => SetValue(value); }

		public float gain24 { get => GetFloat(); set => SetValue(value); }

		public float gain25 { get => GetFloat(); set => SetValue(value); }

		public float gain26 { get => GetFloat(); set => SetValue(value); }

		public float gain27 { get => GetFloat(); set => SetValue(value); }

		public float gain28 { get => GetFloat(); set => SetValue(value); }

		public float gain29 { get => GetFloat(); set => SetValue(value); }

		public float gain30 { get => GetFloat(); set => SetValue(value); }

		public float gain31 { get => GetFloat(); set => SetValue(value); }

		public float gain32 { get => GetFloat(); set => SetValue(value); }

		public bool on { get => GetBoolean(); set => SetBoolean(value); }

		public bool ston { get => GetBoolean(); set => SetBoolean(value); }

		public override void OnPropertyChanged(PropertyChangedEventArgs eventArgs)
		{
			PropertyChanged?.Invoke(this, eventArgs);
		}
	}
}