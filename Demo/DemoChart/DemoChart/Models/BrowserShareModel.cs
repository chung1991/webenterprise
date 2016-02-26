using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Collections.Generic;
using System.IO;
using DemoChart.Utilities;

namespace DemoChart.Models
{
    public class BrowserInformation
    {
        public string Name { get; set; }
        public double Share { get; set; }
        public string Url { get; set; }
        public string ToolTip
        {
            get
            {
                return Name + " " + Share.ToString("#0.##%");
            }
        }
    }

    public class BrowserShareChartData
    {
        public string Title { get; set; }
        public int Width { get; set; }
        public int Height { get; set; }
        public List<BrowserInformation> ShareData { get; set; }

        public MemoryStream ChartImageStream()
        {
            var chart = new BrowserShareChart(this);
            return chart.GetChartImage(Width, Height);
        }

        public string ChartImageMap(string name)
        {
            var chart = new BrowserShareChart(this);
            return chart.GetChartImageMap(Width, Height, name);
        }
    }

    public class BrowserShareRepository
    {
        public static BrowserShareChartData GetBrowserShares()
        {
            var chartData = new BrowserShareChartData()
            {
                Title = "Browser usage on Wikipedia October 2011",
                Width = 450,
                Height = 300,
                ShareData = new List<BrowserInformation>()
            };

            // The following data is the true data from Wikipedia
            chartData.ShareData.Add(new BrowserInformation()
            {
                Name = "IE",
                Share = 0.342,
                Url = "http://en.wikipedia.org/wiki/Internet_Explorer"
            });

            chartData.ShareData.Add(new BrowserInformation()
            {
                Name = "Firefox",
                Share = 0.236,
                Url = "http://en.wikipedia.org/wiki/Firefox"
            });

            chartData.ShareData.Add(new BrowserInformation()
            {
                Name = "Chrome",
                Share = 0.206,
                Url = "http://en.wikipedia.org/wiki/Google_Chrome"
            });

            chartData.ShareData.Add(new BrowserInformation()
            {
                Name = "Safari",
                Share = 0.112,
                Url = "http://en.wikipedia.org/wiki/Safari_(web_browser)"
            });

            chartData.ShareData.Add(new BrowserInformation()
            {
                Name = "Other",
                Share = 0.104,
                Url = null
            });

            return chartData;
        }
    }
}