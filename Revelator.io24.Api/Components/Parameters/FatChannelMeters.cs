using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Presonus.UC.Api.Components.Parameters
{
	public class FatChannelMeters : ParameterBase
	{
		public Param input;
		public Param inputpeak;
		public Param clip;
		public Param gate;
		public Param gatepeak;
		public Param comp;
		public Param comppeak;
		public Param eq;
		public Param eqpeak;
		public Param limit;
		public Param limitpeak;
		public Param gatereduction;
		public Param compreduction;
		public Param limitreduction;

		public FatChannelMeters(string path) : base(path)
		{
		}
	}
}
