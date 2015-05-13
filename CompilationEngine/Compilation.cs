using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;
using System.IO;
using CompilationEngine.Compilers;

namespace CompilationEngine
{
    class Compilation
    {
        static string newPath = Path.Combine(
                                    AppDomain.CurrentDomain.BaseDirectory,
                                    "../../../TemporaryMoved");

        static string exePath = Path.Combine(
                                AppDomain.CurrentDomain.BaseDirectory,
                                "../../../ExeOutputs");

        static string utilityPath = Path.Combine(
                                AppDomain.CurrentDomain.BaseDirectory,
                                "../../../Utils");

        static string evalEnginePath = Path.Combine(
                                    AppDomain.CurrentDomain.BaseDirectory,
                                    "../../../EvaluationEngine/bin/Debug/EvaluationEngine.exe");

        static void Main(string[] args)
        {
            Console.WriteLine("Starting compilation engine.");

            if(args.Length >= 2)
            {
                string filename = args[0];

                string taskID = args[1];

                Console.WriteLine("Accessing file " + filename);

                FileInfo finfo = new FileInfo(filename);

                string userName = finfo.Directory.Name;

                //string outputFile = Path.Combine(exePath, taskID, userName, finfo.Name + ".exe");
                string outputFolder = Path.Combine(exePath, taskID, userName);

                //Directory.CreateDirectory(Path.Combine(exePath, taskID, userName));
                Directory.CreateDirectory(outputFolder);

                int exitCode = 1;

                string extension = Path.GetExtension(filename);

                BaseCompiler compiler = null;

                Console.WriteLine("Using compiler for " + extension);

                if(extension.Equals(".cpp"))
                {
                    compiler = new GCCCompiler();
                    //compiler = new VSCPPCompiler();

                    //exitCode = CompileVCPP(filename, outputFile);
                    //exitCode = CompileGCC(filename, outputFile);
                }
                
                if(extension.Equals(".cs"))
                {
                    //exitCode = CompileCSharp(filename, outputFile);
                    compiler = new VSCSCompiler();
                }

                if(extension.Equals(".py"))
                {
                    compiler = new PythonCompiler();
                }

                if(extension.Equals(".java"))
                {
                    compiler = new JavaCompiler();
                }
				
                if (compiler != null)
                {
                    exitCode = compiler.Compile(utilityPath, filename, outputFolder);
                }

                string outputFile = compiler.GetOutputFile();

                Console.WriteLine("Generated output file " + outputFile);

                if (exitCode == 0)
                {
                    //Compilation done. Can pass on to evaluator.
                    System.Console.WriteLine("Compilation successful!");
                    PassToEvaluation(outputFile, taskID);
                }
                else
                {
                    //Failure. Do stuff accordingly.
                    System.Console.WriteLine("Compilation failed.");

                    //Create empty outputFile
                    StreamWriter sw = new StreamWriter(outputFile);
                    sw.Close();

                    PassToEvaluation(outputFile, taskID);
                    //Generate failure report and flag appropriately
                }
            }

            

            //System.Console.ReadKey(true);
        }

        static int PassToEvaluation(string filename, string taskID)
        {
            ProcessStartInfo start = new ProcessStartInfo();
            start.Arguments = '"' + filename + '"' + " " + taskID;
            start.FileName = evalEnginePath;
            //start.WindowStyle = ProcessWindowStyle.Hidden;
            //start.CreateNoWindow = false;

            using (Process proc = Process.Start(start))
            {
                proc.WaitForExit();

                return proc.ExitCode;
            }
        }
    }
}
