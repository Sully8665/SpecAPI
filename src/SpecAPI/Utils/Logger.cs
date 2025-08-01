using System;

namespace SpecAPI.Utils
{
    public static class Logger
    {
        public static void Log(string message)
        {
            Console.WriteLine($"[LOG] {DateTime.Now}: {message}");
        }
    }
}
