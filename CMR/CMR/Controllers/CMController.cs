using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using CMR.Models;
using PagedList;
using CMR.Security;
using System.Data.Entity;

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
                courses = courses.Where(s => s.AnnualCourse.Course.name.Contains(searchString));
            }

            switch (sortOrder)
            {
                case "name_desc":
                    courses = courses.OrderByDescending(c => c.AnnualCourse.Course.name);
                    break;
                case "Name":
                    courses = courses.OrderBy(c => c.AnnualCourse.Course.name);
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

            items.Add(new SelectListItem { Text = "--- Select one ---", Value = "2" });

            items.Add(new SelectListItem { Text = "Approve", Value = "4"});

            items.Add(new SelectListItem { Text = "Reject", Value = "3" });

            SelectList selectList = new SelectList(items, "Value", "Text",4);

            ViewBag.status = selectList;
            return View(report);
        }
        [HttpPost]
        public ActionResult Approve(int courseMonitoringReportId, int status, String approve_desc)
        {
            CRMContext db = new CRMContext();
            var report = db.CourseMonitoringReports.SingleOrDefault(c => c.CourseMonitoringReportId == courseMonitoringReportId);
            if (report != null)
            {
                report.approveStatusId = status;
                report.approve_desc = approve_desc;
                db.Entry(report).State = EntityState.Modified;
                db.SaveChanges();
            }
            return RedirectToAction("Detail", new { reportId = courseMonitoringReportId });
        }

    }
}