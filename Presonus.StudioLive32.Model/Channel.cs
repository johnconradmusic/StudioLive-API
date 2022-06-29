using Presonus.StudioLive32.Api;
using Presonus.StudioLive32.Api.Models;
using Presonus.StudioLive32.Model.Enums;
using Presonus.UC.Api;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Presonus.StudioLive32.Model
{
	public class Channel : IRoutable
	{
		public ChannelType ChannelType { get; set; }
		public string Route => ChannelType.ToString() + "/" + "ch" + Index.ToString();
		public int Index { get; set; }

		public Channel(ChannelType channelType, int index)
		{
			Index = index;
			ChannelType = channelType;
		}
	}
}
