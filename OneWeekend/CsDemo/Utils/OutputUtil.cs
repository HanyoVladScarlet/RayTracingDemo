using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace CsDemo.Utils
{
    public class OutputUtil
    {
        public static void SaveImage(string content)
        {
            SaveImage("image.ppm", content);
        }

        public static void SaveImage(string name, string content)
        {
            if (Path.GetExtension(name) != ".ppm")
                name += ".ppm";
            var path = Directory.GetCurrentDirectory() + "\\" + name;
            using var sw = new StreamWriter(path);
            sw.WriteLine(content);
            Console.WriteLine($"Image \"{name}\" Saving finishes!");
        }

    }
}
