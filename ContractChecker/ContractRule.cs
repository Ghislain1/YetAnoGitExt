using WpfCommon;

namespace ContractChecker
{
    public class ContractRule : NotificationBase
    {
        private string _name = string.Empty;
        public string Name
        {
            get => _name;
            set => SetProperty(ref _name, value);
        }
    }
}