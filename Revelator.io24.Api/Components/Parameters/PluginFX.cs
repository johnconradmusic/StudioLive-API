using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Presonus.UC.Api.Components.Parameters
{
	public class PluginFX : ParameterBase
	{
		public Param type;
		public Param pluginstate;

		public PluginFX(string path) : base(path)
		{
		}
	}
}
