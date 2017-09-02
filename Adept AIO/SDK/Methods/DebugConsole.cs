using System;

namespace Adept_AIO.SDK.Methods
{
    internal class DebugConsole
    {
        public static void Print(string message, ConsoleColor foregroundColor = ConsoleColor.White, ConsoleColor backgroundColor = ConsoleColor.Black)
        {
            Console.BackgroundColor = backgroundColor;
            Console.ForegroundColor = foregroundColor;
            Console.WriteLine("[" + DateTime.Now + "] " + message);
            Console.ResetColor();
        }
    }
}
