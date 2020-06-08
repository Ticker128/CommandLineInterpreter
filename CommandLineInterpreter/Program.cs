using System;

using System.IO;


namespace CommandLineInterpreter
{
    class Program
    {
        static void Main(string[] args)
        {
            Interpreter interpreter = new Interpreter();
            while (true)
            {
                Console.ForegroundColor = ConsoleColor.Green;
                Console.Write(Directory.GetCurrentDirectory().ToLower() + ">");
                Console.ForegroundColor = ConsoleColor.Yellow;
                interpreter.Interpret();
                Console.WriteLine();
            }
        }
    }
}