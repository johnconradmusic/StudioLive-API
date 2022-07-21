using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Presonus.UC.Api.Components.Parameters
{
	public class SoftPower : ParameterBase
	{
		public Param initiateSoftPower;
		public Param softPowerStage;
		public Param softPowerProgress;

		public SoftPower(string path) : base(path)
		{
		}
	}
}
