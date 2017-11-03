namespace EzEvade_Port.Utils
{
    using System;

    public static class ConsolePrinter
    {
        private static float _lastPrintTime;

        public static void Print(string str)
        {
            var timeDiff = EvadeUtils.TickCount - _lastPrintTime;

            var finalStr = "[" + timeDiff + "] " + str;

            Console.WriteLine(finalStr);

            _lastPrintTime = EvadeUtils.TickCount;
        }
    }
}