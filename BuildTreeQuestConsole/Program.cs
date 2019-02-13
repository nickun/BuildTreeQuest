using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;

namespace BuildTreeQuestConsole
{
    class Program
    {
        static void Main(string[] args)
        {
            // load source test data
            const int genChaptersCount = 500;
            var rnd = new Random(DateTime.Now.Millisecond);

            IList<ProjectLine> testData = TestDataLoader.GenerateDemoData(genChaptersCount);
            File.WriteAllLines(TestDataLoader.DemoGenDataFileName, testData.Select(i => i.Chapter).OrderBy(l => rnd.Next(genChaptersCount)));

            // simple slow build test
            RunTest(testData, SimpleSlowTreeBuilder.BuildTree);

            // put your test here
            //testData = TestDataLoader.LoadDemoData(TestDataLoader.DemoGenDataFileName); // load the same data again
            //RunTest(testData, TreeBuilderNick.BuildTree);

            Console.ReadKey();
        }

        static void RunTest(IList<ProjectLine> testData, Func<IList<ProjectLine>, IList<ProjectLine>> buildTreeTestMethod)
        {
            Stopwatch sw = new Stopwatch();

            Console.Write($"{buildTreeTestMethod.Method.DeclaringType?.Name ?? ""}.{buildTreeTestMethod.Method.Name}[{testData.Count}] ");

            sw.Restart();
            IList<ProjectLine> resTestData = buildTreeTestMethod(testData);
            sw.Stop();

            Console.WriteLine($"elapsed {sw.Elapsed} {(TestDataChecker.IsCorrectDataTreeList(resTestData) ? "PASSED" : "FAILED")}");
        }
    }
}
