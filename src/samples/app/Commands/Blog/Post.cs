using System;

namespace App.Commands.Blog
{
    public static class Post
    {
        public static void Add(string name)
        {
            Console.WriteLine($"Add post: {name}");
        }

        public static void Delete(string name)
        {
            Console.WriteLine($"Delete post: {name}");
        }
    }
}
