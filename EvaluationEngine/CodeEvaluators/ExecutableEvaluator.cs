using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EvaluationEngine.Evaluators
{
    class ExecutableEvaluator : BaseEvaluator
    {
        static string pathToSandboxie = Path.Combine(
                                AppDomain.CurrentDomain.BaseDirectory,
                                "../../../Utils/Sandboxie/Start.exe");

        Process myProcess;
        string filename;

        bool useSandboxie = false;

        public void Dispose()
        {
            if (useSandboxie)
            {
                Process terminateSandboxie = new Process();

                terminateSandboxie.StartInfo.UseShellExecute = true;

                terminateSandboxie.StartInfo.FileName = pathToSandboxie;
                terminateSandboxie.StartInfo.Arguments = "/terminate_all";

                terminateSandboxie.Start();
                while (!terminateSandboxie.HasExited)
                {
                }
            }

            
        }

        public void Start()
        {
            if (myProcess != null)
                myProcess.Start();
        }

        public static bool IsLinux
        {
            get
            {
                int p = (int)Environment.OSVersion.Platform;
                return (p == 4) || (p == 6) || (p == 128);
            }
        }

        public int Initialise(string Filename)
        {
            filename = Filename;
            FileInfo finfo = new FileInfo(filename);

            myProcess = new Process();

            myProcess.StartInfo.UseShellExecute = false;

            myProcess.StartInfo.WorkingDirectory = finfo.Directory.FullName;

            if (!useSandboxie)
            {
                myProcess.StartInfo.FileName = filename;
            }
            else
            {
                //Maybe use Sandboxie
                myProcess.StartInfo.UseShellExecute = true;
                myProcess.StartInfo.FileName = pathToSandboxie;
                myProcess.StartInfo.Arguments = "/wait " + filename;
            }

            /* myProcess.Exited += (object sender, EventArgs e) =>
            {
                //memoryUsage = myProcess.PeakWorkingSet64;
                //System.Console.WriteLine("Max memory " + myProcess.PeakWorkingSet64);
                //exitTime = myProcess.ExitTime;

                //System.Console.WriteLine("Exit time: " + exitTime.ToShortTimeString());
            };*/

            return 0;
        }


        public bool HasExited()
        {
            if(myProcess != null)
                return myProcess.HasExited;
            return true;
        }


        public void RefreshEvaluatorData()
        {
            if (myProcess != null)
                myProcess.Refresh();
        }


        public long GetMemoryUsage()
        {
            if (myProcess != null)
                return myProcess.PeakWorkingSet64;
            return 0;
        }

        public void Kill()
        {
            if (myProcess != null)
                myProcess.Kill();
        }
    }
}
