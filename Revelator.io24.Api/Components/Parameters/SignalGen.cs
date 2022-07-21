using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Presonus.UC.Api.Components.Parameters
{
	public class SignalGen : ParameterBase
	{
		public Param type;
		public Param freq;
		public Param level;

		public SignalGen(string path) : base(path)
		{
		}
	}
}
