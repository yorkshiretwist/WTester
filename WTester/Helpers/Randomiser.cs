using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;

namespace stillbreathing.co.uk.WTester.Helpers
{
    /// <summary>
    /// Generates random data
    /// </summary>
    public class Randomiser
    {
        private const string firstNamesResource = "stillbreathing.co.uk.WTester.Helpers.CSV_Database_of_First_Names.csv";
        private const string lastNamesResource = "stillbreathing.co.uk.WTester.Helpers.CSV_Database_of_Last_Names.csv";

        /// <summary>
        /// Returns a string of alphabetical characters
        /// </summary>
        /// <param name="minLength"></param>
        /// <param name="maxLength"></param>
        /// <param name="withSpaces"></param>
        /// <returns></returns>
        public string GetAlphabeticalText(int minLength, int maxLength, bool withSpaces)
        {
            string output = string.Empty;
            string letters = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ";
            // add a few spaces in if required
            if (withSpaces)
            {
                letters += "      ";
            }
            Random rand = new Random();
            for (int x = minLength; x <= maxLength; x++)
            {
                output += letters.Substring(rand.Next(letters.Length), 1);
            }
            return output;
        }

        /// <summary>
        /// Returns a random number
        /// </summary>
        /// <param name="min"></param>
        /// <param name="max"></param>
        /// <returns></returns>
        public int GetNumber(int min, int max)
        {
            Random rand = new Random();
            return rand.Next(min, max);
        }

        /// <summary>
        /// Returns a random (Western) first name
        /// </summary>
        /// <returns></returns>
        public string GetFirstName()
        {
            if (firstNames == null || !firstNames.Any())
            {
                LoadFirstNames();
            }

            Random rand = new Random();
            return firstNames.ElementAt(rand.Next(firstNames.Count()));
        }

        /// <summary>
        /// Loads the first names from the CSV into memory
        /// </summary>
        private void LoadFirstNames()
        {
            List<string> names = new List<string>();
            Assembly assembly = Assembly.GetExecutingAssembly();
            using (Stream stream = assembly.GetManifestResourceStream(firstNamesResource))
            using (StreamReader reader = new StreamReader(stream))
            {
                while (!reader.EndOfStream)
                {
                    names.Add(reader.ReadLine());
                }
            }
            firstNames = names;
        }

        private IEnumerable<string> firstNames;

        /// <summary>
        /// Returns a random (Western) last name
        /// </summary>
        /// <returns></returns>
        public string GetLastName()
        {
            if (lastNames == null || !lastNames.Any())
            {
                LoadLastNames();
            }

            Random rand = new Random();
            return lastNames.ElementAt(rand.Next(lastNames.Count()));
        }

        /// <summary>
        /// Loads the last names from the CSV into memory
        /// </summary>
        private void LoadLastNames()
        {
            List<string> names = new List<string>();
            Assembly assembly = Assembly.GetExecutingAssembly();
            using (Stream stream = assembly.GetManifestResourceStream(lastNamesResource))
            using (StreamReader reader = new StreamReader(stream))
            {
                while (!reader.EndOfStream)
                {
                    names.Add(reader.ReadLine());
                }
            }
            lastNames = names;
        }

        private IEnumerable<string> lastNames;
    }
}
