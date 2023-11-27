using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;

namespace Cognition.Classes
{
    public class MathModuleController
    {
        public static string rumModule(string fileName, string filePath)
        {
            Process myProcess = new Process();
            string parameters = "";
            ProcessStartInfo processInfo = new ProcessStartInfo("cmd.exe", "/c " + fileName);
            try
            {
                processInfo.UseShellExecute = false;
                processInfo.WorkingDirectory = filePath;

                myProcess.StartInfo = processInfo;
                myProcess.StartInfo.UseShellExecute = false;
                myProcess.StartInfo.RedirectStandardOutput = true;
                myProcess.StartInfo.RedirectStandardInput = true;
                myProcess.StartInfo.CreateNoWindow = true;
                myProcess.Start();
                myProcess.StandardInput.WriteLine("/c exit");
                string output = myProcess.StandardOutput.ReadToEnd();
                return output;
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
        }
    }
}
