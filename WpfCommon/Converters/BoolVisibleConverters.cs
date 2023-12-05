using System.Windows;
using System.Windows.Data;

namespace WpfCommon.Converters
{
    public class TrueVisibleFalseCollapsed : MarkupExtensionConverter
    {
        protected override IValueConverter ProvideValueImpl() => new GenericConverter<bool, Visibility>(flag => flag ? Visibility.Visible : Visibility.Collapsed,default);
    }
    public class TrueCollapsedFalseVisible : MarkupExtensionConverter
    {
        protected override IValueConverter ProvideValueImpl() => new GenericConverter<bool, Visibility>(flag => flag ? Visibility.Collapsed : Visibility.Visible,default);
    }
    public class TrueVisibleFalseHidden : MarkupExtensionConverter
    {
        protected override IValueConverter ProvideValueImpl() => new GenericConverter<bool, Visibility>(flag => flag ? Visibility.Visible : Visibility.Hidden,default);
    }
    public class TrueHiddenFalseVisible : MarkupExtensionConverter
    {
        protected override IValueConverter ProvideValueImpl() => new GenericConverter<bool, Visibility>(flag => flag ? Visibility.Hidden : Visibility.Visible,default);
    }
}