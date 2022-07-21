using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Presonus.UC.Api.Components.Parameters
{
	public class ConsolesGlobal : ParameterBase
	{
		public Param aes_source;

		public ConsolesGlobal(string path) : base(path)
		{
		}
	}
}
