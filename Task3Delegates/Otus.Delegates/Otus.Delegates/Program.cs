using System;
using System.Collections.Generic;

namespace Otus.Delegates
{
    class Program
    {
        static void Main(string[] args)
        {
            var strings = new List<string> { "Apple", "Watermelon", "Banana" };
            var max = strings.GetMax(s => (float)s.Length);
            Console.WriteLine($"Max element: {max}");
            Console.WriteLine();

            var searcher = new FileSearcher();
            searcher.FileFound += (sender, e) =>
            {
                Console.WriteLine($"File: {e.FileName}");
                if (e.FileName.EndsWith(".exe")) e.Cancel = true;
            };

            string path = AppDomain.CurrentDomain.BaseDirectory;
            searcher.Search(path);

            Console.WriteLine("\nDone.");
            Console.ReadLine();
        }
    }
} 