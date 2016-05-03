
using CMR.Models;
using CMR.Security;
using Rotativa;
using Rotativa.MVC;
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
        
        public ActionResult Index()
        {
            CRMContext db = new CRMContext();
            return View(db.CourseMonitoringReports.ToList());
            //return View();
        }

        public ActionResult SummarizeReport()
        {
            return View(StackedChartData.GetData());
        }

        public ActionResult StatisticReport()
        {
            CRMContext db = new CRMContext();
            List<CourseMonitoringReport> cmrList = db.CourseMonitoringReports.ToList();
            var dictionary = new Dictionary<string, StatisticModel>();
            foreach (CourseMonitoringReport a in cmrList)
            {
                string academicYear = Convert.ToString(a.AnnualCourse.academicYear);
                bool isExisted = dictionary.Keys.Any(k => k.Equals(academicYear));
                if (!isExisted)
                {
                    StatisticModel model2 = new StatisticModel();
                    model2.academicYear = Convert.ToInt32(academicYear);
                    model2.approvedCount = 0;
                    model2.pendingCount = 0;
                    model2.rejectedCount = 0;
                    model2.waitingCount = 0;
                    dictionary[academicYear] = model2;
                }
                StatisticModel model = dictionary[academicYear];
                if (a.ApproveStatu.name.Equals("Pending"))
                {
                    model.pendingCount++;
                }
                if (a.ApproveStatu.name.Equals("Approved"))
                {
                    model.approvedCount++;
                }
                if (a.ApproveStatu.name.Equals("Waiting"))
                {
                    model.waitingCount++;
                }
                if (a.ApproveStatu.name.Equals("Rejected"))
                {
                    model.rejectedCount++;
                }
            }
            var dictionary2 = new Dictionary<string, StatisticModel>();
            foreach (var item in dictionary.OrderBy(i => i.Key))
            {
                dictionary2.Add(item.Key,item.Value);
            }

            ViewBag.dict = dictionary2;
            return View();
        }

        public ActionResult ExceptionReport()
        {
            CRMContext db = new CRMContext();
            List<CourseMonitoringReport> cmrList = db.CourseMonitoringReports.ToList();
            var dictionary1 = new Dictionary<string, StatisticModel>();
            foreach (CourseMonitoringReport a in cmrList)
            {
                string academicYear = Convert.ToString(a.AnnualCourse.academicYear);
                bool isExisted = dictionary1.Keys.Any(k => k.Equals(academicYear));
                if (!isExisted)
                {
                    StatisticModel model2 = new StatisticModel();
                    model2.academicYear = Convert.ToInt32(academicYear);
                    model2.pendingCount = 0;
                    dictionary1[academicYear] = model2;
                }
                StatisticModel model = dictionary1[academicYear];
                if (a.ApproveStatu.name.Equals("Pending"))
                {
                    model.pendingCount++;
                }
            }
            var dictionary2 = new Dictionary<string, StatisticModel>();
            foreach (CourseMonitoringReport a in cmrList)
            {
                string academicYear = Convert.ToString(a.AnnualCourse.academicYear);
                bool isExisted = dictionary2.Keys.Any(k => k.Equals(academicYear));
                if (!isExisted)
                {
                    StatisticModel model2 = new StatisticModel();
                    model2.academicYear = Convert.ToInt32(academicYear);
                    model2.pendingCount = 0;
                    dictionary2[academicYear] = model2;
                }
                StatisticModel model = dictionary2[academicYear];
                if (String.IsNullOrEmpty(a.dltComment))
                {
                    model.pendingCount++;
                }
            }
            var dictionary11= new Dictionary<string, StatisticModel>();
            foreach (var item in dictionary1.OrderBy(i => i.Key))
            {
                dictionary11.Add(item.Key, item.Value);
            }
            var dictionary22 = new Dictionary<string, StatisticModel>();
            foreach (var item in dictionary2.OrderBy(i => i.Key))
            {
                dictionary22.Add(item.Key, item.Value);
            }
            ViewBag.dict1 = dictionary11;
            ViewBag.dict2 = dictionary22;
            return View();
        }

        public ActionResult CreateStatusStatisTicReportChart()
        {
            CRMContext db = new CRMContext();
            List<CourseMonitoringReport> cmrList = db.CourseMonitoringReports.ToList();

            var dictionary = new Dictionary<string, StatisticModel>();
            foreach (CourseMonitoringReport a in cmrList)
            {
                string academicYear = Convert.ToString(a.AnnualCourse.academicYear);
                bool isExisted = dictionary.Keys.Any(k => k.Equals(academicYear));
                if (!isExisted)
                {
                    StatisticModel model2 = new StatisticModel();
                    model2.academicYear = Convert.ToInt32(academicYear);
                    model2.approvedCount = 0;
                    model2.pendingCount = 0;
                    model2.rejectedCount = 0;
                    model2.waitingCount = 0;
                    dictionary[academicYear] = model2;
                }
                StatisticModel model = dictionary[academicYear];
                if (a.ApproveStatu.name.Equals("Pending"))
                {
                    model.pendingCount++;
                }
                if (a.ApproveStatu.name.Equals("Approved"))
                {
                    model.approvedCount++;
                }
                if (a.ApproveStatu.name.Equals("Waiting"))
                {
                    model.waitingCount++;
                }
                if (a.ApproveStatu.name.Equals("Rejected"))
                {
                    model.rejectedCount++;
                }
            }
            var dictionary2 = new Dictionary<string, StatisticModel>();
            foreach (var item in dictionary.OrderBy(i => i.Key))
            {
                dictionary2.Add(item.Key, item.Value);
            }
            String[] academicYears = dictionary2.Keys.ToArray<String>();
            Dictionary<string, string[]> pendingDict = new Dictionary<string, string[]>();
            Dictionary<string, string[]> waitingDict = new Dictionary<string, string[]>();
            Dictionary<string, string[]> approveDict = new Dictionary<string, string[]>();
            Dictionary<string, string[]> rejectDict = new Dictionary<string, string[]>();

            String[] pendingList = null;
            String[] watingList = null;
            String[] approveList = null;
            String[] rejectList = null;

            for (int i = 0; i < academicYears.Count(); i++) {
                String year = academicYears[i];
                StatisticModel theModel = dictionary2[year];

                if (i == 0)
                {
                    pendingList = new String[academicYears.Count()];
                    watingList = new String[academicYears.Count()];
                    approveList = new String[academicYears.Count()];
                    rejectList = new String[academicYears.Count()];

                    pendingDict["something"] = pendingList;
                    waitingDict["something"] = watingList;
                    approveDict["something"] = approveList;
                    rejectDict["something"] = rejectList;
                }

                pendingList[i] = Convert.ToString(theModel.pendingCount);
                watingList[i] = Convert.ToString(theModel.waitingCount);
                approveList[i] = Convert.ToString(theModel.approvedCount);
                rejectList[i] = Convert.ToString(theModel.rejectedCount);

            }
            
            var chart = new Chart(width: 800, height: 400, theme: ChartTheme.Green)
                .AddTitle("Status statistic of Course Monitoring Report Chart")
                .AddLegend()
                .AddSeries(chartType: "StackedColumn",
                                xValue: academicYears,
                                yValues: pendingDict[pendingDict.Keys.ElementAt(0)],
                                name: "Pending")
                .AddSeries(chartType: "StackedColumn",
                                xValue: academicYears,
                                yValues: waitingDict[waitingDict.Keys.ElementAt(0)],
                                name: "Waiting")
                .AddSeries(chartType: "StackedColumn",
                                xValue: academicYears,
                                yValues: rejectDict[rejectDict.Keys.ElementAt(0)],
                                name: "Rejected")
                .AddSeries(chartType: "StackedColumn",
                                xValue: academicYears,
                                yValues: approveDict[approveDict.Keys.ElementAt(0)],
                                name: "Approved")
                .GetBytes("png");

            return File(chart, "image/bytes");
        }

        public ActionResult CreatePendingReportChart()
        {
            CRMContext db = new CRMContext();
            List<CourseMonitoringReport> cmrList = db.CourseMonitoringReports.ToList();
            var dictionary = new Dictionary<string, StatisticModel>();
            foreach (CourseMonitoringReport a in cmrList)
            {
                string academicYear = Convert.ToString(a.AnnualCourse.academicYear);
                bool isExisted = dictionary.Keys.Any(k => k.Equals(academicYear));
                if (!isExisted)
                {
                    StatisticModel model2 = new StatisticModel();
                    model2.academicYear = Convert.ToInt32(academicYear);
                    model2.pendingCount = 0;
                    dictionary[academicYear] = model2;
                }
                StatisticModel model = dictionary[academicYear];
                if (a.ApproveStatu.name.Equals("Pending"))
                {
                    model.pendingCount++;
                }
            }
            var dictionary2 = new Dictionary<string, StatisticModel>();
            foreach (var item in dictionary.OrderBy(i => i.Key))
            {
                dictionary2.Add(item.Key, item.Value);
            }
            String[] academicYears = dictionary2.Keys.ToArray<String>();
            Dictionary<string, string[]> pendingDict = new Dictionary<string, string[]>();
            String[] pendingList = null;
   

            for (int i = 0; i < academicYears.Count(); i++)
            {
                String year = academicYears[i];
                StatisticModel theModel = dictionary2[year];

                if (i == 0)
                {
                    pendingList = new String[academicYears.Count()];
                    pendingDict["something"] = pendingList;

                }

                pendingList[i] = Convert.ToString(theModel.pendingCount);
            }
            var chart = new Chart(width: 600, height: 300, theme: ChartTheme.Green)
            .AddTitle("Pending Course Monitoring Report Chart")
            .AddSeries(chartType: "Bar",
                            xValue: academicYears,
                            yValues: pendingDict[pendingDict.Keys.ElementAt(0)])
                            .GetBytes("png");
            return File(chart, "image/bytes");
        }
        
        public ActionResult CreateNoResponseReportChart()
        {
            CRMContext db = new CRMContext();
            List<CourseMonitoringReport> cmrList = db.CourseMonitoringReports.ToList();
            var dictionary = new Dictionary<string, StatisticModel>();
            foreach (CourseMonitoringReport a in cmrList)
            {
                string academicYear = Convert.ToString(a.AnnualCourse.academicYear);
                bool isExisted = dictionary.Keys.Any(k => k.Equals(academicYear));
                if (!isExisted)
                {
                    StatisticModel model2 = new StatisticModel();
                    model2.academicYear = Convert.ToInt32(academicYear);
                    model2.pendingCount = 0;
                    dictionary[academicYear] = model2;
                }
                StatisticModel model = dictionary[academicYear];
                if (String.IsNullOrEmpty(a.dltComment))
                {
                    model.pendingCount++;
                }
            }
            var dictionary2 = new Dictionary<string, StatisticModel>();
            foreach (var item in dictionary.OrderBy(i => i.Key))
            {
                dictionary2.Add(item.Key, item.Value);
            }
            String[] academicYears = dictionary2.Keys.ToArray<String>();
            Dictionary<string, string[]> pendingDict = new Dictionary<string, string[]>();
            String[] pendingList = null;

            for (int i = 0; i < academicYears.Count(); i++)
            {
                String year = academicYears[i];
                StatisticModel theModel = dictionary2[year];

                if (i == 0)
                {
                    pendingList = new String[academicYears.Count()];
                    pendingDict["something"] = pendingList;

                }

                pendingList[i] = Convert.ToString(theModel.pendingCount);
            }

            var chart = new Chart(width: 600, height: 300, theme: ChartTheme.Green)
            .AddTitle("No Response Course Monitoring Report Chart")
            .AddSeries(chartType: "Bar",
                            xValue: academicYears,
                            yValues: pendingDict[pendingDict.Keys.ElementAt(0)])
                            .GetBytes("png");
            return File(chart, "image/bytes");
        }

        public ActionResult PrintStatisticReport()
        {
            return new ActionAsPdf("StatisticReport"){ FileName = "Statistic.pdf" };
        }

        public ActionResult PrintExceptionReport()
        {
            return new ActionAsPdf("ExceptionReport") { FileName = "ExceptionReport.pdf" };
        }
    }
}