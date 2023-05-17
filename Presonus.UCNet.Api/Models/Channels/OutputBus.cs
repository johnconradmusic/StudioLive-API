
using Presonus.UCNet.Api.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;

namespace Presonus.UCNet.Api.Models.Channels
{
	public class OutputBus : RoutableChannel
	{
		public OutputBus(ChannelTypes type, int index, MixerStateService mixerStateService) : base(type, index, mixerStateService)
		{

		}

		public float auxpremode { get => GetValue(); set => SetValue(value); }

		public static List<string> auxpremode_values = new() { "Pre", "Pre2", "Post" };
		public float busmode { get => GetValue(); set => SetValue(value); }
		public float busdelay { get => GetValue(); set => SetValue(value); }



	}
}
