using System;
using System.IO;

namespace App.Commands
{
    public static class FileSystem
    {
        public static void Dir(string path)
        {
            path = path ?? Environment.CurrentDirectory;

            if(!Directory.Exists(path))
            {
                Console.WriteLine("Invalid directory path");
                return;
            }

            var files = Directory.GetFiles(path);
            if(files.Length > 0)
            {
                Console.WriteLine("Files:");
                Console.WriteLine("-----------");

                foreach (var file in files)
                {
                    Console.WriteLine(file);
                }

            }

            var directories = Directory.GetDirectories(path);
            if (directories.Length > 0)
            {
                if(files.Length > 0)
                {
                    Console.WriteLine();
                }

                Console.WriteLine("Folders:");
                Console.WriteLine("-----------");

                foreach (var dir in directories)
                {
                    Console.WriteLine(dir);
                }
            }
        }
    }
}
