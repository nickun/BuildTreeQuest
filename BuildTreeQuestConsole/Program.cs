using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;

namespace BuildTreeQuestConsole
{
    class Program
    {
        static void Main(string[] args)
        {
            // simple slow build test
            RunTest(SimpleSlowTreeBuilder.BuildTree);

            // put your test here
            //RunTest(NickTreeBuilder.BuildTree);

            Console.ReadKey();
        }

        static void RunTest(Func<IList<ProjectLine>, IList<ProjectLine>> buildTreeTestMethod)
        {
            Stopwatch sw = new Stopwatch();

            // load source test data
            IList<ProjectLine> testData = TestDataLoader.LoadDemoData();

            Console.Write($"{buildTreeTestMethod.Method.DeclaringType?.Name ?? ""}.{buildTreeTestMethod.Method.Name}");

            sw.Restart();
            IList<ProjectLine> resTestData = buildTreeTestMethod(testData);
            sw.Stop();

            Console.WriteLine($"elapsed {sw.Elapsed} {(TestDataChecker.IsCorrectDataTreeList(resTestData) ? "PASSED" : "FAILED")}");
        }
    }
}
