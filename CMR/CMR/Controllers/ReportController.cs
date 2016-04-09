using CMR.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Helpers;
using System.Web.Mvc;

namespace CMR.Controllers
{
    public class ReportController : Controller
    {
        // GET: Report
        public ActionResult Index()
        {
            CRMContext db = new CRMContext();
            return View(db.CourseMonitoringReports.ToList());
        }

        public ActionResult CreateLine(int id)
        {
            //Create bar chart
            var chart = new Chart(width: 600, height: 200)
            .AddSeries(chartType: "line",
                            xValue: new[] { "10 ", "50", "30 ", "70" },
                            yValues: new[] { "50", "70", "90", "110" })
                            .GetBytes("png");
            return File(chart, "image/bytes");
        }
    }
}