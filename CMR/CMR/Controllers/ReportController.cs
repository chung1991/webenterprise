
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
     [CustomAuthorize(Roles = "Admin")]
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

        [CustomAuthorize(Roles = "Admin")]
        public ActionResult SummarizeReport()
        {
            return View(StackedChartData.GetData());
        }

        [CustomAuthorize(Roles = "Admin")]
        public ActionResult StatisticReport()
        {
            CRMContext db = new CRMContext();
            return View(db.CourseMonitoringReports.ToList());
        }

        [CustomAuthorize(Roles = "Admin")]
        public ActionResult ExceptionReport()
        {
            CRMContext db = new CRMContext();
            return View(db.CourseMonitoringReports.ToList());
        }

        public ActionResult CreateStatusStatisTicReportChart()
        {
            CRMContext db = new CRMContext();
            //Create bar chart
            var chart = new Chart(width: 800, height: 400, theme: ChartTheme.Green)
                .AddTitle("Status statistic of Course Monitoring Report Chart")
                .AddLegend()
                .AddSeries(chartType: "StackedColumn",
                                xValue: new[] { "2012 ", "2013", "2014 ", "2015", "2016" },
                                yValues: new[] { "12", "14", "17", "10", "8" },
                                name: "Pending")
                .AddSeries(chartType: "StackedColumn",
                                xValue: new[] { "2012 ", "2013", "2014 ", "2015", "2016" },
                                yValues: new[] { "5", "9", "20", "11", "33" },
                                name: "Waiting")
                .AddSeries(chartType: "StackedColumn",
                                xValue: new[] { "2012 ", "2013", "2014 ", "2015", "2016" },
                                yValues: new[] { "2", "0", "15", "7", "25" },
                                name: "Rejected")
                .AddSeries(chartType: "StackedColumn",
                                xValue: new[] { "2012 ", "2013", "2014 ", "2015", "2016" },
                                yValues: new[] { "20", "30", "40", "50", "60" },
                                name: "Approved")
                .GetBytes("png");
            return File(chart, "image/bytes");
        }

        public ActionResult CreatePendingReportChart()
        {
            //Create bar chart
            var chart = new Chart(width: 600, height: 300, theme: ChartTheme.Green)
            .AddTitle("Pending Course Monitoring Report Chart")
            .AddLegend()
            .AddSeries(chartType: "Bar",
                            xValue: new[] { "2012 ", "2013", "2014 ", "2015", "2016", },
                            yValues: new[] { "12", "14", "17", "10", "8" })
                            .GetBytes("png");
            return File(chart, "image/bytes");
        }

        public ActionResult CreateNoResponseReportChart()
        {
            //Create bar chart
            var chart = new Chart(width: 600, height: 300, theme: ChartTheme.Green)
            .AddTitle("No Response Course Monitoring Report Chart")
            .AddLegend()
            .AddSeries(chartType: "Bar",
                            xValue: new[] { "2012 ", "2013", "2014 ", "2015", "2016", },
                            yValues: new[] { "4", "9", "5", "2", "7" })
                            .GetBytes("png");
            return File(chart, "image/bytes");
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
            var chart = new Chart(width: 300, height: 200, theme : ChartTheme.Vanilla)
            .AddTitle("Stack Column 100")
            .AddLegend()
            .AddSeries(chartType: "StackedColumn",
                            xValue: new[] { "2012 ", "2013", "2014 ", "2015", "2016"},
                            yValues: new[] { "20", "20", "20", "20", "20" },
                            name: "Wait", markerStep : 4)
            .AddSeries(chartType: "StackedColumn",
                            xValue: new[] { "2012 ", "2013", "2014 ", "2015", "2016"},
                            yValues: new[] { "10", "30", "10", "40", "5" },
                            name: "Approved", markerStep: 5)
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