using Presonus.UCNet.Api.Services;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Presonus.UCNet.Api.Models
{
	public class SignalGen : ParameterRouter
	{
		public SignalGen(MixerStateService mixerStateService) : base("signalgen", -1, mixerStateService)
		{
		}

		public float type { get => GetValue(); set => SetValue(value); }
		public string[] type_values => new string[] { "Pink Noise", "White Noise", "Sine" };

		public float freq { get => GetValue(); set => SetValue(value); }
		public float level { get => GetValue(); set => SetValue(value); }

		public override event PropertyChangedEventHandler PropertyChanged;

		public override void OnPropertyChanged(PropertyChangedEventArgs eventArgs)
		{
			PropertyChanged?.Invoke(this, eventArgs);
		}
	}
}
