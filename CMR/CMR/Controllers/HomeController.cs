using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace CMR.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            if (User.IsInRole("Admin"))
            {
                return RedirectToAction("Index", "Account");
            }
            if (User.IsInRole("CL"))
            {
                return RedirectToAction("Index", "CL");
            }
            if (User.IsInRole("CM"))
            {
                return RedirectToAction("Index", "CM");
            }
            if (User.IsInRole("DLT"))
            {
                return RedirectToAction("Index", "DLT");
            }
            if (User.IsInRole("PVC"))
            {
                return RedirectToAction("Index", "PVC");
            }
            return View();
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
    }
}