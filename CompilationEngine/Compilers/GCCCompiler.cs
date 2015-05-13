using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CompilationEngine.Compilers
{
    class GCCCompiler : BaseCompiler
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



            outputFile = Path.Combine(outputFolder, finfo.Name + ".exe");

            System.Console.WriteLine("I will compile file " + filename);

            System.Console.WriteLine("This file " + (File.Exists(filename) ? "exists." : "does not exists."));

            int exitCode;
            ProcessStartInfo start = new ProcessStartInfo();
            //start.Arguments = "-static -static-libgcc -static-libstdc++ -mwindows -Ofast -o " + outputFile + " " + filename;
            //start.Arguments = "-static -fno-optimize-sibling-calls -fno-strict-aliasing -lm -s -x c++ -Wl,--stack=268435456 -Ofast -o " + outputFile + " " + filename;

            start.Arguments = "-static -static-libgcc -static-libstdc++ -O3 -o " + '"' + outputFile + '"' + " " + '"' + filename + '"';


            System.Console.WriteLine("Arguments passed: " + start.Arguments);

            if (!IsLinux)
            {
                Console.WriteLine("Running on Windows");
                start.FileName = Path.Combine(utilityPath, "gcc/bin/g++.exe");
            }
            else
            {
                Console.WriteLine("Running on Linux");
                start.FileName = "g++";
            }

            //start.WindowStyle = ProcessWindowStyle.Hidden;
            //start.CreateNoWindow = true;

            start.WorkingDirectory = Path.Combine(utilityPath, "gcc/bin");

            // Run the external process & wait for it to finish
            using (Process proc = Process.Start(start))
            {
                proc.WaitForExit();

                // Retrieve the app's exit code
                exitCode = proc.ExitCode;
            }

            return exitCode;
        }

        public bool AcceptsExtension(string extension)
        {
            return extension.Equals(".cpp");
        }


    }
}
