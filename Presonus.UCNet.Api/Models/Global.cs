using Presonus.UCNet.Api.Attributes;
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
	public class Global : ParameterRouter, INotifyPropertyChanged
	{
		public Global(MixerStateService mixerStateService) : base(ChannelTypes.NONE, -1, mixerStateService)
		{
		}

		public override event PropertyChangedEventHandler PropertyChanged;

		public override void OnPropertyChanged(PropertyChangedEventArgs eventArgs)
		{
			PropertyChanged?.Invoke(this, eventArgs);
		}

		[ParameterPath("global/devicename")]
		public string devicename { get => GetString(); set => SetString(value); }


	}
}
