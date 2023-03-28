using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Presonus.UCNet.Api.Helpers
{

	public static class RangeMapper
	{
		public static double Map(double value, double fromMin, double fromMax, double toMin, double toMax, Func<double, double> curveFunction = null)
		{
			double range = fromMax - fromMin;
			double offsetValue = value - fromMin;
			double normalValue = offsetValue / range;
			double mappedValue;

			if (curveFunction != null)
			{
				normalValue = curveFunction(normalValue);
			}

			mappedValue = normalValue * (toMax - toMin) + toMin;

			return mappedValue;
		}
	}
}
public static class CurveFunctions
{
	public static double Logarithmic(double value, double exponent)
	{
		return Math.Pow(value, exponent);
	}
}

