// <copyright company="Ghislain One Inc.">
//  Copyright (c) GhislainOne
//  This computer program includes confidential, proprietary
//  information and is a trade secret of GhislainOne. All use,
//  disclosure, or reproduction is prohibited unless authorized in
//  writing by an officer of Ghis. All Rights Reserved.
// </copyright>

namespace YetAnoGitExt.ControlExt.Converters;

using MaterialDesignThemes.Wpf;
using System.Globalization;
using System.Windows.Data;

/// <summary>
/// Base class for the converters controlling the visibility and enabled state of window caption buttons.
/// </summary>
public abstract class WindowCaptionButtonBaseConverter : IMultiValueConverter
{
    /// <summary>
    /// Identifier for the minimize caption button.
    /// </summary>
    public string MinimizeButtonName => "minimizeButton";

    /// <summary>
    /// Identifier for the maximize/restore caption button.
    /// </summary>
    public string MaximizeRestoreButtonName => "maximizeRestoreButton";

    /// <summary>
    /// Identifier for the close caption button.
    /// </summary>
    public string CloseButtonName => "closeButton";

    /// <summary>
    /// Creates a new <see cref="WindowCaptionButtonBaseConverter" />.
    /// </summary>
    public WindowCaptionButtonBaseConverter() { }

    public abstract object Convert(object[] values, Type targetType, object parameter, CultureInfo culture);

    public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
    {
       return  new  []{ Binding.DoNothing};
    
    }
}