# OCAPE
Online Compiler and Performance Evaluator (OCAPE) for C++, C#, Python and Java

root
|
|-CompilationEngine - contains source code and .exe for the Compilation Engine
|
|-DemoFiles - contains a selection of demonstration source code
|
|-EvaluationEngine - contains source code and .exe for the Evaluation Engine
|
|-ExeOutputs - folder where results are stored after compilation, evaluation and report generation
|
|-ReportEngine - contains source code and .exe for the Report Generator
|
|-SubmissionQueue - contains source code and .exe for the Submission Queue
|
|-Tasks - stores task parameters and tests for a task
|
|-TemporaryMoved - stores source code for compilation
|
|-Uploads - stores source code uploaded by users
|
|-Utils - contains binaries and scripts for compiling C++ and C# code on Windows
|
|-WebFrontend - contains source code and .dll for the Website Frontend
|
|-readme.txt - this file

All components, apart from the Website Interface, can be compiled using Visual Studio 2010, 2012 or 2013, or the Mono dmcs compiler on Linux. 
The Website Interface requires an installation of Visual Studio 2013 to compile, as well as an installation of IIS to run (supplied with Visual Studio).