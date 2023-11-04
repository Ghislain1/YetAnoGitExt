using RevisionViewer;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Windows;

namespace EntryPoint
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            this.InitializeComponent();
            PopulateTreeView();
          
        }

        private void PopulateTreeView()
        {
            var factory = new DataStructureBuilderMock();
            var root = factory.BuildSampleData();
            if (root is Revision revision)
            {
                this.DataContext = new ObservableCollection<RevisionTreeViewModel> { new RevisionTreeViewModel(new[] { revision }) };             
            }
                
        }

        public void DisplayTreeItems()
        {
            var factory = new DataStructureBuilderMock();
            ITreeItem rootItem = factory.BuildSampleData();

            (int level, ITreeItem item)[] queue = BuildQueue(0, rootItem);

            foreach ((int level, ITreeItem item) in queue)
            {
                WriteTreeItem(level, item);
            }
        }

        private (int level, ITreeItem item)[] BuildQueue(int level, ITreeItem treeItem)
        {
            List<(int level, ITreeItem item)> result = new()
            {
                (level,treeItem)
            };

            foreach (var item in treeItem.Children)
            {
                result.AddRange(BuildQueue(level+1, item));
            }

            return result.ToArray();
        }

        private void WriteTreeItem(int level, ITreeItem item)
        {
            string levelString = new string(Enumerable.Range(0, level).Select(_ => '-').ToArray());

            StringBuilder builder = new();
            builder.Append(levelString);
            builder.Append(item.Label);
            builder.Append($"({item.GetType().Name})");
            if (item is Revision revision)
            {
                builder.Append($" {revision.Version}");
            }


            Debug.WriteLine(builder.ToString());
        }

    
}
}