using System.Collections.ObjectModel;
using System.Linq;
using System.Windows.Input;
using WpfCommon;
using WpfCommon.Command;

namespace ContractChecker.UI
{
    public interface IWindowCloseMessenger
    {
        void Close();
    }

    public class RuleEditorViewModel : NotificationBase
    {
        public ObservableCollection<RuleViewModel> Rules { get; }
        public ICommand AddRuleCommand { get; }
        public ICommand<RuleViewModel> RemoveRuleCommand { get; }
        public ICommand ApplyCommand { get; }
        public ICommand DiscardCommand { get; }

        private bool _isApplied;

        public bool IsApplied
        {
            get => _isApplied;
            set => SetProperty(ref _isApplied, value);
        }

        public RuleEditorViewModel(IContractCheckService contractCheckService, IWindowCloseMessenger closeMessenger)
        {
            Rules = new ObservableCollection<RuleViewModel>(contractCheckService.Rules.Select(x => new RuleViewModel(x)));

            AddRuleCommand = new DelegateCommand(() =>
            {
                var rule = new ContractRule();
                Rules.Add(new RuleViewModel(rule));
            });

            RemoveRuleCommand = new DelegateCommand<RuleViewModel>((deleteViewModel) =>
            {
                Rules.Remove(deleteViewModel);
            });

            ApplyCommand = new DelegateCommand(() =>
            {
                contractCheckService.Rules.Clear();

                foreach (var rule in Rules)
                {
                    contractCheckService.Rules.Add(rule.Model);
                }
                IsApplied = true;
                closeMessenger.Close();
            });

            DiscardCommand = new DelegateCommand(closeMessenger.Close);
        }
    }
}