using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace Otus.ParallelFiles
{
    class Program
    {
        static async Task Main(string[] args)
        {
            string folderPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "TestFiles");

            if (!Directory.Exists(folderPath))
            {
                Console.WriteLine($"Папка не найдена: {folderPath}");
                return;
            }

            var files = Directory.GetFiles(folderPath, "*.txt");

            if (files.Length == 0)
            {
                Console.WriteLine("В папке testfils нет .txt файлов.");
                return;
            }

            Console.WriteLine($"Найдено файлов: {files.Length}");
            Console.WriteLine(new string('-', 30));

            Console.WriteLine("Сценарий 1: Одна Task на каждый файл");
            var sw1 = Stopwatch.StartNew();

            var tasks1 = files.Select(file => Task.Run(() =>
            {
                string content = File.ReadAllText(file);
                return content.Count(c => c == ' ');
            })).ToArray();

            var results1 = await Task.WhenAll(tasks1);
            sw1.Stop();

            Console.WriteLine($"Всего пробелов: {results1.Sum()}");
            Console.WriteLine($"Время выполнения: {sw1.ElapsedMilliseconds} ms");
            Console.WriteLine(new string('-', 30));

            Console.WriteLine("Сценарий 2: Одна Task на каждую строку");
            var sw2 = Stopwatch.StartNew();

            var fileTasks = files.Select(async file =>
            {
                var lines = File.ReadAllLines(file);
                var lineTasks = lines.Select(line => Task.Run(() => line.Count(c => c == ' '))).ToArray();
                var lineResults = await Task.WhenAll(lineTasks);
                return lineResults.Sum();
            }).ToArray();

            var results2 = await Task.WhenAll(fileTasks);
            sw2.Stop();

            Console.WriteLine($"Всего пробелов: {results2.Sum()}");
            Console.WriteLine($"Время выполнения: {sw2.ElapsedMilliseconds} ms");
            Console.WriteLine(new string('-', 30));

            Console.WriteLine("Работа завершена. Нажмите любую клавишу...");
            Console.ReadKey();
        }
    }
}