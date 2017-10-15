namespace Adept_AIO.SDK.Generic
{
    using System;

    class GetRandom
    {
        private static Random _random;

        public static void Init() { _random = new Random(); }

        public static int Next(int min, int max) => _random.Next(min, max);
    }
}