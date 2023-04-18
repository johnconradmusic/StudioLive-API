
using Presonus.UCNet.Api.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Presonus.UCNet.Api.Models.Channels
{
	public class OutputDACBus : OutputBus
	{
		public OutputDACBus(ChannelTypes channelType, int index, MixerStateService mixerStateService) : base(channelType, index, mixerStateService)
		{
		}
	}
}
