namespace UnitTests.RevisionManager;

using System.Collections.Generic;
using System.Linq;
using System.Text;
using Xunit;
using Xunit.Abstractions;
public interface ITreeItem
{
   public IList<ITreeItem> Children { get; }
}
public class GitRevisionServiceTest
{
    private ITestOutputHelper _logger;

    public GitRevisionServiceTest(ITestOutputHelper logger)
    {
        _logger = logger;
    }

    [Fact]
    public void DisplayTreeItems()
    {
        //var factory = new DataStructureBuilderMock();
        //ITreeItem rootItem = factory.BuildSampleData();

        //(int level, ITreeItem item)[] queue = BuildQueue(0, rootItem);

        //foreach ((int level, ITreeItem item) in queue)
        //{
        //    WriteTreeItem(level, item);
        //}
    }

    private (int level, object item)[] BuildQueue(int level, object treeItem)
    {
        List<(int level, object item)> result = new()
        {
           
        };


        foreach (var item in (treeItem as ITreeItem).Children)
        {
            result.AddRange(BuildQueue(level + 1, item));
        }

        return result.ToArray();
    }

    private void WriteTreeItem(int level, object item)
    {
        string levelString = new string(Enumerable.Range(0, level).Select(_ => '-').ToArray());

        StringBuilder builder = new();
        builder.Append(levelString);
        builder.Append(item);
        builder.Append($"({item.GetType().Name})");
        _logger.WriteLine(builder.ToString());
    }

}