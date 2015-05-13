using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace CompilationEngine.Compilers
{
    class JavaCompiler : BaseCompiler
    {
        string outputFile;
        public string GetOutputFile()
        {
            return outputFile;
        }


        public static bool IsLinux
        {
            get
            {
                int p = (int)Environment.OSVersion.Platform;
                return (p == 4) || (p == 6) || (p == 128);
            }
        }

        public int Compile(string utilityPath, string filename, string outputFolder)
        {
            FileInfo finfo = new FileInfo(filename);

            string[] correctLines = System.IO.File.ReadAllLines(filename);
            int i = 0;
            foreach(string line in correctLines)
            {
                correctLines[i] = Regex.Replace(line, @"public[\s]+class.*{?", "public class " + Path.GetFileNameWithoutExtension(filename));
                if(line.Contains("{") && !line.Equals(correctLines[i]))
                {
                    correctLines[i] += "{";
                }
                i++;
            }

            System.IO.StreamWriter file = new System.IO.StreamWriter(filename);
            foreach (string line in correctLines)
            {
                file.WriteLine(line);
            }

            file.Close();
            
            outputFile = Path.Combine(outputFolder, Path.GetFileNameWithoutExtension(filename) + ".class");

            System.Console.WriteLine("I will compile file " + filename);

            System.Console.WriteLine("This file " + (File.Exists(filename) ? "exists." : "does not exists."));

            int exitCode;
            ProcessStartInfo start = new ProcessStartInfo();
            start.Arguments = "-d " + '"' + outputFolder + '"' + " " + '"' + filename + '"';

            System.Console.WriteLine("Arguments passed: " + start.Arguments);

            //start.FileName = Path.Combine(jdk, "javac.exe");
            if (!IsLinux)
            {
                start.FileName = "javac.exe";
            }
            else
            {
                start.FileName = "javac";
            }
            start.UseShellExecute = false;

            start.RedirectStandardError = true;
            start.RedirectStandardOutput = true;

            start.WorkingDirectory = finfo.Directory.FullName;

            //start.WindowStyle = ProcessWindowStyle.Hidden;
            //start.CreateNoWindow = true;

            //start.WorkingDirectory = Path.Combine(jdk);

            Console.WriteLine(start.FileName);

            // Run the external process & wait for it to finish
            using (Process proc = Process.Start(start))
            {
                proc.WaitForExit();

                // Retrieve the app's exit code
                exitCode = proc.ExitCode;

                System.Console.WriteLine(proc.StandardError.ReadToEnd());
                System.Console.WriteLine(proc.StandardOutput.ReadToEnd());
            }

            //File.Copy(Path.Combine(outputFolder, Path.GetFileNameWithoutExtension(filename) + ".class"), outputFile, true);
            //File.Delete(Path.Combine(outputFolder, Path.GetFileNameWithoutExtension(filename) + ".class"));

            return exitCode;
        }

        public bool AcceptsExtension(string extension)
        {
            return extension.Equals(".java");
        }

    }
}
