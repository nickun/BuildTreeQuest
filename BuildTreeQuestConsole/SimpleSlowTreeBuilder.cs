using System.Collections.Generic;
using System.Linq;

namespace BuildTreeQuestConsole
{
    public class SimpleSlowTreeBuilder
    {
        public static IList<ProjectLine> BuildTree(IList<ProjectLine> testData)
        {
            IDictionary<string, ProjectLine> nodeCollector = testData.ToDictionary(i => i.Chapter);
            var chapters = nodeCollector.Values.OrderBy(item => item).Select(item => item.Chapter);

            foreach (var chapter in chapters)
            {
                int id = nodeCollector[chapter].Id;
                foreach (string subitemKey in chapters.Where(item => item.StartsWith(chapter + ".")))
                    nodeCollector[subitemKey].ParentId = id;
            }

            return testData;
        }
    }
}