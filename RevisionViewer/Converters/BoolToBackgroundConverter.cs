using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;
using System.Windows.Media;

namespace RevisionViewer.Converters
{
    [ValueConversion(typeof(bool), typeof(Visibility))]
    public sealed class BoolToBackgroundConverter : IValueConverter
    {
        enum Parameters
        {
            Normal, Inverted
        }

        public object Convert(object value, Type targetType,
                              object parameter, CultureInfo culture)
        {
            var boolValue = (bool)value;
            var direction = (Parameters)Enum.Parse(typeof(Parameters), (string)parameter);

            if (direction == Parameters.Inverted)
                return !boolValue ? Brushes.Transparent : Brushes.WhiteSmoke;

            return boolValue ? Brushes.WhiteSmoke : Brushes.Transparent;
        }

        public object ConvertBack(object value, Type targetType,
            object parameter, CultureInfo culture)
        {
            return null;
        }
    }
}
