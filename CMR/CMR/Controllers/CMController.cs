using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using CMR.Models;
using PagedList;
using CMR.Security;
using System.Data.Entity;
using System.Net.Mail;
using System.Threading.Tasks;
using System.Web.Helpers;

namespace CMR.Controllers
{
    [CustomAuthorize(Roles = "CM")]
    public class CMController : Controller
    {
        public ActionResult Index(string sortOrder,String currentFilter, String searchString, int? page)
        {
            CRMContext db = new CRMContext();
            var currUser = User.Identity;
            String userName = currUser.Name;
            ViewBag.NameSortParm = sortOrder == "Name" ? "name_desc" : "Name";
            ViewBag.StatusSortParm = sortOrder == "Status" ? "status_desc" : "Status";
            ViewBag.currentSort = sortOrder;

            var courses = (from cmr in db.CourseMonitoringReports
                          join ac in db.AnnualCourses on cmr.annualCourseId equals ac.annualCourseId
                          join c in db.Courses on ac.courseId equals c.courseId
                          join fac in db.Faculties on c.facultyId equals fac.facultyId
                          join a in db.Accounts on fac.cmAccount equals a.accountId
                          where a.userName == userName && cmr.approveStatusId != 1
                          select cmr
                        );

            if (searchString != null)
            {
                page = 1;
            }
            else
            {
                searchString = currentFilter;
            }

            ViewBag.currentFilter = searchString;

            if (!String.IsNullOrEmpty(searchString))
            {
                courses = courses.Where(s => s.AnnualCourse.Course.courseName.Contains(searchString));
            }

            switch (sortOrder)
            {
                case "name_desc":
                    courses = courses.OrderByDescending(c => c.AnnualCourse.Course.courseName);
                    break;
                case "Name":
                    courses = courses.OrderBy(c => c.AnnualCourse.Course.courseName);
                    break;
                case "status_desc":
                    courses = courses.OrderByDescending(c => c.approveStatusId);
                    break;
                case "Status":
                    courses = courses.OrderBy(c => c.approveStatusId);
                    break;
                default:
                    courses = courses.OrderBy(c => c.CourseMonitoringReportId);
                    break;
            }

            int pageSize = 10;
            int pageNumber = (page ?? 1);
            return View(courses.ToPagedList(pageNumber, pageSize));
        }

        public ActionResult Detail(int reportId) {
            CRMContext db = new CRMContext();
            var report = db.CourseMonitoringReports.SingleOrDefault(c => c.CourseMonitoringReportId == reportId);
            if (report == null)
            {
                return HttpNotFound();
            }
            List<SelectListItem> items = new List<SelectListItem>();

            items.Add(new SelectListItem { Text = "-- Select One --", Value = "2" });
            items.Add(new SelectListItem { Text = "Approve", Value = "4", Selected = (4 == report.approveStatusId ? true : false) });
            items.Add(new SelectListItem { Text = "Reject", Value = "3", Selected=(3==report.approveStatusId  ? true : false) });

            ViewBag.listEvent = items;

            return View(report);
        }

        [HttpPost]
        public ActionResult Approve(int courseMonitoringReportId, int status, String approve_desc)
        {
            CRMContext db = new CRMContext();
            
            var report = db.CourseMonitoringReports.SingleOrDefault(c => c.CourseMonitoringReportId == courseMonitoringReportId);
            if (report != null)
            {
                var statusName = "";
                report.approveStatusId = status;
                report.approve_desc = approve_desc;
                db.Entry(report).State = EntityState.Modified;
                db.SaveChanges();
                var userName = User.Identity.Name;
                var user = db.Accounts.SingleOrDefault(u => u.userName == userName);
                var ClEmail = report.AnnualCourse.Account.Profile.email;
                var pvcEmail = report.AnnualCourse.Course.Faculty.Account2.Profile.email;
                var dltEmail = report.AnnualCourse.Course.Faculty.Account1.Profile.email;

                if (status == 3)
                {
                     statusName = "rejected";
                }
                else if (status == 4)
                {
                    statusName = "approved";
                }

                var uri = HttpContext.Request.Url;
                String url = uri.GetLeftPart(UriPartial.Authority);
                String clURL = url + "/CL/ReportDetail?id=" + report.CourseMonitoringReportId;
                
                var body = "<p>Email From: {0} ({1})</p><p>Message: {2}</p><p>Link: {3}</p>";
                var message = string.Format(body, user.Profile.name, user.Profile.email, "Your report have been " + statusName + " by " + user.Profile.name, clURL);
                Task.Run(async () => await CustomHtmlHelpers.Helpers.sendMail(ClEmail, message));

                String dltURL = url + "/DLT/Detail?reportId=" + report.CourseMonitoringReportId;
                var dtlbody = "<p>Email From: {0} ({1})</p><p>Message: {2}</p><p>Link: {3}</p>";
                var dltmessage = string.Format(dtlbody, user.Profile.name, user.Profile.email, "There is a report of " + report.AnnualCourse.Account.Profile.name, dltURL);
                Task.Run(async () => await CustomHtmlHelpers.Helpers.sendMail(dltEmail, dltmessage));

                String pvcURL = url + "/PVC/Detail?reportId=" + report.CourseMonitoringReportId;
                var pvcbody = "<p>Email From: {0} ({1})</p><p>Message: {2}</p><p>Link: {3}</p>";
                var pvcmessage = string.Format(pvcbody, report.AnnualCourse.Account.Profile.name, report.AnnualCourse.Account.Profile.email, "There is a report of " + report.AnnualCourse.Account.Profile.name, pvcURL);
                Task.Run(async () => await CustomHtmlHelpers.Helpers.sendMail(pvcEmail, pvcmessage));
                
            }
            return RedirectToAction("Detail", new { reportId = courseMonitoringReportId });
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

    }
}