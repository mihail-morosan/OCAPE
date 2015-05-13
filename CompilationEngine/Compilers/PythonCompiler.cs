using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace CompilationEngine.Compilers
{
    class PythonCompiler : BaseCompiler
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
            outputFile = Path.Combine(outputFolder, finfo.Name);

            Console.WriteLine("PYTHON");
            Console.WriteLine("Copying from " + filename + " to " + outputFile);
            File.Copy(filename, outputFile, true);

            return 0;
        }

        public bool AcceptsExtension(string extension)
        {
            return extension.Equals(".py");
        }
    }
}
