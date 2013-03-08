using System;
using System.Collections.Generic;
using System.IO;
using stillbreathing.co.uk.WTester;

namespace stillbreathing.co.uk.WTesterConsoleApp
{
    class Program
    {
        [STAThread]
        static void Main(string[] args)
        {
            if (args.Length == 0)
            {
                Prompt();
            }
            else
            {
                Start(args[0]);
            }
        }

        static void Start(string filename)
        {
            if (!File.Exists(filename))
            {
                Console.WriteLine("The file '{0}' was not found.", filename);
                Prompt();
            }
            else
            {
                StartTest(filename);
            }
        }

        static void Prompt()
        {
            Console.WriteLine("Enter the full path to the WTest script:");
            Start(Console.ReadLine());
        }

        static void StartTest(string filename)
        {
            var testLines = new List<string>();
            string[] lines = File.ReadAllLines(filename);
            var tester = new WTest(lines);
            tester.RunTest(new WTest.PreActionDelegate(PreAction), new WTest.ActionResultDelegate(Report));
            Console.ForegroundColor = ConsoleColor.Gray;
            Console.WriteLine("Type 'exit' then press enter to close the program, or enter the name of another WTest script");
            string command = Console.ReadLine().Trim().ToLower();
            if (command != "exit")
            {
                Start(command);
            }
        }

        static void PreAction(string functionName, List<object> parameters, string message)
        {
            Console.ForegroundColor = ConsoleColor.Gray;
            Console.WriteLine(message);
        }

        static void Report(string functionName, List<object> parameters, bool success, string message)
        {
            string callingFunction = "$." + functionName + "(";
            foreach (object parameter in parameters)
            {
                callingFunction += parameter is string ? "\"" + parameter + "\", " : parameter + ", ";
            }
            callingFunction = callingFunction.Trim().Trim(',');
            callingFunction += ")";

            Console.ForegroundColor = ConsoleColor.Gray;
            Console.WriteLine(callingFunction);

            Console.ForegroundColor = success ? ConsoleColor.Green : ConsoleColor.Red;
            Console.WriteLine(message);
        }
    }
}
