
using CMR.Models;
using CMR.Security;
using Rotativa;
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
        [CustomAuthorize(Roles = "Admin")]
        public ActionResult Index()
        {
            CRMContext db = new CRMContext();
            return View(db.CourseMonitoringReports.ToList());
            //return View();
        }

        public ActionResult CreateArea()
        {
            //Create bar chart
            var chart = new Chart(width: 600, height: 200, theme : ChartTheme.Vanilla)
            .AddTitle("Area")
            .AddLegend()
            .AddSeries(chartType: "Area",
                            xValue: new[] { "2012 ", "2013", "2014 ", "2015", "2016", },
                            yValues: new[] { "70", "50", "20", "90", "100" })
                            .GetBytes("png");
            return File(chart, "image/bytes");
        }

        public ActionResult CreateStackColumn100()
        {
            //Create bar chart
            var chart = new Chart(width: 300, height: 200)
            .AddTitle("Stack Column 100")
            .AddLegend()
            .AddSeries(chartType: "StackedColumn100",
                            xValue: new[] { "2012 ", "2013", "2014 ", "2015", "2016"},
                            yValues: new[] { "100", "100", "100", "100", "100" })
            .AddSeries(chartType: "StackedColumn100",
                            xValue: new[] { "2012 ", "2013", "2014 ", "2015", "2016"},
                            yValues: new[] { "20", "10", "10", "50", "90" })
                            .GetBytes("png");
            return File(chart, "image/bytes");
        }

        public ActionResult CreateBoxPlot()
        {
            //Create bar chart
            var chart = new Chart(width: 600, height: 200, theme : ChartTheme.Vanilla)
            .AddTitle("BoxPlot")
            .AddLegend()
            .AddSeries(chartType: "BoxPlot",
                            xValue: new[] { "2012 ", "2013", "2014 ", "2015", "2016", },
                            yValues: new[] { "70", "50", "20", "90", "100" })
                            .GetBytes("png");
            return File(chart, "image/bytes");
        }

        public ActionResult CreateBar()
        {
            //Create bar chart
            var chart = new Chart(width: 600, height: 200, theme: ChartTheme.Vanilla)
            .AddTitle("Bar")
            .AddLegend()
            .AddSeries(chartType: "Bar",
                            xValue: new[] { "2012 ", "2013", "2014 ", "2015", "2016", },
                            yValues: new[] { "70", "50", "20", "90", "100" })
                            .GetBytes("png");
            return File(chart, "image/bytes");
        }

        public ActionResult CreateBubble()
        {
            //Create bar chart
            var chart = new Chart(width: 600, height: 200, theme: ChartTheme.Vanilla)
            .AddTitle("Bubble")
            .AddLegend()
            .AddSeries(chartType: "Bubble",
                            xValue: new[] { "2012 ", "2013", "2014 ", "2015", "2016", },
                            yValues: new[] { "70", "50", "20", "90", "100" })
                            .GetBytes("png");
            return File(chart, "image/bytes");
        }

        public ActionResult CreateCandlestick()
        {
            //Create bar chart
            var chart = new Chart(width: 600, height: 200, theme: ChartTheme.Vanilla)
            .AddTitle("Candlestick")
            .AddLegend()
            .AddSeries(chartType: "Candlestick",
                            xValue: new[] { "2012 ", "2013", "2014 ", "2015", "2016", },
                            yValues: new[] { "70", "50", "20", "90", "100" })
                            .GetBytes("png");
            return File(chart, "image/bytes");
        }

        public ActionResult CreateColumn()
        {
            //Create bar chart
            var chart = new Chart(width: 600, height: 200, theme: ChartTheme.Vanilla)
            .AddTitle("Column")
            .AddLegend()
            .AddSeries(chartType: "Column",
                            xValue: new[] { "2012 ", "2013", "2014 ", "2015", "2016", },
                            yValues: new[] { "70", "50", "20", "90", "100" })
                            .GetBytes("png");
            return File(chart, "image/bytes");
        }

        public ActionResult CreateDoughnut()
        {
            //Create bar chart
            var chart = new Chart(width: 600, height: 200, theme: ChartTheme.Vanilla)
            .AddTitle("Doughnut")
            .AddLegend()
            .AddSeries(chartType: "Doughnut",
                            xValue: new[] { "2012 ", "2013", "2014 ", "2015", "2016", },
                            yValues: new[] { "70", "50", "20", "90", "100" })
                            .GetBytes("png");
            return File(chart, "image/bytes");
        }

        public ActionResult CreateErrorBar()
        {
            //Create bar chart
            var chart = new Chart(width: 600, height: 200, theme: ChartTheme.Vanilla)
            .AddTitle("ErrorBar")
            .AddLegend()
            .AddSeries(chartType: "ErrorBar",
                            xValue: new[] { "2012 ", "2013", "2014 ", "2015", "2016", },
                            yValues: new[] { "70", "50", "20", "90", "100" })
                            .GetBytes("png");
            return File(chart, "image/bytes");
        }

        public ActionResult CreateFastLine()
        {
            //Create bar chart
            var chart = new Chart(width: 600, height: 200, theme: ChartTheme.Vanilla)
            .AddTitle("FastLine")
            .AddLegend()
            .AddSeries(chartType: "FastLine",
                            xValue: new[] { "2012 ", "2013", "2014 ", "2015", "2016", },
                            yValues: new[] { "70", "50", "20", "90", "100" })
                            .GetBytes("png");
            return File(chart, "image/bytes");
        }

        public ActionResult CreateFastPoint()
        {
            //Create bar chart
            var chart = new Chart(width: 600, height: 200, theme: ChartTheme.Vanilla)
            .AddTitle("FastPoint")
            .AddLegend()
            .AddSeries(chartType: "FastPoint",
                            xValue: new[] { "2012 ", "2013", "2014 ", "2015", "2016", },
                            yValues: new[] { "70", "50", "20", "90", "100" })
                            .GetBytes("png");
            return File(chart, "image/bytes");
        }

        public ActionResult CreateFunnel()
        {
            //Create bar chart
            var chart = new Chart(width: 600, height: 200, theme: ChartTheme.Vanilla)
            .AddTitle("Funnel")
            .AddLegend()
            .AddSeries(chartType: "Funnel",
                            xValue: new[] { "2012 ", "2013", "2014 ", "2015", "2016", },
                            yValues: new[] { "70", "50", "20", "90", "100" })
                            .GetBytes("png");
            return File(chart, "image/bytes");
        }

        public ActionResult CreateKagi()
        {
            //Create bar chart
            var chart = new Chart(width: 600, height: 200, theme: ChartTheme.Vanilla)
            .AddTitle("Kagi")
            .AddLegend()
            .AddSeries(chartType: "Kagi",
                            xValue: new[] { "2012 ", "2013", "2014 ", "2015", "2016", },
                            yValues: new[] { "70", "50", "20", "90", "100" })
                            .GetBytes("png");
            return File(chart, "image/bytes");
        }

        public ActionResult CreateLine()
        {
            //Create bar chart
            var chart = new Chart(width: 600, height: 200, theme: ChartTheme.Vanilla)
            .AddTitle("Line")
            .AddLegend()
            .AddSeries(chartType: "Line",
                            xValue: new[] { "2012 ", "2013", "2014 ", "2015", "2016", },
                            yValues: new[] { "70", "50", "20", "90", "100" })
                            .GetBytes("png");
            return File(chart, "image/bytes");
        }

        public ActionResult CreatePie()
        {
            //Create bar chart
            var chart = new Chart(width: 600, height: 200, theme: ChartTheme.Vanilla)
            .AddTitle("Pie")
            .AddLegend()
            .AddSeries(chartType: "Pie",
                            xValue: new[] { "2012 ", "2013", "2014 ", "2015", "2016", },
                            yValues: new[] { "70", "50", "20", "90", "100" })
                            .GetBytes("png");
            return File(chart, "image/bytes");
        }

        public ActionResult CreatePoint()
        {
            //Create bar chart
            var chart = new Chart(width: 600, height: 200, theme: ChartTheme.Vanilla)
            .AddTitle("Point")
            .AddLegend()
            .AddSeries(chartType: "Point",
                            xValue: new[] { "2012 ", "2013", "2014 ", "2015", "2016", },
                            yValues: new[] { "70", "50", "20", "90", "100" })
                            .GetBytes("png");
            return File(chart, "image/bytes");
        }

        public ActionResult CreateRangeBar()
        {
            //Create bar chart
            var chart = new Chart(width: 600, height: 200, theme: ChartTheme.Vanilla)
            .AddTitle("RangeBar")
            .AddLegend()
            .AddSeries(chartType: "RangeBar",
                            xValue: new[] { "2012 ", "2013", "2014 ", "2015", "2016", },
                            yValues: new[] { "70", "50", "20", "90", "100" })
                            .GetBytes("png");
            return File(chart, "image/bytes");
        }

        public ActionResult CreateRange()
        {
            //Create bar chart
            var chart = new Chart(width: 600, height: 200, theme: ChartTheme.Vanilla)
            .AddTitle("Range")
            .AddLegend()
            .AddSeries(chartType: "Range",
                            xValue: new[] { "2012 ", "2013", "2014 ", "2015", "2016", },
                            yValues: new[] { "70", "50", "20", "90", "100" })
                            .GetBytes("png");
            return File(chart, "image/bytes");
        }

        public ActionResult CreatePyramid()
        {
            //Create bar chart
            var chart = new Chart(width: 600, height: 200, theme: ChartTheme.Vanilla)
            .AddTitle("Pyramid")
            .AddLegend()
            .AddSeries(chartType: "Pyramid",
                            xValue: new[] { "2012 ", "2013", "2014 ", "2015", "2016", },
                            yValues: new[] { "70", "50", "20", "90", "100" })
                            .GetBytes("png");
            return File(chart, "image/bytes");
        }

        public ActionResult CreateRadar()
        {
            //Create bar chart
            var chart = new Chart(width: 600, height: 200, theme: ChartTheme.Vanilla)
            .AddTitle("Radar")
            .AddLegend()
            .AddSeries(chartType: "Radar",
                            xValue: new[] { "2012 ", "2013", "2014 ", "2015", "2016", },
                            yValues: new[] { "70", "50", "20", "90", "100" })
                            .GetBytes("png");
            return File(chart, "image/bytes");
        }

        public ActionResult CreateRenko()
        {
            //Create bar chart
            var chart = new Chart(width: 600, height: 200, theme: ChartTheme.Vanilla)
            .AddTitle("Renko")
            .AddLegend()
            .AddSeries(chartType: "Renko",
                            xValue: new[] { "2012 ", "2013", "2014 ", "2015", "2016", },
                            yValues: new[] { "70", "50", "20", "90", "100" })
                            .GetBytes("png");
            return File(chart, "image/bytes");
        }

        public ActionResult CreateSpline()
        {
            //Create bar chart
            var chart = new Chart(width: 600, height: 200, theme: ChartTheme.Vanilla)
            .AddTitle("Spline")
            .AddLegend()
            .AddSeries(chartType: "Spline",
                            xValue: new[] { "2012 ", "2013", "2014 ", "2015", "2016", },
                            yValues: new[] { "70", "50", "20", "90", "100" })
                            .GetBytes("png");
            return File(chart, "image/bytes");
        }

        public ActionResult CreateSplineArea()
        {
            //Create bar chart
            var chart = new Chart(width: 600, height: 200, theme: ChartTheme.Vanilla)
            .AddTitle("SplineArea")
            .AddLegend()
            .AddSeries(chartType: "SplineArea",
                            xValue: new[] { "2012 ", "2013", "2014 ", "2015", "2016", },
                            yValues: new[] { "70", "50", "20", "90", "100" })
                            .GetBytes("png");
            return File(chart, "image/bytes");
        }

        public ActionResult CreateSplineRange()
        {
            //Create bar chart
            var chart = new Chart(width: 600, height: 200, theme: ChartTheme.Vanilla)
            .AddTitle("SplineRange")
            .AddLegend()
            .AddSeries(chartType: "SplineRange",
                            xValue: new[] { "2012 ", "2013", "2014 ", "2015", "2016", },
                            yValues: new[] { "70", "50", "20", "90", "100" })
                            .GetBytes("png");
            return File(chart, "image/bytes");
        }

        public ActionResult CreateStackedArea()
        {
            //Create bar chart
            var chart = new Chart(width: 600, height: 200, theme: ChartTheme.Vanilla)
            .AddTitle("StackedArea")
            .AddLegend()
            .AddSeries(chartType: "StackedArea",
                            xValue: new[] { "2012 ", "2013", "2014 ", "2015", "2016", },
                            yValues: new[] { "70", "50", "20", "90", "100" })
                            .GetBytes("png");
            return File(chart, "image/bytes");
        }

        public ActionResult CreateStepLine()
        {
            //Create bar chart
            var chart = new Chart(width: 600, height: 200, theme: ChartTheme.Vanilla)
            .AddTitle("StepLine")
            .AddLegend()
            .AddSeries(chartType: "StepLine",
                            xValue: new[] { "2012 ", "2013", "2014 ", "2015", "2016", },
                            yValues: new[] { "70", "50", "20", "90", "100" })
                            .GetBytes("png");
            return File(chart, "image/bytes");
        }

        public ActionResult CreateStackedBar()
        {
            //Create bar chart
            var chart = new Chart(width: 600, height: 200, theme: ChartTheme.Vanilla)
            .AddTitle("StackedBar")
            .AddLegend()
            .AddSeries(chartType: "StackedBar",
                            xValue: new[] { "2012 ", "2013", "2014 ", "2015", "2016", },
                            yValues: new[] { "70", "50", "20", "90", "100" })
                            .GetBytes("png");
            return File(chart, "image/bytes");
        }

        public ActionResult CreateStackedColumn()
        {
            //Create bar chart
            var chart = new Chart(width: 600, height: 200, theme: ChartTheme.Vanilla)
            .AddTitle("StackedColumn")
            .AddLegend()
            .AddSeries(chartType: "StackedColumn",
                            xValue: new[] { "2012 ", "2013", "2014 ", "2015", "2016", },
                            yValues: new[] { "70", "50", "20", "90", "100" })
                            .GetBytes("png");
            return File(chart, "image/bytes");
        }

        public ActionResult CreateStock()
        {
            //Create bar chart
            var chart = new Chart(width: 600, height: 200, theme: ChartTheme.Vanilla)
            .AddTitle("Stock")
            .AddLegend()
            .AddSeries(chartType: "Stock",
                            xValue: new[] { "2012 ", "2013", "2014 ", "2015", "2016", },
                            yValues: new[] { "70", "50", "20", "90", "100" })
                            .GetBytes("png");
            return File(chart, "image/bytes");
        }

        public ActionResult PrintReport()
        {
            return new ActionAsPdf("Index") { FileName = "SummaryReport.pdf" };
        }

        public ActionResult CreateThreeLineBreak()
        {
            //Create bar chart
            var chart = new Chart(width: 600, height: 200, theme: ChartTheme.Vanilla)
            .AddTitle("ThreeLineBreak")
            .AddLegend()
            .AddSeries(chartType: "ThreeLineBreak",
                            xValue: new[] { "2012 ", "2013", "2014 ", "2015", "2016", },
                            yValues: new[] { "70", "50", "20", "90", "100" })
                            .GetBytes("png");
            return File(chart, "image/bytes");
        }
    }
}