using System;
using System.Collections.Generic;
using System.Text;

namespace NthPrimeNumber
{
    public abstract class BasePrimeNumberFinder
    {
        public abstract int FindNthPrime(int n);

        protected virtual bool IsPrime(int num)
        {
            if (num == 2)
            {
                return true;
            }

            if (num % 2 == 0 || num <= 1)
            {
                return false;
            }

            for (var i = 3; i <= Math.Sqrt(num); i += 2)
            {
                if (num % i == 0)
                {
                    return false;
                }
            }

            return true;
        }
    }
}
