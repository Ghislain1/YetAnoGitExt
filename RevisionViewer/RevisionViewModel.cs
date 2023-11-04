using SvgResourceGenerator;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel; 
using System.Linq; 
using System.Threading.Tasks;
using WpfCommon;
using WpfCommon.Command;

namespace RevisionViewer
{
    public class RevisionViewModel : NotificationBase, ITreeItemViewModel
    {
        private readonly Dictionary<int, IconType> LevelToIconTypes = new Dictionary<int, IconType> { { 0, IconType.Icon_Home_Svgs }, { 1, IconType.Icon_List_Svgs }, { 2, IconType.Icon_Location_Pin_Svgs }, { 3, IconType.Icon_Format_1_Svgs } };
        private ITreeItem model;

        public RevisionViewModel(int level, Revision item)
        {
            Level = level;
            model = item;
            Title = item.Label;
            IsSelected = false;
            IsExpanded = false;
            LastModifiedAt = item.LastModified;
            Version = item.Version;
            IconType = level switch
            {
                0 => LevelToIconTypes[level],
                1 => LevelToIconTypes[level],
                2 => LevelToIconTypes[level],
                3 => LevelToIconTypes[level],
                _ => LevelToIconTypes[2]
            };
            if (level==0)
            {
                 IsViewHistoryButtonVisible = true;
                 IsHideHistoryButtonVisible = false;
                 IsShowHistoryButtonVisible = false;
            }
            if(!(model.Children?.Any()== true))
            {
                IsSetLatestButtonVisible = true;    
            }

            // Comands : TODO@GhZe later

            ViewHistoryCommand = new AsyncDelegateCommand<object>((input) =>
            {
                if (input is not RevisionViewModel revisionViewModel)
                {
                    return Task.Delay(100);
                }
                revisionViewModel.IsHideHistoryButtonVisible = true;
                revisionViewModel.IsShowHistoryButtonVisible = false;
                revisionViewModel.IsExpanded = true;

                return Task.Delay(100);
            });
            SetLatestCommand = new AsyncDelegateCommand<object>((input) =>
            {
                if (input is not RevisionViewModel revisionViewModel)
                {
                    return Task.Delay(100);
                }
                revisionViewModel.LastModifiedAt = DateTime.Now;
               
                      return Task.Delay(100);
            });

            HideHistoryCommand = new AsyncDelegateCommand<object>((input) => 
            {

                if (input is not RevisionViewModel revisionViewModel)
                {
                    return Task.Delay(100);
                }
                revisionViewModel.IsHideHistoryButtonVisible = false;
                revisionViewModel.IsShowHistoryButtonVisible = true;
                revisionViewModel.IsExpanded = false;

                return Task.Delay(100); 
            });

           ShowHistoryCommand = new AsyncDelegateCommand<object>(( input) =>
            {
                if(input is not RevisionViewModel revisionViewModel)
                {
                    return Task.Delay(100);
                }
                revisionViewModel.IsHideHistoryButtonVisible = true;
                revisionViewModel.IsShowHistoryButtonVisible = false;
                revisionViewModel.IsExpanded = true;

                return Task.Delay(100);
            });


        }

        private string _title;
        public string Title
        {
            get => _title;
            set => SetProperty(ref _title, value);
        }
        private string _version;
        public string Version
        {
            get => _version;
            set => SetProperty(ref _version, value);
        }

        private bool _isViewHistoryButtonVisible;
        public bool IsViewHistoryButtonVisible
        {
            get => _isViewHistoryButtonVisible;
            set => SetProperty(ref _isViewHistoryButtonVisible, value);
        }
        private bool _isHideHistoryButtonVisible;
        public bool IsHideHistoryButtonVisible
        {
            get => _isHideHistoryButtonVisible;
            set => SetProperty(ref _isHideHistoryButtonVisible, value);
        }
        private bool _isShowHistoryButtonVisible;
        public bool IsShowHistoryButtonVisible
        {
            get => _isShowHistoryButtonVisible;
            set => SetProperty(ref _isShowHistoryButtonVisible, value);
        }
        private bool _isSetLatestButtonVisible;
        public bool IsSetLatestButtonVisible
        {
            get => _isSetLatestButtonVisible;
            set => SetProperty(ref _isSetLatestButtonVisible, value);
        }
        

        private DateTime _lastModifiedAt;

        public DateTime LastModifiedAt
        {
            get => _lastModifiedAt;
            set => SetProperty(ref _lastModifiedAt, value);
        }

        private ObservableCollection<ITreeItemViewModel> children;
        public ObservableCollection<ITreeItemViewModel> Children
        {
            get => children;
            set => SetProperty(ref children, value);
        }

        private bool _isSelected;

        public bool IsSelected
        {
            get => _isSelected;
            set => SetProperty(ref _isSelected, value);
        }

        private int _level;
        public int Level
        {
            get => _level;
            set
            {
                if (SetProperty(ref _level, value))
                {

                }
            }
        }

        private string _iconName;
        public string IconName
        {
            get => _iconName;
            set => SetProperty(ref _iconName, value);
        }

        private IconType _iconType;
        public IconType IconType
        {
            get => _iconType;
            set => SetProperty(ref _iconType, value);
        }


        /// <summary>
        /// Indicates if this item can be expanded
        /// </summary>
        public bool CanExpand => model.Children?.Any() == true;
        private bool _isExpanded;
        /// <summary>
        /// Indicates if the current item is expanded or not
        /// </summary>
        public bool IsExpanded
        {
            get => _isExpanded;

            set
            {
                SetProperty(ref _isExpanded, value);

                // If the UI tells us to expand...
                if (value == true)
                {
                    // Find all children
                    Expand();
                    this.IsShowHistoryButtonVisible = false;
                    this.IsHideHistoryButtonVisible = true;
                }

                // If the UI tells us to close
                else
                {
                    ClearChildren();
                }

            }
        }

        /// <summary>
        /// Removes all children from the list, adding a dummy item to show the expand icon if required
        /// </summary>
        private void ClearChildren()
        {
            // Clear items
            Children = new ObservableCollection<ITreeItemViewModel>();

            // Show the expand arrow if we are not  last level
            if (model?.Children?.Any() == true)
            {
                Children.Add(null);
            }

        }

        public IAsyncCommand HideHistoryCommand { get; }
        public IAsyncCommand ShowHistoryCommand { get; }
        public IAsyncCommand ViewHistoryCommand { get; }
        public IAsyncCommand SetLatestCommand { get; }
        


        /// <summary>
        ///  Expands current item  and finds all children
        /// </summary>
        private void Expand()
        {
            // We cannot expand when no child exists
            if (!(model.Children?.Any()==true))
            {
                return;
            }

            // Find all children
            var children = model.Children?.Select(content =>
            {
                var nextLevel = Level + 1;

                if (content is Separator separator)
                {
                    var separatorVM = new SeparatorViewModel(nextLevel, separator);
                    return separatorVM;
                }
                else
                {
                    var vm = new RevisionViewModel(nextLevel, content as Revision);
                    return vm as ITreeItemViewModel;
                }


            });

            Children = new ObservableCollection<ITreeItemViewModel>(children.ToArray());

        }
    }


    public class SeparatorViewModel : NotificationBase, ITreeItemViewModel
    {

        public SeparatorViewModel(int level, Separator item)
        {
            this.Title = item.Label?.ToUpper();
        }
        private string _title;
        public string Title
        {
            get => _title;
            set => SetProperty(ref _title, value);
        }

        private ObservableCollection<ITreeItemViewModel> children;
        public ObservableCollection<ITreeItemViewModel> Children
        {
            get => children;
            set => SetProperty(ref children, value);
        }
    }

    public interface ITreeItemViewModel
    {
        public string Title { get; }
        public ObservableCollection<ITreeItemViewModel> Children { get; }
    }
}