using System;

namespace App.Commands
{
    public static class Types
    {
        public static void Void()
        {
            Console.Write("Void");
        }

        public static void String(string val)
        {
            Console.WriteLine($"String: {val}");
        }

        public static void Guid(Guid val)
        {
            Console.WriteLine($"Guid: {val}");
        }

        public static void Uri(Uri val)
        {
            Console.WriteLine($"Url: {val}");
        }
    }
}
