using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.UI.DataVisualization.Charting;
using WebFrontend.Models;

namespace WebFrontend.Controllers
{
    public static class GraphGeneration
    {
        public static IEnumerable<TSource> DistinctBy<TSource, TKey>
            (this IEnumerable<TSource> source, Func<TSource, TKey> keySelector)
        {
            HashSet<TKey> seenKeys = new HashSet<TKey>();
            foreach (TSource element in source)
            {
                if (seenKeys.Add(keySelector(element)))
                {
                    yield return element;
                }
            }
        }
        public static MvcHtmlString LanguageChart(this HtmlHelper html, List<OCAPESubmissionReport> reports, int chartType)
        {
            System.Web.UI.DataVisualization.Charting.Chart chart1 = new System.Web.UI.DataVisualization.Charting.Chart();
            chart1.Width = 412;
            chart1.Height = 296;
            chart1.RenderType = RenderType.ImageTag;
            chart1.ImageLocation = "TempImages\\ChartPic" + Guid.NewGuid() + ".png";

            // Add all of your chart attributes here...
            ChartArea ca = new ChartArea("Test");
            ca.AxisX.MajorGrid.LineColor = Color.LightGray;
            ca.AxisY.MajorGrid.LineColor = Color.LightGray;

            
            chart1.ChartAreas.Add(ca);

            Series se = new Series("Series", 25);

            se.ChartType = SeriesChartType.Bar;
            int i=0;

            var xvals = new List<string>();
            var yvals = new List<double>();

            if (chartType == 1)
            {
                ca.AxisY.Title = "Runtime in ms";
                foreach (OCAPESubmissionReport language in reports.DistinctBy(p => p.Submission.Extension))
                {
                    i++;
                    string extension = language.Submission.Extension;

                    double value = reports.Where(p => p.Submission.Extension.Equals(extension)).Min(p => p.AverageRuntime());

                    //se.Points.Add(new DataPoint(i, value));
                    yvals.Add(value);
                    xvals.Add(extension);
                }
            }

            if(chartType == 2)
            {
                ca.AxisY.Title = "Runtime in ms";
                foreach (OCAPESubmissionReport language in reports.DistinctBy(p => p.Submission.Extension))
                {
                    i++;
                    string extension = language.Submission.Extension;

                    double value = reports.Where(p => p.Submission.Extension.Equals(extension)).Average(p => p.AverageRuntime());

                    //se.Points.Add(new DataPoint(i, value));
                    yvals.Add(value);
                    xvals.Add(extension);
                }
            }

            if (chartType == 3)
            {
                ca.AxisY.Title = "Memory usage in kb";
                foreach (OCAPESubmissionReport language in reports.DistinctBy(p => p.Submission.Extension))
                {
                    i++;
                    string extension = language.Submission.Extension;

                    double value = reports.Where(p => p.Submission.Extension.Equals(extension)).Average(p => p.AverageMemoryUsage());

                    //se.Points.Add(new DataPoint(i, value));
                    yvals.Add(value);
                    xvals.Add(extension);
                }
            }

            if(chartType == 4)
            {
                chart1.Height = 600;
                ca.AxisY.Title = "Runtime in ms";
                foreach (OCAPESubmissionReport language in reports.DistinctBy(p => p.Submission.Extension))
                {
                    i++;
                    string extension = language.Submission.Extension;

                    var reps = reports.Where(p => p.Submission.Extension.Equals(extension)).OrderBy(p => p.AverageRuntime()).Take(3);

                    foreach (var rep in reps)
                    {
                        //se.Points.Add(new DataPoint(i, value));
                        yvals.Add(rep.AverageRuntime());
                        xvals.Add(rep.Submission.OwnerNote + " - " + extension);
                    }
                }
            }

            if (chartType == 5)
            {
                ca.AxisY.Title = "Score";
                foreach (OCAPESubmissionReport language in reports.DistinctBy(p => p.Submission.Extension))
                {
                    i++;
                    string extension = language.Submission.Extension;

                    double value = reports.Where(p => p.Submission.Extension.Equals(extension)).Max(p => p.Score);

                    //se.Points.Add(new DataPoint(i, value));
                    yvals.Add(value);
                    xvals.Add(extension);
                }
            }

            se.Points.DataBindXY(xvals, yvals);

            chart1.Series.Add(se);

            // Render chart control
            chart1.SaveImage(AppDomain.CurrentDomain.BaseDirectory + chart1.ImageLocation);
            var tag = new TagBuilder("img");
            tag.Attributes.Add("src", "/" + chart1.ImageLocation.Replace("\\", "/"));
            tag.Attributes.Add("style", "height:" + chart1.Height + "px;width:" + chart1.Width + "px;border-width:0px;");

            return new MvcHtmlString(tag.ToString());
        }
    }
}