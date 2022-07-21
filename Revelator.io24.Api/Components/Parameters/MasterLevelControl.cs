using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Presonus.UC.Api.Components.Parameters
{
	public class MasterLevelControl : Channel
	{
		public Param slavedelta;

		public MasterLevelControl(string path) : base(path)
		{
		}
	}
}
