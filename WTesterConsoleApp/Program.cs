using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
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
                Console.WriteLine(String.Format("The file '{0}' was not found.", filename));
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
            List<string> testLines = new List<string>();
            string[] lines = File.ReadAllLines(filename);
            WTest tester = new WTest(lines);
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
            Console.ForegroundColor = ConsoleColor.Gray;
            string callingFunction = "$." + functionName + "(";
            foreach (object parameter in parameters)
            {
                if (parameter is string)
                {
                    callingFunction += "\"" + parameter.ToString() + "\", ";
                }
                else
                {
                    callingFunction += parameter.ToString() + ", ";
                }
            }
            callingFunction = callingFunction.Trim().Trim(',');
            callingFunction += ")";
            Console.WriteLine(callingFunction);
            if (success)
            {
                Console.ForegroundColor = ConsoleColor.Green;
            }
            else
            {
                Console.ForegroundColor = ConsoleColor.Red;
            }
            Console.WriteLine(message);
        }
    }
}
