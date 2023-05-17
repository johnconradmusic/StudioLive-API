
using Presonus.UCNet.Api.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Presonus.UCNet.Api.Models.Channels
{
	public class OutputDACBus : RoutableChannel
	{
		public OutputDACBus(ChannelTypes channelType, int index, MixerStateService mixerStateService) : base(channelType, index, mixerStateService)
		{
		}

		public float auxpremode { get => GetValue(); set => SetValue(value); }
		public float busmode { get => GetValue(); set => SetValue(value); }
		public float busdelay { get => GetValue(); set => SetValue(value); }

		public static List<string> auxpremode_values = new() { "Pre", "Pre2", "Post" };

		public static List<string> busmode_values = new() { "Aux", "Subgroup", "Matrix" };

		public static List<string> bussource_values = new() { "Mixer", "Network" };

		public bool lr_assign { get => GetBoolean(); set => SetBoolean(value); }

		public float bussrc { get => GetValue(); set => SetValue(value); }


	}
}
