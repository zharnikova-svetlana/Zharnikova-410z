using System;

namespace Otus.Prototype
{
    public interface IMyCloneable<out T>
    {
        T MyClone();
    }

    public class Device : IMyCloneable<Device>, ICloneable
    {
        public string Brand { get; set; }

        public Device() { }

        public Device(Device previous)
        {
            this.Brand = previous.Brand;
        }

        public virtual Device MyClone() => new Device(this);

        public object Clone() => MyClone();
    }

    public class Computer : Device, IMyCloneable<Computer>
    {
        public int RamSize { get; set; }

        public Computer() { }

        public Computer(Computer previous) : base(previous)
        {
            this.RamSize = previous.RamSize;
        }

        public override Computer MyClone() => new Computer(this);

        new public object Clone() => MyClone();
    }

    public class Laptop : Computer, IMyCloneable<Laptop>
    {
        public bool HasTouchscreen { get; set; }

        public Laptop() { }

        public Laptop(Laptop previous) : base(previous)
        {
            this.HasTouchscreen = previous.HasTouchscreen;
        }

        public override Laptop MyClone() => new Laptop(this);

        new public object Clone() => MyClone();
    }

    class Program
    {
        static void Main(string[] args)
        {
            var originalLaptop = new Laptop
            {
                Brand = "Asus",
                RamSize = 16,
                HasTouchscreen = true
            };

            var clonedLaptop = originalLaptop.MyClone();

            Console.WriteLine("Original: " + originalLaptop.Brand + ", RAM: " + originalLaptop.RamSize);
            Console.WriteLine("Clone:    " + clonedLaptop.Brand + ", RAM: " + clonedLaptop.RamSize);

            clonedLaptop.Brand = "MSI";
            Console.WriteLine("\nAfter modification:");
            Console.WriteLine("Original: " + originalLaptop.Brand);
            Console.WriteLine("Clone:    " + clonedLaptop.Brand);

            Console.ReadKey();
        }
    }
}