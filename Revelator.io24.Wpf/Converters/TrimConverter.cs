using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;

namespace Revelator.io24.Wpf.Converters
{
    public class TrimConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is ushort ushortValue)
            {
                var floatValue = ushortValue / (float)ushort.MaxValue;

                //Using same calulcation as for +10 to -96 is almost correct.
                //Max value is +0dB, therefor we need to subtract -10dB
                //Also the range seems different, 0dB to -80dB seems to be the most correct...
                //However, I have no idea with the gain reduction meter
                // (this is usually never higher reduction than -10dB, so hard to test in the other ranges).
                return floatValue;
            }
            return value;
        }
    


        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
