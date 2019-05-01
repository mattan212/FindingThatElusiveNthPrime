using System;
using System.Collections.Generic;
using System.Text;

namespace NthPrimeNumber
{
    public class PrimeNumberFinderPrimeIterator : PrimeNumberFinderMemo
    {
        public override int FindNthPrime(int n)
        {
            return base.FindNthPrime(n);
        }

        protected override bool IsPrime(int num)
        {
            if (num == 2)
            {
                return true;
            }

            if (num % 2 == 0 || num <= 1)
            {
                return false;
            }

            var limit = Math.Sqrt(num);
            foreach (var prime in Primes)
            {
                if (prime > limit)
                {
                    break;
                }
                if (num % prime == 0)
                {
                    return false;
                }
            }

            return true;
        }
    }
}
