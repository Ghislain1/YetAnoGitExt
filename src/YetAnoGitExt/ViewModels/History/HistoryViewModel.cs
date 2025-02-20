namespace YetAnoGitExt.ViewModels.History;

using Prism.Mvvm;

using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using YetAnoGitExt.Core;
using YetAnoGitExt.Core.Models;

public class HistoryViewModel : BindableBase, ISelectableViewModel
{
              private bool isSelected;
              private string? name;
              private string? description;
              private char code;
              private double numeric;
              private string? kind;
              private string? files;
              private string gitFullPathExe = "C:\\Program Files\\Git\\bin\\git.exe";
              private string workingDirectory = "C:\\git\\ZoeProg";
              private string revisionFilter = "--max-count=100000 --exclude=refs/notes/commits --all";

              private ObservableCollection<GitRevision> gitRevisionCollection = null!;

              private readonly IGitRevisionService gitRevisionService;
              public HistoryViewModel(IGitRevisionService gitRevisionService)
              {
                            this.gitRevisionService = gitRevisionService;
                            this.LoadRevisionsAsync();
              }
              private async Task LoadRevisionsAsync()
              {
                            var cancellationTokenSource = new CancellationTokenSource();

                            var arguments = "-c log.showSignature=false log -z --pretty=format:\"%H%T%P%n%at%n%ct%n%aN%n%aE%n%cN%n%cE%n%B\" --max-count=100000 --exclude=refs/notes/commits --all --";
                            var outputEncoding = Encoding.UTF8;
                            var items = await gitRevisionService.GetLogAsync(revisionFilter, cancellationTokenSource.Token, this.gitFullPathExe, this.workingDirectory, arguments, outputEncoding);
                            this.GitRevisionCollection = new ObservableCollection<GitRevision>(items);
                            this.GitRevisionCollection.First().IsLastCommitt = true;
              }

              public string WorkingDirectory
              {
                            get => this.workingDirectory;
                            set => this.SetProperty(ref this.workingDirectory, value);
              }
              public ObservableCollection<GitRevision> GitRevisionCollection
              {
                            get => this.gitRevisionCollection;
                            set => this.SetProperty(ref this.gitRevisionCollection, value);
              }
              private GitRevision selectedGitRevision;
              public GitRevision SelectedGitRevision
              {
                            get => this.selectedGitRevision;
                            set
                            {
                                          if (this.SetProperty(ref this.selectedGitRevision, value))
                                          {
                                                        // value.IsLastCommitt= true;
                                          }
                            }
              }
              public bool IsSelected
              {
                            get => this.isSelected;
                            set => this.SetProperty(ref this.isSelected, value);
              }

              public char Code
              {
                            get => this.code;
                            set => this.SetProperty(ref this.code, value);
              }

              public string? Name
              {
                            get => this.name;
                            set => this.SetProperty(ref this.name, value);
              }

              public string? Description
              {
                            get => this.description;
                            set => this.SetProperty(ref this.description, value);
              }

              public double Numeric
              {
                            get => this.numeric;
                            set => this.SetProperty(ref this.numeric, value);
              }

              public string? Kind
              {
                            get => this.kind;
                            set => this.SetProperty(ref this.kind, value);
              }

              public string? Files
              {
                            get => this.files;
                            set => this.SetProperty(ref this.files, value);
              }
}
