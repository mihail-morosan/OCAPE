using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace WebFrontend.Models
{
    public class OCAPESubmissionReportRun
    {
        [Index]
        public int ID { get; set; }
        public virtual OCAPESubmissionReport Report { get; set; }
        public int Test { get; set; }
        public int IsCorrect { get; set; }
        public int MemoryUsage { get; set; }
        public int TimePassed { get; set; }

        public double IsWithinConstraints(bool absoluteScoring)
        {
            if(absoluteScoring)
                return (MemoryUsage <= Report.Submission.Task.MaxMemoryUsage && TimePassed <= Report.Submission.Task.MaxTimeToRun && IsCorrect == 100) ? 1 : 0;
            else
                return (MemoryUsage <= Report.Submission.Task.MaxMemoryUsage && TimePassed <= Report.Submission.Task.MaxTimeToRun && IsCorrect > 0) ? (double)IsCorrect/100.0 : 0;
        }

        public static List<OCAPESubmissionReportRun> GetFromJSON(string json)
        {
            return JsonConvert.DeserializeObject<List<OCAPESubmissionReportRun>>(json);
        }
    }
}