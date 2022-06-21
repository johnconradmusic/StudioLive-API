using Revelator.io24.Api.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Revelator.io24.Api.Models
{
	public class ValueRange
	{

		public ValueRange(float min, float max, Unit unit)
		{
			Min = min;
			Max = max;
			Unit = unit;
		}

		public float Min { get; set; }
		public float Max { get; set; }
		public Unit Unit { get; set; }
		
	}
}
