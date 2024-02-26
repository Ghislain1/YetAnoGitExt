namespace YetAnoGitExt.ViewModels;

using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public class LeftSideViewModel
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
                Name = "Material Design",
                Description = "Material Design in XAML Toolkit",
                Kind="Grid"
            },
            new SelectableViewModel
            {
                Code = 'D',
                Name = "Dragablz",
                Description = "Dragablz Tab Control",
                Kind = "Fries"
            },
            new SelectableViewModel
            {
                Code = 'P',
                Name = "Predator",
                Description = "If it bleeds, we can kill it",
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