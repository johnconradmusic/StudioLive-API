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
			if (eventArgs.PropertyName == "username")
				PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("name"));


		}

		public string Name { get => GetString("username"); set => SetString(value, "username"); }
		public float Volume { get => GetValue("volume"); set => SetValue(value, "volume"); }
	}
}
