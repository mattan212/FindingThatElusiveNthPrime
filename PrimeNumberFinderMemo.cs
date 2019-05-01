using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NthPrimeNumber
{
    public class PrimeNumberFinderMemo : BasePrimeNumberFinder
    {
        protected List<int> Primes = new List<int>() { 2, 3 };

        public override int FindNthPrime(int n)
        {
            var current = Primes.Last() + 2;
            while (Primes.Count < n)
            {
                if (IsPrime(current))
                {
                    Primes.Add(current);
                }
                current += 2;
            }
            return Primes[n - 1];
        }
    }
}
