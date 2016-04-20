using CMR.Models;
using CMR.Security;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Helpers;
using System.Web.Mvc;

namespace CMR.Controllers
{
    [CustomAuthorize(Roles = "PVC")]
    public class PVCController : Controller
    {
        public ActionResult Index()
        {
            CRMContext db = new CRMContext();
            var currUser = User.Identity;
            String userName = currUser.Name;

            var courses = (from cmr in db.CourseMonitoringReports
                           join ac in db.AnnualCourses on cmr.annualCourseId equals ac.annualCourseId
                           join c in db.Courses on ac.courseId equals c.courseId
                           join fac in db.Faculties on c.facultyId equals fac.facultyId
                           join a in db.Accounts on fac.pvcAccount equals a.accountId
                           where a.userName == userName && cmr.approveStatusId == 4
                           select cmr
                        );

            return View(courses);
        }
        public ActionResult Detail(int reportId)
        {
            CRMContext db = new CRMContext();
            var report = db.CourseMonitoringReports.SingleOrDefault(c => c.CourseMonitoringReportId == reportId);
            if (report == null)
            {
                return HttpNotFound();
            }
            return View(report);
        }

        public ActionResult CreateScoreChart(CourseMonitoringReport acr)
        {
            String scoreA = acr.markA.ToString();
            String scoreB = acr.markB.ToString();
            String scoreC = acr.markC.ToString();
            String scoreD = acr.markD.ToString();
            //Create bar chart
            var chart = new Chart(width: 300, height: 200, theme: ChartTheme.Blue)
            .AddTitle("Score Statistic")
            .AddLegend()
            .AddSeries(chartType: "pie",
                            xValue: new[] { "Excellent", "Good", "Ok", "NG" },
                            yValues: new[] { scoreA, scoreB, scoreC, scoreD })
                            .GetBytes("png");
            return File(chart, "image/bytes");
        }


        public ActionResult CreateResultChart(CourseMonitoringReport acr)
        {
            String scoreA = acr.markA.ToString();
            String scoreB = acr.markB.ToString();
            String scoreC = acr.markC.ToString();
            String scoreD = acr.markD.ToString();

            String pass = (acr.markA + acr.markB + acr.markC).ToString();
            String fail = acr.markD.ToString();
            //Create bar chart
            var chart = new Chart(width: 300, height: 200, theme: ChartTheme.Blue)
            .AddTitle("Result Statistic")
            .AddLegend()
            .AddSeries(chartType: "pie",
                            xValue: new[] { "Passed", "Failed" },
                            yValues: new[] { pass, fail })
                            .GetBytes("png");
            return File(chart, "image/bytes");
        }

        [HttpPost]
        public ActionResult SubmitComment(int courseMonitoringReportId, String comment_content)
        {
            System.Diagnostics.Debug.WriteLine("SubmitComment " + comment_content);

            if (ModelState.IsValid)
            {
                CRMContext db = new CRMContext();
                Comment cmt = new Comment();
                cmt.content = comment_content;
                var acc = db.Accounts.SingleOrDefault(a => a.userName == User.Identity.Name);
                cmt.accountId = acc.accountId;
                cmt.time = DateTime.Now;
                cmt.monitoringReportId = courseMonitoringReportId;
                db.Comments.Add(cmt);
                db.SaveChanges();
                return RedirectToAction("Detail", new { reportId = courseMonitoringReportId });
            }
            return View();
        }
    }
}