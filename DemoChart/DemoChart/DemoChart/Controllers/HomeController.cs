using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Mvc;
using DemoChart.Models;
using DemoChart.Utilities;

namespace DemoChart.Controllers
{
    [HandleError]
    public class HomeController : Controller
    {
        [HttpGet]
        public ActionResult Index()
        {
            var chartData = BrowserShareRepository.GetBrowserShares();
            return View(chartData);
        }

        [HttpGet]
        public FileResult GetChart()
        {
            var chartData = BrowserShareRepository.GetBrowserShares();
            return File(chartData.ChartImageStream().GetBuffer()
                , @"image/png", "BrowserShareChart.png");
        }

        [HttpGet]
        public FileResult GetPdf()
        {
            var chartData = BrowserShareRepository.GetBrowserShares();
            var chartStream = chartData.ChartImageStream();

            return File(PdfUtility.GetSimplePdf(chartStream).GetBuffer()
                , @"application/pdf", "BrowserShareChart.pdf");
        }
    }
}