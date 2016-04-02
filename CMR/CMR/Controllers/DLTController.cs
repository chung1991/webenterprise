using CMR.Models;
using CMR.Security;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
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
    }
}