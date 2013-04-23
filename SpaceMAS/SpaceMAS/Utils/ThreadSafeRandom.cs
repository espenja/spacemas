using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SpaceMAS.Utils
{
    class ThreadSafeRandom
    {
        private static readonly Random Random = new Random();

        public static int Next(int min, int max)
        {
            lock (Random)
            {
                return Random.Next(min, max);
            }
        }
        public static int Next(int max)
        {
            lock (Random)
            {
                return Random.Next(max);
            }
        }
    }
}
