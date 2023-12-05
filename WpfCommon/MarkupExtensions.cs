using System;
using System.Windows.Data;
using System.Windows.Markup;

namespace WpfCommon
{
    public abstract class MarkupExtensionConverter : MarkupExtension
    {
        private IValueConverter _converter;

        public override object ProvideValue(IServiceProvider serviceProvider)
        {
            return _converter ??= ProvideValueImpl();
        }

        protected abstract IValueConverter ProvideValueImpl();
    }

    public class MarkupExtensionConverterGeneric<T> : MarkupExtensionConverter where T : IValueConverter, new()
    {
        protected override IValueConverter ProvideValueImpl()
        {
            return new T();
        }
    }
}