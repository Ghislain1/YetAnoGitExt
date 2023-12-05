using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;

namespace RevisionViewer.Converters
{
    [ValueConversion(typeof(bool), typeof(Visibility))]
    public sealed class NullToVisibilityConverter : IValueConverter
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
                    return Visibility.Visible;

                return Visibility.Collapsed;
            }
            else
            {
                return Visibility.Visible;
            }            
        }

        public object ConvertBack(object value, Type targetType,
            object parameter, CultureInfo culture)
        {
            return null;
        }
    }
}
