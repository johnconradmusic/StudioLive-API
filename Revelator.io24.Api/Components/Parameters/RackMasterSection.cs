using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Presonus.UC.Api.Components.Parameters
{
	public class RackMasterSection : ParameterBase
	{
		public Param wifi_host = new("WIFI Host", ParamType.TOGGLE);
		public Param wifi_client = new("WIFI Client", ParamType.TOGGLE);
		public Param ethernet = new("Ethernet", ParamType.TOGGLE);
		public Param sd_activity = new("SD Activity", ParamType.TOGGLE);
		public Param network_mode = new("NetworkMode", ParamType.TOGGLE);

		public RackMasterSection(string path) : base(path)
		{
		}
	}
}
