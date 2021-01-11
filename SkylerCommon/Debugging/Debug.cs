using System;
using System.Collections.Generic;
using System.Text;

namespace SkylerCommon.Debugging
{
    public static class Debug
    {
        public static bool ProgramInDebugMode { get; set; } = true;

        static object OneAtATime { get; set; } = new object();

        //TODO: Add importance checks to not log everything.
        static void LogWithColor(object message,ConsoleColor color = ConsoleColor.White)
        {
            lock (OneAtATime)
            {
                Console.ForegroundColor = color;

                //TODO: Add time markers.

                if (ProgramInDebugMode)
                {
                    Console.WriteLine(message);
                }

                Console.ForegroundColor = ConsoleColor.White;
            }
        }

        public static void LogWarning(object message) => LogWithColor($"Warning: {message}",ConsoleColor.Yellow);

        public static void Log(string message) => LogWithColor(message,ConsoleColor.White);

        public static void LogError(object message, bool Crash = false)
        {
            LogWithColor($"Error: {message}", ConsoleColor.Red);

            if (Crash)
                Debug.Crash();
        }

        public static void Crash()
        {
            LogError("Indefinite emulation hault.");

            ThrowNotImplementedException();
        }

        public static void ThrowException(Exception exception) => throw exception;

        public static void ThrowNotImplementedException(string message = "") => ThrowException(new NotImplementedException(message));
    }
}
