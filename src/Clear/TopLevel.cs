using System;
using System.ComponentModel.DataAnnotations;

namespace Clear
{
    public static class TopLevel
    {
        /// <summary>
        /// Adds two numbers together
        /// </summary>
        /// <param name="first">The first number</param>,
        /// <param name="second">The second number</param>
        [Display(
            ShortName = "a",
            Name = "Add",
            Description = "Adds two numbers together",
            Prompt = "add --first 1 --second 2"
        )]
        public static void Add(int? first, int second = 0)
        {
            var result = first.GetValueOrDefault() + second;
            Console.WriteLine(result);
        }

        public static int Subtract(int first, int second)
        {
            var result = first - second;
            Console.WriteLine(result);
            return result;

        }

        public static int Modulo(int first, int second)
        {
            var result = first % second;
            Console.WriteLine(result);
            return result;
        }

        public static float Multiply(float first, float second = 1)
        {
            var result = first * second;
            Console.WriteLine(result);
            return result;
        }

        public static float Divide(float first, float second)
        {
            var result = first / second;
            Console.WriteLine(result);
            return result;
        }

        public static bool IsTrue(bool isTrue = true)
        {
            Console.WriteLine(isTrue);
            return isTrue;
        }

        public static DateTime GetDate()
        {
            Console.WriteLine(DateTime.Now);
            return DateTime.Now;
        }
    }
}
