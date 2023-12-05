using System;
using System.Globalization;
using System.Windows.Data;

namespace RevisionViewer.Converters
{
    [ValueConversion(typeof(bool), typeof(double))]
    public sealed class NullToWidthConverter : IValueConverter
    {
        enum Parameters
        {
            Normal, Inverted
        }

        public object Convert(object value, Type targetType,
                              object parameter, CultureInfo culture)
        {
            if (value == null)
            {
                var direction = (Parameters)Enum.Parse(typeof(Parameters), (string)parameter);

                if (direction == Parameters.Inverted)
                    return 20;

                return 0;
            }
            else
            {
                return 20;
            }            
        }

        public object ConvertBack(object value, Type targetType,
            object parameter, CultureInfo culture)
        {
            return null;
        }
    }
}
