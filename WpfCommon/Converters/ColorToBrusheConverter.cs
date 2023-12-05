using System;
using System.Collections.Generic;
using System.Globalization;
using System.Windows;
using System.Windows.Data;
using System.Windows.Media;

namespace WpfCommon.Converters
{
    public class ColorToBrushConverter : IValueConverter
    {
        private static readonly Dictionary<Color, Brush> KnowBrushes = new();
        private static readonly Color FallbackColor = Colors.Black;

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is Color color)
            {
                return ConvertToBrush(color);
            }
            return DependencyProperty.UnsetValue;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is SolidColorBrush solidColorBrush)
            {
                return solidColorBrush.Color;
            }
            return FallbackColor;
        }

        public Brush ConvertToBrush(Color color)
        {
            if (KnowBrushes.TryGetValue(color, out Brush result))
            {
                return result;
            }

            return KnowBrushes[color] = new SolidColorBrush(color);
        }
    }

    public class ColorToBrushConverterExtension : MarkupExtensionConverterGeneric<ColorToBrushConverter>
    {
    }
}