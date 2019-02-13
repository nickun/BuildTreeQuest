using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Attributes.Jobs;
using BenchmarkDotNet.Running;

namespace BuildTreeQuestConsole
{
    [ClrJob]
    public class Program
    {
        //[Benchmark(Baseline = true)]
        //public void Test_SimpleSlowTreeBuilder()
        //{
        //    IList<ProjectLine> testData = TestDataLoader.LoadDemoData(TestDataLoader.DemoGenDataFileName);
        //    IList<ProjectLine> resTestData = SimpleSlowTreeBuilder.BuildTree(testData);
        //}

        // put your test here
        [Benchmark]
        public void Test_TreeBuilderTM()
        {
            IList<ProjectLine> testData = TestDataLoader.LoadDemoData(TestDataLoader.DemoGenDataFileName);
            IList<ProjectLine> resTestData = TreeBuilderTM.BuildTree(testData);
		}
		
		[Benchmark]
        public void Test_TreeBuilderNick()
        {
            IList<ProjectLine> testData = TestDataLoader.LoadDemoData(TestDataLoader.DemoGenDataFileName);
            IList<ProjectLine> resTestData = TreeBuilderNick.BuildTree(testData);
        }

        static void Main(string[] args)
        {
            // generate source test data
            //const int genChaptersCount = 500;
            //var rnd = new Random(DateTime.Now.Millisecond);

            //IList<ProjectLine> testData = TestDataLoader.GenerateDemoData(genChaptersCount);
            //File.WriteAllLines(TestDataLoader.DemoGenDataFileName, testData.Select(i => i.Chapter).OrderBy(l => rnd.Next(genChaptersCount)));

            // simple slow build test
            IList<ProjectLine> testData = TestDataLoader.LoadDemoData(TestDataLoader.DemoGenDataFileName);
            RunTest(testData, SimpleSlowTreeBuilder.BuildTree);

            // put your test here
            testData = TestDataLoader.LoadDemoData(TestDataLoader.DemoGenDataFileName); // load the same data again
            RunTest(testData, TreeBuilderTM.BuildTree);

            testData = TestDataLoader.LoadDemoData(TestDataLoader.DemoGenDataFileName);
            RunTest(testData, TreeBuilderNick.BuildTree);

            var summary = BenchmarkRunner.Run<Program>();

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
