using System;
using System.Collections.Generic;
using System.Text;

namespace WebServer
{
    public class Logger
    {
        public ConsoleColor DefaultColor { get; set; }

        public Logger(ConsoleColor? defaultColor = null)
        {
            DefaultColor = defaultColor ?? ConsoleColor.White;
            Console.ForegroundColor = DefaultColor;
        }

        public void Write(string text, ConsoleColor? color = null)
        {
            Console.ForegroundColor = color != null ? (ConsoleColor)color : DefaultColor;
            Console.WriteLine("[{0}] {1}", DateTime.Now.ToString(), text);

            Console.ForegroundColor = ConsoleColor.Gray;
        }
    }
}
