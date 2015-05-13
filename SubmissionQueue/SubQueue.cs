using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Threading;
using System.Diagnostics;

namespace SubmissionQueue
{
    class SubQueue
    {
        static string basePath = Path.Combine(
                                    AppDomain.CurrentDomain.BaseDirectory,
                                    "../../../Uploads");
        static string newPath = Path.Combine(
                            AppDomain.CurrentDomain.BaseDirectory,
                            "../../../TemporaryMoved");
        static void Main(string[] args)
        {
            Console.WriteLine("Submission queue started.");
            bool TimeToEnd = false;
            List<string> directories;
            List<string> files;
            while(!TimeToEnd)
            {
                //Check Uploads directory recursively
                
                directories = new List<string>(Directory.GetDirectories(basePath));

                foreach (string taskID in directories) //User
                {
                    
                    var users = new List<string>(Directory.GetDirectories(Path.Combine(basePath, taskID)));
                    foreach (string user in users)
                    
                    {
                        string userName = new DirectoryInfo(user).Name;
                        string actTaskID = new DirectoryInfo(taskID).Name;

                        files = new List<string>(Directory.GetFiles(Path.Combine(basePath, actTaskID, user)));
                        foreach (string file in files)
                        {
                            //Move new files to TemporaryMoved
                            Directory.CreateDirectory(Path.Combine(newPath, actTaskID, userName));
                            string newDest = Path.Combine(newPath, actTaskID, userName, Path.GetFileName(file));

                            GC.Collect();

                            File.Copy(file, newDest, true);
                            //File.Move(file, newDest);
                            File.Delete(file);

                            Console.WriteLine("Sent " + newDest + " to compiler.");
                            //Pass new files to compiler
                            PassToCompiler(newDest, actTaskID);
                        }

                        files.Clear();
                    }

                    users.Clear();
                }

                directories.Clear();

                Thread.Sleep(1000);
            }
        }

        static string compilationEnginePath = Path.Combine(
                                    AppDomain.CurrentDomain.BaseDirectory,
                                    "../../../CompilationEngine/bin/Debug/CompilationEngine.exe");

        static int PassToCompiler(string filename, string taskID)
        {
            ProcessStartInfo start = new ProcessStartInfo();
            start.Arguments = '"' + filename + '"' + " " + taskID;
            start.FileName = compilationEnginePath;
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
