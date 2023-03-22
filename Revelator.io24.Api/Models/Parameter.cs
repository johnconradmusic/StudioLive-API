using Presonus.StudioLive32.Api.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Presonus.UCNet.Api.Models
{
	public class Parameter
	{
		public string Path { get; set; }
		public object Value { get; set; }
		public Type Type => Value?.GetType();
		public string DisplayName => Path.Split('/').Last();

		public float Min { get; set; }
		public float Max { get; set; }
		public float Default { get; set; }
		public float Mid { get; set; }
		public Curve Curve { get; set; }
		public Unit Unit { get; set; }

		public float GetSliderValue(float inputValue)
		{
			// Apply the curve function based on the Curve property here
			// Example: inputValue = ApplyCurveFunction(inputValue, Curve);

			return inputValue;
		}

	}

	public enum Curve
	{
		Linear
	}

}
