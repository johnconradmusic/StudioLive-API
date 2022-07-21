using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Presonus.UC.Api.Components.Parameters
{
	public class Bluetooth : ParameterBase
	{
		public Param pair;
		public Param forget;
		public Param device1_name;
		public Param device2_name;
		public Param linkState;

		public Bluetooth(string path) : base(path)
		{
		}
	}
}
