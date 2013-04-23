using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SpaceMAS.Utils
{
    class UtilRandom
    {
        private static readonly Random Random = new Random();

        public static int Next(int min, int max)
        {
            {
                return Random.Next(min, max);
            }
        }
        public static int Next(int max)
        {
            {
                return Random.Next(max);
            }
        }
    }
}
