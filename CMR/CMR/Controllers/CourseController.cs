using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using CMR.Models;
using CMR.Security;
using System.Data.Entity;
using PagedList;
using System.Net;

namespace CMR.Controllers
{
    public class CourseController : Controller
    {
        CRMContext db = new CRMContext();

        public ActionResult ViewAllCourse(string sortOrder, String currentFilter, int? page)
        {

            ViewBag.SortCourseName = sortOrder == "Name" ? "name_desc" : "Name";
            ViewBag.SortFacultyName = sortOrder == "Faculty" ? "faculty_desc" : "Faculty";
            ViewBag.SortStatus = sortOrder == "Status" ? "status_desc" : "Status";
            ViewBag.SortCount = sortOrder == "Count" ? "count_desc" : "Count";
            ViewBag.currentSort = sortOrder;

            var courses = (from c in db.Courses select c);
            //var test = (from c in db.Courses join an in db.AnnualCourses on c.courseId equals an.courseId  select c );


            switch (sortOrder)
            {
                case "name_desc":
                    courses = courses.OrderByDescending(c => c.courseName);
                    break;
                case "Name":
                    courses = courses.OrderBy(c => c.courseName);
                    break;
                case "faculty_desc":
                    courses = courses.OrderByDescending(c => c.Faculty.facultyName);
                    break;
                case "Faculty":
                    courses = courses.OrderBy(c => c.Faculty.facultyName);
                    break;
                case "status_desc":
                    courses = courses.OrderByDescending(c => c.Status);
                    break;
                case "Status":
                    courses = courses.OrderBy(c => c.Status);
                    break;
                case "count_desc":
                    courses = courses.OrderByDescending(c => c.AnnualCourses.Count);
                    break;
                case "Count":
                    courses = courses.OrderBy(c => c.AnnualCourses.Count);
                    break;
                default:
                    courses = courses.OrderBy(c => c.courseId);
                    //courses = courses.OrderBy(c => c.AnnualCourses.Count);
                    break;
            }

            ViewBag.Faculty = new SelectList(db.Faculties, "facultyName", "facultyName");
            ViewBag.state = new SelectList(getListStatus(), "Value", "Text");

            int pageSize = 10;
            int pageNumber = (page ?? 1);
            return View(courses.ToPagedList(pageNumber, pageSize));
        }

        [HttpPost]
        public ActionResult ViewAllCourse(String txtCourseName, String txtFacultyName, String txtStatus, int? page)
        {
            ViewBag.currentFilter = txtCourseName;
            ViewBag.Faculty = new SelectList(db.Faculties, "facultyName", "facultyName", txtFacultyName);
            ViewBag.state = new SelectList(getListStatus(), "Value", "Text", txtStatus);
            int pageSize = 10;
            int pageNumber = (page ?? 1);
            if (txtCourseName == "" && txtFacultyName == "" && txtStatus == "")
            {
                ModelState.AddModelError("", "Please, typing any field for searching !");
                var courses = from c in db.Courses where c.courseName == "" select c;
                return View(courses.ToPagedList(pageNumber, pageSize));
            }
            else
            {
                List<Course> listCourse = getCourseSearching(txtCourseName, txtFacultyName, txtStatus);
                return View(listCourse.ToPagedList(pageNumber, pageSize));
            }
        }

        public List<Course> getCourseSearching(String txtCourseName, String txtFacultyName, String txtStatus)
        {
            List<Course> listCourse = new List<Course>();
            if (txtCourseName != "" && txtFacultyName == "" && txtStatus == "")
            {
                listCourse = (from c in db.Courses where c.courseName.Contains(txtCourseName) select c).ToList();
            }
            if (txtCourseName == "" && txtFacultyName != "" && txtStatus == "")
            {
                listCourse = (from c in db.Courses join f in db.Faculties on c.facultyId equals f.facultyId where f.facultyName == txtFacultyName select c).ToList();
            }
            if (txtCourseName == "" && txtFacultyName == "" && txtStatus != "")
            {
                listCourse = (from c in db.Courses where c.Status == txtStatus select c).ToList();
            }
            if (txtCourseName != "" && txtFacultyName != "" && txtStatus == "")
            {
                listCourse = (from c in db.Courses join f in db.Faculties on c.facultyId equals f.facultyId where c.courseName.Contains(txtCourseName) && f.facultyName == txtFacultyName select c).ToList();
            }
            if (txtCourseName != "" && txtFacultyName == "" && txtStatus != "")
            {
                listCourse = (from c in db.Courses where c.courseName.Contains(txtCourseName) && c.Status == txtStatus select c).ToList();
            }
            if (txtCourseName == "" && txtFacultyName != "" && txtStatus != "")
            {
                listCourse = (from c in db.Courses join f in db.Faculties on c.facultyId equals f.facultyId where f.facultyName == txtFacultyName && c.Status == txtStatus select c).ToList();
            }
            if (txtCourseName != "" && txtFacultyName != "" && txtStatus != "")
            {
                listCourse = (from c in db.Courses join f in db.Faculties on c.facultyId equals f.facultyId where c.courseName.Contains(txtCourseName) && f.facultyName == txtFacultyName && c.Status == txtStatus select c).ToList();
            }
            return listCourse;

        }

        public String getFacultyNameFromFacultyId(int? fId)
        {
            Faculty faculty = db.Faculties.SingleOrDefault(f => f.facultyId == fId);
            return faculty.facultyName;
        }

        [CustomAuthorize(Roles = "Admin")]
        public ActionResult AddCourse()
        {
            ViewBag.roles = new SelectList(db.Faculties, "facultyId", "facultyName");
            return View();
        }

        [HttpPost]
        [CustomAuthorize(Roles = "Admin")]
        public ActionResult AddCourse(Course c)
        {
            if (ModelState.IsValid)
            {
                //c.courseName = c.courseName + " - " + getFacultyNameFromFacultyId(c.facultyId);
                if (IsCourseExisted(c, false))
                {
                    db.sp_InsertCourse(c.courseName, c.facultyId);
                    db.SaveChanges();
                    return RedirectToAction("ViewAllCourse");
                }
                else
                    ModelState.AddModelError("courseName", "This course name have been existed !");
            }

            ViewBag.roles = new SelectList(db.Faculties, "facultyId", "facultyName", c.facultyId);
            return View(c);
        }

        [CustomAuthorize(Roles = "Admin")]
        public ActionResult EditCourse(int courseId)
        {
            Course course;
            try
            {
                course = db.Courses.SingleOrDefault(c => c.courseId == courseId);
                ViewBag.roles = new SelectList(db.Faculties, "facultyId", "facultyName", course.facultyId);
                ViewBag.state = new SelectList(getListStatus(), "Value", "Text", course.Status);
                return View(course);
            }
            catch (NullReferenceException)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
        }

        [HttpPost]
        [CustomAuthorize(Roles = "Admin")]
        public ActionResult EditCourse(Course edit)
        {
            if (ModelState.IsValid)
            {
                if (IsCourseExisted(edit, true))
                {
                    //if (CourseCanBeUpdated(edit))
                    //{
                        Course course = db.Courses.SingleOrDefault(c => c.courseId == edit.courseId);
                        db.Entry(course).CurrentValues.SetValues(edit);
                        db.SaveChanges();
                        return RedirectToAction("ViewAllCourse");
                    //}
                    //else
                    //    ModelState.AddModelError("Status", "This course has annual course !");
                }
                else
                    ModelState.AddModelError("courseName", "This course name have been existed !");
            }
            ViewBag.state = new SelectList(getListStatus(), "Value", "Text", edit.Status);
            ViewBag.roles = new SelectList(db.Faculties, "facultyId", "facultyName", edit.facultyId);
            return View(edit);
        }

        public ActionResult DeleteCourse(int courseId)
        {
            Course course = db.Courses.SingleOrDefault(c => c.courseId == courseId);
            db.AnnualCourses.RemoveRange(course.AnnualCourses);
            db.Courses.Remove(course);
            db.SaveChanges();
            return RedirectToAction("ViewAllCourse");
        }

        //public bool CourseCanBeUpdated(Course edit)
        //{
        //    int anCourse = 0;
        //    anCourse = db.AnnualCourses.Count(ac => ac.Course.courseId == edit.courseId && ac.Status != "Finish");
        //    if (anCourse > 0)
        //    {
        //        return false;
        //    }
        //    else
        //    {
        //        return true;
        //    }
        //}

        public List<SelectListItem> getListStatus()
        {
            List<SelectListItem> items = new List<SelectListItem>();
            items.Add(new SelectListItem { Text = "Activate", Value = "Activate" });
            items.Add(new SelectListItem { Text = "Cancel", Value = "Cancel" });
            return items;
        }

        public bool IsCourseExisted(Course manipulateCourse, bool edit)
        {
            if (edit == false)//--> Add Course
            {
                var course = db.Courses.SingleOrDefault(c => c.courseName == manipulateCourse.courseName && c.facultyId == manipulateCourse.facultyId);
                if (course == null)//--> there is no course have same name with addCourse(can be Added to DB)
                    return true;
            }
            else
            {
                var course = db.Courses.SingleOrDefault(c => c.courseName == manipulateCourse.courseName && c.facultyId == manipulateCourse.facultyId && c.courseId != manipulateCourse.courseId);
                if (course == null)//--> there is no course have same name with addCourse(can be Added to DB)
                    return true;
            }
            return false;
        }
    }
}