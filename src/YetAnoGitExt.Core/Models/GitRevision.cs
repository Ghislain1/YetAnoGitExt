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
using System.Linq;
using System.Text;
using System.Threading.Tasks;
public interface IGitItem
{
    ObjectId? ObjectId { get; }

    string? Guid { get; }
}
public class GitRevision : IGitItem
{


    public GitRevision(ObjectId objectId)
    {
        this.ObjectId = objectId;
    }
    public IReadOnlyList<ObjectId>? ParentIds { get; set; }


    public ObjectId? TreeGuid { get; internal set; }

    public DateTime AuthorDate => FromUnixTimeSeconds(AuthorUnixTime);
    public long AuthorUnixTime { get; set; }
    private static DateTime FromUnixTimeSeconds(long unixTime)
          => unixTime == 0 ? DateTime.MaxValue : DateTimeOffset.FromUnixTimeSeconds(unixTime).LocalDateTime;
    public string? Author { get; internal set; }
    public string? ReflogSelector { get; internal set; }
    public string? AuthorEmail { get; internal set; }
    public DateTime CommitDate => FromUnixTimeSeconds(CommitUnixTime);
    public string? Committer { get; set; }
    public string? CommitterEmail { get; set; }
    public long CommitUnixTime { get; set; }
    public string Subject { get; set; } = "";
    public bool HasMultiLineMessage { get; set; }
    public bool HasNotes { get; set; }
    public override string ToString() => $"{this.ObjectId.ToShortString()}:{Subject}";
    public ObjectId ObjectId { get; }
    string? _body;
    public string? Body
    {
        // Body is not stored by default for older commits to reduce memory usage
        // Body do not have to be stored explicitly if same as subject and not multiline
        get => _body ?? (!HasMultiLineMessage ? Subject : null);
        set => _body = value;
    }
    public string Guid => ObjectId.ToString();

    public GitRevision Clone()
    {
        return (GitRevision)MemberwiseClone();
    }
}