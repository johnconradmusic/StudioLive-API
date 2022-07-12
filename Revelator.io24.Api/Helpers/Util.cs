using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Presonus.UC.Api.Helpers
{
    public static class Util
    {
        public static float Map(float oldRangeMin, float oldRangeMax, float newRangeMin, float newRangeMax, float value)
        {
            //setter (value within range to percentage 0-1) 
            var topOfRange = oldRangeMax - oldRangeMin; // absolute
            value = (value - oldRangeMin) / topOfRange;



            //getter (percentage 0-1 to value wthin range)
            topOfRange = newRangeMax - newRangeMin;
            value = (value * topOfRange) + newRangeMin;
            return value;

            var retVal = (oldRangeMax - oldRangeMin) * ((value - newRangeMin) / (newRangeMax - newRangeMin)) + oldRangeMin;
                return retVal;
            
        }
        static float[] range1 = new float[4] { -84, -58, 0, 0.27f };
        static float[] range2 = new float[4] { -58, -40, 0.27f, 0.46f };
        static float[] range3 = new float[4] { -40, 0, 0.46f, 0.73f };
        static float[] range4 = new float[4] { 0, 10, 0.73f, 1f };
        public static float GetFloatFromDB(float db)
        {
            float[][] ranges = new float[4][] { range1, range2, range3, range4 };
            foreach (var range in ranges)
            {
                if (db >= range[0] && db <= range[1]) //is between values in this range
                {
                    return Map(range[0], range[1], range[2], range[3], db);
                }
            }
            return 0;
        }
        public static float GetDBFromFloat(float floatValue)
        {
            //value = value / (float)ushort.MaxValue;


            float[][] ranges = new float[4][] { range1, range2, range3, range4 };
            foreach (var range in ranges)
            {
                if (floatValue >= range[2] && floatValue <= range[3]) //is between values in this range
                {
                    return Map(range[2], range[3], range[0], range[1], floatValue);
                }
            }
            return 0;

            var a = 0.47f;
            var b = 0.09f;
            var c = 0.004f;

            if (floatValue >= a)
            {
                var y = (floatValue - a) / (1 - a);
                return (float)Math.Round(y * 20) - 10;
            }

            if (floatValue >= b)
            {
                var y = floatValue / (a - b);
                return (float)Math.Round(y * 30) - 47;
            }

            if (floatValue >= c)
            {
                var y = floatValue / (b - c);
                return (float)Math.Round(y * 20) - 61;
            }

            {
                var y = floatValue / (c - 0.0001111f);
                return (float)Math.Round(y * 35) - 96;
            }

            return floatValue - 10;
        }

    }
}
