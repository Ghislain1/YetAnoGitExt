using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;

namespace WpfCommon.Converters
{
    public class GenericConverter<TSource,TDest> : IValueConverter
    {
        private readonly Func<TSource, TDest> _converter;
        private readonly Func<TDest, TDest> _backConverter;

        public GenericConverter(Func<TSource, TDest> converter,Func<TDest,TDest> backConverter)
        {
            _converter = converter;
            _backConverter = backConverter;
        }

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is null && default(TSource) is null)
            {
                return _converter(default);
            }
            else if (value is TSource source && _converter is not null)
            {
                return _converter(source);
            }
            return DependencyProperty.UnsetValue;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is null && default(TDest) is null)
            {
                return _backConverter(default);
            }
            else if (value is TDest dest && _backConverter is not null)
            {
                return _backConverter(dest);
            }

            return DependencyProperty.UnsetValue;
        }
    }
}