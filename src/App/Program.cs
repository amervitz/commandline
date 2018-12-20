using App.Commands;
using Clear;
using System;

namespace App
{
    class Program
    {
        static void Main(string[] args)
        {
            // run commands in a class
            // var output = Router.Run(typeof(Calculator), args);

            // run commands and subcommands in a namespace
            var output = Router.Run("App.Commands", args);
            Console.Write(output);
        }
    }
}
