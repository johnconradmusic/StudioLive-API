using Revelator.io24.Api.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Revelator.io24.Api.Models
{
	public class ReturnChannel : ChannelBase
	{
		public ReturnChannel(string routingPrefix, RawService rawService) : base(routingPrefix, rawService) { }

		[RouteValueRange(0, 20, Enums.Unit.db)]
		public float trim { get => GetValue(); set => SetValue(value); }

	}
}
