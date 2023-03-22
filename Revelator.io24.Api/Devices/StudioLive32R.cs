using Presonus.StudioLive32.Api;
using Presonus.UCNet.Api.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Presonus.UCNet.Api.Devices
{
	public class StudioLive32R : Device
	{
		public StudioLive32R(MixerStateService rawService) : base(rawService, lineChannels: 32, returnChannels: 3, auxChannels: 16, fxReturns: 4)
		{

		}
	}
}
