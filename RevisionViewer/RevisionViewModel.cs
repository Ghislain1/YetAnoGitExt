using System.Collections.ObjectModel;
using WpfCommon;

namespace RevisionViewer
{
    public class RevisionViewModel : NotificationBase
    {
        public static RevisionViewModel CopyRevisionViewModel(RevisionViewModel revisionViewModel)
        {
            var rev = new RevisionViewModel();
            rev.Title = revisionViewModel.Title;
            rev.LastModifiedAt = revisionViewModel.LastModifiedAt;

            return rev;
        }

        private bool _isSeparator;
        public bool IsSeparator
        {
            get => _isSeparator;
            set => SetProperty(ref _isSeparator, value);
        }

        private string _title;
        public string Title
        {
            get => _title;
            set => SetProperty(ref _title, value);
        }

        private string _action;
        public string Action
        {
            get => _action;
            set => SetProperty(ref _action, value);
        }

        private string _version;
        public string Version
        {
            get => _version;
            set => SetProperty(ref _version, value);
        }

        private bool _isLatestVersion;
        public bool IsLatestVersion
        {
            get => _isLatestVersion;
            set => SetProperty(ref _isLatestVersion, value);
        }

        public string DefaultIcon { get; set; }

        private string _icon;
        public string Icon
        {
            get => _icon;
            set => SetProperty(ref _icon, value);
        }
        private string _lastModifiedAt;
        public string LastModifiedAt
        {
            get => _lastModifiedAt;
            set => SetProperty(ref _lastModifiedAt, value);
        }

        private ObservableCollection<RevisionViewModel> _children = new ();
        public ObservableCollection<RevisionViewModel> Children
        {
            get => _children;
            set => SetProperty(ref _children, value);
        }

        private ObservableCollection<RevisionViewModel> _revisions = new();
        public ObservableCollection<RevisionViewModel> Revisions
        {
            get => _revisions;
            set => SetProperty(ref _revisions, value);
        }

        private bool _isSelected;
        public bool IsSelected
        {
            get => _isSelected;
            set 
            { 
                SetProperty(ref _isSelected, value);
                Icon = value ? "Icon_Location_Pin_Svgs" : DefaultIcon;
            }            
        }

        private bool _isExpanded;
        public bool IsExpanded
        {
            get => _isExpanded;
            set
            {
                SetProperty(ref _isExpanded, value);
                Action = IsLatestVersion ? (value ? "Hide history" : "View history") : "Set latest";
            }
        }
    }
}