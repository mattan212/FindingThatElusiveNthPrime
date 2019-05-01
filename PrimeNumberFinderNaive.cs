using System;
using System.Collections.Generic;
using System.Text;

namespace NthPrimeNumber
{
    public class PrimeNumberFinderNaive : BasePrimeNumberFinder
    {
        public override int FindNthPrime(int n)
        {
            var primes = new List<int> { 2 };
            var current = 3;
            while (primes.Count < n)
            {
                if (IsPrime(current))
                {
                    primes.Add(current);
                }
                current += 2;
            }
            return primes[n - 1];
        }
    }
}
