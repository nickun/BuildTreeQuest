using System.Collections.Generic;
using System.Linq;

namespace BuildTreeQuestConsole
{
    public class TreeBuilderAlexV
    {
        public static IList<ProjectLine> BuildTree(IList<ProjectLine> testData)
        {
            var lookup = testData.ToDictionary(td => td.Chapter, td => td);
            return testData.Select(pl =>
                {
                    int p = pl.Chapter.LastIndexOf('.');
                    if (p >= 0)
                    {
                        string parentChapter = pl.Chapter.Substring(0, p);
                        if (lookup.ContainsKey(parentChapter))
                        {
                            var parentpl = lookup[parentChapter];
                            pl.ParentId = parentpl.Id;
                            parentpl.HasChildren = true;
                        }
                    }
                    return pl;
                })
                .ToList();
        }
    }
}