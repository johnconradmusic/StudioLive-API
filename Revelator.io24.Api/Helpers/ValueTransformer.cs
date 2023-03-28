using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MathNet.Numerics.RootFinding;

namespace Presonus.UCNet.Api.Helpers
{
	public enum CurveFormula
	{
		Exponential,
		Logarithmic,
		Linear,
		InverseLog
	}
	public enum Units
	{
		DB, HZ, MS
	}
	public class ValueTransformer
	{

		private static double MapValueToLogarithmicRange(double value, double minValue, double maxValue)
		{
			if (value < 0 || value > 1)
			{
				throw new ArgumentOutOfRangeException(nameof(value), "Value must be between 0 and 1.");
			}

			double logMin = Math.Log10(minValue);
			double logMax = Math.Log10(maxValue);

			double mappedValue = Math.Pow(10, logMin + (logMax - logMin) * value);
			return mappedValue;
		}

		private static double MapValueToExponentialRange(double value, double minValue, double maxValue, double exponent)
		{
			if (value < 0 || value > 1)
			{
				throw new ArgumentOutOfRangeException(nameof(value), "Value must be between 0 and 1.");
			}

			double mappedValue = minValue + (maxValue - minValue) * Math.Pow(value, exponent);
			return mappedValue;
		}

		private static double MapValueToLinearRange(double value, double minValue, double maxValue)
		{
			if (value < 0 || value > 1)
			{
				throw new ArgumentOutOfRangeException(nameof(value), "Value must be between 0 and 1.");
			}

			double mappedValue = minValue + (maxValue - minValue) * value;
			return mappedValue;
		}


		public static double MapLinearValueToInverseLogRange(double position)
		{
			const double a = 3.1623e-5;
			const double b = 10.36;
			return a * Math.Exp(b * position);
		}

		private static float LinearToVolume(float value)
		{
			var a = 0.47f;
			var b = 0.09f;
			var c = 0.004f;

			if (value >= a)
			{
				var y = (value - a) / (1f - a);
				return (float)Math.Round((y * 20) - 10, 2);
			}

			if (value >= b)
			{
				var y = value / (a - b);
				return (float)Math.Round((y * 30) - 47, 2);
			}

			if (value >= c)
			{
				var y = value / (b - c);
				return (float)Math.Round((y * 20) - 61, 2);
			}

			{
				var y = value / (c - 0.0001111f);
				return (float)Math.Round((y * 35) - 96, 2);
			}
		}

		public static double Transform(float input_value, double min_value, double max_value, CurveFormula curve_formula, double exponent = 2)
		{
			return curve_formula switch
			{
				CurveFormula.Exponential => MapValueToExponentialRange(input_value, min_value, max_value, exponent),
				CurveFormula.Logarithmic => MapValueToLogarithmicRange(input_value, min_value, max_value),
				CurveFormula.Linear => MapValueToLinearRange(input_value, min_value, max_value),
				CurveFormula.InverseLog => LinearToVolume(input_value),
				_ => throw new ArgumentOutOfRangeException(nameof(curve_formula), "Invalid curve formula specified."),
			};
		}
	}


}
