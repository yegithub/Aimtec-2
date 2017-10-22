namespace Adept_AIO.SDK.Generic
{
    using System;

    class GetRandom
    {
        private static Random _random;

        public GetRandom() { _random = new Random(); }

        public static int Next(int min, int max) { return _random.Next(min, max); }
    }
}