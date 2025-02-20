// <copyright company="Ghislain One Inc.">
//  Copyright (c) GhislainOne
//  This computer program includes confidential, proprietary
//  information and is a trade secret of GhislainOne. All use,
//  disclosure, or reproduction is prohibited unless authorized in
//  writing by an officer of Ghis. All Rights Reserved.
// </copyright>

namespace YetAnoGitExt.Core;

using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public class ArgumentBuilder : IEnumerable
{
              private readonly StringBuilder _arguments = new(capacity: 16);
              public bool IsEmpty => _arguments.Length == 0;

              public IEnumerator GetEnumerator()
              {
                            throw new NotImplementedException();
              }

              public void Add(string? s)
              {
                            if (string.IsNullOrWhiteSpace(s))
                            {
                                          return;
                            }

                            if (_arguments.Length != 0)
                            {
                                          _arguments.Append(' ');
                            }

                            _arguments.Append(s);
              }

              public void AddRange(IEnumerable<string> args)
              {
                            args = args.Where(a => !string.IsNullOrEmpty(a));
                            foreach (string s in args)
                            {
                                          Add(s);
                            }
              }

              public override string ToString()
              {
                            return _arguments.ToString();
              }
}
