using System;
using System.IO;
using System.Text.Encodings.Web;
using System.Text.Json;
using System.Text.Unicode;
using System.Xml.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using Otus.Task1.Models;

namespace Otus.Task1
{
    class Program
    {
        static SaveFile Generate1()
        {
            var res = new SaveFile();
            res.Coords = (1241.44, 124145.4);
            res.CurrentLocation = "Dungeon";
            res.User = new User { Level = 10, Name = "Пушкин", Gender = Gender.Male };
            res.Items = new[] { new Item() { Name = "Топор", Quantity = 2 } };
            return res;
        }

        static SaveFile Generate2()
        {
            var res = new SaveFile();
            res.Coords = (121.44, 124.4);
            res.CurrentLocation = "Subway";
            res.User = new User { Level = 10, Name = "Feodorov", Gender = Gender.Female };
            res.Items = new[] { new Item() { Name = "Stick", Quantity = -2 } };
            return res;
        }

        static void SerializeBinary(SaveFile sf)
        {
            try
            {
#pragma warning disable SYSLIB0011 
                var formatter = new BinaryFormatter();
                using (var stream = new FileStream("save.bin", FileMode.Create))
                {
                    formatter.Serialize(stream, sf);
                }
                Console.WriteLine("Binary serialization success.");
            }
            catch (Exception ex) { Console.WriteLine($"Binary Error: {ex.Message}"); }
        }

        static void SerializeJson(SaveFile sf)
        {
            try
            {
                var options = new JsonSerializerOptions
                {
                    Encoder = JavaScriptEncoder.Create(UnicodeRanges.All),
                    WriteIndented = true
                };

                string jsonString = JsonSerializer.Serialize(sf, options);
                File.WriteAllText("save.json", jsonString);
                Console.WriteLine("JSON serialization success.");
            }
            catch (Exception ex) { Console.WriteLine($"JSON Error: {ex.Message}"); }
        }

        static void SerializeXml(SaveFile sf)
        {
            try
            {
                var xmlSerializer = new XmlSerializer(typeof(SaveFile));
                using (var writer = new StreamWriter("save.xml"))
                {
                    xmlSerializer.Serialize(writer, sf);
                }
                Console.WriteLine("XML serialization success.");
            }
            catch (Exception ex) { Console.WriteLine($"XML Error: {ex.Message}"); }
        }

        static void Main(string[] args)
        {
            var g1 = Generate1();

            Console.WriteLine("--- Processing Object 1 ---");
            SerializeBinary(g1);
            SerializeJson(g1);
            SerializeXml(g1);

            Console.WriteLine("\n--- Processing Object 2 ---");
            try
            {
                var g2 = Generate2();
                SerializeBinary(g2);
                SerializeJson(g2);
                SerializeXml(g2);
            }
            catch (Exception ex) { Console.WriteLine($"Error: {ex.Message}"); }

            Console.WriteLine("\nDone!");
            Console.ReadLine();
        }
    }
}