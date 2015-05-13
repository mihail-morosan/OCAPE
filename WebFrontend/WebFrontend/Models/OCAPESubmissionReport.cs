using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace WebFrontend.Models
{
    public class OCAPESubmissionReport
    {
        [Key]
        [ForeignKey("Submission")]
        public int SubmissionID { get; set; }
        public string Result { get; set; }

        public double Score { get; set; }

        public string CompiledSuccessfully { get; set; }

        public virtual List<OCAPESubmissionReportRun> OCAPESubmissionReportRuns { get; set; }

        //public int? SubmissionID { get; set; }
        //public virtual List<OCAPESubmission> OCAPESubmissions { get; set; }
        //[InverseProperty("Report")]
        public virtual OCAPESubmission Submission { get; set; }

        /*public OCAPESubmissionReport(OCAPESubmission OCAPESubmission)
        {
            this.OCAPESubmission = OCAPESubmission;
            OCAPESubmissionReportRuns = new List<OCAPESubmissionReportRun>();
            //OCAPESubmissions = new List<OCAPESubmission>();
        }*/

        public OCAPESubmissionReport()
        {

            OCAPESubmissionReportRuns = new List<OCAPESubmissionReportRun>();
        }

        public double AverageRuntime()
        {
            if (OCAPESubmissionReportRuns.Count > 0)
            {
                var results = OCAPESubmissionReportRuns.Where(s => s.IsCorrect > 0);
                if(results.Count()>0)
                    return results.Average(s => s.TimePassed);
                else
                    return -1;
            }
            else
            {
                return -1;
            }
        }

        public double AverageMemoryUsage()
        {
            if (OCAPESubmissionReportRuns.Count > 0)
            {
                var results = OCAPESubmissionReportRuns.Where(s => s.IsCorrect > 0);
                if (results.Count() > 0)
                    return results.Average(s => s.MemoryUsage);
                else
                    return -1;
            }
            else
            {
                return -1;
            }
        }
    }
}