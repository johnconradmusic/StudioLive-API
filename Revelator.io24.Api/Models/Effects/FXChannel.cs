using Presonus.StudioLive32.Api.Models;
using Presonus.UC.Api.Devices;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Presonus.StudioLive32.Api.Models
{
	public class FXChannel : ChannelBase
	{
		public FXChannel(string routingPrefix, RawService rawService, Device device) : base(routingPrefix, rawService, device) { }

		public float type { get => GetValue(); set => SetValue(value); }
		public float predelay { get => GetValue(); set => SetValue(value); }
		public float reflection { get => GetValue(); set => SetValue(value); }
		public float size { get => GetValue(); set => SetValue(value); }
		public float lpf { get => GetValue(); set => SetValue(value); }
		public float hpf { get => GetValue(); set => SetValue(value); }
		public float lfdamp_freq { get => GetValue(); set => SetValue(value); }
		public float lfdamp_gain { get => GetValue(); set => SetValue(value); }
		public float delay_l { get => GetValue(); set => SetValue(value); }
		public float delay_r { get => GetValue(); set => SetValue(value); }
		public float fb_l { get => GetValue(); set => SetValue(value); }
		public float fb_r { get => GetValue(); set => SetValue(value); }
		public float spread { get => GetValue(); set => SetValue(value); }
		public float fb_l_lpf { get => GetValue(); set => SetValue(value); }
		public float fb_l_hpf { get => GetValue(); set => SetValue(value); }
		public float fb_r_lpf { get => GetValue(); set => SetValue(value); }
		public float fb_r_hpf { get => GetValue(); set => SetValue(value); }



	}
}
