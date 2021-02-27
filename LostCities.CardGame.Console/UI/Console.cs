using System;
using System.Collections.Generic;
using System.Linq;
using SysConsole = System.Console;

namespace LostCities.CardGame.Console.UI
{
    // Wrapper for System.Console
    public static class Console
    {
        private static readonly int NumberOfChars = 50;
        private static ConsoleColor foregroundColor;

        public static ConsoleColor ForegroundColor
        {
            get { return foregroundColor; }
            set
            {
                SysConsole.ForegroundColor = value;
                foregroundColor = value;
            }
        }

        public static void Write(object value)
        {
            SysConsole.Write(value);
        }

        public static void Write(ConsoleColor color, object value, bool beautify)
        {
            ForegroundColor = color;

            if (!beautify)
                SysConsole.Write(value);
            else
            {
                var array = value.ToString().Split("\\n").ToList();
                array.ForEach(l => WriteLine(ConsoleColor.Green, l));
            }

            SysConsole.ResetColor();
        }

        public static void WriteLine(object value)
        {
            SysConsole.WriteLine(value);
        }

        public static void WriteLine(ConsoleColor color, object value)
        {
            ForegroundColor = color;
            SysConsole.WriteLine(value);
            SysConsole.ResetColor();
        }

        public static string ReadLine()
        {
            return SysConsole.ReadLine();
        }

        public static void DisplayMenu(ConsoleColor consoleColor, List<string> menuItems)
        {
            ForegroundColor = consoleColor;

            SysConsole.WriteLine(new String('-', NumberOfChars));

            foreach (string menuItem in menuItems)
                SysConsole.WriteLine(menuItem);

            SysConsole.WriteLine(new String('-', NumberOfChars));
            SysConsole.ResetColor();
        }

        public static void DisplayAssemblyInfo(string assemblyName, string assemblyVersion)
        {
            ForegroundColor = ConsoleColor.Green;
            SysConsole.WriteLine(new String('*', NumberOfChars) + "\r\n");
            SysConsole.WriteLine($"      {assemblyName}");
            SysConsole.WriteLine($"      version: {assemblyVersion}" + "\r\n");
            SysConsole.WriteLine(new String('*', NumberOfChars) + "\r\n");
            SysConsole.ResetColor();
        }

        public static void ResetColor()
        {
            SysConsole.ResetColor();
        }
    }
}
