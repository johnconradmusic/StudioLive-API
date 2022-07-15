using Presonus.StudioLive32.Api;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Presonus.UC.Api.Devices
{
	public class StudioLive32R : Device
	{
		public StudioLive32R(RawService rawService) : base(rawService, lineChannels: 32, returnChannels: 3, auxChannels: 16, fxReturns: 4)
		{

		}
	}
}
