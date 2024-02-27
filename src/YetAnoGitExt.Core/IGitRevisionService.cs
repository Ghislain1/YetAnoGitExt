// <copyright company="Ghislain One Inc.">
//  Copyright (c) GhislainOne
//  This computer program includes confidential, proprietary
//  information and is a trade secret of GhislainOne. All use,
//  disclosure, or reproduction is prohibited unless authorized in
//  writing by an officer of Ghis. All Rights Reserved.
// </copyright>

namespace YetAnoGitExt.Core;

using CommunityToolkit.HighPerformance.Buffers;
using System.Buffers.Text;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading;
using YetAnoGitExt.Core.Consts;
using YetAnoGitExt.Core.Extensions;
using YetAnoGitExt.Core.Models;

public interface IGitRevisionService
{
    List<GitRevision> GetLog(string revisionFilter, CancellationToken cancellationToken, string gitFullPathExe, string workingDirectory, string arguments, Encoding? outputEncoding = null);
    Task<List<GitRevision>> GetLogAsync(string revisionFilter, CancellationToken cancellationToken, string gitFullPathExe, string workingDirectory, string arguments, Encoding? outputEncoding = null);
}