using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Presonus.UC.Api.Components.Parameters
{
	public class AVBStageboxGlobal : ParameterBase
	{
		public Param identify;
		public Param stagebox_mode;

		public AVBStageboxGlobal(string path) : base(path)
		{
		}
	}
}
