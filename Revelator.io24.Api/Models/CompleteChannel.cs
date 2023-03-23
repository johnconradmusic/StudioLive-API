using Presonus.StudioLive32.Api.Attributes;
using Presonus.StudioLive32.Api.Models;
using Presonus.UCNet.Api.NewDataModel;
using Presonus.UCNet.Api.Services;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Presonus.UCNet.Api.Models
{
	public class CompleteChannel : ChannelRouter
	{
		public CompleteChannel(ChannelTypes channelType, int index, MixerStateService mixerStateService) : base(channelType, index, mixerStateService)
		{
		}

		public override event PropertyChangedEventHandler PropertyChanged;

		protected override void OnPropertyChanged(PropertyChangedEventArgs eventArgs)
		{
			Console.WriteLine("channel prop changed " + eventArgs.PropertyName);
		}

		[ParameterPath("username")] public string Name { get => GetString(); set => SetString(value); }
		[ParameterPath("volume")] public float Volume { get => GetValue(); set => SetValue(value); }
	}
}
