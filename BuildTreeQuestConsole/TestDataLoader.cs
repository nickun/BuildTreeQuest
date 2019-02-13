using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

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
    }
}