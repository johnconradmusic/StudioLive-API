using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Presonus.UC.Api.Components.Parameters
{
	public class StereoLineInput :InputChannel
	{
		public Param trim;

		public StereoLineInput(string path) : base(path)
		{
		}
	}
}
