using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Input;
using ContractChecker;
using ContractChecker.UI;
using RevisionViewer.Views;
using WpfCommon;
using WpfCommon.Command;

namespace EntryPoint
{
    public class Feature : NotificationBase
    {
        public string Label { get; set; }
        public ICommand Command { get; set; }
    }

    public class MainWindowViewModel : NotificationBase
    {
        public Feature[] Features { get; }

        public MainWindowViewModel()
        {
            Features = BuildSampleFeatureEntryPoints().ToArray();
        }

        IEnumerable<Feature> BuildSampleFeatureEntryPoints()
        {
            yield return new Feature()
            {
                Label = "Open Revision Viewer",
                Command = new DelegateCommand(() =>
                {
                    new Window()
                    {
                        Content = new RevisionTreeView()
                    }.Show();
                })
            };
            yield return new Feature()
            {
                Label = "Open Contract Checker",
                Command = new DelegateCommand(() =>
                {
                    var service = new DummyContractCheckService();
                    var lines = service.Lines;
                    var dataContext = new ContractCheckerViewModel(service, lines);

                    foreach (var id in Enumerable.Range(0,5))
                    {
                        service.Rules.Add(new ContractRule()
                        {
                            Name = $"Dummy {id}"
                        });
                    }

                    var view = new ContractCheckerWindow()
                    {
                        DataContext = dataContext,
                        WindowStartupLocation = WindowStartupLocation.CenterOwner
                    };
                    view.Show();
                })
            };
        }
    }
}