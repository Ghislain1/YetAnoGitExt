// <copyright company="Ghislain One Inc.">
//  Copyright (c) GhislainOne
//  This computer program includes confidential, proprietary
//  information and is a trade secret of GhislainOne. All use,
//  disclosure, or reproduction is prohibited unless authorized in
//  writing by an officer of Ghis. All Rights Reserved.
// </copyright>

namespace EntryPoint
{
    using NLog.Config;
    using RevisionViewer;
    using System;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using WpfCommon;

    internal class MainWindowViewModel : NotificationBase
    {
        private readonly DataStructureBuilderMock _dataStructureBuilderMock;
        public MainWindowViewModel(DataStructureBuilderMock dataStructureBuilderMock)
        {
            _dataStructureBuilderMock = dataStructureBuilderMock;
            this.PopulateTreeViewAsync();
        }
        private ObservableCollection<RevisionTreeViewModel> _revisionTreeCollection;
        public ObservableCollection<RevisionTreeViewModel> RevisionTreeCollection
        {
            get => _revisionTreeCollection;
            set => SetProperty(ref _revisionTreeCollection, value);
        }

        private async Task PopulateTreeViewAsync()
        {
            var dataResult = new List<RevisionTreeViewModel>();
            await Task.Run(() =>
            {
                var root = _dataStructureBuilderMock.BuildSampleData();
                if (root is Revision revision)
                {
                    dataResult.Add(new RevisionTreeViewModel(new[] { revision }));
                }
            });

            RevisionTreeCollection = new ObservableCollection<RevisionTreeViewModel>(dataResult);
        }
    }
}

