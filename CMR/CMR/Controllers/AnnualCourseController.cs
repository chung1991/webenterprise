using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using CMR.Models;
using System.Web.Security;
using CMR.Security;
using System.Net;
using System.Data.Entity;
using PagedList;

namespace CMR.Controllers
{
    public class AnnualCourseController : Controller
    {
        CRMContext db = new CRMContext();

        [CustomAuthorize(Roles = "Admin")]
        public ActionResult AddAnnualCourse()
        {
           // ViewBag.roles = new SelectList(db.Courses.Where(x => x.Status != "Cancel"), "courseId", "courseName");

            ViewBag.roles = new SelectList((from s in db.Courses where s.Status != "Cancel"
                                            select new
                                            {
                                                courseId = s.courseId,
                                                courseName = s.courseName + " - " + s.Faculty.facultyName
                                            }), "courseId", "courseName", null);

            return View();
        }



        [HttpPost]
        [CustomAuthorize(Roles = "Admin")]
        public ActionResult AddAnnualCourse(AnnualCourse ac)
        {
            if (ModelState.IsValid)
            {
                if (ac.academicYear >= DateTime.Now.Date)
                {
                    db.sp_InsertAnnualCourse(ac.academicYear, ac.courseId);
                    db.SaveChanges();
                    return RedirectToAction("ViewAllAnnualCourse");
                }
                else
                    ModelState.AddModelError("academicYear", "Academic year must be date in present or future !");
            }

            ViewBag.roles = new SelectList((from s in db.Courses
                select new
                {
                    courseId = s.courseId,
                    courseName = s.courseName + "-" + s.Faculty.facultyName 
                }), "courseId","courseName",ac.courseId);

            //ViewBag.roles = new SelectList(db.Courses, "courseId", "courseName", ac.courseId);
            return View(ac);
        }


        public ActionResult ViewAllAnnualCourse(string sortOrder, String currentFilter, int? courseId, String Status, int? page)
        {
            ViewBag.SortAcademicYear = sortOrder == "Year" ? "year_desc" : "Year";
            ViewBag.SortUserName = sortOrder == "UserName" ? "username_desc" : "UserName";
            ViewBag.SortCourseName = sortOrder == "CourseName" ? "coursename_desc" : "CourseName";
            ViewBag.SortStatus = sortOrder == "Status" ? "status_desc" : "Status";
            ViewBag.currentSort = sortOrder;
            ViewBag.state = new SelectList(getListStatus(), "Value", "Text");
            int pageSize = 10;
            int pageNumber = (page ?? 1);
            if (courseId == null)
            {
                var anCourses = from anc in db.AnnualCourses select anc;
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
                    case "status_desc":
                        anCourses = anCourses.OrderByDescending(c => c.Status);
                        break;
                    case "Status":
                        anCourses = anCourses.OrderBy(c => c.Status);
                        break;
                    default:
                        anCourses = anCourses.OrderBy(c => c.courseId);
                        break;
                }
                return View(anCourses.ToPagedList(pageNumber, pageSize));
            }
            else
            {
                var anCourses = from anc in db.AnnualCourses where anc.courseId == courseId && anc.Status.Equals(Status) select anc;
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
                    case "status_desc":
                        anCourses = anCourses.OrderByDescending(c => c.Status);
                        break;
                    case "Status":
                        anCourses = anCourses.OrderBy(c => c.Status);
                        break;
                    default:
                        anCourses = anCourses.OrderBy(c => c.courseId);
                        break;
                }
                return View(anCourses.ToPagedList(pageNumber, pageSize));
            }
        }


        [HttpPost]
        public ActionResult ViewAllAnnualCourse(String txtAcademicYearFrom, String txtAcademicYearTo, String txtKeyWord, String txtStatus, int? page)
        {
            List<AnnualCourse> annucalCourses = new List<AnnualCourse>();
            ViewBag.From = txtAcademicYearFrom;
            ViewBag.To = txtAcademicYearTo;
            ViewBag.state = new SelectList(getListStatus(), "Value", "Text", txtStatus);
            int pageSize = 10;
            int pageNumber = (page ?? 1);
            if (txtAcademicYearFrom == "" && txtAcademicYearTo == "" && txtKeyWord == "" && txtStatus == "")
            {
                ModelState.AddModelError("", "Please, typing any field for searching !");
                var annualCourses = from anc in db.AnnualCourses where anc.Course.courseName == "" select anc;
                return View(annualCourses.ToPagedList(pageNumber, pageSize));
            }
            else
            {
                List<AnnualCourse> listAnnualCourse = getAnnualCourseSearching(txtAcademicYearFrom, txtAcademicYearTo, txtKeyWord, txtStatus);
                return View(listAnnualCourse.ToPagedList(pageNumber, pageSize));
            }
        }



        public List<AnnualCourse> getAnnualCourseSearching(String txtAcademicYearFrom, String txtAcademicYearTo, String txtKeyWord, String txtStatus)
        {
            if (txtAcademicYearFrom != "" && txtAcademicYearTo == "" && txtKeyWord == "" && txtStatus == "")
            {
                DateTime dt = DateTime.Parse(txtAcademicYearFrom);
                var annualCourses = (from anc in db.AnnualCourses where anc.academicYear >= dt select anc).ToList();
                return annualCourses;
            }
            if (txtAcademicYearFrom == "" && txtAcademicYearTo != "" && txtKeyWord == "" && txtStatus == "")
            {
                DateTime dt = DateTime.Parse(txtAcademicYearTo);
                var annualCourses = (from anc in db.AnnualCourses where anc.academicYear <= dt select anc).ToList();
                return annualCourses;
            }
            if (txtAcademicYearFrom == "" && txtAcademicYearTo == "" && txtKeyWord != "" && txtStatus == "")
            {
                var annualCourses = (from anc in db.AnnualCourses where anc.Account.userName.Contains(txtKeyWord) || anc.Course.courseName.Contains(txtKeyWord) select anc).ToList();
                return annualCourses;
            }
            if (txtAcademicYearFrom == "" && txtAcademicYearTo == "" && txtKeyWord == "" && txtStatus != "")
            {
                var annualCourses = (from anc in db.AnnualCourses where anc.Status == txtStatus select anc).ToList();
                return annualCourses;
            }
            if (txtAcademicYearFrom != "" && txtAcademicYearTo != "" && txtKeyWord == "" && txtStatus == "")
            {
                DateTime dtFrom = DateTime.Parse(txtAcademicYearFrom);
                DateTime dtTo = DateTime.Parse(txtAcademicYearTo);
                var annualCourses = (from anc in db.AnnualCourses where anc.academicYear >= dtFrom && anc.academicYear <= dtTo select anc).ToList();
                return annualCourses;
            }
            if (txtAcademicYearFrom != "" && txtAcademicYearTo == "" && txtKeyWord != "" && txtStatus == "")
            {
                DateTime dtFrom = DateTime.Parse(txtAcademicYearFrom);
                var annualCourses = (from anc in db.AnnualCourses where anc.academicYear >= dtFrom && (anc.Account.userName.Contains(txtKeyWord) || anc.Course.courseName.Contains(txtKeyWord)) select anc).ToList();
                return annualCourses;
            }
            if (txtAcademicYearFrom != "" && txtAcademicYearTo == "" && txtKeyWord == "" && txtStatus != "")
            {
                DateTime dtFrom = DateTime.Parse(txtAcademicYearFrom);
                var annualCourses = (from anc in db.AnnualCourses where anc.academicYear >= dtFrom && anc.Status == txtStatus select anc).ToList();
                return annualCourses;
            }
            if (txtAcademicYearFrom != "" && txtAcademicYearTo != "" && txtKeyWord != "" && txtStatus == "")
            {
                DateTime dtFrom = DateTime.Parse(txtAcademicYearFrom);
                DateTime dtTo = DateTime.Parse(txtAcademicYearTo);
                var annualCourses = (from anc in db.AnnualCourses where (anc.academicYear >= dtFrom && anc.academicYear <= dtTo) && (anc.Account.userName.Contains(txtKeyWord) || anc.Course.courseName.Contains(txtKeyWord)) select anc).ToList();
                return annualCourses;
            }
            if (txtAcademicYearFrom != "" && txtAcademicYearTo != "" && txtKeyWord == "" && txtStatus != "")
            {
                DateTime dtFrom = DateTime.Parse(txtAcademicYearFrom);
                DateTime dtTo = DateTime.Parse(txtAcademicYearTo);
                var annualCourses = (from anc in db.AnnualCourses where (anc.academicYear >= dtFrom && anc.academicYear <= dtTo) && (anc.Status == txtStatus) select anc).ToList();
                return annualCourses;
            }
            if (txtAcademicYearFrom == "" && txtAcademicYearTo != "" && txtKeyWord != "" && txtStatus == "")
            {
                DateTime dtTo = DateTime.Parse(txtAcademicYearTo);
                var annualCourses = (from anc in db.AnnualCourses where anc.academicYear <= dtTo && (anc.Account.userName.Contains(txtKeyWord) || anc.Course.courseName.Contains(txtKeyWord)) select anc).ToList();
                return annualCourses;
            }
            if (txtAcademicYearFrom == "" && txtAcademicYearTo != "" && txtKeyWord == "" && txtStatus != "")
            {
                DateTime dtTo = DateTime.Parse(txtAcademicYearTo);
                var annualCourses = (from anc in db.AnnualCourses where anc.academicYear <= dtTo && (anc.Status == txtStatus) select anc).ToList();
                return annualCourses;
            }
            if (txtAcademicYearFrom == "" && txtAcademicYearTo != "" && txtKeyWord != "" && txtStatus != "")
            {
                DateTime dtTo = DateTime.Parse(txtAcademicYearTo);
                var annualCourses = (from anc in db.AnnualCourses where anc.academicYear <= dtTo && (anc.Account.userName.Contains(txtKeyWord) || anc.Course.courseName.Contains(txtKeyWord)) && (anc.Status == txtStatus) select anc).ToList();
                return annualCourses;
            }
            if (txtAcademicYearFrom == "" && txtAcademicYearTo == "" && txtKeyWord != "" && txtStatus != "")
            {
                var annualCourses = (from anc in db.AnnualCourses where (anc.Account.userName.Contains(txtKeyWord) || anc.Course.courseName.Contains(txtKeyWord)) && (anc.Status == txtStatus) select anc).ToList();
                return annualCourses;
            }


            if (txtAcademicYearFrom != "" && txtAcademicYearTo != "" && txtKeyWord != "" && txtStatus != "")
            {
                DateTime dtFrom = DateTime.Parse(txtAcademicYearFrom);
                DateTime dtTo = DateTime.Parse(txtAcademicYearTo);
                var annualCourses = (from anc in db.AnnualCourses where (anc.academicYear >= dtFrom && anc.academicYear <= dtTo) && (anc.Account.userName.Contains(txtKeyWord) || anc.Course.courseName.Contains(txtKeyWord)) && (anc.Status == txtStatus) select anc).ToList();
                return annualCourses;
            }

            return null;
        }

        [CustomAuthorize(Roles = "Admin")]
        public ActionResult EditAnnualCourse(int annualCourseId)
        {
            AnnualCourse anCourse;
            try
            {
                anCourse = db.AnnualCourses.SingleOrDefault(ac => ac.annualCourseId == annualCourseId);
                ViewBag.state = new SelectList(getListStatus(), "Value", "Text", anCourse.Status);
                ViewBag.roles = new SelectList(db.Courses, "courseId", "courseName", anCourse.courseId);
                return View(anCourse);
            }
            catch (NullReferenceException)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
        }

        [CustomAuthorize(Roles = "Admin")]
        [HttpPost]
        public ActionResult EditAnnualCourse(AnnualCourse edit)
        {
            if (ModelState.IsValid)
            {
                AnnualCourse course = db.AnnualCourses.SingleOrDefault(ac => ac.annualCourseId == edit.annualCourseId);
                db.Entry(course).CurrentValues.SetValues(edit);
                db.SaveChanges();

                return RedirectToAction("ViewAllAnnualCourse");
            }
            ViewBag.state = new SelectList(getListStatus(), "Value", "Text", edit.Status);
            ViewBag.roles = new SelectList(db.Courses, "courseId", "courseName", edit.courseId);
            return View(edit);
        }

        [CustomAuthorize(Roles = "Admin")]
        public ActionResult DeleteAnnualCourse(int annualCourseId)
        {
            AnnualCourse anCourse = db.AnnualCourses.SingleOrDefault(ac => ac.annualCourseId == annualCourseId);
            db.AnnualCourses.Remove(anCourse);
            db.SaveChanges();
            return RedirectToAction("ViewAllAnnualCourse");
        }

        public List<SelectListItem> getListStatus()
        {
            List<SelectListItem> items = new List<SelectListItem>();
            items.Add(new SelectListItem { Text = "Activate", Value = "Activate" });
            items.Add(new SelectListItem { Text = "Wait", Value = "Wait" });
            items.Add(new SelectListItem { Text = "Finish", Value = "Finish" });
            return items;
        }

    }
}