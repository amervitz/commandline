using System;

namespace App.Commands.Internet.Web
{
    public static class Google
    {
        public static string Search(string query)
        {
            return $"https://www.google.com/search?q={query}";
        }
    }
}
