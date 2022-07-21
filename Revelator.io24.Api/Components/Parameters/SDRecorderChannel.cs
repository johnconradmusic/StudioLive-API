using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Presonus.UC.Api.Components.Parameters
{
	public class SDRecorderChannel : ParameterBase
	{
		public Param name;
		public Param recordArmed;

		public SDRecorderChannel(string path) : base(path)
		{
		}
	}
}
