using System;
using System.Collections.Generic;

namespace PenguinShuffle.Utils
{
    public static class NumberUtils
    {
        public static List<int> MakeRange(int start, int fin)
        {
            var ars = new List<int>();

            for (var nX = start; nX < fin; nX++)
            {
                ars.Add(nX);
            }
            return ars;
        }

        public static List<T> Shuffle<T>(List<T> list)
        {
            var n = list.Count;
            while (n > 1)
            {
                n--;

                var k = RandomUtil.RandomInt(0, n + 1);
                var value = list[k];
                list[k] = list[n];
                list[n] = value;
            }
            return list;
        }

        public static double Distance(int x1, int y1, int x2, int y2)
        {
            return Math.Sqrt(Math.Pow(x2 - x1, 2) + Math.Pow(y2 - y1, 2));
        }
    }
}