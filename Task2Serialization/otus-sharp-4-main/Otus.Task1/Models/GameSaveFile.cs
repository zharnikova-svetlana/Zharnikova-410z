using System;
using System.Xml.Serialization;
using System.Text.Json.Serialization;

namespace Otus.Task1.Models
{
    public enum Gender
    {
        None = 0,
        Male = 1,
        Female = 2,
    }

    [Serializable]
    public class Item
    {
        public string Name { get; set; }

        private int _quantity;
        public int Quantity
        {
            get => _quantity;
            set
            {
                if (value < 0) throw new ArgumentException("Quantity cannot be less than 0");
                _quantity = value;
            }
        }
    }

    [Serializable]
    public class User
    {
        [XmlAttribute("level")]
        public int Level { get; set; }

        [JsonPropertyName("u")]
        [XmlElement("u")]
        public string Name { get; set; }

        [XmlIgnore]
        [JsonIgnore]
        public Gender Gender { get; set; }

        [XmlElement("Gender")]
        [JsonPropertyName("Gender")]
        public string GenderString
        {
            get => Gender == Gender.Male ? "m" : Gender == Gender.Female ? "f" : "none";
            set => Gender = value == "m" ? Gender.Male : value == "f" ? Gender.Female : Gender.None;
        }
    }

    [Serializable]
    [XmlRoot("u")]
    public class GameStatus
    {
        public string CurrentLocation { get; set; }

        public User User { get; set; }

        public Item[] Items { get; set; }

        [XmlIgnore]
        [JsonIgnore]
        public (double, double) Coords { get; set; }
    }

    [Serializable]
    public class SaveFile : GameStatus
    {
        public DateTime CreatedDate { get; set; }
        public DateTime? SaveDate { get; set; }
        public string FileName { get; set; }
    }
}