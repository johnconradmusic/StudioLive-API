using Revelator.io24.Api.Attributes;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Revelator.io24.Api.Models.Auxes
{
	public class AuxChannel : ChannelBase
	{
		public AuxChannel(string routePrefix, RawService rawService) : base(routePrefix, rawService) { }

		public int auxpremode { get => (int)GetValue(); set => SetValue(value); }
		public int busmode { get => (int)GetValue(); set => SetValue(value); }
		public float busdelay { get => GetValue(); set => SetValue(value); }
		[RouteValueRange(0, 1000, Enums.Unit.hz)][RouteValue("filter/hpf")] public float hipass { get => GetValue(); set => SetValue(value); }
		public bool lr_assign { get => GetBoolean(); set => SetBoolean(value); }
		public int bussrc { get => (int)GetValue(); set => SetValue(value); }

	}
}
