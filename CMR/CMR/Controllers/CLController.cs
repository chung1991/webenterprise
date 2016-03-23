using CMR.Models;
using CMR.Security;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
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
        public ActionResult CreateReport(CourseMonitoringReport acr)
        {
            if (ModelState.IsValid)
            {
                CRMContext db = new CRMContext();
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
        public ActionResult EditReport(CourseMonitoringReport acr)
        {
            CRMContext db = new CRMContext();
            if (ModelState.IsValid)
            {
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