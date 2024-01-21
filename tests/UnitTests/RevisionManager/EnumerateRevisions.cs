namespace UnitTests.RevisionManager
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using RevisionViewer;
    using Xunit;
    using Xunit.Abstractions;

    public class EnumerateRevisions
    {
        private ITestOutputHelper _logger;

        public EnumerateRevisions(ITestOutputHelper logger)
        {
            _logger = logger;
        }

        [Fact]
        public void DisplayTreeItems()
        {
            var factory = new DataStructureBuilderMock();
            ITreeItem rootItem = factory.BuildSampleData();

            (int level, ITreeItem item)[] queue = BuildQueue(0, rootItem);

            foreach ((int level, ITreeItem item) in queue)
            {
                WriteTreeItem(level, item);
            }
        }

        private (int level, ITreeItem item)[] BuildQueue(int level, ITreeItem treeItem)
        {
            List<(int level, ITreeItem item)> result = new()
            {
                (level,treeItem)
            };

            foreach (var item in treeItem.Children)
            {
                result.AddRange(BuildQueue(level + 1, item));
            }

            return result.ToArray();
        }

        private void WriteTreeItem(int level, ITreeItem item)
        {
            string levelString = new string(Enumerable.Range(0, level).Select(_ => '-').ToArray());

            StringBuilder builder = new();
            builder.Append(levelString);
            builder.Append(item.Label);
            builder.Append($"({item.GetType().Name})");
            if (item is Revision revision)
            {
                builder.Append($" {revision.Version}");
            }


            _logger.WriteLine(builder.ToString());
        }

    }
}