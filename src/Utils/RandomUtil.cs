using System;

namespace PenguinShuffle.Utils
{
    public class RandomUtil
    {
        private static readonly Random r;

        static RandomUtil()
        {
/*
            if (seed != null)
            {
                Seed = seed.Value;
                r = new Random(Seed);
            }
            else
            {
                */
            r = new Random(); /*
            }       */
        }

        public static int Seed { get; set; }

        public static bool RandomBool()
        {
            return r.Next(0, 100) < 50;
        }

        public static int RandomInt(int lower, int higher)
        {
            return r.Next(lower, higher);
        }

        public static bool RandomPercentUnder(int cutoff)
        {
            return r.Next(0, 100) < cutoff;
        }
    }
}