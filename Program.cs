using BenchmarkDotNet.Attributes;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

namespace NthPrimeNumber
{
    class Program
    {
        static void Main(string[] args)
        {
            //Uncomment and Run this to pre compute and store prime numbers in your blob storage.
            //The storage credentials for the blob storage should be configured in the appsettings.json file. 
            //I used Azure Storage Emulator for this code. You should have that installed in order for this 
            //to work, otherwise create a storage account in Azure, and provide its credentials in the settings file.
            //PreCompute();

            //As talked about in the post, benchmarking can be misleading in memoized or cached context.
            //Uncomment the next lines in order to run benchmarks.
            //Make sure your build is set to Release mode.
            //BenchmarkRunner.Run<PrimeNumberBenchmark>();

            //As mentioned in the post, these are two test suites I used to run for getting the metrics.
            //Uncomment the desired tests and the runner function to run the tests.
            //The detailed version prints output a little prettier, but requires more lines.
            //var tests1 = new List<int> { 1, 2, 3, 4, 5, 10, 100, 1000, 10000 };
            //var tests2 = new List<int> { 21302, 76201, 314098, 654321, 654322, 1234, 123456, 889901, 362880, 999999, 1000000};
            //RunTestsSimple(tests1);
            //RunTestsDetailed(tests1);

            Console.WriteLine("Reached the end of code. Press any key to close.");
            Console.ReadKey();
        }        

        static void PreCompute()
        {
            var preCompute = new PrimeNumberFinderPreCompute();
            preCompute.PreCompute(1_000_000);
        }

        static void RunTestsSimple(List<int> test)
        {
            var primeFinderNaive = new PrimeNumberFinderMemo();
            var primeFinderMemo = new PrimeNumberFinderMemo();
            var primeFinderIterator = new PrimeNumberFinderPrimeIterator();
            var preCompute = new PrimeNumberFinderPreCompute();

            MeasureAndPrint(primeFinderNaive, test, nameof(primeFinderNaive));
            MeasureAndPrint(primeFinderMemo, test, nameof(primeFinderMemo));
            MeasureAndPrint(primeFinderIterator, test, nameof(primeFinderIterator));
            MeasureAndPrint(preCompute, test, nameof(preCompute));
        }

        static void RunTestsDetailed(List<int> test)
        {
            var primeFinderNaive = new PrimeNumberFinderMemo();
            var primeFinderMemo = new PrimeNumberFinderMemo();
            var primeFinderIterator = new PrimeNumberFinderPrimeIterator();
            var preCompute = new PrimeNumberFinderPreCompute();

            MeasureAndPrintDetailed(primeFinderNaive, test, nameof(primeFinderNaive));
            MeasureAndPrintDetailed(primeFinderMemo, test, nameof(primeFinderMemo));
            MeasureAndPrintDetailed(primeFinderIterator, test, nameof(primeFinderIterator));
            MeasureAndPrintDetailed(preCompute, test, nameof(preCompute));
        }

        static void MeasureAndPrint(BasePrimeNumberFinder numberFinder, List<int> test, string name)
        {
            var stopwatch = new Stopwatch();
            stopwatch.Start();
            foreach (var current in test.Select((value, i) => (value, i)))
            {
                Console.Write("{0}: {1}, ", current.i, numberFinder.FindNthPrime(current.value));
            }
            Console.WriteLine();
            stopwatch.Stop();
            Console.WriteLine("{0} took {1}ms\n", name, stopwatch.ElapsedMilliseconds);
        }

        static void MeasureAndPrintDetailed(BasePrimeNumberFinder numberFinder, List<int> test, string name)
        {
            var stopwatch = new Stopwatch();
            stopwatch.Start();
            foreach (var current in test)
            {
                Console.WriteLine("Prime[{0}]: {1}, ", current, numberFinder.FindNthPrime(current));
            }
            Console.WriteLine();
            stopwatch.Stop();
            Console.WriteLine("{0} took {1}ms\n", name, stopwatch.ElapsedMilliseconds);
        }
    }

    public class PrimeNumberBenchmark
    {
        private int _testInput = 100_000;
        private PrimeNumberFinderNaive _naive = new PrimeNumberFinderNaive();
        private PrimeNumberFinderMemo _memo = new PrimeNumberFinderMemo();
        private PrimeNumberFinderPrimeIterator _iterator = new PrimeNumberFinderPrimeIterator();
        private PrimeNumberFinderPreCompute _preCompute = new PrimeNumberFinderPreCompute();

        [Benchmark]
        public void TestNaive()
        {
            _naive.FindNthPrime(_testInput);
        }

        [Benchmark]
        public void TestMemo()
        {
            _memo.FindNthPrime(_testInput);
        }

        [Benchmark]
        public void TestPrimeIterator()
        {
            _iterator.FindNthPrime(_testInput);
        }

        [Benchmark]
        public void TestPrimePreCompute()
        {
            _preCompute.FindNthPrime(_testInput);
        }
    }
}
