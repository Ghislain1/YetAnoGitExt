namespace UnitTests.RevisionManager;

using System.Collections.Generic;

public interface ITreeItem
{
    public IList<ITreeItem> Children { get; }
}
