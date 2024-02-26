// <copyright company="Ghislain One Inc.">
//  Copyright (c) GhislainOne
//  This computer program includes confidential, proprietary
//  information and is a trade secret of GhislainOne. All use,
//  disclosure, or reproduction is prohibited unless authorized in
//  writing by an officer of Ghis. All Rights Reserved.
// </copyright>

namespace YetAnoGitExt;


using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using YetAnoGitExt.ControlExt.Controls;

/// <summary>
/// Interaction logic for MainWindow.xaml
/// </summary>
public partial class ShellWindow : WindowExt
{
    public ShellWindow()
    {
        this.InitializeComponent();
        this.Loaded += this.ShellWindow_Loaded;
    }

    private void ShellWindow_Loaded(object sender, RoutedEventArgs e)
    {
        var sd = this.Style;
        var rd = (ResourceDictionary)Application.LoadComponent(new Uri("/YetAnoGitExt.ControlExt;component/Themes/Generic.xaml", UriKind.Relative));
    }
}