using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using System.Web.UI.DataVisualization.Charting;
using WebFrontend.Models;

namespace WebFrontend.Controllers
{
    [Authorize]
    public class TasksController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        private UserManager<ApplicationUser> manager;

        public TasksController()
        {
            manager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(db));
        }

        // GET: Tasks
        public ActionResult Index()
        {
            return View(db.Tasks.ToList());
        }

        // GET: Tasks/Leaderboard/5
        public ActionResult Leaderboard(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            OCAPETask oCAPETask = db.Tasks.Find(id);
            if (oCAPETask == null)
            {
                return HttpNotFound();
            }

            if(!oCAPETask.IsLeaderboardPublic && !IsCorrectUserLoggedIn(oCAPETask.Owner.Id))
            {
                return HttpNotFound();
            }

            //All submissions with a report
            var allSubmissions = db.Submissions.Where(s => (s.Task.ID == id && s.Report != null)).OrderBy(g=>-g.Report.Score);

            //var allSubReportRuns = db.SubmissionReportRuns.Where(r => (r.Report.OCAPESubmissions.FirstOrDefault().Task.ID == id)).OrderBy((g) => (g.TimePassed));

            //ViewBag.AllReportRuns = allSubReportRuns.ToList();

            ViewBag.AllSubmissions = allSubmissions.ToList();

            return View(oCAPETask);
        }

        // GET: Tasks/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            OCAPETask oCAPETask = db.Tasks.Find(id);
            if (oCAPETask == null)
            {
                return HttpNotFound();
            }

            var userID = User.Identity.GetUserId();

            //Update user's submissions (if report exists)
            var unfilledsubs = db.Submissions.Where(s => (s.Task.ID == id && s.Owner.Id.Equals(userID) && s.Report == null));
            if (unfilledsubs.Count() > 0)
            {
                var user = manager.FindById(User.Identity.GetUserId());
                string exePath = Path.Combine(
                                    AppDomain.CurrentDomain.BaseDirectory,
                                    "../../ExeOutputs", id.ToString(), user.Email);
                foreach (OCAPESubmission s in unfilledsubs)
                {
                    string filename = Path.Combine(exePath, "submission" + s.ID + s.Extension + ".exe.report.txt");

                    if (Directory.Exists(exePath))
                    {
                        string[] files = System.IO.Directory.GetFiles(exePath, "submission" + s.ID + "*report.txt");
                        if (files.Length > 0)
                            filename = files[0];

                        if (System.IO.File.Exists(filename))
                        {
                            OCAPESubmissionReport sr = new OCAPESubmissionReport();
                            sr.Result = System.IO.File.ReadAllText(filename);

                            if (sr.Result.Substring(0, 4).Equals("FAIL"))
                            {
                                sr.CompiledSuccessfully = "FAIL";
                            }
                            else
                            {
                                sr.CompiledSuccessfully = "TRUE";
                                foreach (OCAPESubmissionReportRun srr in OCAPESubmissionReportRun.GetFromJSON(sr.Result))
                                {
                                    srr.Report = sr;
                                    db.SubmissionReportRuns.Add(srr);
                                    //sr.Score += ((s.Task.MaxTimeToRun - srr.TimePassed) * s.Task.TimeToRunWeight + (s.Task.MaxMemoryUsage - srr.MemoryUsage) * s.Task.MemoryUsageWeight) * (srr.IsCorrect ? 1 : 0);
                                }

                            }

                            sr.Submission = s;

                            db.SubmissionReports.Add(sr);

                            s.Report = sr;

                            s.UpdateScore();

                            db.Entry(s).State = EntityState.Modified;
                        }
                    }
                }
                db.SaveChanges();
            }

            //Get all this user's submissions
            var submissions = db.Submissions.Where(s => (s.Task.ID == id && s.Owner.Id.Equals(userID)));

            string testPath = Path.Combine(
                                    AppDomain.CurrentDomain.BaseDirectory,
                                    "../../Tasks", id.ToString(), "Tests");

            if (Directory.Exists(testPath))
            {
                var testList = Directory.GetFiles(testPath);
                ViewBag.testList = testList.ToList();
            }
            else
            {
                ViewBag.testList = new List<string>();
            }

            ViewBag.Submissions = submissions.ToList();

            ViewBag.Task_ID = id;

            return View(oCAPETask);
        }

        
        //POST: Tasks/UploadFiles
        public ActionResult UploadFiles(IEnumerable<HttpPostedFileBase> files, string OwnerNotes, int Task_ID)
        {
            OCAPETask oCAPETask = db.Tasks.Find(Task_ID);

            //if(IsCorrectUserLoggedIn(oCAPETask.Owner.Id))

            var r = new List<ViewDataUploadFilesResult>();
            string path = "";
            foreach (HttpPostedFileBase file in files)
            {
                if (file != null && file.ContentLength > 0)
                    try
                    {
                        var user = manager.FindById(User.Identity.GetUserId());

                        path = Path.Combine(
                                        AppDomain.CurrentDomain.BaseDirectory,
                                        "../../Uploads", Task_ID.ToString(), user.Email);

                        //REMEMBER TO SANITIZE FILENAME

                        Directory.CreateDirectory(path);


                        //Create a submission
                        OCAPESubmission ocs = new OCAPESubmission();
                        //ocs.Filename = Filename;
                        ocs.Task = oCAPETask;
                        ocs.CreatedAt = DateTime.Now;
                        var currentUser = manager.FindById(User.Identity.GetUserId());

                        ocs.Owner = currentUser;
                        ocs.Extension = Path.GetExtension(file.FileName);

                        ocs.OwnerNote = OwnerNotes;
                        
                        db.Submissions.Add(ocs);
                        db.SaveChanges();



                        path = Path.Combine(path,
                            //                Path.GetFileName(file.FileName));
                                            "submission" + ocs.ID.ToString() + ocs.Extension);

                        //Save file
                        file.SaveAs(path);
                        ViewBag.Message = "File uploaded successfully";

                        ocs.Filename = path;

                        db.Entry(ocs).State = EntityState.Modified;
                        db.SaveChanges();

                        //Update submission with file path

                        r.Add(new ViewDataUploadFilesResult()
                        {
                            Name = path,
                            Length = file.ContentLength
                        });
                    }
                    catch (Exception ex)
                    {
                        ViewBag.Message = "ERROR:" + ex.Message.ToString();
                    }
                else
                {
                    ViewBag.Message = "You have not specified a file.";
                }
            }

            //return View("Details", "Tasks", Task_ID);
            
            //return RedirectToAction("Create", "OCAPESubmissions", new { Filename = path, Task_ID = Task_ID });

            return RedirectToAction("Details", new { id = Task_ID });
        }

        // GET: Tasks/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Tasks/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create([Bind(Include = "ID,Title,Description,ScoringType,CompilerToUse,MaxMemoryUsage,EvaluationsPerTest,MaxTimeToRun,StartDate,DeadlineDate,IsWeightingPublic,IsLeaderboardPublic,IsMaxBoundaryPublic,MemoryUsageWeight,TimeToRunWeight")] OCAPETask oCAPETask)
        {
            var currentUser = await manager.FindByIdAsync(User.Identity.GetUserId());
            if (ModelState.IsValid)
            {
                oCAPETask.Owner = currentUser;
                oCAPETask.CreateDate = DateTime.Now;
                db.Tasks.Add(oCAPETask);
                //db.SaveChanges();
                await db.SaveChangesAsync();

                oCAPETask.GenerateTaskDetailsFolder();

                return RedirectToAction("Index");
            }

            return View(oCAPETask);
        }

        // GET: Tasks/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            OCAPETask oCAPETask = db.Tasks.Find(id);
            if (oCAPETask == null)
            {
                return HttpNotFound();
            }

            if(!IsCorrectUserLoggedIn(oCAPETask.Owner.Id))
            {
                return HttpNotFound();
            }

            return View(oCAPETask);
        }

        // POST: Tasks/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ID,Title,Description,ScoringType,CreateDate,CompilerToUse,MaxMemoryUsage,EvaluationsPerTest,MaxTimeToRun,StartDate,DeadlineDate,IsWeightingPublic,IsLeaderboardPublic,IsMaxBoundaryPublic,MemoryUsageWeight,TimeToRunWeight")] OCAPETask oCAPETask)
        {
            if (ModelState.IsValid)
            {
                //OCAPETask oct = db.Tasks.Find(oCAPETask.ID);
                //oCAPETask.CreateDate = oct.CreateDate;
                db.Entry(oCAPETask).State = EntityState.Modified;

                foreach(OCAPESubmission s in db.Submissions.Where(r => r.Task.ID == oCAPETask.ID))
                {
                    s.UpdateScore();

                    db.Entry(s).State = EntityState.Modified;
                }

                db.SaveChanges();

                oCAPETask.GenerateTaskDetailsFolder();
                return RedirectToAction("Index");
            }
            return View(oCAPETask);
        }

        // GET: Tasks/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            OCAPETask oCAPETask = db.Tasks.Find(id);
            if (oCAPETask == null)
            {
                return HttpNotFound();
            }
            if (!IsCorrectUserLoggedIn(oCAPETask.Owner.Id))
            {
                return HttpNotFound();
            }
            return View(oCAPETask);
        }

        // POST: Tasks/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            OCAPETask oCAPETask = db.Tasks.Find(id);
            if (!IsCorrectUserLoggedIn(oCAPETask.Owner.Id))
            {
                return HttpNotFound();
            }
            db.Tasks.Remove(oCAPETask);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        // POST: Tasks/PurgeSubmissions
        [HttpPost, ActionName("PurgeSubmissions")]
        [ValidateAntiForgeryToken]
        public ActionResult PurgeSubmissions(int taskId)
        {
            OCAPETask oCAPETask = db.Tasks.Find(taskId);
            if (!IsCorrectUserLoggedIn(oCAPETask.Owner.Id))
            {
                return HttpNotFound();
            }

            var subs = db.Submissions.Where(s => s.Task.ID == taskId);

            foreach (OCAPESubmission sub in subs)
            {
                if ((sub.Report == null && (DateTime.Now - sub.CreatedAt).Days > 5) || (sub.Report != null && sub.Report.CompiledSuccessfully.Equals("FAIL")))
                {
                    if (sub.Report != null)
                    {
                        db.SubmissionReportRuns.RemoveRange(sub.Report.OCAPESubmissionReportRuns);
                        //foreach (OCAPESubmissionReportRun srr in sub.Report.OCAPESubmissionReportRuns)
                        {
                        //    db.SubmissionReportRuns.Remove(srr);
                        }
                        db.SubmissionReports.Remove(sub.Report);
                    }
                    db.Submissions.Remove(sub);
                }
            }

            db.SaveChanges();

            return RedirectToAction("Details", new { id = taskId });
        }

        public ActionResult Graphs(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            OCAPETask oCAPETask = db.Tasks.Find(id);
            if (oCAPETask == null)
            {
                return HttpNotFound();
            }

            var subs = db.Submissions.Where(p => p.Task.ID == id && p.Report!=null);
            var subreps = new List<OCAPESubmissionReport>();
            foreach(OCAPESubmission s in subs)
            {
                if (!s.Report.CompiledSuccessfully.Equals("FAIL") && s.Report.Score > 0)
                {
                    subreps.Add(s.Report);
                }
            }
            ViewBag.Reports = subreps;

            return View(oCAPETask);
        }

        [HttpPost]
        public ActionResult UploadTestPair(int? id, HttpPostedFileBase test, HttpPostedFileBase output)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            OCAPETask oCAPETask = db.Tasks.Find(id);
            if (oCAPETask == null)
            {
                return HttpNotFound();
            }
            if (!IsCorrectUserLoggedIn(oCAPETask.Owner.Id))
            {
                return HttpNotFound();
            }
            string path = Path.Combine(
                                    AppDomain.CurrentDomain.BaseDirectory,
                                    "../../Tasks", id.ToString());

            Directory.CreateDirectory(Path.Combine(path,"Tests"));
            Directory.CreateDirectory(Path.Combine(path, "Correct"));

            //TODO: change this to in[NR].txt

            //Find next [NR]
            int NR = 0;
            bool foundNext = true;

            while(foundNext)
            {
                NR++;

                if(System.IO.File.Exists(Path.Combine(path, "Tests", "in" + NR.ToString() + ".txt")))
                {

                }
                else
                {
                    foundNext = false;
                }
            }

            string path1 = Path.Combine(path, "Tests",
                             "in" + NR.ToString() + ".txt");

            test.SaveAs(path1);

            //TODO: change this to out[NR].txt
            string path2 = Path.Combine(path, "Correct",
                             "out" + NR.ToString() + ".txt");

            output.SaveAs(path2);

            ViewBag.Message = "File uploaded successfully";

            return RedirectToAction("Edit", oCAPETask);
        }

        protected bool IsCorrectUserLoggedIn(string Id)
        {
            if (User.Identity.GetUserId() != Id)
            {
                return false;
            }
            return true;
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }


    }
}
