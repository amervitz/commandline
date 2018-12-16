using System;

namespace App.Commands.Internet.Web
{
    public static class Google
    {
        public static void Search(string query)
        {
            Console.WriteLine("https://www.google.com/?q=" + query);
        }
    }
}
