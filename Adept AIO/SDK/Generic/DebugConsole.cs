namespace Adept_AIO.SDK.Generic
{
    using System;

    class DebugConsole
    {
        public static void Write(string message, ConsoleColor foregroundColor = ConsoleColor.White, ConsoleColor backgroundColor = ConsoleColor.Black)
        {
            Console.BackgroundColor = backgroundColor;
            Console.ForegroundColor = foregroundColor;
            Console.WriteLine("[" + DateTime.Now + "] " + message);
            Console.ResetColor();
        }
    }
}