using System;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media;
using SvgResourceGenerator;
using WpfCommon;
using WpfCommon.Command;

namespace ContractChecker.UI
{
    public class ContractCheckerViewModel : NotificationBase
    {
        private readonly IContractCheckService _contractCheckService;
        private ObservableCollection<ResultViewModel> _results = new();
        public ObservableCollection<ResultViewModel> Results
        {
            get => _results;
            set => SetProperty(ref _results, value);
        }
        public IAsyncCommand ExecuteContractCheckerCommand { get; }
        public IAsyncCommand<ResultViewModel> RecheckRuleCommand { get; }
        public ICommand OpenEditPreferenceCommand { get; }

        public int RulesItemsCount => _contractCheckService.Rules.Count;
        public int FulFilledItemsCount => Results.Count(x => x.ResultStatus == ResultStatus.FulFilled);
        public int ConflictedItemsCount => Results.Count(x => x.ResultStatus == ResultStatus.Conflicted);
        public int NotMentionItemsCount => Results.Count(x => x.ResultStatus == ResultStatus.NotMentioned);

        private bool _hasRuleChanged;

        public bool HasRuleChanged
        {
            get => _hasRuleChanged;
            set => SetProperty(ref _hasRuleChanged, value);
        }

        public ContractCheckerViewModel(IContractCheckService contractCheckService,string[] lines)
        {
            _contractCheckService = contractCheckService;
            ExecuteContractCheckerCommand = new AsyncDelegateCommand(async ()=>
            {
                var results = await contractCheckService.ValidateContractAsync(contractCheckService.Rules.ToArray(), lines);
                Results = new ObservableCollection<ResultViewModel>(results.Select(x=>new ResultViewModel(x,lines[x.LineId])));
                UpdateCounts();
            });

            RecheckRuleCommand = new AsyncDelegateCommand<ResultViewModel>(async (ruleViewModel) =>
            {
                ContractRule rule = contractCheckService.Rules.FirstOrDefault(x => x.Name == ruleViewModel.Rule);
                if (rule is null)
                {
                    return;
                }
                ResultViewModel replaceTarget = Results.FirstOrDefault(x => x.Rule == rule.Name);
                if (replaceTarget is null)
                {
                    return;
                }

                replaceTarget.IsLoading = true;

                ContractResult[] results = await contractCheckService.ValidateContractAsync(new[] { rule }, lines);
                Debug.Assert(results.Length > 0);
                ContractResult result = results[0];

                int index = Results.IndexOf(replaceTarget);
                Results.RemoveAt(index);
                Results.Insert(index,new ResultViewModel(result,lines[result.LineId]));
                UpdateCounts();
            });

            OpenEditPreferenceCommand = new DelegateCommand(() =>
            {
                var editorView = new RuleEditorWindow()
                {
                    WindowStartupLocation = WindowStartupLocation.CenterOwner,
                };
                var editorViewModel = new RuleEditorViewModel(contractCheckService,editorView);
                editorView.DataContext = editorViewModel;
                editorView.ShowDialog();

                OnPropertyChanged(nameof(RulesItemsCount));
                if (editorViewModel.IsApplied)
                {
                    HasRuleChanged = true;
                }
            });
        }

        private void UpdateCounts()
        {
            OnPropertyChanged(nameof(FulFilledItemsCount));
            OnPropertyChanged(nameof(ConflictedItemsCount));
            OnPropertyChanged(nameof(NotMentionItemsCount));
        }
    }

    public class ResultViewModel : NotificationBase
    {
        public ResultViewModel(ContractResult result, string line)
        {
            Rule = result.Rule;
            ResultStatus = result.ResultStatus;
            Line = line;
        }

        public ResultStatus ResultStatus { get; }

        public IconType Icon => GetIconType();

        public Color IconColor => GetIconColor();

        public string Line { get; }
        public string Rule { get; }

        private bool _isLoading;
        public bool IsLoading
        {
            get => _isLoading;
            set => SetProperty(ref _isLoading, value);
        }

        private IconType GetIconType()
        {
            return ResultStatus switch
            {
                ResultStatus.FulFilled => IconType.Icon_Check_Svgs,
                ResultStatus.NotMentioned => IconType.Icon_Question_Svgs,
                ResultStatus.Conflicted => IconType.Icon_X_Svgs,
                _ => throw new ArgumentOutOfRangeException()
            };
        }

        private Color GetIconColor()
        {
            return ResultStatus switch
            {
                ResultStatus.FulFilled => Colors.GreenYellow,
                ResultStatus.NotMentioned => Colors.DarkOrange,
                ResultStatus.Conflicted => Colors.IndianRed,
                _ => throw new ArgumentOutOfRangeException()
            };
        }
    }
}