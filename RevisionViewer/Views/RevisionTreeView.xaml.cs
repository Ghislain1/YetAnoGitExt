using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace RevisionViewer.Views
{
    /// <summary>
    /// Interaction logic for RevisionTreeView.xaml
    /// </summary>
    public partial class RevisionTreeView
    {
        public ICommand TreeView_ActionCommand { get; set; }
        public RevisionTreeView()
        {
            InitializeComponent();
            var viewModel = new RevisionTreeViewModel();
            DataContext = viewModel;
            TreeView_ActionCommand = viewModel.TreeView_ActionCommand();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            var buttonSender = (Button) sender;

            if (buttonSender?.Tag is RevisionViewModel revisionViewModel)
            {
                TreeView_ActionCommand?.Execute(revisionViewModel);
            }
        }
    }
}
