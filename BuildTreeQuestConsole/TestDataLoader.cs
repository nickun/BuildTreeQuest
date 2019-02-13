using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using BuildTreeQuestConsole.Helpers;

namespace BuildTreeQuestConsole
{
    public class TestDataLoader
    {
        public const string DemoDataFileName = "DemoData.txt";
        public const string DemoGenDataFileName = "DemoDataGen.txt";

        public static IList<ProjectLine> LoadDemoData()
        {
            return LoadDemoData(DemoDataFileName);
        }

        public static IList<ProjectLine> LoadDemoData(string fileName)
        {
            int idCnt = 1;
            var ret = File.ReadAllLines(fileName)
                .Select(l => new ProjectLine
                {
                    Id = idCnt++,
                    Chapter = l
                }).ToArray();

            return ret;
        }

        public static IList<ProjectLine> GenerateDemoData(int totalQuantity)
        {
            const int maxDepth = 6;
            int maxRootLines = (int)Math.Round(Math.Sqrt(totalQuantity) / 2.0);
            var rnd = new Random(DateTime.Now.Millisecond);
            var res = new Dictionary<string, ProjectLine>(totalQuantity);

            int idCnt = 1;

            while (res.Count < totalQuantity)
            {
                // generate some chapter
                int[] chapter = GenerateChapter(rnd, maxDepth,
                    maxRootLines, (int) (maxRootLines * 4.0));

                // generate items
                var genChapter = new int[chapter.Length];
                for (int iPart = 0; iPart < chapter.Length; iPart++)
                {
                    for (int iNum = 1; iNum <= chapter[iPart]; iNum++)
                    {
                        genChapter[iPart] = iNum;
                        string genChapterStr = ChapterArrayToString(genChapter);
                        if (!res.ContainsKey(genChapterStr))
                        {
                            res[genChapterStr] = new ProjectLine
                            {
                                Id = idCnt++,
                                Chapter = genChapterStr
                            };

                            if (res.Count >= totalQuantity)
                                break;
                        }
                    }
                    if (res.Count >= totalQuantity)
                        break;
                }
            }

            return res.Values.ToArray();
        }

        private static string ChapterArrayToString(int[] chapter)
        {
            var sb = new StringBuilder();
            foreach (var chapterPart in chapter)
            {
                if (chapterPart > 0)
                    sb.Append(chapterPart + ".");
            }

            return sb.ToString().TrimEnd('.');
        }

        private static int[] GenerateChapter(Random rnd, int maxDepth, int maxRootLines, int maxLastLevelLines)
        {
            int numParts = rnd.Next(1, maxDepth);
            var res = new int[numParts];

            for (int i = 0; i < numParts; i++)
            {
                int num = i == 0
                    ? rnd.Next(1, maxRootLines)
                    : i == maxDepth-1 || i == maxDepth-2
                        ? rnd.Next(1, maxLastLevelLines)
                        : rnd.Next(1, 3);
                res[i] = num;
            }

            return res;
        }


        [DebuggerDisplay("Id: {ID}, Children: {ChildrenCount}")]
        class NodeGenerate
        {
            public int ID;

            private ProjectLine _nodeItem;
            private IDictionary<int, NodeGenerate> _children;

            public int ChildrenCount => _children?.Count ?? 0;

            public ProjectLine NodeItem
            {
                get { return _nodeItem; }
                set
                {
                    // take parent from the node
                    var parentItem = ParentNode?.NodeItem;
                    if (parentItem != null && value != null)
                        value.ParentId = parentItem.Id;

                    // set me as parent for all my children
                    if (_nodeItem == null && value != null)
                        _children?.Values.ForEachExt(c => c.SetParentNodeItemId(value.Id));
                    _nodeItem = value;
                }
            }

            public NodeGenerate ParentNode { get; }

            public NodeGenerate(int id)
            {
                ID = id;
            }

            private NodeGenerate(int id, NodeGenerate parentNode)
            {
                ID = id;
                ParentNode = parentNode;
            }

            public NodeGenerate GetOrCreateChildNode(int childNum)
            {
                NodeGenerate childNode;
                var children = _children;
                if (children == null)
                {
                    childNode = new NodeGenerate(childNum, this);
                    _children = new Dictionary<int, NodeGenerate> { { childNum, childNode } };
                    return childNode;
                }

                if (!children.TryGetValue(childNum, out childNode))
                    children[childNum] = childNode = new NodeGenerate(childNum, this);

                return childNode;
            }

            private void SetParentNodeItemId(int parentId)
            {
                if (_nodeItem != null)
                    _nodeItem.ParentId = parentId;
            }
        }
    }
}