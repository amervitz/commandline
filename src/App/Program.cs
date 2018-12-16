using App.Commands;
using Clear;

namespace App
{
    class Program
    {
        static void Main(string[] args)
        {
            // run commands in a class
            var output = Router.Run(typeof(Calculator), args);

            // run commands and subcommands in a namespace
            var output2 = Router.Run("App.Commands", args);
        }
    }
}
