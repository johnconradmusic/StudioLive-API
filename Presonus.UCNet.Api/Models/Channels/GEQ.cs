using Presonus.UCNet.Api.Attributes;
using Presonus.UCNet.Api.Services;
using System.ComponentModel;

namespace Presonus.UCNet.Api.Models.Channels
{
	public class GEQ : ParameterRouter
	{
		public GEQ(int index, MixerStateService mixerStateService) : base("geq/ch", index, mixerStateService)
		{
		}

		public override event PropertyChangedEventHandler PropertyChanged;

		public float gain1 { get => GetValue(); set => SetValue(value); }

		public float gain2 { get => GetValue(); set => SetValue(value); }

		public float gain3 { get => GetValue(); set => SetValue(value); }

		public float gain4 { get => GetValue(); set => SetValue(value); }

		public float gain5 { get => GetValue(); set => SetValue(value); }

		public float gain6 { get => GetValue(); set => SetValue(value); }

		public float gain7 { get => GetValue(); set => SetValue(value); }

		public float gain8 { get => GetValue(); set => SetValue(value); }

		public float gain9 { get => GetValue(); set => SetValue(value); }

		public float gain10 { get => GetValue(); set => SetValue(value); }

		public float gain11 { get => GetValue(); set => SetValue(value); }

		public float gain12 { get => GetValue(); set => SetValue(value); }

		public float gain13 { get => GetValue(); set => SetValue(value); }

		public float gain14 { get => GetValue(); set => SetValue(value); }

		public float gain15 { get => GetValue(); set => SetValue(value); }

		public float gain16 { get => GetValue(); set => SetValue(value); }

		public float gain17 { get => GetValue(); set => SetValue(value); }

		public float gain18 { get => GetValue(); set => SetValue(value); }

		public float gain19 { get => GetValue(); set => SetValue(value); }

		public float gain20 { get => GetValue(); set => SetValue(value); }

		public float gain21 { get => GetValue(); set => SetValue(value); }

		public float gain22 { get => GetValue(); set => SetValue(value); }

		public float gain23 { get => GetValue(); set => SetValue(value); }

		public float gain24 { get => GetValue(); set => SetValue(value); }

		public float gain25 { get => GetValue(); set => SetValue(value); }

		public float gain26 { get => GetValue(); set => SetValue(value); }

		public float gain27 { get => GetValue(); set => SetValue(value); }

		public float gain28 { get => GetValue(); set => SetValue(value); }

		public float gain29 { get => GetValue(); set => SetValue(value); }

		public float gain30 { get => GetValue(); set => SetValue(value); }

		public float gain31 { get => GetValue(); set => SetValue(value); }

		public float gain32 { get => GetValue(); set => SetValue(value); }

		public bool on { get => GetBoolean(); set => SetBoolean(value); }

		public bool ston { get => GetBoolean(); set => SetBoolean(value); }

		public override void OnPropertyChanged(PropertyChangedEventArgs eventArgs)
		{
			PropertyChanged?.Invoke(this, eventArgs);
		}
	}
}