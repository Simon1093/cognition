using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;


public class FileManager
{
    public static string ReadFile(string path)
    {
        string content = System.IO.File.ReadAllText(@path);
        return content;
    }
}

