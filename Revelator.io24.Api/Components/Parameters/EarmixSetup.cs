using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Presonus.UC.Api.Components.Parameters
{
	public class EarmixSetup : ParameterBase
	{
		public Param mixerlist;
		public Param selected_name;
		public Param identify;
		public Param apply;
		public Param apply_all;
		public Param inputroute_1_8;
		public Param inputroute_9_16;

		public EarmixSetup(string path) : base(path)
		{
		}
	}
}
