// <copyright company="Ghislain One Inc.">
//  Copyright (c) GhislainOne
//  This computer program includes confidential, proprietary
//  information and is a trade secret of GhislainOne. All use,
//  disclosure, or reproduction is prohibited unless authorized in
//  writing by an officer of Ghis. All Rights Reserved.
// </copyright>

namespace YetAnoGitExt;

using Prism.Mvvm;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using YetAnoGitExt.Core;
using YetAnoGitExt.Core.Models;

public  class ShellWindowViewModel: BindableBase
{
    private IGitRevisionService gitRevisionService;
    private string gitFullPathExe = "C:\\Program Files\\Git\\bin\\git.exe";
    private string workingDirectory = "C:\\git\\ZoeProg";
    private string revisionFilter = "--max-count=100000 --exclude=refs/notes/commits --all";
    public string WorkingDirectory
    {
        get =>  this.workingDirectory; 
        set => this.SetProperty(ref this.workingDirectory, value); 
    }
    private ObservableCollection<GitRevision> gitRevisionCollection;
    public ObservableCollection<GitRevision> GitRevisionCollection
    {
        get => this.gitRevisionCollection;
        set => this.SetProperty(ref this.gitRevisionCollection, value);
    }
    
    public ShellWindowViewModel()
    {
        this.gitRevisionService= new GitRevisionService();
    
        this.LoadDataAsync();
    }

    private async Task LoadDataAsync()
    {
        var cancellationTokenSource = new CancellationTokenSource();    
        
        string arguments = "-c log.showSignature=false log -z --pretty=format:\"%H%T%P%n%at%n%ct%n%aN%n%aE%n%cN%n%cE%n%B\" --max-count=100000 --exclude=refs/notes/commits --all --";
       var outputEncoding = Encoding.UTF8;
        var items = await gitRevisionService.GetLogAsync(revisionFilter,cancellationTokenSource.Token, this.gitFullPathExe, this.workingDirectory, arguments, outputEncoding);
        this.GitRevisionCollection = new ObservableCollection<GitRevision>(items);
      
    }
}
