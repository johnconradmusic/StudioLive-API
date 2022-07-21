using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Presonus.UC.Api.Components.Parameters
{
	public class OutputDACChannel : RoutableChannel
	{
		public Param lr_assign;
		public Param bussrc;

		public OutputDACChannel(string path) : base(path)
		{
		}
	}
}
