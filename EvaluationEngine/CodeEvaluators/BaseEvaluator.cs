using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EvaluationEngine.Evaluators
{
    interface BaseEvaluator
    {
        int Initialise(string Filename);
        void Dispose();
        bool HasExited();
        void RefreshEvaluatorData();
        long GetMemoryUsage();
        void Start();
        void Kill();
    }
}
