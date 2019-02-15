using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;

namespace BuildTreeQuestConsole
{
    public class TreeBuilderTM
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        static string chap(string chapter)
        {
            int dot = chapter.LastIndexOf('.');
            if (dot < 0) return null;
            return chapter.Substring(0, dot);
        }

        public static IList<ProjectLine> BuildTree1(IList<ProjectLine> testData)
        {
            var nodeCollector = testData.ToDictionary(i => i.Chapter);
            var chapters = testData.OrderBy(pl =>
            {
                char[] chars = pl.Chapter.ToCharArray();
                int len = chars.Length;
                int n = 0;
                for (int i = len - 1; i >= 0; i--)
                    if (chars[i] == '.')
                        n++;
                return n;
            });
            return chapters.Select(c => {
                var k = chap(c.Chapter);
                if (k == null) return c;
                if (!nodeCollector.TryGetValue(k, out var val) || !val.HasChildren) return null;
                val.HasChildren = true;
                c.ParentId = val.Id;
                return c;
            }).Where(c => c != null).ToList();
        }

        public static IList<ProjectLine> BuildTree(IList<ProjectLine> testData)
        {
            var nodeCollector = testData.ToDictionary(i => i.Chapter);
            int count = testData.Count;

            for (int i = 0; i < count; i++)
            {
                var c = testData[i];
                int dot = c.Chapter.LastIndexOf('.');
                if (dot < 0) continue;
                if (nodeCollector.TryGetValue(c.Chapter.Substring(0, dot), out var val))
                {
                    val.HasChildren = true;
                    c.ParentId = val.Id;
                }
            }

            return testData;
        }

        public static IList<ProjectLine> BuildTreeParallel(IList<ProjectLine> testData)
        {
            var nodeCollector = testData.ToDictionary(i => i.Chapter);

            var rangePartitioner = Partitioner.Create(0, testData.Count);
            Parallel.ForEach(rangePartitioner, (range, loopState) =>
            {
                for (int i = range.Item1; i < range.Item2; i++)
                {
                    var c = testData[i];
                    int dot = c.Chapter.LastIndexOf('.');
                    if (dot < 0) continue;
                    if (nodeCollector.TryGetValue(c.Chapter.Substring(0, dot), out var val))
                    {
                        val.HasChildren = true;
                        c.ParentId = val.Id;
                    }
                }
            });

            return testData;
        }
    }
}