// <copyright company="Ghislain One Inc.">
//  Copyright (c) GhislainOne
//  This computer program includes confidential, proprietary
//  information and is a trade secret of GhislainOne. All use,
//  disclosure, or reproduction is prohibited unless authorized in
//  writing by an officer of Ghis. All Rights Reserved.
// </copyright>

namespace YetAnoGitExt.Core.Services;
using YetAnoGitExt;
using YetAnoGitExt.Core;
using YetAnoGitExt.Core.Services;

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

public class GitRevisionService : IGitRevisionService
{
    private readonly Encoding _logOutputEncoding = Encoding.UTF8;
    private readonly long _oldestBody;
    private const int _offsetDaysForOldestBody = 6 * 30; // about 6 months

    private const string _reflogSelectorFormat = "%gD%n";
    private const string _notesPrefix = "Notes:";
    private const string _notesMarker = $"\n{_notesPrefix}";
    private const string _notesFormat = $"%n{_notesPrefix}%n%N";
    // Trace info for parse errors
    private int _noOfParseError = 0;

    // reflog selector to identify stashes
    private bool _hasReflogSelector;

    // Include Git Notes for the commit
    private bool _hasNotes;

    // Buffer to decode subject
    private char[] _decodeBuffer = new char[4096];
    private bool TryParseRevision(in ReadOnlySpan<byte> buffer, [NotNullWhen(returnValue: true)] out GitRevision? revision)
    {
        // The 'chunk' of data contains a complete git log item, encoded.
        // This method decodes that chunk and produces a revision object.

        // All values which can be read directly from the byte array are arranged
        // at the beginning of the chunk. The latter part of the chunk will require
        // decoding as a string.

        if (buffer.Length < ObjectId.Sha1CharCount * 2)
        {
            ParseAssert($"Log parse error, not enough data: {buffer.Length}");
            revision = default;
            return false;
        }

        #region Object ID, Tree ID, Parent IDs

        // The first 40 bytes are the revision ID and the tree ID back to back
        if (!ObjectId.TryParse(buffer.Slice(0, ObjectId.Sha1CharCount), out var objectId) ||
            !ObjectId.TryParse(buffer.Slice(ObjectId.Sha1CharCount, ObjectId.Sha1CharCount), out var treeId))
        {
            ParseAssert($"Log parse error, object id: {buffer.Length}({buffer.Slice(0, ObjectId.Sha1CharCount).ToString()}");
            revision = default;
            return false;
        }

        var offset = ObjectId.Sha1CharCount * 2;

        // Next we have zero or more parent IDs separated by ' ' and terminated by '\n'
        var noParents = CountParents(in buffer, offset);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        int CountParents(in ReadOnlySpan<byte> array, int baseOffset)
        {
            var count = 0;
            while (baseOffset < array.Length && array[baseOffset] != '\n')
            {
                if (count > 0)
                {
                    // Except for the first parent, advance after the space
                    baseOffset++;
                }

                baseOffset += ObjectId.Sha1CharCount;
                count++;

                if (baseOffset >= array.Length || (char)array[baseOffset] is not ('\n' or ' '))
                {
                    // Parse error, not using ParseAssert (or increasing _noOfParseError)
                    ParseAssert($"Log parse error, unexpected contents in the parent array: {baseOffset - offset} for {objectId}");
                    return -1;
                }
            }

            return count;
        }

        ObjectId[] parentIds;
        if (noParents <= 0)
        {
            offset++;
            parentIds = Array.Empty<ObjectId>();
        }
        else
        {
            parentIds = new ObjectId[noParents];

            for (var parentIndex = 0; parentIndex < noParents; parentIndex++)
            {
                if (!ObjectId.TryParse(buffer.Slice(offset, ObjectId.Sha1CharCount), out var parentId))
                {
                    ParseAssert($"Log parse error, parent {parentIndex} for {objectId}");
                    revision = default;
                    return false;
                }

                parentIds[parentIndex] = parentId;
                offset += ObjectId.Sha1CharCount + 1;
            }
        }

        #endregion

        #region Timestamps

        // Decimal ASCII seconds since the unix epoch
        if (!Utf8Parser.TryParse(buffer.Slice(offset), out long authorUnixTime, out var bytesConsumed))
        {
            ParseAssert($"Log parse error, not enough data for authortime: {buffer.Length} {offset} {buffer.Slice(offset).ToString()}");
            revision = default;
            return false;
        }

        offset += bytesConsumed + 1;
        if (!Utf8Parser.TryParse(buffer.Slice(offset), out long commitUnixTime, out bytesConsumed))
        {
            ParseAssert($"Log parse error, not enough data for committime: {buffer.Length} {offset} {buffer.Slice(offset).ToString()}");
            revision = default;
            return false;
        }

        offset += bytesConsumed + 1;

        #endregion

        #region Encoded string values (names, emails, subject, body)

        // The remaining must be decoded (for above utf8/ascii must work)
        // First records are decoded on the stack
        // The attributes are decoded in the order they are defined in the format string
        revision = new GitRevision(objectId)
        {
            ParentIds = parentIds,
            TreeGuid = treeId,

            Author = GetNextLine(buffer),
            AuthorEmail = GetNextLine(buffer),
            AuthorUnixTime = authorUnixTime,
            Committer = GetNextLine(buffer),
            CommitterEmail = GetNextLine(buffer),
            CommitUnixTime = commitUnixTime
        };

        // Body is occasionally big, like linux repo has 35K bytes, the buffer is over 100K
        // Use a backing buffer on the heap
        var maxChars = _logOutputEncoding.GetMaxByteCount(buffer.Slice(offset).Length);
        if (maxChars > _decodeBuffer.Length)
        {
            // Default should be sufficient for most repos, Linux though has
            // unencoded of 36K, which results in maxChars being greater than 100K
            var newSize = _decodeBuffer.Length;
            while (newSize < maxChars)
            {
                newSize *= 2;
            }

            _decodeBuffer = new char[newSize];
        }

        var decodedLength = _logOutputEncoding.GetChars(buffer.Slice(offset), _decodeBuffer);
        var decoded = _decodeBuffer.AsSpan(0, decodedLength).TrimEnd();

        // reflogSelector are only used when listing stashes
        if (_hasReflogSelector)
        {
            var lineLength = decoded.IndexOf('\n');
            if (lineLength == -1)
            {
                ParseAssert($"Log parse error, parent no reflogselector for {objectId}");
                revision = default;
                return false;
            }

            revision.ReflogSelector = lineLength > 0 ? decoded.Slice(0, lineLength).ToString() : null;
            decoded = decoded.Slice(lineLength + 1);
        }

        // Keep a full multi-line message body within the last six months (by default).
        // Note also that if body and subject are identical (single line), the body never need to be stored
        var keepBody = authorUnixTime >= _oldestBody;

        // Subject can also be defined as the contents before empty line (%s for --pretty),
        // this uses the alternative definition of first line in body.
        var lengthSubject = decoded.IndexOfAny(Delimiters.LineAndVerticalFeed);
        revision.HasMultiLineMessage = _hasNotes
            ? decoded.Length != lengthSubject + _notesMarker.Length + 1 // Notes must always include the notes marker
            : lengthSubject >= 0;

        revision.Subject = (lengthSubject >= 0
            ? decoded.Slice(0, lengthSubject).TrimEnd()
            : decoded)
            .ToString();

        if (keepBody && revision.HasMultiLineMessage)
        {
            // Handle '\v' (Shift-Enter) as '\n' for users that by habit avoid Enter to 'send'
            var currentOffset = lengthSubject;
            int verticalFeedIndex;
            while ((verticalFeedIndex = decoded.Slice(currentOffset).IndexOf('\v')) >= 0)
            {
                currentOffset += verticalFeedIndex;
                decoded[currentOffset] = '\n';
                currentOffset++;
            }

            // Removes empty Notes markers (this is the most common case)
            var hasNonEmptyNotes = _hasNotes;
            if (hasNonEmptyNotes)
            {
                if (decoded.EndsWith(_notesMarker))
                {
                    // Remove the empty marker
                    decoded = decoded[..^_notesMarker.Length].TrimEnd();
                    hasNonEmptyNotes = false;
                }
            }

            if (hasNonEmptyNotes)
            {
                // Format Notes, add indentation
                var notesStartIndex = ((ReadOnlySpan<char>)decoded).IndexOf(_notesMarker, StringComparison.Ordinal);

                StringBuilder message = new();
                currentOffset = notesStartIndex + _notesMarker.Length + 1;
                message.Append(decoded.Slice(0, currentOffset));
                while (currentOffset < decoded.Length)
                {
                    message.Append("    ");
                    var lineLength = decoded.Slice(currentOffset).IndexOf('\n');
                    if (lineLength == -1)
                    {
                        message.Append(decoded.Slice(currentOffset));
                        break;
                    }
                    else
                    {
                        message.Append(decoded.Slice(currentOffset, lineLength))
                            .Append('\n');
                    }

                    currentOffset += lineLength + 1;
                }

                revision.Body = message.ToString();
            }
            else
            {
                revision.Body = decoded.ToString();
            }
        }

        if (_hasNotes)
        {
            revision.HasNotes = true;
        }

        if (revision.Author is null || revision.AuthorEmail is null || revision.Committer is null || revision.CommitterEmail is null || revision.Subject is null || keepBody && revision.HasMultiLineMessage && revision.Body is null)
        {
            ParseAssert($"Log parse error, decoded fields ({revision.Subject}::{revision.Body}) for {objectId}");
            revision = default;
            return false;
        }


        #endregion

        return true;

        // Authors etc are limited, use a shared string pool
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        string? GetNextLine(in ReadOnlySpan<byte> s)
        {
            if (offset >= s.Length)
            {
                return null;
            }

            var lineLength = s.Slice(offset).IndexOf((byte)'\n');
            if (lineLength == -1)
            {
                // A line must be terminated
                return null;
            }

            var r = s.Slice(offset, lineLength);
            offset += lineLength + 1;
            return StringPool.Shared.GetOrAdd(r, _logOutputEncoding);
        }

        void ParseAssert(string message)
        {
            _noOfParseError++;
            // DebugHelpers.Assert(!Debugger.IsAttached || _noOfParseError > 1, message);
            Trace.WriteLineIf(_noOfParseError < 10, message);
        }
    }
    public List<GitRevision> GetLog(string revisionFilter, CancellationToken cancellationToken, string gitFullPathExe, string workingDirectory, string arguments, Encoding? outputEncoding = null)
    {
        var result = new List<GitRevision>();
        var errorEncoding = Encoding.Default;
        var processStartInfo = new ProcessStartInfo()
        {
            UseShellExecute = false,
            Verb = string.Empty,
            ErrorDialog = false,
            CreateNoWindow = true,
            RedirectStandardInput = false,
            RedirectStandardOutput = true,
            RedirectStandardError = true,
            StandardOutputEncoding = outputEncoding,
            StandardErrorEncoding = errorEncoding,
            FileName = gitFullPathExe,
            Arguments = arguments,
            WorkingDirectory = workingDirectory
        };
        try
        {
            var process = Process.Start(processStartInfo);
            foreach (var chunk in process.StandardOutput.BaseStream.SplitLogOutput())
            {
                if (this.TryParseRevision(chunk.Span, out var revision))
                {
                    result.Add(revision);
                }
            }
        }
        catch (Exception)
        {

            throw;
        }

        return result;
    }

    public Task<List<GitRevision>> GetLogAsync(string revisionFilter, CancellationToken cancellationToken, string gitFullPathExe, string workingDirectory, string arguments, Encoding? outputEncoding = null) => Task.Run(() => this.GetLog(revisionFilter, cancellationToken, gitFullPathExe, workingDirectory, arguments, outputEncoding));

}