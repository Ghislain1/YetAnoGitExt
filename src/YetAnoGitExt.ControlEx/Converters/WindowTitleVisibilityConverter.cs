// <copyright company="Ghislain One Inc.">
//  Copyright (c) GhislainOne
//  This computer program includes confidential, proprietary
//  information and is a trade secret of GhislainOne. All use,
//  disclosure, or reproduction is prohibited unless authorized in
//  writing by an officer of Ghis. All Rights Reserved.
// </copyright>




namespace YetAnoGitExt.ControlExt.Converters;
using System;
using System.Globalization;
using System.Windows.Data;
using System.Windows;

/// <summary>
/// Converts a <see cref="WindowStyle" /> of a window into a <see cref="Visibility" /> of the whole title bar.
/// </summary>
public class WindowTitleVisibilityConverter : IMultiValueConverter {
              /// <summary>
              /// Creates a new <see cref="WindowTitleVisibilityConverter" />.
              /// </summary>
              public WindowTitleVisibilityConverter() { }

              public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture) {
                            try {
                                          if (values != null && values.Length >= 1) {
                                                        WindowStyle windowStyle = (WindowStyle)values[0];

                                                        if (windowStyle != WindowStyle.None) {
                                                                      return Visibility.Visible;
                                                        }
                                                        else {
                                                                      return Visibility.Collapsed;
                                                        }
                                          }
                            }
                            catch (Exception) {
                                          // use the default return value below
                            }

                            return Visibility.Visible;
              }

              public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture) {
                            throw new NotImplementedException();
              }
}