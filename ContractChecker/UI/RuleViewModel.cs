using WpfCommon;

namespace ContractChecker.UI
{
    public class RuleViewModel : NotificationBase
    {
        public ContractRule Model { get; }
        private string _rule;

        public string Rule
        {
            get => _rule;
            set => SetProperty(ref _rule, value);
        }

        public RuleViewModel(ContractRule model)
        {
            Model = model;
            Rule = model.Name;
        }
    }
}