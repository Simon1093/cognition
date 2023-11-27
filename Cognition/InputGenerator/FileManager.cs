using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace input_generator.Classes
{
    public class FileManager
    {
        public static string ReadFile(string path)
        {
            string content = System.IO.File.ReadAllText(@path);
            return content;
        }

        public static void WriteFile(string data)
        {
            System.IO.File.WriteAllText("generated-matrix.txt", data);
        }
    }
}
