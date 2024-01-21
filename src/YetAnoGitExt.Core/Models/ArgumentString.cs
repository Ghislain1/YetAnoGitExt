// <copyright company="Ghislain One Inc.">
//  Copyright (c) GhislainOne
//  This computer program includes confidential, proprietary
//  information and is a trade secret of GhislainOne. All use,
//  disclosure, or reproduction is prohibited unless authorized in
//  writing by an officer of Ghis. All Rights Reserved.
// </copyright>

namespace YetAnoGitExt.Core.Models;

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

[DebuggerDisplay("{" + nameof(Arguments) + "}")]
public readonly struct ArgumentString
{
    public string? Arguments { get; }
    public int Length { get => Arguments?.Length ?? 0; }

    private ArgumentString(string arguments)
    {
        Arguments = arguments ?? throw new ArgumentNullException(nameof(arguments));
    }

    public static implicit operator ArgumentString(string? args) => new(args ?? "");
    public static implicit operator ArgumentString(ArgumentBuilder args) => new(args.ToString());
    public static implicit operator string(ArgumentString args) => args.Arguments ?? "";
    public override string ToString() => Arguments ?? "";
}
