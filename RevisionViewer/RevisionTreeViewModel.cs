using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WpfCommon;

namespace RevisionViewer
{
    public class RevisionTreeViewModel : NotificationBase
    {
        private ObservableCollection<RevisionViewModel> _revisions;

        public ObservableCollection<RevisionViewModel> Revisions
        {
            get => _revisions;
            set => SetProperty(ref _revisions, value);
        }
        public RevisionTreeViewModel(Revision[] revisions)
        {
            // do implement something.
            _ = LoadResivonViewModelAsync(revisions);
        }

        private async Task LoadResivonViewModelAsync(Revision[] revisionArray)
        {
            var revisionList = new List<RevisionViewModel>();
            // await Task.Run(() => revisionList.AddRange(revisionArray.SelectMany(root => BuildQueue(0, root)).Select(levelItem => CreateRevisionViewModel(levelItem.level, levelItem.item))));
            await Task.Run(() =>
            {
                foreach (var root in revisionArray)
                {
                    foreach (var levelItem in BuildQueue(0, root))
                    {
                        if (levelItem.level == 0 && levelItem.item is Revision revision)
                        {
                            revisionList.Add(new RevisionViewModel(levelItem.level, revision));
                        }
                    }
                }
            });



            Revisions = new ObservableCollection<RevisionViewModel>(revisionList);

            // Locals Methods
            (int level, ITreeItem item)[] BuildQueue(int level, ITreeItem treeItem)
            {
                List<(int level, ITreeItem item)> result = new() { (level, treeItem) };

                foreach (var item in treeItem.Children)
                {
                    result.AddRange(BuildQueue(level+1, item));
                }

                return result.ToArray();
            }
        }
    }
}