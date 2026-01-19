using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Text.Json;

namespace ReflectionTask
{
    class F
    {
        int i1;
        int i2;
        int i3;
        int i4;
        int i5;
        public int[] mas;

        public F()
        {
            i1 = 1; i2 = 2; i3 = 3; i4 = 4; i5 = 5;
            mas = new int[] { 1, 2 };
        }

        public F Get() => new F();
    }

    public static class MySerializer
    {
        public static string ToCsv(object obj)
        {
            var type = obj.GetType();
            var fields = type.GetFields(BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.Public);

            List<string> values = new List<string>();
            foreach (var field in fields)
            {
                var value = field.GetValue(obj);
                if (field.FieldType.IsArray)
                {
                    var arr = (IEnumerable)value;
                    var elements = new List<string>();
                    foreach (var el in arr) elements.Add(el.ToString());
                    values.Add("[" + string.Join(";", elements) + "]");
                }
                else
                {
                    values.Add(value?.ToString() ?? "");
                }
            }
            return string.Join(",", values);
        }

        public static T FromCsv<T>(string csv) where T : new()
        {
            var obj = new T();
            var fields = typeof(T).GetFields(BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.Public);
            var values = csv.Split(',');

            for (int i = 0; i < fields.Length; i++)
            {
                var field = fields[i];
                var valStr = values[i];

                if (field.FieldType.IsArray)
                {
                    var raw = valStr.Trim('[', ']').Split(';');
                    var array = Array.CreateInstance(field.FieldType.GetElementType(), raw.Length);
                    for (int j = 0; j < raw.Length; j++)
                        array.SetValue(Convert.ChangeType(raw[j], field.FieldType.GetElementType()), j);
                    field.SetValue(obj, array);
                }
                else
                {
                    field.SetValue(obj, Convert.ChangeType(valStr, field.FieldType));
                }
            }
            return obj;
        }
    }

    class Program
    {
        static void Main(string[] args)
        {
            var f = new F();
            int iterations = 100000;
            string csvString = "";

            var sw = Stopwatch.StartNew();
            for (int i = 0; i < iterations; i++)
            {
                csvString = MySerializer.ToCsv(f);
            }
            sw.Stop();
            long mySerTime = sw.ElapsedMilliseconds;

            var swConsole = Stopwatch.StartNew();
            Console.WriteLine(csvString);
            swConsole.Stop();
            double consoleTime = swConsole.Elapsed.TotalMilliseconds;

            sw.Restart();
            for (int i = 0; i < iterations; i++)
            {
                var newF = MySerializer.FromCsv<F>(csvString);
            }
            sw.Stop();
            long myDeserTime = sw.ElapsedMilliseconds;

            sw.Restart();
            string jsonString = "";
            var options = new JsonSerializerOptions { IncludeFields = true };
            for (int i = 0; i < iterations; i++)
            {
                jsonString = JsonSerializer.Serialize(f, options);
            }
            sw.Stop();
            long jsonSerTime = sw.ElapsedMilliseconds;

            sw.Restart();
            for (int i = 0; i < iterations; i++)
            {
                JsonSerializer.Deserialize<F>(jsonString, options);
            }
            sw.Stop();
            long jsonDeserTime = sw.ElapsedMilliseconds;

            Console.WriteLine();
            Console.WriteLine($"Reflection CSV Serializing: {mySerTime} ms");
            Console.WriteLine($"Reflection CSV Deserializing: {myDeserTime} ms");
            Console.WriteLine($"Standard JSON Serializing: {jsonSerTime} ms");
            Console.WriteLine($"Standard JSON Deserializing: {jsonDeserTime} ms");
            Console.WriteLine($"Console output time: {consoleTime} ms");

            Console.ReadLine();
        }
    }
} 