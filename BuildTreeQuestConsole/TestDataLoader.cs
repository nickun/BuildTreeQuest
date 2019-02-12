using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace BuildTreeQuestConsole
{
    public class TestDataLoader
    {
        private const string DemoDataFileName = "DemoData.txt";

        public static IList<ProjectLine> LoadDemoData()
        {
            int idCnt = 1;
            var ret = File.ReadAllLines(DemoDataFileName)
                .Select(l => new ProjectLine
                {
                    Id = idCnt++,
                    Chapter = l
                }).ToArray();

            return ret;
        }
    }
}