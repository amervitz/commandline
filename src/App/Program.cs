using App.Commands;
using Clear;
using System;

namespace App
{
    public class Program
    {
        static void Main(string[] args)
        {
            // run commands in a class
            // var output = Router.Invoke(typeof(Calculator), args);

            // run commands and subcommands in a namespace
            var output = Router.Invoke("App.Commands", args);
            Console.Write(output);

            // run a specific method with 4 parameters and a return value
            // var output = Router.Invoke<string, string, string, string, string>(Overloads.Combine, args);
            // Console.Write(output);

            // run a specific method with no parameters and no return value
            // Router.Invoke(Types.Void, args);

            // run a method with 2 int parameters and an int return value
            // Router.Invoke<int?, int, int>(Calculator.Add, args);
        }
    }
}
