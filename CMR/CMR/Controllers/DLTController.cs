using CMR.Models;
using CMR.Security;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Helpers;
using System.Web.Mvc;

namespace CMR.Controllers
{
    [CustomAuthorize(Roles = "DLT")]
    public class DLTController : Controller
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
                           join a in db.Accounts on fac.dltAccount equals a.accountId
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

        public ActionResult ResponseComment(int courseMonitoringReportId, String dltComment)
        {
            CRMContext db = new CRMContext();
            var report = db.CourseMonitoringReports.SingleOrDefault(c => c.CourseMonitoringReportId == courseMonitoringReportId);
            if (report != null)
            {
                report.dltComment = dltComment;
                db.Entry(report).State = EntityState.Modified;
                db.SaveChanges();
                var clEmail = report.AnnualCourse.Account.Profile.email;
                var userName = User.Identity.Name;
                var user = db.Accounts.SingleOrDefault(u => u.userName == userName);
                var body = "<p>Email From: {0} ({1})</p><p>Message: {2}</p><p>Content: {3}</p><p>Link: {4}</p>";
                var uri = HttpContext.Request.Url;
                String url = uri.GetLeftPart(UriPartial.Authority);

                url = url + "/CL/ReportDetail?id=" + report.CourseMonitoringReportId;
                var message = string.Format(body, user.Profile.name, user.Profile.email, "Your report has been repsoned.",dltComment,url);
                Task.Run(async () => await CustomHtmlHelpers.Helpers.sendMail(clEmail, message));

            }
            return RedirectToAction("Detail", new { reportId = courseMonitoringReportId });
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