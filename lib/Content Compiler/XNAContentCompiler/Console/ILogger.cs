using System;

using Con = System.Console;
namespace XNAContentCompiler.Console
{
    public interface ILogger
    {
        void Error(string formatString,  params string[] parameters);
        void Error(string message);

        void Message(string formatString, params string[] parameters);
        void Message(string message);

        void Success(string formatString, params string[] parameters);
        void Success(string message);

        void Warn(string formatString, params string[] parameters);
        void Warn(string message);
    }

    class ConsoleLogger : ILogger
    {
        private void PrintFormat(string formatString, ConsoleColor color,  params object[] args)
        {
            Print(string.Format(formatString, args), color);
        }

        private void Print(string message, ConsoleColor color = ConsoleColor.White)
        {
            var prevColor = Con.ForegroundColor;
            Con.ForegroundColor = color;

            Con.WriteLine(message);

            Con.ForegroundColor = prevColor;
        }

        public void Error(string formatString, params string[] parameters)
        {
            PrintFormat(formatString, ConsoleColor.Red, parameters);
        }

        public void Error(string message)
        {
            Print(message, ConsoleColor.Red);
        }

        public void Message(string formatString, params string[] parameters)
        {
            PrintFormat(formatString, ConsoleColor.White, parameters);
        }

        public void Message(string message)
        {
            Print(message);
        }

        public void Success(string formatString, params string[] parameters)
        {
            PrintFormat(formatString, ConsoleColor.Green, parameters);
        }

        public void Success(string message)
        {
            Print(message, ConsoleColor.Green);
        }
        public void Warn(string formatString, params string[] parameters)
        {
            PrintFormat(formatString, ConsoleColor.Yellow, parameters);
        }

        public void Warn(string message)
        {
            Print(message, ConsoleColor.Yellow);
        }
    }
}