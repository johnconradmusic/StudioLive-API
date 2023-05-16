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

		[ParameterPath("geq/gain1")]
		public float gain1 { get => GetValue(); set => SetValue(value); }

		[ParameterPath("geq/gain2")]
		public float gain2 { get => GetValue(); set => SetValue(value); }

		[ParameterPath("geq/gain3")]
		public float gain3 { get => GetValue(); set => SetValue(value); }

		[ParameterPath("geq/gain4")]
		public float gain4 { get => GetValue(); set => SetValue(value); }

		[ParameterPath("geq/gain5")]
		public float gain5 { get => GetValue(); set => SetValue(value); }

		[ParameterPath("geq/gain6")]
		public float gain6 { get => GetValue(); set => SetValue(value); }

		[ParameterPath("geq/gain7")]
		public float gain7 { get => GetValue(); set => SetValue(value); }

		[ParameterPath("geq/gain8")]
		public float gain8 { get => GetValue(); set => SetValue(value); }

		[ParameterPath("geq/gain9")]
		public float gain9 { get => GetValue(); set => SetValue(value); }

		[ParameterPath("geq/gain10")]
		public float gain10 { get => GetValue(); set => SetValue(value); }

		[ParameterPath("geq/gain11")]
		public float gain11 { get => GetValue(); set => SetValue(value); }

		[ParameterPath("geq/gain12")]
		public float gain12 { get => GetValue(); set => SetValue(value); }

		[ParameterPath("geq/gain13")]
		public float gain13 { get => GetValue(); set => SetValue(value); }

		[ParameterPath("geq/gain14")]
		public float gain14 { get => GetValue(); set => SetValue(value); }

		[ParameterPath("geq/gain15")]
		public float gain15 { get => GetValue(); set => SetValue(value); }

		[ParameterPath("geq/gain16")]
		public float gain16 { get => GetValue(); set => SetValue(value); }

		[ParameterPath("geq/gain17")]
		public float gain17 { get => GetValue(); set => SetValue(value); }

		[ParameterPath("geq/gain18")]
		public float gain18 { get => GetValue(); set => SetValue(value); }

		[ParameterPath("geq/gain19")]
		public float gain19 { get => GetValue(); set => SetValue(value); }

		[ParameterPath("geq/gain20")]
		public float gain20 { get => GetValue(); set => SetValue(value); }

		[ParameterPath("geq/gain21")]
		public float gain21 { get => GetValue(); set => SetValue(value); }

		[ParameterPath("geq/gain22")]
		public float gain22 { get => GetValue(); set => SetValue(value); }

		[ParameterPath("geq/gain23")]
		public float gain23 { get => GetValue(); set => SetValue(value); }

		[ParameterPath("geq/gain24")]
		public float gain24 { get => GetValue(); set => SetValue(value); }

		[ParameterPath("geq/gain25")]
		public float gain25 { get => GetValue(); set => SetValue(value); }

		[ParameterPath("geq/gain26")]
		public float gain26 { get => GetValue(); set => SetValue(value); }

		[ParameterPath("geq/gain27")]
		public float gain27 { get => GetValue(); set => SetValue(value); }

		[ParameterPath("geq/gain28")]
		public float gain28 { get => GetValue(); set => SetValue(value); }

		[ParameterPath("geq/gain29")]
		public float gain29 { get => GetValue(); set => SetValue(value); }

		[ParameterPath("geq/gain30")]
		public float gain30 { get => GetValue(); set => SetValue(value); }

		[ParameterPath("geq/gain31")]
		public float gain31 { get => GetValue(); set => SetValue(value); }

		[ParameterPath("geq/gain32")]
		public float gain32 { get => GetValue(); set => SetValue(value); }

		[ParameterPath("geq/on")]
		public bool on { get => GetBoolean(); set => SetBoolean(value); }

		[ParameterPath("geq/ston")]
		public bool ston { get => GetBoolean(); set => SetBoolean(value); }

		public override void OnPropertyChanged(PropertyChangedEventArgs eventArgs)
		{
			PropertyChanged?.Invoke(this, eventArgs);
		}
	}
}