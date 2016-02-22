using CMR.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace CMR.Controllers
{
    public class CLController : Controller
    {
        public ActionResult Index()
        {
            CRMContext db = new CRMContext();
            var currUser =User.Identity;
            String userName = currUser.Name;
            var course = (from ac in db.AnnualCourses
                         join cl in db.CLAnnualCourses on ac.annualCourseId equals cl.annualCourseId
                         join a in db.Accounts on cl.CLId equals a.accountId
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
        public ActionResult CreateReport(AnnualCourseRecord acr)
        {
            if (ModelState.IsValid)
            {
                CRMContext db = new CRMContext();
                db.AnnualCourseRecords.Add(acr);
                db.SaveChanges();
                return RedirectToAction("ReportList");
            }
            return View();
        }

        public ActionResult ReportList()
        {
            CRMContext db = new CRMContext();
            return View(db.AnnualCourseRecords.ToList());
        }

        public ActionResult ReportDetail(int id)
        {
            CRMContext db = new CRMContext();
            AnnualCourseRecord acr= db.AnnualCourseRecords.SingleOrDefault(a=>a.annualCourseRecordId==id);
            if (acr == null)
            {
                return HttpNotFound();
            }

            return View(acr);
        }

        public Boolean haveReport(int annualCourseId)
        {
            CRMContext db = new CRMContext();
            AnnualCourseRecord acr = db.AnnualCourseRecords.SingleOrDefault(a => a.annualCourseId == annualCourseId);
            return acr==null ? false : true;
        }
    }
}