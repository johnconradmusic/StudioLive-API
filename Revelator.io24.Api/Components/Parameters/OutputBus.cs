using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Presonus.UC.Api.Components.Parameters
{
	public class OutputBus : Channel
	{
		public Param auxpremode;
		public Param busmode;
		public Param busdelay;


		public OutputBus(string path) : base(path)
		{
		}
	}
}
