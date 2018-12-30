using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace App.Commands
{
    [DisplayName("calc")]
    public static class Calculator
    {
        public static int Add(int? first, int second = 0)
        {
            var result = first.GetValueOrDefault() + second;
            return result;
        }

        [Display(
            Name = "add",
            Description = "Adds two numbers together"
        )]
        public static int AddWithAttributes
        (
            [Display(
                ShortName = "f",
                Name = "operand1",
                Description = "The first operand"
            )]
            int? first,

            [Display(
                ShortName = "s",
                Name = "operand2",
                Description = "The second operand"
            )]
            int second = 0
        )
        {
            var result = first.GetValueOrDefault() + second;
            return result;
        }

        public static int Subtract(int first, int second)
        {
            var result = first - second;
            return result;
        }

        public static int Modulo(int first, int second)
        {
            var result = first % second;
            return result;
        }

        public static float Multiply(float first, float second = 1)
        {
            var result = first * second;
            return result;
        }

        public static float Divide(float first, float second)
        {
            var result = first / second;
            return result;
        }

        public static bool IsTrue(bool isTrue = true)
        {
            return isTrue == true;
        }

        public static DateTime GetDate()
        {
            return DateTime.Now;
        }
    }
}
