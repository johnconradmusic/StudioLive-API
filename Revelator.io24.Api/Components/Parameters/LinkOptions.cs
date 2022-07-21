using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Presonus.UC.Api.Components.Parameters
{
	public class LinkOptions : ParameterBase
	{
		public Param ch_gain;
		public Param pan;
		public Param fader;
		public Param dyn;
		public Param ch_name = new("Ch. Name", ParamType.TOGGLE);
		public Param ins_fx;

		public LinkOptions(string path) : base(path)
		{
		}
	}
}
