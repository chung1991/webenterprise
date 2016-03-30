using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI.DataVisualization.Charting;
using CMR.Models;

namespace CMR.Utilities
{
    public class BrowserShareChart : MyChartBase
    {
        private BrowserShareChartData chartData;

        public BrowserShareChart(BrowserShareChartData chartData)
        {
            this.chartData = chartData;
        }

        // Add Chart Title
        protected override void AddChartTitle()
        {
            ChartTitle = chartData.Title;
        }

        // Override the AddChartSeries method to provide the chart data
        protected override void AddChartSeries()
        {
            ChartSeriesData = new List<Series>();
            var series = new Series()
            {
                ChartType = SeriesChartType.Pie,
                BorderWidth = 1
            };

            var shares = chartData.ShareData;
            foreach (var share in shares)
            {
                var point = new DataPoint();
                point.IsValueShownAsLabel = true;
                point.AxisLabel = share.Name;
                point.ToolTip = share.Name + " " +
                      share.Share.ToString("#0.##%");
                if (share.Url != null)
                {
                    point.MapAreaAttributes = "href=\"" +
                          share.Url + "\"";
                }
                point.YValues = new double[] { share.Share };
                point.LabelFormat = "P1";
                series.Points.Add(point);
            }

            ChartSeriesData.Add(series);
        }
    }
}