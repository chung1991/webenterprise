﻿using CMR.Models;
using CMR.Security;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Net.Mail;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using PagedList;

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

        public ActionResult ViewAvaibleAnnualCourse(string sortOrder, String currentFilter, int? page)
        {
            CRMContext db = new CRMContext();
            ViewBag.SortAcademicYear = sortOrder == "Year" ? "year_desc" : "Year";
            ViewBag.SortUserName = sortOrder == "UserName" ? "username_desc" : "UserName";
            ViewBag.SortCourseName = sortOrder == "CourseName" ? "coursename_desc" : "CourseName";
            ViewBag.currentSort = sortOrder;
          
            int pageSize = 10;
            int pageNumber = (page ?? 1);
            
                var anCourses = from anc in db.AnnualCourses where anc.Status.Equals("Wait") select anc;
                switch (sortOrder)
                {
                    case "year_desc":
                        anCourses = anCourses.OrderByDescending(ac => ac.academicYear);
                        break;
                    case "Year":
                        anCourses = anCourses.OrderBy(c => c.academicYear);
                        break;
                    case "username_desc":
                        anCourses = anCourses.OrderByDescending(c => c.Account.userName);
                        break;
                    case "UserName":
                        anCourses = anCourses.OrderBy(c => c.Account.userName);
                        break;
                    case "coursename_desc":
                        anCourses = anCourses.OrderByDescending(c => c.Course.courseName);
                        break;
                    case "CourseName":
                        anCourses = anCourses.OrderBy(c => c.Course.courseName);
                        break;
                    default:
                        anCourses = anCourses.OrderBy(c => c.courseId);
                        break;
                }
                return View(anCourses.ToPagedList(pageNumber, pageSize));
            
            
        }

        [HttpPost]
        public ActionResult ViewAvaibleAnnualCourse(String txtAcademicYearFrom, String txtAcademicYearTo, String txtKeyWord,  int? page)
        {
            CRMContext db = new CRMContext();
            List<AnnualCourse> annucalCourses = new List<AnnualCourse>();
            ViewBag.From = txtAcademicYearFrom;
            ViewBag.To = txtAcademicYearTo;
           
            int pageSize = 10;
            int pageNumber = (page ?? 1);
            if (txtAcademicYearFrom == "" && txtAcademicYearTo == "" && txtKeyWord == "")
            {
                ModelState.AddModelError("", "Please, typing any field for searching !");
                var annualCourses = from anc in db.AnnualCourses where anc.Course.courseName == ""  select anc;
                return View(annualCourses.ToPagedList(pageNumber, pageSize));
            }
            else
            {
                List<AnnualCourse> listAnnualCourse = getAnnualCourseSearching(txtAcademicYearFrom, txtAcademicYearTo, txtKeyWord);
                return View(listAnnualCourse.ToPagedList(pageNumber, pageSize));

            }
        }

        public List<AnnualCourse> getAnnualCourseSearching(String txtAcademicYearFrom, String txtAcademicYearTo, String txtKeyWord)
        {
            CRMContext db = new CRMContext();
            if (txtAcademicYearFrom != "" && txtAcademicYearTo == "" && txtKeyWord == "")
            {
                DateTime dt = DateTime.Parse(txtAcademicYearFrom);
                var annualCourses = (from anc in db.AnnualCourses where anc.academicYear >= dt && anc.Status == "Wait" select anc).ToList();
                return annualCourses;
            }
            if (txtAcademicYearFrom == "" && txtAcademicYearTo != "" && txtKeyWord == "")
            {
                DateTime dt = DateTime.Parse(txtAcademicYearTo);
                var annualCourses = (from anc in db.AnnualCourses where anc.academicYear <= dt && anc.Status == "Wait" select anc).ToList();
                return annualCourses;
            }
            if (txtAcademicYearFrom == "" && txtAcademicYearTo == "" && txtKeyWord != "" )
            {
                var annualCourses = (from anc in db.AnnualCourses where anc.Course.courseName.Contains(txtKeyWord) && anc.Status == "Wait" select anc).ToList();
                return annualCourses;
            }
           
            if (txtAcademicYearFrom != "" && txtAcademicYearTo != "" && txtKeyWord == "")
            {
                DateTime dtFrom = DateTime.Parse(txtAcademicYearFrom);
                DateTime dtTo = DateTime.Parse(txtAcademicYearTo);
                var annualCourses = (from anc in db.AnnualCourses where (anc.academicYear >= dtFrom && anc.academicYear <= dtTo) && anc.Status == "Wait" select anc).ToList();
                return annualCourses;
            }
            if (txtAcademicYearFrom != "" && txtAcademicYearTo == "" && txtKeyWord != "")
            {
                DateTime dtFrom = DateTime.Parse(txtAcademicYearFrom);
                var annualCourses = (from anc in db.AnnualCourses where anc.academicYear >= dtFrom && anc.Status == "Wait" && anc.Course.courseName.Contains(txtKeyWord) select anc).ToList();
                return annualCourses;
            }
            
           
            if (txtAcademicYearFrom == "" && txtAcademicYearTo != "" && txtKeyWord != "")
            {
                DateTime dtTo = DateTime.Parse(txtAcademicYearTo);
                var annualCourses = (from anc in db.AnnualCourses where anc.academicYear <= dtTo && anc.Status == "Wait" && anc.Course.courseName.Contains(txtKeyWord) select anc).ToList();
                return annualCourses;
            }

            if (txtAcademicYearFrom != "" && txtAcademicYearTo != "" && txtKeyWord != "")
            {
                DateTime dtFrom = DateTime.Parse(txtAcademicYearFrom);
                DateTime dtTo = DateTime.Parse(txtAcademicYearTo);
                var annualCourses = (from anc in db.AnnualCourses where (anc.academicYear >= dtFrom && anc.academicYear <= dtTo) && anc.Status == "Wait" && anc.Course.courseName.Contains(txtKeyWord) select anc).ToList();
                return annualCourses;
            }
            return null;
        }


        public ActionResult TakeAnnualCourse(int annualCourseId)
        {
            CRMContext db = new CRMContext();
            AnnualCourse anCourse = db.AnnualCourses.SingleOrDefault(ac => ac.annualCourseId == annualCourseId);
            Account a = (Account)System.Web.HttpContext.Current.Session["UserSession"];
            anCourse.clAccount = a.accountId;
            anCourse.Status = "Activate";
            db.SaveChanges();

            return RedirectToAction("Index");
        }

    }
}