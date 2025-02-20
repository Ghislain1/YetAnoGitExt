// <copyright company="Ghislain One Inc.">
//  Copyright (c) GhislainOne
//  This computer program includes confidential, proprietary
//  information and is a trade secret of GhislainOne. All use,
//  disclosure, or reproduction is prohibited unless authorized in
//  writing by an officer of Ghis. All Rights Reserved.
// </copyright>




namespace YetAnoGitExt.ControlExt.Converters;
using YetAnoGitExt.ControlExt.Converters;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Windows.Data;
using System.Windows;


/// <summary>
/// Converts a <see cref="WindowStyle" /> and icon of a window into a <see cref="Visibility" /> for the icon.
/// </summary>
public class WindowTitleBarIconVisibilityConverter : IMultiValueConverter
{
              /// <summary>
              /// Creates a new <see cref="WindowTitleBarIconVisibilityConverter" />.
              /// </summary>
              public WindowTitleBarIconVisibilityConverter() { }

              public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
              {
                            try
                            {
                                          if (values != null && values.Length == 2)
                                          {
                                                        object icon = values[0];
                                                        WindowStyle windowStyle = (WindowStyle)values[1];

                                                        if (icon != null && (windowStyle == WindowStyle.SingleBorderWindow || windowStyle == WindowStyle.ThreeDBorderWindow))
                                                        {
                                                                      return Visibility.Visible;
                                                        }
                                                        else
                                                        {
                                                                      return Visibility.Collapsed;
                                                        }
                                          }
                            }
                            catch (Exception)
                            {
                                          // use the default return value below
                            }

                            return Visibility.Visible;
              }

              public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
              {
                            throw new NotImplementedException();
              }
}
