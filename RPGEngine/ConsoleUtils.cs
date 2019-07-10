using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using System.Text.RegularExpressions;

namespace Aze.Utilities
{
    class MenuItem
    {
        public string KeyPress { set; get; }
        public string Description { set; get; }
        public string Method { set; get; }

        public MenuItem(string keyPress, string description, string method)
        {
            KeyPress = keyPress;
            Description = description;
            Method = method;
        }
    }

    static class ConsoleUtils
    {
        static readonly List<MenuItem> Menu = new List<MenuItem>();

        public static void DisplayMenu()
        {
            foreach (MenuItem line in Menu)
            {
                LogOptions("{0}\t{1}", line.KeyPress, line.Description);
            }
        }

        public static void AddMenuItem(string keyPress, string description, string method)
        {
            Menu.Add(new MenuItem(keyPress, description, method));
        }

        public static string SearchMethod(string keyPress)
        {
            foreach (MenuItem line in Menu)
            {
                if (line.KeyPress == keyPress) return line.Method;
            }
            return "DisplayMenu";
        }

        public static string GetFromConsole(string prompt, params object[] argRep)
        {
            Console.Write(prompt, argRep);
            return Console.ReadLine();
        }
        public static void LogDanger(string text, params object[] argText)
        {
            LogPrompt(ConsoleColor.Red, text, argText);
        }

        public static void LogWarning(string text, params object[] argText)
        {
            LogPrompt(ConsoleColor.Yellow, text, argText);
        }

        public static void LogInfo(string text, params object[] argText)
        {
            LogPrompt(ConsoleColor.Cyan, text, argText);
        }

        public static void LogSuccess(string text, params object[] argText)
        {
            LogPrompt(ConsoleColor.Green, text, argText);
        }

        public static void LogOptions(string text, params object[] argText)
        {
            WriteLine(ConsoleColor.Magenta, text, argText);
        }

        public static void Indent(int length,string filler=" ")
        {
            for (int i=0;i<length;i++) Console.Write(filler);
        }

        public static void WriteLine(ConsoleColor c, string value, params object[] prms)
        {
            var regEx = new Regex("{[0-9]+}");
            var matches = regEx.Matches(value);
            int i = 0;
            int newLenght = (value.Length - 3 * (prms.Length) + (int)prms.Sum(x => (x.ToString()).Length));
            var valueArr = value.ToCharArray();
            foreach (Match m in matches)
            {
                while (i < m.Index && i < valueArr.Length)
                {
                    Console.Write(valueArr[i]);
                    i++;
                }

                if (valueArr[i].Equals('{') && i + m.Value.Length < valueArr.Length && valueArr[i + m.Value.Length].Equals('}'))
                {
                    i += m.Value.Length;
                    Console.Write(m.Value.Trim('{', '}'));
                }
                else if (int.Parse(m.Value.Trim('{', '}')) < prms.Length)
                {
                    i += m.Value.Length;
                    Console.ForegroundColor = c;
                    Console.Write(prms[int.Parse(m.Value.Trim('{', '}'))]);
                    Console.ResetColor();
                }
                else
                {
                    throw new Exception("Index must be greater than or equal to zero and less than the size of the argument list.");
                }
            }
            while (i < valueArr.Length)
            {
                Console.Write(valueArr[i]);
                i++;
            }
            Console.WriteLine();
        }

        public static void LogPrompt(ConsoleColor color, string text, params object[] argText)
        {
            Console.ForegroundColor = color;
            Console.WriteLine(text, argText);
            Console.ResetColor();
        }
    }
}
