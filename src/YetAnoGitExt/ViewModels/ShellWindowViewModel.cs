// <copyright company="Ghislain One Inc.">
//  Copyright (c) GhislainOne
//  This computer program includes confidential, proprietary
//  information and is a trade secret of GhislainOne. All use,
//  disclosure, or reproduction is prohibited unless authorized in
//  writing by an officer of Ghis. All Rights Reserved.
// </copyright>
namespace YetAnoGitExt.ViewModels;
using YetAnoGitExt;
using YetAnoGitExt.ViewModels;
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

public class ShellWindowViewModel : BindableBase
{
              private IGitRevisionService gitRevisionService;
              public LeftSideViewModel LeftSideViewModel { get; set; }
              public ShellWindowViewModel()
              {
                            this.gitRevisionService = new GitRevisionService();
                            this.LeftSideViewModel = new LeftSideViewModel(this.gitRevisionService);

              }

}
