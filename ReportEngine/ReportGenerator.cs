using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ReportEngine
{
    class ReportGenerator
    {
        static string exePath = Path.Combine(
                                    AppDomain.CurrentDomain.BaseDirectory,
                                    "../../../ExeOutputs");

        static void Main(string[] args)
        {
            Console.WriteLine("Report Generation Started.");
            if (args.Length >= 3)
            {
                string filename = args[0];

                string taskID = args[1];

                string subname = args[2];

                System.Console.WriteLine("The task ID is " + taskID);
                System.Console.WriteLine("Filename is " + filename);
                System.Console.WriteLine("Submission name is " + subname);


                string reportPath = Path.Combine(exePath, subname + ".report.txt");
                System.IO.StreamWriter file = new System.IO.StreamWriter(reportPath);

                System.Console.WriteLine("Writing to " + reportPath);

                if (!filename.Equals("FAIL"))
                {

                    string[] reportLines = System.IO.File.ReadAllLines(filename);

                    file.WriteLine("[");

                    string s = "";
                    for (int i = 0; i < reportLines.Count(); i++)
                    {
                        s = CSVToJSON(reportLines[i]);
                        if (s.Length > 4)
                        {
                            if (i < reportLines.Count() - 1)
                                s += ",";
                            file.WriteLine(s);
                        }
                    }

                    file.WriteLine("]");
                }
                else
                {
                    //Compilation failed
                    file.WriteLine("FAIL");
                }

                file.Close();
            }

            return;
        }

        static string CSVToJSON(string csv)
        {
            List<string> items = csv.Split(',').ToList();

            if (items.Count >= 4)
            {

                //System.Console.WriteLine(csv);
                //System.Console.WriteLine(items.Count);

                string res = "{";

                res += '"' + "Test" + '"' + ":" + '"' + items[0] + '"' + ", ";
                //res += '"' + "IsCorrect" + '"' + ":" + (items[1].Equals("1") ? "true" : "false") + ", ";
                res += '"' + "IsCorrect" + '"' + ":" + '"' + items[1] + '"' + ", ";
                res += '"' + "MemoryUsage" + '"' + ":" + '"' + items[2] + '"' + ", ";
                res += '"' + "TimePassed" + '"' + ":" + '"' + items[3] + '"';

                res += "}";

                return res;
            }
            return "";
        }
    }
}
