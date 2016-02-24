using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using DemoChart.Models;
using DemoChart.Utilities;
using System.Configuration;
using System.Web.Routing;
using System.Web.SessionState;
using System.Web.Helpers;

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

        public ActionResult CreateBar()
        {
            //Create bar chart
            var chart = new Chart(width:300,height:200)
            .AddSeries(     chartType: "bar",
                            xValue: new[] { "10 ", "50", "30 ", "70" },
                            yValues: new[] { "50", "70", "90", "110" })
                            .GetBytes("png");
            return File(chart, "image/bytes");
        }

        public ActionResult CreatePie()
        {
            //Create bar chart
            var chart = new Chart(width: 300, height: 200)
            .AddSeries(chartType: "pie",
                            xValue: new[] { "10 ", "50", "30 ", "70" },
                            yValues: new[] { "50", "70", "90", "110" })
                            .GetBytes("png");
            return File(chart, "image/bytes");
        }

        public ActionResult CreateLine()
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