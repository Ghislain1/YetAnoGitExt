namespace YetAnoGitExt.ViewModels;

using Prism.Mvvm;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using YetAnoGitExt.Core;
using YetAnoGitExt.Core.Models;
using YetAnoGitExt.Core.Services;
using YetAnoGitExt.ViewModels.History;

public class LeftSideViewModel : BindableBase
{

    public ObservableCollection<ISelectableViewModel> Items { get; set; }

    private readonly IGitRevisionService gitRevisionService;
    public LeftSideViewModel(IGitRevisionService gitRevisionService)
    {
        this.gitRevisionService = gitRevisionService;
        this.Items = CreateData(gitRevisionService);
    }
    private ISelectableViewModel selectableViewModel;
    public ISelectableViewModel SelectableViewModel
    {
        get => this.selectableViewModel;
        set => this.SetProperty(ref this.selectableViewModel, value);
    }
    private static ObservableCollection<ISelectableViewModel> CreateData(IGitRevisionService gitRevisionService)
    {
        return new ObservableCollection<ISelectableViewModel>
        {
            new HistoryViewModel(gitRevisionService)
            {
                Code = 'M',
                Name = "History",
                Description = "View Branch History",
                Kind="History"
            },
           new HistoryViewModel(gitRevisionService)
            {
                Code = 'D',
                Name = "Branch",
                Description = "Manage Branches",
                Kind = "DirectionsFork"
            },
           new HistoryViewModel(gitRevisionService)
            {
                Code = 'P',
                Name = "Remote",
                Description = "Manage Remotes",
                Kind = "AcUnit"
            },
         new HistoryViewModel(gitRevisionService)
            {
                Code = 'P',
                Name = "Users",
                Description = "If it bleeds, we can kill it",
               Kind = "Account"
            }
               ,
           new HistoryViewModel(gitRevisionService)
            {
                Code = 'P',
                Name = "Teams",
                Description = "If it bleeds, we can kill it",
                 Kind = "Teams"
            }
        };
    }
}