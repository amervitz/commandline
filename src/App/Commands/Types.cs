using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace App.Commands
{
    public static class Types
    {
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
