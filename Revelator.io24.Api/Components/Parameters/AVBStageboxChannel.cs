using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Presonus.UC.Api.Components.Parameters
{
	public class AVBStageboxChannel : ParameterBase
	{
		public Param _48v;
		public Param preampgain;
		public Param preampmode;
		public Param gaincomp;

		public AVBStageboxChannel(string path) : base(path)
		{
		}
	}
}
