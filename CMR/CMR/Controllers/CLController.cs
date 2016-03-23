using CMR.Models;
using CMR.Security;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Net.Mail;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace CMR.Controllers
{
    [CustomAuthorize(Roles="CL")]
    public class CLController : Controller
    {
        public ActionResult Index()
        {
            CRMContext db = new CRMContext();
            var currUser =User.Identity;
            String userName = currUser.Name;
            var course = (from ac in db.AnnualCourses
                         join a in db.Accounts on ac.clAccount equals a.accountId
                         where a.userName == userName
                         select ac).ToList();
            return View(course);
        }

        public ActionResult CreateReport(String annualCourseId)
        {
            ViewBag.annualCourseId = annualCourseId;
            return View();
        }


        [HttpPost]
        public async Task<ActionResult> CreateReport(CourseMonitoringReport acr, String waiting)
        {
            
            if (ModelState.IsValid)
            {
                CRMContext db = new CRMContext();

                if (waiting != null)
                {
                    acr.approveStatusId = 2;
                    var userName = User.Identity.Name;
                    var user = db.Accounts.SingleOrDefault(u => u.userName == userName);
                    var AnnualCourse = db.AnnualCourses.SingleOrDefault(a => a.annualCourseId == acr.annualCourseId);
                    int CMId = (int)AnnualCourse.Course.Faculty.cmAccount;
                    var Cm = db.Accounts.SingleOrDefault(u => u.accountId == CMId);

                    var body = "<p>Email From: {0} ({1})</p><p>Message:</p><p>{2}</p>";
                    var message = new MailMessage();
                    message.To.Add(new MailAddress(Cm.Profile.email));
                    message.Subject = "Course Monitoring Report";
                    message.Body = string.Format(body, user.Profile.name, user.Profile.email, "There is a report That you need to approve");
                    message.IsBodyHtml = true;
                    using (var smtp = new SmtpClient())
                    {
                        await smtp.SendMailAsync(message);

                    }
                }

                db.CourseMonitoringReports.Add(acr);
                db.SaveChanges();
                return RedirectToAction("ReportList");
            }
            return View();
        }

        public ActionResult ReportList()
        {
            CRMContext db = new CRMContext();
            return View(db.CourseMonitoringReports.ToList());
        }

        public ActionResult ReportDetail(int id)
        {
            CRMContext db = new CRMContext();
            CourseMonitoringReport acr = db.CourseMonitoringReports.SingleOrDefault(a => a.CourseMonitoringReportId == id);
            if (acr == null)
            {
                return HttpNotFound();
            }

            return View(acr);
        }

        public ActionResult EditReport(int id)
        {
            CRMContext db = new CRMContext();
            CourseMonitoringReport acr = db.CourseMonitoringReports.SingleOrDefault(a => a.CourseMonitoringReportId == id);
            if (acr == null)
            {
                return HttpNotFound();
            }

            return View(acr);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> EditReport(CourseMonitoringReport acr, String waiting)
        {
            CRMContext db = new CRMContext();
            if (ModelState.IsValid)
            {
                if (waiting != null)
                {
                    acr.approveStatusId=2;

                    var userName = User.Identity.Name;
                    var user = db.Accounts.SingleOrDefault(u => u.userName == userName);
                    var AnnualCourse = db.AnnualCourses.SingleOrDefault(a => a.annualCourseId == acr.annualCourseId);
                    int CMId =(int) AnnualCourse.Course.Faculty.cmAccount;
                    var Cm = db.Accounts.SingleOrDefault(u => u.accountId == CMId);

                    var body = "<p>Email From: {0} ({1})</p><p>Message:</p><p>{2}</p>";
                    var message = new MailMessage();
                    message.To.Add(new MailAddress(Cm.Profile.email));
                    message.Subject = "Course Monitoring Report";
                    message.Body = string.Format(body,user.Profile.name, user.Profile.email, "There is a report That you need to approve");
                    message.IsBodyHtml = true;
                    using (var smtp = new SmtpClient())
                    {
                        await smtp.SendMailAsync(message);

                    }
                }
                db.Entry(acr).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("ReportList");
            }
            return View(acr);
        }

        public ActionResult DeleteReport(int id)
        {
            CRMContext db = new CRMContext();
            CourseMonitoringReport acr = db.CourseMonitoringReports.SingleOrDefault(a => a.CourseMonitoringReportId == id);
            if (acr == null)
            {
                return HttpNotFound();
            }
            db.CourseMonitoringReports.Remove(acr);
            db.SaveChanges();

            return RedirectToAction("ReportList");
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
				return RedirectToAction("ReportDetail", new { id = courseMonitoringReportId });
			}
			return View();
		}
    }
}