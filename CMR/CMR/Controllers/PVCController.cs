using CMR.Models;
using CMR.Security;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
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
    }
}