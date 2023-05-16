using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MathNet.Numerics.Integration;
using MathNet.Numerics.RootFinding;
using Presonus.UCNet.Api;
using static Presonus.UCNet.Api.Helpers.Ease;

namespace Presonus.UCNet.Api.Helpers
{
	public enum CurveFormula
	{
		Exponential,
		Logarithmic,
		Linear,
		InverseLog,
		LinearToVolume,
		Ratio,
		Skew
	}
	public enum Units
	{
		NONE, DB, HZ, MS, PAN, HZ_24OFF, RATIO, PERCENT
	}
	public class ValueTransformer
	{

		public static double TryEasingFunctions(float input, float outputMin, float outputMax)
		{

			Console.WriteLine($"Input: {input}, ExpoIn {outputMin + (outputMax - outputMin) * Ease.ExpoIn(input)}");
			Console.WriteLine($"Input: {input}, QuadIn {outputMin + (outputMax - outputMin) * Ease.QuadIn(input)}");
			Console.WriteLine($"Input: {input}, QuintIn {outputMin + (outputMax - outputMin) * Ease.QuintIn(input)}");
			Console.WriteLine($"Input: {input}, CubeIn {outputMin + (outputMax - outputMin) * Ease.CubeIn(input)}");

			// Add Logarithmic easing functions
			Console.WriteLine($"Input: {input}, LogIn {outputMin + (outputMax - outputMin) * Ease.LogIn(input)}");
			Console.WriteLine($"Input: {input}, LogOut {outputMin + (outputMax - outputMin) * Ease.LogOut(input)}");

			// Add Power easing functions
			for (float exponent = 2f; exponent < 4.5; exponent += 0.5f)
			{

				Console.WriteLine($"Exponent: {exponent}");
				Easer Power3In = Ease.CreatePowerInEasing(exponent);
				//Easer Power3Out = Ease.CreatePowerOutEasing(exponent);
				Console.WriteLine($"Input: {input}, Power3In {outputMin + (outputMax - outputMin) * Power3In(input)}");
				//Console.WriteLine($"Input: {input}, Power3Out {outputMin + (outputMax - outputMin) * Power3Out(input)}");

			}

			return 0;
		}

		public static double EaseWithinRange(Easer easer, float input, double outputMin, double outputMax)
		{
			return (outputMin + (outputMax - outputMin) * easer(input));
		}

		private static double MapValueToLogarithmicRange(double value, double minValue, double maxValue)
		{
			if (value < 0 || value > 1)
			{
				throw new ArgumentOutOfRangeException(nameof(value), "Value must be between 0 and 1.");
			}
			bool offset = false;
			if (minValue == 0)
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



		static double Interpolate(double input, double input1, double output1, double input2, double output2)
		{
			return output1 + ((input - input1) * (output2 - output1) / (input2 - input1));
		}

		public static float LinearToMeter(float value)
		{
			return (LinearToVolume(value) + 84) / 94;
		}

		public static float LinearToVolume(float value)

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

		static double MapLinearValueToRatioCurve(double value)
		{
			return 10.37493 - 94.3837 * Math.Exp(-2.213165 * value); // output value of y

		}

		public static string Transform(float input_value, float min_value, float max_value, CurveFormula curve_formula, Units units, float exponent = 2)
		{
			input_value = Math.Clamp(input_value, 0, 1);

			var strVal = curve_formula switch
			{
				CurveFormula.Exponential => MapValueToExponentialRange(input_value, min_value, max_value, exponent),
				CurveFormula.Logarithmic => MapValueToLogarithmicRange(input_value, min_value, max_value),
				CurveFormula.Linear => MapValueToLinearRange(input_value, min_value, max_value),
				CurveFormula.LinearToVolume => LinearToVolume(input_value),
				CurveFormula.InverseLog => MapLinearValueToInverseLogRange(input_value),
				CurveFormula.Ratio => MapLinearValueToRatioCurve(input_value),
				CurveFormula.Skew => EaseWithinRange(SkewIn, input_value, min_value, max_value),
				_ => throw new ArgumentOutOfRangeException(nameof(curve_formula), "Invalid curve formula specified."),
			};

			return FormatValueWithUnit(strVal, units);
		}
		public static string FormatValueWithUnit(double value, Units unit)
		{
			string unitString;

			switch (unit)
			{
				case Units.MS:
					if (value >= 1000)
					{
						value /= 1000;
						unitString = "s";
					}
					else
					{
						unitString = "ms";
					}
					break;
				case Units.HZ:
				case Units.HZ_24OFF:
					if (value >= 1000)
					{
						value /= 1000;
						unitString = "kHz";
					}
					else
					{
						unitString = "Hz";
					}
					break;
				case Units.DB:
					unitString = "dB";
					break;
				case Units.NONE:
					unitString = "";
					break;
				case Units.RATIO:
					unitString = "to 1";
					break;
				case Units.PERCENT:
					unitString = "percent";
					
					break;
				case Units.PAN:
					int panPercentage;
					float centerTolerance = 0.005f; // Tolerance range for the center

					// Check if the value is within the center tolerance range
					if (Math.Abs(value - 0.5f) <= centerTolerance)
					{
						return "Center";
					}
					// If the value is less than 0.5, it means pan to the left
					else if (value < 0.5f)
					{
						panPercentage = (int)((0.5f - value) * 200); // Calculate the percentage for left panning
						return $"Pan Left {panPercentage}%";
					}
					// If the value is greater than 0.5, it means pan to the right
					else
					{
						panPercentage = (int)((value - 0.5f) * 200); // Calculate the percentage for right panning
						return $"Pan Right {panPercentage}%";
					}
				// Add other units and their string representations as needed
				default:
					throw new ArgumentException("Invalid unit.");
			}

			if (unit == Units.HZ_24OFF && value <= 24)
			{
				return "Off";
			}
			string formatString = value % 1 == 0 ? "G" : "G3";
			return value.ToString(formatString) + " " + unitString;
		}




	}


}
