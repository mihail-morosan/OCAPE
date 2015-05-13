using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EvaluationEngine.Evaluators
{
    class PythonEvaluator : BaseEvaluator
    {
        string filename;
        Process myProcess;

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
            myProcess.StartInfo.UseShellExecute = true;
            myProcess.StartInfo.WorkingDirectory = finfo.Directory.FullName;

            //myProcess.StartInfo.FileName = "cmd.exe";
            //myProcess.StartInfo.Arguments = "/c python " + filename;

            if (!IsLinux)
            {
                myProcess.StartInfo.FileName = "python.exe";
            }
            else
            {
                myProcess.StartInfo.FileName = "python";
            }

            myProcess.StartInfo.Arguments = '"' + filename + '"';

            return 0;
        }

        public void Dispose()
        {
            
        }

        public bool HasExited()
        {
            if (myProcess != null)
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

        public void Start()
        {
            if (myProcess != null)
                myProcess.Start();
        }

        public void Kill()
        {
            if (myProcess != null)
                myProcess.Kill();
        }
    }
}
