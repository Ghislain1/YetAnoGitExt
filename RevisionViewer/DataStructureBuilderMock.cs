using System;
using System.Collections.Generic;

namespace RevisionViewer
{
    public class DataStructureBuilderMock
    {
        /// <summary>
        ///　Generate sample data. Please use it for development.
        /// </summary>
        /// <returns></returns>
        public ITreeItem BuildSampleData()
        {
            return new Revision()
            {
                Label = "Build based on the current doc but for different goals",
                LastModified = DateTime.Now,
                Children = new ITreeItem[]
                {
                    new Revision()
                    {
                        Children = new ITreeItem[]
                        {
                            new Separator()
                            {
                                Label = "INHERITOR",
                            },
                            new Revision()
                            {
                                Label = "Built based on the current doc but for different goals",
                                LastModified = DateTime.Now,
                            },
                            new Revision()
                            {
                                Label = "Inheritor document [Client A]",
                                LastModified = DateTime.Now,
                                Children = BuildSubRevisions("Inheritor document [Client A]",10),
                            },
                            new Revision()
                            {
                                Label = "Inheritor document [Client B]",
                                LastModified = DateTime.Now,
                                Children = BuildSubRevisions("Inheritor document [Client B]",15),
                            }
                        },
                    },
                    new Separator()
                    {
                        Label = "VARIANTS",
                    },
                    new Revision()
                    {
                        Label = "A variant of the current doc, mostly is for another customer",
                    },
                    new Revision()
                    {
                        Label = "Another variant",
                        Version = "final",
                        Children = BuildSubRevisions("Another variant",5),
                    },
                    new Revision()
                    {
                        Label = "Valiant of the current doc [Client X]",
                        Version = "final",
                        Children = BuildSubRevisions("Valiant of the current doc [Client X]",20),
                    },
                    new Revision()
                    {
                        Label = "Valiant of the current doc [Client Y]",
                        Version = "final",
                        Children = BuildSubRevisions("Valiant of the current doc [Client Y]",30),
                    },
                    new Revision()
                    {
                        Label = "Valiant of the current doc [Client Z]",
                        Version = "final",
                        Children = BuildSubRevisions("Valiant of the current doc [Client Z]",30),
                    },
                    new Revision()
                    {
                        Label = "Stress test data",
                        Version = "test",
                        Children = BuildSubRevisions("Stress test data",1000),
                    },
                }
            };
        }

        private ITreeItem[] BuildSubRevisions(string label, int versionCount)
        {
            List<ITreeItem> subRevision = new();
            for (int i = 0; i < versionCount; ++i)
            {
                int version = i + 1;
                subRevision.Add(new Revision()
                {
                    Label = label,
                    LastModified = DateTime.Now,
                    Version = $"v{version}",
                });
            }

            return subRevision.ToArray();
        }
    }
}