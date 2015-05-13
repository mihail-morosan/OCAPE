using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CompilationEngine.Compilers
{
    interface BaseCompiler
    {
        int Compile(string utilityPath, string filename, string outputFile);
        bool AcceptsExtension(string extension);
        string GetOutputFile();

    }
}
