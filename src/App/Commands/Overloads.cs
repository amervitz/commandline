using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace App.Commands
{
    public static class Overloads
    {
        public static string Combine(int first, int second)
        {
            return $"Combine(int first, int second): {first}{second}";
        }

        public static string Combine(int first, int second, int third)
        {
            return $"Combine(int first, int second, int third): {first}{second}{third}";
        }

        public static string Combine(double first, double second, double third)
        {
            return $"Combine(float first, float second, float third): {first}{second}{third}";
        }

        public static void Tester()
        {
            var x = Combine(1, 2, 3);
            var y = Combine(1, 2, 3.0);
        }
    }
}
