using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Presonus.UCNet.Api.Helpers
{
	public static class Util
	{
		public static float Map(float oldRangeMin, float oldRangeMax, float newRangeMin, float newRangeMax, float value)
		{
			//convert initial value within old range to percentage
			var topOfRange = oldRangeMax - oldRangeMin; // absolute
			float percentageValue = (value - oldRangeMin) / topOfRange;

			//convert percentage to new value within new range
			topOfRange = newRangeMax - newRangeMin; // absolute
			float newValue = (percentageValue * topOfRange) + newRangeMin;
			newValue = Math.Clamp(newValue, newRangeMin, newRangeMax);
			return newValue;
		}

		static List<(float db, float floatVal)> VolumeDBRanges = new List<(float db, float floatVal)>
		{
			new(-84, 0),
			new(-58, .0095f),
			new(-30, .2f),
			new(-17, .37f),
			new(-9, .5f),
			new(0, .73f),
			new(10, 1)
		};

		static List<(float freq, float floatVal)> FrequencyRanges = new List<(float hz, float floatVal)>
		{
			new(36, 0),
			new(100, .1643951f),
			new(200, .2759f),
			new(500, .4233f),
			new(750, .488f),
			new(1000, .5349068f),
			new(2000, .64644f),
			new(5000, .79388f),
			new(7500, .85912f),
			new(10000, .90541834f),
			new(20000,1)

		};

		public static float GetFloatFromFrequency(float freq)
		{
			for (int i = 0; i < FrequencyRanges.Count - 1; i++)
				if (freq >= FrequencyRanges[i].freq && freq <= FrequencyRanges[i + 1].freq)
					return Map(FrequencyRanges[i].freq, FrequencyRanges[i + 1].freq, FrequencyRanges[i].floatVal, FrequencyRanges[i + 1].floatVal, freq);
			return 0;
		}

		public static float GetFrequencyFromFloat(float floatValue)
		{
			for (int i = 0; i < FrequencyRanges.Count - 1; i++)
				if (floatValue >= FrequencyRanges[i].floatVal && floatValue <= FrequencyRanges[i + 1].floatVal)
					return Map(FrequencyRanges[i].floatVal, FrequencyRanges[i + 1].floatVal, FrequencyRanges[i].freq, FrequencyRanges[i + 1].freq, floatValue);

			return 0;
		}

		public static float GetFloatFromDB(float db)
		{
			for (int i = 0; i < VolumeDBRanges.Count - 1; i++)
				if (db >= VolumeDBRanges[i].db && db <= VolumeDBRanges[i + 1].db)
					return Map(VolumeDBRanges[i].db, VolumeDBRanges[i + 1].db, VolumeDBRanges[i].floatVal, VolumeDBRanges[i + 1].floatVal, db);


			return 0;
		}
		public static float GetDBFromFloat(float floatValue)
		{
			//value = value / (float)ushort.MaxValue;
			for (int i = 0; i < VolumeDBRanges.Count - 1; i++)
				if (floatValue >= VolumeDBRanges[i].floatVal && floatValue <= VolumeDBRanges[i + 1].floatVal)
					return Map(VolumeDBRanges[i].floatVal, VolumeDBRanges[i + 1].floatVal, VolumeDBRanges[i].db, VolumeDBRanges[i + 1].db, floatValue);

			return 0;
		}

	}
}
