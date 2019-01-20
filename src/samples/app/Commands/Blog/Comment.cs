using System;

namespace App.Commands.Blog
{
    public static class Comment
    {
        public static void Add(string name)
        {
            Console.WriteLine($"Add comment: {name}");
        }

        public static void Delete(string name)
        {
            Console.WriteLine($"Delete comment: {name}");
        }
    }
}
