using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Presonus.UC.Api.Components.Parameters
{
	public class NetworkSetup : ParameterBase
	{
		public Param ipaddress;
		public Param subnet;
		public Param gateway;
		public Param assignmode;
		public Param refresh;
		public Param setupactivity;
		public Param status;
		public Param scan;
		public Param networklist;
		public Param signalstrength;

		public NetworkSetup(string path) : base(path)
		{
		}
	}
}
