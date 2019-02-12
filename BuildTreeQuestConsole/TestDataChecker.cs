using System.Collections.Generic;
using System.Linq;

namespace BuildTreeQuestConsole
{
    public class TestDataChecker
    {
        /// <summary>
        /// Check data integrity.
        /// For simplicity of the experiment assume that data has no gaps in the list.
        /// </summary>
        public static bool IsCorrectDataTreeList(IList<ProjectLine> testData)
        {
            IDictionary<int, ProjectLine> testDataDict = testData.ToDictionary(i => i.Id);

            foreach (var item in testData)
            {
                // a child without parent
                if (item.Chapter.Contains('.') && item.ParentId == 0)
                    return false;

                // root item
                if (!item.Chapter.Contains('.'))
                {
                    if (item.ParentId != 0)
                        return false;
                    continue;
                }

                // unknown parent id
                if (!testDataDict.TryGetValue(item.ParentId, out ProjectLine parentItem))
                    return false;

                // incorrect parent
                if (!IsCorrectChildParentPair(item.Chapter, parentItem.Chapter))
                    return false;
            }

            return true;
        }

        private static bool IsCorrectChildParentPair(string childChapter, string parentChapter)
        {
            return childChapter.StartsWith(parentChapter + ".");
        }
    }
}