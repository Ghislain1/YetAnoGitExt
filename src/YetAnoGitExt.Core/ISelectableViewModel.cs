﻿// <copyright company="Ghislain One Inc.">
//  Copyright (c) GhislainOne
//  This computer program includes confidential, proprietary
//  information and is a trade secret of GhislainOne. All use,
//  disclosure, or reproduction is prohibited unless authorized in
//  writing by an officer of Ghis. All Rights Reserved.
// </copyright>

namespace YetAnoGitExt.Core;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public interface ISelectableViewModel
{
              string? Name { get; set; }
              string? Description { get; set; }
              string? Kind { get; set; }
              bool IsSelected { get; set; }
              char Code { get; set; }
}
