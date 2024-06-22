// <copyright company="Ghislain One Inc.">
//  Copyright (c) GhislainOne
//  This computer program includes confidential, proprietary
//  information and is a trade secret of GhislainOne. All use,
//  disclosure, or reproduction is prohibited unless authorized in
//  writing by an officer of Ghis. All Rights Reserved.
// </copyright>

namespace YetAnoGitExt.ControlExt.Converters;

using System.Globalization;
using System.Windows;
using System.Windows.Data;

/// <summary>
/// Converts a <see cref="WindowStyle" /> and a <see cref="ResizeMode" /> of a window into a <see cref="Visibility" /> of an according caption button.
/// </summary>
public class WindowCaptionButtonVisibilityConverter : WindowCaptionButtonBaseConverter {
              /// <summary>
              /// Creates a new <see cref="WindowCaptionButtonVisibilityConverter" />.
              /// </summary>
              public WindowCaptionButtonVisibilityConverter() : base() { }

              public override object Convert(object[] values, Type targetType, object parameter, CultureInfo culture) {
                            try {
                                          if (values != null && values.Length == 3) {
                                                        string buttonName = (string)values[0];
                                                        WindowStyle windowStyle = (WindowStyle)values[1];
                                                        ResizeMode resizeMode = (ResizeMode)values[2];

                                                        if (buttonName == CloseButtonName) {
                                                                      if (windowStyle != WindowStyle.None) {
                                                                                    return Visibility.Visible;
                                                                      }
                                                                      else {
                                                                                    return Visibility.Collapsed;
                                                                      }
                                                        }
                                                        else {
                                                                      if (resizeMode != ResizeMode.NoResize
                                                                          && (windowStyle == WindowStyle.SingleBorderWindow || windowStyle == WindowStyle.ThreeDBorderWindow)) {
                                                                                    return Visibility.Visible;
                                                                      }
                                                                      else {
                                                                                    return Visibility.Collapsed;
                                                                      }
                                                        }
                                          }
                            }
                            catch (Exception) {
                                          // use the default return value below
                            }

                            return Visibility.Visible;
              }
}