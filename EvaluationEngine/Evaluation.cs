using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;
using EvaluationEngine.Evaluators;

namespace EvaluationEngine
{
    class Evaluation
    {
        static string exePath = Path.Combine(
                                    AppDomain.CurrentDomain.BaseDirectory,
                                    "../../../ExeOutputs");

        static string taskPath = Path.Combine(
                                AppDomain.CurrentDomain.BaseDirectory,
                                "../../../Tasks");

        static string pathToSandboxie = Path.Combine(
                                AppDomain.CurrentDomain.BaseDirectory,
                                "../../../Utils/Sandboxie/Start.exe");

        static string reportEnginePath = Path.Combine(
                                    AppDomain.CurrentDomain.BaseDirectory,
                                    "../../../ReportEngine/bin/Debug/ReportEngine.exe");

        static string TestDataExists(string taskID, int runNr)
        {
            string filename = "in" + runNr.ToString() + ".txt";
            string fullFilename = Path.Combine(taskPath, taskID, "Tests", filename);
            string fullOutputname = Path.Combine(taskPath, taskID, "Correct", "out" + runNr.ToString() + ".txt");
            if(File.Exists(fullFilename) && File.Exists(fullOutputname))
            {
                return fullFilename;
            }

            return "";
        }

        static void Main(string[] args)
        {
            if(args.Length >= 2)
            {
                string filename = args[0];

                string taskID = args[1];

                string raw_data_report = "";

                FileInfo finfo = new FileInfo(filename);

                int maxIterations = 400;
                int maxMemoryusage = 0;
                int nrEvaluations = 1;
                int scoringType = 1;

                StreamReader sr = new StreamReader(Path.Combine(taskPath, taskID, taskID));

                maxIterations = Int32.Parse(sr.ReadLine());

                maxMemoryusage = Int32.Parse(sr.ReadLine()); //5 times that amount of time, to accomodate changes later

                nrEvaluations = Int32.Parse(sr.ReadLine());

                scoringType = Int32.Parse(sr.ReadLine());

                System.Console.WriteLine("Time limit: " + maxIterations.ToString() + " (iterations)");
                System.Console.WriteLine("Max memory usage: " + maxMemoryusage.ToString());

                //We only take files that actually have 10bytes or more in them
                if (finfo.Length > 10)
                {
                    System.Console.WriteLine("The task ID is " + taskID);

                    //Evaluator Initialisation
                    BaseEvaluator evaluator = null;

                    if (finfo.Extension.Equals(".exe"))
                    {
                        evaluator = new ExecutableEvaluator();
                    }

                    if(finfo.Extension.Equals(".py"))
                    {
                        evaluator = new PythonEvaluator();
                    }

                    if(finfo.Extension.Equals(".class"))
                    {
                        evaluator = new JavaEvaluator();
                    }

                    evaluator.Initialise(filename);

                    try
                    {
                        int iteration = 0;
                        long memoryUsage = 0;

                        //Copy input.txt and output.txt

                        //bool correctOutput = false;
                        double totalAccuracyPercentage = 0;
                        //int errorReason = -1; //no error
                        int currentRun = 1;
                        int secondaryElapsedTime = 0;

                        string runExists = TestDataExists(taskID, currentRun);

                        while (runExists.Length > 0)
                        {
                            Console.WriteLine("Test #" + currentRun);

                            File.Copy(runExists, Path.Combine(finfo.Directory.FullName, "input.txt"), true);

                            double tRunTime = 0;
                            double tMemUsage = 0;
                            //correctOutput = true;
                            totalAccuracyPercentage = 0;

                            for (int evalRun = 0; evalRun < nrEvaluations; evalRun++)
                            {
                                iteration = 0; 
                                Console.WriteLine();
                                Console.WriteLine("Evaluation #" + evalRun);
                                try
                                {
                                    if (File.Exists(Path.Combine(finfo.Directory.FullName, "output.txt")))
                                    {
                                        File.Delete(Path.Combine(finfo.Directory.FullName, "output.txt"));
                                    }

                                    Stopwatch sw = Stopwatch.StartNew();

                                    evaluator.Start();

                                    //Evaluate

                                    while (!evaluator.HasExited())
                                    {
                                        evaluator.RefreshEvaluatorData();
                                        try
                                        {
                                            iteration++;

                                            memoryUsage = evaluator.GetMemoryUsage();

                                            Thread.Sleep(5);

                                            if (iteration > maxIterations)
                                            {
                                                evaluator.Kill();
                                            }
                                        }
                                        catch (Exception e)
                                        {
                                            Console.WriteLine(e.Message);
                                        }
                                    }

                                    sw.Stop();

                                    double thisRunResult = 0;
                                    try
                                    {
                                        thisRunResult = CompareToCorrect(
                                            Path.Combine(finfo.Directory.FullName, "output.txt"), 
                                            Path.Combine(taskPath, taskID, "Correct", "out" + currentRun.ToString() + ".txt"), 
                                            scoringType == 1);
                                        totalAccuracyPercentage += thisRunResult;
                                    }
                                    catch (Exception e)
                                    {
                                        Console.WriteLine(" -- Crashed in opening the output file --");
                                        System.Console.WriteLine(e.Message);
                                        //correctOutput = false;
                                        //errorReason = -3; //no output file
                                        totalAccuracyPercentage = 0;
                                    }

                                    double runTime = sw.Elapsed.TotalMilliseconds;

                                    Console.WriteLine("Memory usage Private: " + (memoryUsage / 1024.0).ToString() + "kb");
                                    Console.WriteLine("Exit time: " + runTime);
                                    Console.WriteLine("Secondary exit time: " + secondaryElapsedTime);
                                    Console.WriteLine("Correct output: " + thisRunResult.ToString());

                                    tRunTime += runTime;
                                    tMemUsage += memoryUsage;
                                } catch (Exception e)
                                {
                                    Console.WriteLine(" -- Crashed in evaluation --");
                                    Console.WriteLine(e.Message);
                                    totalAccuracyPercentage = 0;
                                }
                            } // End multiple evaluations

                            tRunTime = tRunTime / nrEvaluations;
                            tMemUsage = tMemUsage / nrEvaluations;

                            double averageAccuracy = 0;

                            int result = 0;

                            //if (scoringType == 1)
                            {
                            //    result = correctOutput ? 100 : 0;
                            }
                            //else
                            {
                                averageAccuracy = (totalAccuracyPercentage / nrEvaluations) * 100;
                                result = (int)averageAccuracy;
                            }

                            if (result < 0)
                                result = 0;

                            //Do CSV
                            //raw_data_report += currentRun + "," + (correctOutput ? "1" : "0") + "," + (memoryUsage / 1024.0).ToString() + "," + (long)runTime + " \n";

                            //raw_data_report += currentRun + "," + (correctOutput ? "1" : "0") + "," + ((long)(tMemUsage / 1024.0)).ToString() + "," + (long)tRunTime + " \n";

                            raw_data_report += currentRun + "," + result.ToString() + "," + ((long)(tMemUsage / 1024.0)).ToString() + "," + (long)tRunTime + " \n";

                            currentRun++;
                            runExists = TestDataExists(taskID, currentRun);
                        } //End while(run exists)
                    }
                    catch (Exception e)
                    {
                        Console.WriteLine(e.Message);
                    }

                    //Write report

                    string reportPath = Path.Combine(exePath, filename + ".raw.txt");
                    System.IO.StreamWriter file = new System.IO.StreamWriter(reportPath);
                    file.Write(raw_data_report);

                    file.Close();

                    PassToReportGeneration(reportPath, taskID, filename);

                    evaluator.Dispose();
                }
                else
                {
                    //Evaluation also failed
                    PassToReportGeneration("FAIL", taskID, filename);
                }
            }
            //Console.ReadKey();
        }

        static double CompareToCorrect(string testFile, string correctFile, bool isAbsoluteScoring)
        {
            double accuracy = 1;

            Console.WriteLine(" -Reading Correct file");
            string[] correctLines = System.IO.File.ReadAllLines(correctFile);

            Console.WriteLine(" -Reading User Output file");
            string[] outputLines = System.IO.File.ReadAllLines(testFile);

            int incorrectLines = 0;

            if (correctLines.Length != outputLines.Length)
            {
                System.Console.WriteLine(correctLines.Length + " " + outputLines.Length);
                //correctOutput = false;
                //errorReason = -2; //Incorrect nr of lines
                accuracy = -2;
            }
            else
            {

                //Compare to output.txt
                for (int i = 0; i < correctLines.Count(); i++)
                {
                    try
                    {
                        string cLine = correctLines[i];

                        //Don't care about any garbage data being placed where an empty line would be
                        if (cLine.Length > 0)
                        {
                            string oLine = outputLines[i];

                            if (cLine.CompareTo(oLine) != 0)
                            {
                                //This test failed
                                if (isAbsoluteScoring)
                                {
                                    accuracy = -1;
                                    i = correctLines.Count();
                                    //correctOutput = false;
                                }
                                else
                                {
                                    incorrectLines++;
                                }
                            }
                        }

                    }
                    catch (Exception e)
                    {
                        Console.WriteLine(" -- Crashed in comparing outputs --");
                        System.Console.WriteLine(e.Message);
                    }
                }
            }

            if (!isAbsoluteScoring)
            {
                accuracy = (1.0 - ((double)incorrectLines / correctLines.Count()));
            }

            return accuracy;
        }

        static int PassToReportGeneration(string filename, string taskID, string submissionname)
        {
            ProcessStartInfo start = new ProcessStartInfo();
            start.Arguments = '"' + filename + '"' + " " + taskID + " " + '"' + submissionname + '"';
            start.FileName = reportEnginePath;
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
