using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MathNet.Numerics.Integration;
using MathNet.Numerics.RootFinding;

namespace Presonus.UCNet.Api.Helpers
{
	public enum CurveFormula
	{
		Exponential,
		Logarithmic,
		Linear,
		InverseLog,
		LinearToVolume,
		Ratio
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
			bool offset = false;
			if(minValue ==0)
			{
				offset = true;
				minValue += 1;
				maxValue += 1;
			}

			double logMin = Math.Log10(minValue);
			double logMax = Math.Log10(maxValue);

			double mappedValue = Math.Pow(10, logMin + (logMax - logMin) * value);
			if (offset) mappedValue -= 1;
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

		static double MapLinearValueToRatioCurve(double value, double min, double max)
		{
			return 10.37493 - 94.3837 * Math.Exp(-2.213165 * value); // output value of y

		}

		public static double Transform(float input_value, double min_value, double max_value, CurveFormula curve_formula, double exponent = 2)
		{
			input_value = Math.Clamp(input_value, 0, 1);
			return curve_formula switch
			{
				CurveFormula.Exponential => MapValueToExponentialRange(input_value, min_value, max_value, exponent),
				CurveFormula.Logarithmic => MapValueToLogarithmicRange(input_value, min_value, max_value),
				CurveFormula.Linear => MapValueToLinearRange(input_value, min_value, max_value),
				CurveFormula.LinearToVolume => LinearToVolume(input_value),
				CurveFormula.InverseLog => MapLinearValueToInverseLogRange(input_value),
				CurveFormula.Ratio => MapLinearValueToRatioCurve(input_value, min_value, max_value),
				_ => throw new ArgumentOutOfRangeException(nameof(curve_formula), "Invalid curve formula specified."),
			};
		}
	}


}
