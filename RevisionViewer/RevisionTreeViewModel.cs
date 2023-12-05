using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Windows.Input;
using WpfCommon;
using System.Linq;
using WpfCommon.Command;

namespace RevisionViewer
{
    public class RevisionTreeViewModel : NotificationBase
    {
        #region Commands
        public ICommand TreeView_ActionCommand() => new DelegateCommand<RevisionViewModel>(TreeView_Action);

        private void TreeView_Action(RevisionViewModel revisionViewModel)
        {
            //  If revisionViewModel is null, return
            if (revisionViewModel == null)
                return;

            //Check if that fle has a version, if it doesn't, go out
            if (string.IsNullOrEmpty(revisionViewModel.Version))
                return;

            //  If revision view model is latest version, expand / collapse
            if (revisionViewModel.IsLatestVersion)
            {
                revisionViewModel.IsExpanded ^= true;
                revisionViewModel.IsSelected = true;
            }

            //  If revision is not latest version, set it on the position of the Parent (of Revisions), as latest version,
            //  move previous latest version to it's position, and set 'Latest version' to false for it
            //  re-order and reload items

            else
            {
                //  Find position of the parent element 
                var positionOfParentElementInMainArray = Array.FindIndex(RootItem.Children, item => item.Label == revisionViewModel.Title);

                //  Get parent element
                var parentElement = (Revision)RootItem.Children.FirstOrDefault(item => item.Label == revisionViewModel.Title);
                //  Create backup of the parent element (that will eb set on the position of the new 'Latest Version')
                var backUpParentElement = new Revision();
                backUpParentElement.Label = parentElement.Label;
                backUpParentElement.LastModified = parentElement.LastModified;
                backUpParentElement.Version = parentElement.Version;

                var newRevisionElement = new Revision();
                newRevisionElement.Version = revisionViewModel.Version;
                newRevisionElement.Label = revisionViewModel.Title;
                newRevisionElement.LastModified = DateTime.Parse(revisionViewModel.LastModifiedAt);

                foreach (ITreeItem item in parentElement.Children)
                {
                    if (item is Revision itemRevision1)
                    {
                        if (itemRevision1.Version != revisionViewModel.Version)
                        {
                            newRevisionElement.Children = newRevisionElement.Children.Append(itemRevision1).ToArray();
                        }
                        else
                        {
                            newRevisionElement.Children = newRevisionElement.Children.Append(backUpParentElement).ToArray();
                        }
                    }
                    else if (item is Separator itemSeparator)
                    {
                        newRevisionElement.Children = newRevisionElement.Children.Append(itemSeparator).ToArray();
                    }
                }


                RootItem.Children[positionOfParentElementInMainArray] = newRevisionElement;
                (int level, ITreeItem item)[] queue = BuildQueue(0, RootItem);
                RevisionViewModel revisionViewModelF = CreateRevisionViewModel(queue);

                Revisions.Clear();

                Revisions.Add(revisionViewModelF);

                Revisions[0].Children[positionOfParentElementInMainArray].IsExpanded = true;
                Revisions[0].Children[positionOfParentElementInMainArray].IsSelected = true;

            }
        }

        #endregion


        #region Observable properties
        private string _headline= @"Lorem ipsum dolor sit amet, consectetur adipiscing elit, sed do eiusmod tempor incididunt ut labore et dolore magna aliqua. " + 
                                    "Ut enim ad minim veniam, quis nostrud exercitation ullamco laboris nisi ut aliquip ex ea.";
        public string Headline
        {
            get => _headline;
            set => SetProperty(ref _headline, value);
        }

        private ObservableCollection<RevisionViewModel> _revisions = new();

        public ObservableCollection<RevisionViewModel> Revisions
        {
            get => _revisions;
            set => SetProperty(ref _revisions, value);
        }

        #endregion

        #region Constructor

        private ITreeItem RootItem;
        public RevisionTreeViewModel()
        {
            var factory = new DataStructureBuilderMock();
            RootItem = factory.BuildSampleData();

            (int level, ITreeItem item)[] queue = BuildQueue(0, RootItem);
            RevisionViewModel revisionViewModel = CreateRevisionViewModel(queue);
            Revisions.Add(revisionViewModel);

        }


        private RevisionViewModel CreateRevisionViewModel((int level, ITreeItem item)[] queue)
        {
            RevisionViewModel revisionViewModelLevel0 = new RevisionViewModel();
            RevisionViewModel revisionViewModelLevel1 = new RevisionViewModel();
            RevisionViewModel revisionViewModelLevel2 = new RevisionViewModel();
            RevisionViewModel revisionViewModelLevel3 = new RevisionViewModel();

            foreach (var item in queue)
            {
                if (item.level == 0)
                {
                    if (item.item is Revision itemRevision)
                    {
                        revisionViewModelLevel0 = new RevisionViewModel
                        {
                            Title = itemRevision.Label,
                            Version = itemRevision.Version,
                            LastModifiedAt = itemRevision.LastModified.ToString("yyyy.MM.dd h:mm ") + itemRevision.LastModified.ToString("tt").ToLower(),
                            Children = new ObservableCollection<RevisionViewModel>(),
                            Icon = GetIcon(item.level),
                            IsLatestVersion = true,
                            DefaultIcon = GetIcon(item.level),
                            IsExpanded = true
                    };
                    }
                }
                if (item.level == 1)
                {
                    if (item.item is Revision itemRevision)
                    {
                        revisionViewModelLevel1 = new RevisionViewModel
                        {
                            Title = itemRevision.Label,
                            Version = itemRevision.Version,
                            LastModifiedAt = itemRevision.LastModified.ToString("yyyy.MM.dd h:mm ") + itemRevision.LastModified.ToString("tt").ToLower(),
                            Children = new ObservableCollection<RevisionViewModel>(),
                            Icon = GetIcon(item.level),
                            DefaultIcon = GetIcon(item.level),
                            Action = "View history",
                            IsExpanded = false,
                        };
                    }
                    else if (item.item is Separator itemSeparator)
                    {
                        revisionViewModelLevel1 = new RevisionViewModel
                        {
                            Title = $"{itemSeparator.Label}",
                            IsSeparator = true,
                            
                        };
                    }
                    if (revisionViewModelLevel1.Title == revisionViewModelLevel0.Title)
                        revisionViewModelLevel0.Revisions.Add(revisionViewModelLevel1);
                    if (revisionViewModelLevel0.Revisions?.Count > 0)
                    {
                        revisionViewModelLevel0.IsLatestVersion = true;
                        revisionViewModelLevel0.Action = "View history";
                    }
                    revisionViewModelLevel0.Children.Add(revisionViewModelLevel1);
                }
                if (item.level == 2)
                {
                    revisionViewModelLevel2 = new RevisionViewModel();
                    if (item.item is Revision itemRevision)
                    {
                        revisionViewModelLevel2 = new RevisionViewModel
                        {
                            Title = itemRevision.Label,
                            Version = itemRevision.Version,
                            LastModifiedAt = itemRevision.LastModified.ToString("yyyy.MM.dd h:mm ") + itemRevision.LastModified.ToString("tt").ToLower(),
                            Children = new ObservableCollection<RevisionViewModel>(),
                            Icon = GetIcon(item.level),
                            DefaultIcon = GetIcon(item.level), 
                            Action = "Set latest",
                            IsExpanded = false
                        };
                    }
                    else if (item.item is Separator itemSeparator)
                    {
                        revisionViewModelLevel2 = new RevisionViewModel
                        {
                            Title = $"{itemSeparator.Label}",
                            IsSeparator = true,
                        };
                    }
                    if (revisionViewModelLevel2.Title == revisionViewModelLevel1.Title)
                        revisionViewModelLevel1.Revisions.Add(revisionViewModelLevel2);
                    if (revisionViewModelLevel1.Revisions?.Count > 0)
                    {
                        revisionViewModelLevel1.IsLatestVersion = true;
                        revisionViewModelLevel1.Action = "View history";
                    }
                    revisionViewModelLevel1.Children.Add(revisionViewModelLevel2);
                }
                if (item.level == 3)
                {
                    revisionViewModelLevel3 = new RevisionViewModel();
                    if (item.item is Revision itemRevision)
                    {
                        revisionViewModelLevel3 = new RevisionViewModel
                        {
                            Title = itemRevision.Label,
                            Version = itemRevision.Version,
                            LastModifiedAt = itemRevision.LastModified.ToString("yyyy.MM.dd h:mm ") + itemRevision.LastModified.ToString("tt").ToLower(),
                            Children = new ObservableCollection<RevisionViewModel>(),
                            Icon = GetIcon(item.level),
                            DefaultIcon = GetIcon(item.level),
                            Action = "Set latest",
                            IsExpanded = false
                        };
                    }
                    else if (item.item is Separator itemSeparator)
                    {
                        revisionViewModelLevel3 = new RevisionViewModel
                        {
                            Title = $"{itemSeparator.Label}",
                            IsSeparator = true,
                        };
                    }
                    if (revisionViewModelLevel3.Title == revisionViewModelLevel2.Title)
                        revisionViewModelLevel2.Revisions.Add(revisionViewModelLevel3);
                    if (revisionViewModelLevel2.Revisions?.Count > 0)
                    {
                        revisionViewModelLevel2.IsLatestVersion = true;
                        revisionViewModelLevel2.Action = "View history";
                    }
                    revisionViewModelLevel2.Children.Add(revisionViewModelLevel3);
                }
            }

            return revisionViewModelLevel0;
        }

        private static string GetIcon(int level)
        {
            return level switch
            {
                0 => "Icon_Home_Svgs",
                1 => "Icon_Doc_Svgs",
                2 => "Icon_Clock_Svgs",
                3 => "Icon_Thunder_Svgs",
                _ => "",
            };
        }

        private (int level, ITreeItem item)[] BuildQueue(int level, ITreeItem treeItem)
        {
            List<(int level, ITreeItem item)> result = new()
            {
                (level,treeItem)
            };

            foreach (var item in treeItem.Children)
            {
                result.AddRange(BuildQueue(level + 1, item));
            }

            return result.ToArray();
        }



        //public RevisionTreeViewModel()
        //{
        //    var factory = new DataStructureBuilderMock();
        //    ITreeItem rootItem = factory.BuildSampleData();

        //    RevisionViewModel revisionViewModel = CreateRevisionViewModel(0, rootItem);
        //    Revisions.Add(revisionViewModel);
        //}

        #endregion


        #region Funcs
        //private RevisionViewModel CreateRevisionViewModel(int level, ITreeItem item)
        //{
        //    RevisionViewModel revisionViewModel = new RevisionViewModel();

        //    if (item is Revision itemRevision)
        //    {
        //        revisionViewModel = new RevisionViewModel
        //        {
        //            Title = itemRevision.Label,
        //            Version = itemRevision.Version,
        //            LastModifiedAt = itemRevision.LastModified.ToString("yyyy.MM.dd h:mm ") + itemRevision.LastModified.ToString("tt").ToLower(),
        //            Children = new ObservableCollection<RevisionViewModel>(),
        //            Icon = GetIcon(level),
        //            DefaultIcon = GetIcon(level)
        //        };
        //    }
        //    else if (item is Separator itemSeparator)
        //    {
        //        revisionViewModel = new RevisionViewModel
        //        {
        //            Title = $"{itemSeparator.Label}",
        //            IsSeparator = true,
        //        };
        //    }

        //    foreach (var child in item.Children)
        //    {
        //        revisionViewModel.Children.Add(CreateRevisionViewModel(level + 1, child));
        //    }            

        //    return revisionViewModel;
        //}



        #endregion
    }
}


//public RevisionTreeViewModel()
//{
//    var factory = new DataStructureBuilderMock();
//    ITreeItem rootItem = factory.BuildSampleData();

//    (int level, ITreeItem item)[] queue = BuildQueue(0, rootItem);
//    RevisionViewModel revisionViewModel = CreateRevisionViewModel(queue);
//    Revisions.Add(revisionViewModel);

//}

//private RevisionViewModel CreateRevisionViewModel((int level, ITreeItem item)[] queue)
//{
//    RevisionViewModel revisionViewModelLevel0 = new RevisionViewModel();
//    RevisionViewModel revisionViewModelLevel1 = new RevisionViewModel();
//    RevisionViewModel revisionViewModelLevel2 = new RevisionViewModel();
//    RevisionViewModel revisionViewModelLevel3 = new RevisionViewModel();

//    foreach (var item in queue)
//    {
//        if (item.level == 0)
//        {
//            if (item.item is Revision itemRevision)
//            {
//                revisionViewModelLevel0 = new RevisionViewModel
//                {
//                    Title = $"{itemRevision.Label}{(!string.IsNullOrEmpty(itemRevision.Version) ? $" {itemRevision.Version}" : string.Empty)}",
//                    LastModifiedAt = itemRevision.LastModified,
//                    Children = new ObservableCollection<RevisionViewModel>(),
//                    Icon = "Icon_Doc_Svgs"
//                };
//            }
//        }
//        if (item.level == 1)
//        {
//            if (item.item is Revision itemRevision)
//            {
//                revisionViewModelLevel1 = new RevisionViewModel
//                {
//                    Title = $"{itemRevision.Label}{(!string.IsNullOrEmpty(itemRevision.Version) ? $" {itemRevision.Version}" : string.Empty)}",
//                    LastModifiedAt = itemRevision.LastModified,
//                    Children = new ObservableCollection<RevisionViewModel>(),
//                    Icon = "Icon_Angle_Down_Svgs"
//                };
//            }
//            else if (item.item is Separator itemSeparator)
//            {
//                revisionViewModelLevel1 = new RevisionViewModel
//                {
//                    Title = $"{itemSeparator.Label}"
//                };
//            }
//            revisionViewModelLevel0.Children.Add(revisionViewModelLevel1);
//        }
//        if (item.level == 2)
//        {
//            revisionViewModelLevel2 = new RevisionViewModel();
//            if (item.item is Revision itemRevisionLevel)
//            {
//                revisionViewModelLevel2 = new RevisionViewModel
//                {
//                    Title = $"{itemRevisionLevel.Label}{(!string.IsNullOrEmpty(itemRevisionLevel.Version) ? $" {itemRevisionLevel.Version}" : string.Empty)}",
//                    LastModifiedAt = itemRevisionLevel.LastModified,
//                    Children = new ObservableCollection<RevisionViewModel>(),
//                    Icon = "Icon_Arrow_Below_Svgs"
//                };
//            }
//            else if (item.item is Separator itemSeparator)
//            {
//                revisionViewModelLevel2 = new RevisionViewModel
//                {
//                    Title = $"{itemSeparator.Label}"
//                };
//            }
//            revisionViewModelLevel1.Children.Add(revisionViewModelLevel2);
//        }
//        if (item.level == 3)
//        {
//            revisionViewModelLevel3 = new RevisionViewModel();
//            if (item.item is Revision itemRevisionLevel)
//            {
//                revisionViewModelLevel3 = new RevisionViewModel
//                {
//                    Title = $"{itemRevisionLevel.Label}{(!string.IsNullOrEmpty(itemRevisionLevel.Version) ? $" {itemRevisionLevel.Version}" : string.Empty)}",
//                    LastModifiedAt = itemRevisionLevel.LastModified,
//                    Children = new ObservableCollection<RevisionViewModel>(),
//                    Icon = "Icon_Caution_Triangle_Svgs"
//                };
//            }
//            else if (item.item is Separator itemSeparator)
//            {
//                revisionViewModelLevel3 = new RevisionViewModel
//                {
//                    Title = $"{itemSeparator.Label}"
//                };
//            }
//            revisionViewModelLevel2.Children.Add(revisionViewModelLevel3);
//        }
//    }

//    return revisionViewModelLevel0;
//}

//private RevisionViewModel CreateRevisionViewModel(Revision revision)
//{
//    RevisionViewModel revisionViewModel = new RevisionViewModel
//    {
//        Title = $"{revision.Label}{(!string.IsNullOrEmpty(revision.Version) ? $" {revision.Version}" : string.Empty)}",
//        LastModifiedAt = revision.LastModified,
//        Children = new ObservableCollection<RevisionViewModel>(),
//    };

//    foreach (var item in revision.Children)
//    {
//        if (item is Revision itemRevision)
//        {
//            RevisionViewModel childViewModel = CreateRevisionViewModel(itemRevision);
//            revisionViewModel.Children.Add(childViewModel);
//        }
//        else if (item is Separator sep)
//        {
//            revisionViewModel.Children.Add(new RevisionViewModel
//            {
//                Title = sep.Label
//            });
//        }
//    }

//    return revisionViewModel;
//}

//public RevisionTreeViewModel(Revision[] revisions)
//{
//    // do implement something.

//    foreach (Revision revision in revisions)
//    {
//        RevisionViewModel revisionViewModelMain = new ();
//        revisionViewModelMain.Title = $"{revision.Label}{(!string.IsNullOrEmpty(revision.Version) ? $" {revision.Version}" : string.Empty)}" ;
//        revisionViewModelMain.LastModifiedAt = revision.LastModified;
//        revisionViewModelMain.Children = new();
//        revisionViewModelMain.IsSelected = true;
//        foreach (var item in revision.Children)
//        {
//            RevisionViewModel revisionViewModelFirst = new();
//            if (item is Revision itemRevision)
//            {
//                revisionViewModelFirst.Title = $"{itemRevision.Label}{(!string.IsNullOrEmpty(itemRevision.Version) ? $" {itemRevision.Version}" : string.Empty)}";
//                revisionViewModelFirst.LastModifiedAt = itemRevision.LastModified;

//                if (itemRevision?.Children?.Length > 0)
//                {
//                    foreach (var itemRevision1 in itemRevision.Children)
//                    {
//                        RevisionViewModel revisionViewModelSecond = new();

//                        if (itemRevision1 is Revision itemRevision2)
//                        {
//                            revisionViewModelSecond.Title = $"{itemRevision2.Label}{(!string.IsNullOrEmpty(itemRevision2.Version) ? $" {itemRevision2.Version}" : string.Empty)}";
//                            revisionViewModelSecond.LastModifiedAt = itemRevision2.LastModified;
//                        }
//                        else if (item is Separator sep1)
//                        {
//                            revisionViewModelSecond.Title = $"{sep1.Label}";
//                        }
//                        revisionViewModelFirst.Children.Add(revisionViewModelSecond);
//                    };
//                }
//            }
//            else if (item is Separator sep)
//            {
//                revisionViewModelFirst.Title = $"{sep.Label}";
//            }
//            revisionViewModelMain.Children.Add(revisionViewModelFirst);
//        };

//        Revisions.Add(revisionViewModelMain);
//    }
//}