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
            return View();
        }
    }
}