namespace YetAnoGitExt.ViewModels;

using Prism.Mvvm;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public class LeftSideViewModel : BindableBase
{

    public ObservableCollection<SelectableViewModel> Items { get; set; }


    public LeftSideViewModel()
    {
        this.Items = CreateData();
    }

    private static ObservableCollection<SelectableViewModel> CreateData()
    {
        return new ObservableCollection<SelectableViewModel>
        {
            new SelectableViewModel
            {
                Code = 'M',
                Name = "History",
                Description = "View Branch History",
                Kind="History"
            },
            new SelectableViewModel
            {
                Code = 'D',
                Name = "Branch",
                Description = "Manage Branches",
                Kind = "DirectionsFork"
            },
            new SelectableViewModel
            {
                Code = 'P',
                Name = "Remote",
                Description = "Manage Remotes",
                Kind = "AcUnit"
            },
               new SelectableViewModel
            {
                Code = 'P',
                Name = "Users",
                Description = "If it bleeds, we can kill it",
               Kind = "Account"
            }
               ,
                  new SelectableViewModel
            {
                Code = 'P',
                Name = "Teams",
                Description = "If it bleeds, we can kill it",
                 Kind = "Teams"
            }
        };
    }
}