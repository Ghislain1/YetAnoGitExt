// <copyright company="Ghislain One Inc.">
//  Copyright (c) GhislainOne
//  This computer program includes confidential, proprietary
//  information and is a trade secret of GhislainOne. All use,
//  disclosure, or reproduction is prohibited unless authorized in
//  writing by an officer of Ghis. All Rights Reserved.
// </copyright>

namespace YetAnoGitExt.Core.Consts;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

internal static class Delimiters {
              public static readonly char[] LineFeed = { '\n' };
              public static readonly char[] LineAndVerticalFeed = { '\n', '\v' };
              public static readonly char[] Space = { ' ' };
              public static readonly char[] Tab = { '\t' };
              public static readonly char[] Null = { '\0' };
              public static readonly char[] TabAndSpace = { '\t', ' ' };
              public static readonly char[] TabAndLineFeedAndCarriageReturn = { '\t', '\n', '\r' };
              public static readonly char[] NullAndLineFeed = { '\0', '\n' };
              public static readonly char[] LineFeedAndCarriageReturn = { '\n', '\r' };
              public static readonly char[] ForwardSlash = { '/' };
              public static readonly char[] Colon = { ':' };
              public static readonly char[] Comma = { ',' };
              public static readonly char[] Period = { '.' };
              public static readonly char[] Hash = { '#' };
              public static readonly char[] PathSeparators = { Path.DirectorySeparatorChar, Path.AltDirectorySeparatorChar };
}