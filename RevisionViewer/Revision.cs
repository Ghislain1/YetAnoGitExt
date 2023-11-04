using System;
using System.Collections.Generic;
using System.Linq;

namespace RevisionViewer
{
    public interface ITreeItem
    {
        public string Label { get; }
        public ITreeItem[] Children { get; }
    }

    public class Revision : ITreeItem
    {
        public ITreeItem[] Children { get; set; } = Array.Empty<ITreeItem>();
        public IEnumerable<Revision> Revisions => Children.OfType<Revision>();
        public string Label { get; set; } = string.Empty;
        public string Version { get; set; } = string.Empty; // "v1", "v2", "final" or something. if exists this property, you will sh
        public DateTime LastModified { get; set; }
    }

    public class Separator : ITreeItem
    {
        public string Label { get; set; } // inheritor, variant or something.
        public ITreeItem[] Children => Array.Empty<ITreeItem>();
    }
}