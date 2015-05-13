using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CompilationEngine.Compilers
{
    class VSCPPCompiler : BaseCompiler
    {
        string outputFile;
        public string GetOutputFile()
        {
            return outputFile;
        }

        public int Compile(string utilityPath, string filename, string outputFolder)
        {
            FileInfo finfo = new FileInfo(filename);
            outputFile = Path.Combine(outputFolder, finfo.Name + ".exe");

            int exitCode;
            ProcessStartInfo processInfo;
            Process process;

            processInfo = new ProcessStartInfo("cmd.exe", "/c " + Path.Combine(utilityPath, "VC++/VsDevCmd.bat") + " & cl /w /F268435456 /EHsc /O2 /GA /Fe" + '"' + outputFile + '"' + " " + '"' + filename + '"');

            processInfo.UseShellExecute = false;

            processInfo.RedirectStandardError = true;
            processInfo.RedirectStandardOutput = true;

            process = Process.Start(processInfo);
            process.WaitForExit();

            // *** Read the streams ***
            string output = process.StandardOutput.ReadToEnd();
            string error = process.StandardError.ReadToEnd();

            exitCode = process.ExitCode;

            Console.WriteLine("Standard Output >> " + (String.IsNullOrEmpty(output) ? "(none)" : output));
            Console.WriteLine("Errors >> " + (String.IsNullOrEmpty(error) ? "(none)" : error));
            Console.WriteLine("ExitCode: " + exitCode.ToString(), "ExecuteCommand");
            process.Close();

            return exitCode;
        }


        public bool AcceptsExtension(string extension)
        {
            return extension.Equals(".cpp");
        }
    }
}
