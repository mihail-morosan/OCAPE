using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.IO;
using System.Linq;
using System.Web;

namespace WebFrontend.Models
{
    public class OCAPETask
    {
        public int ID { get; set; }
        [DisplayName("Title")]
        public string Title { get; set; }
        [DisplayName("Description")]
        public string Description { get; set; }

        [DisplayName("Is Task Public")]
        public bool IsPublic { get; set; }

        [DisplayName("Is Leaderboard Public")]
        public bool IsLeaderboardPublic { get; set; }

        public DateTime CreateDate { get; set; }
        [DisplayFormat(DataFormatString = "{0:d}")]
        [DisplayName("Start Date")]
        public DateTime StartDate { get; set; }
        [DisplayFormat(DataFormatString = "{0:d}")]
        [DisplayName("Deadline")]
        public DateTime DeadlineDate { get; set; }
        
        public virtual ApplicationUser Owner { get; set; }

        [DisplayName("Compiler To Use")]
        public string CompilerToUse { get; set; }

        [DisplayName("Are Restrictions Public")]
        public bool IsMaxBoundaryPublic { get; set; }
        [DisplayName("Maximum Memory Usage (kb)")]
        public long MaxMemoryUsage { get; set; }
        [DisplayName("Maximum Runtime (ms)")]
        public long MaxTimeToRun { get; set; }

        [DisplayName("Is Weighing Public")]
        public bool IsWeightingPublic { get; set; }

        [DisplayName("Absolute (1) or Relative (2) Scoring")]
        public int ScoringType { get; set; }

        [DisplayName("Memory Usage Scoring Weight")]
        public double MemoryUsageWeight { get; set; }
        [DisplayName("Runtime Scoring Weight")]
        public double TimeToRunWeight { get; set; }

        [DisplayName("Number of Evaluations per Test")]
        public double EvaluationsPerTest { get; set; }

        public void GenerateTaskDetailsFolder()
        {
            string path = Path.Combine(
                                    AppDomain.CurrentDomain.BaseDirectory,
                                    "../../Tasks", ID.ToString());

            Directory.CreateDirectory(path);

            System.IO.StreamWriter file = new System.IO.StreamWriter(Path.Combine(path, ID.ToString()));

            file.WriteLine(MaxTimeToRun.ToString());
            file.WriteLine(MaxMemoryUsage.ToString());

            file.WriteLine(EvaluationsPerTest.ToString());

            file.WriteLine(ScoringType.ToString());
            //file.WriteLine(TimeToRunWeight.ToString());
            //file.WriteLine(MemoryUsageWeight.ToString());

            file.Close();
        }
    }

    
}