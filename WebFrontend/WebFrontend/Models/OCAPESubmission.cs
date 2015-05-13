using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace WebFrontend.Models
{
    public class OCAPESubmission
    {
        [Key]
        public int ID { get; set; }
        public virtual ApplicationUser Owner { get; set; }
        public virtual OCAPETask Task { get; set; }

        public int? ReportID { get; set; }
        //[ForeignKey("ReportID"), InverseProperty("Submission")]
        public virtual OCAPESubmissionReport Report { get; set; }
        public string Filename { get; set; }
        public string Extension { get; set; }
        public DateTime CreatedAt { get; set; }
        public string OwnerNote { get; set; }

        public void UpdateScore()
        {
            if (Report != null)
            {
                Report.Score = 0;
                foreach (OCAPESubmissionReportRun srr in Report.OCAPESubmissionReportRuns)
                {
                    if (Task.TimeToRunWeight != 0 || Task.MemoryUsageWeight != 0)
                    {
                        Report.Score += ((Task.MaxTimeToRun - srr.TimePassed) * Task.TimeToRunWeight + (Task.MaxMemoryUsage - srr.MemoryUsage) * Task.MemoryUsageWeight) * (srr.IsWithinConstraints(Task.ScoringType == 1));
                    }
                    else
                    {
                        Report.Score += 10 * (srr.IsWithinConstraints(Task.ScoringType == 1));
                    }
                }
            }
        }
    }
}