using System;
using System.Linq;
using System.Windows.Forms.DataVisualization.Charting;
using ElectionInfo.Model;

namespace ElectionInfo.WebSite
{
    public class ParametersDistributionChartBuilder : ChartBuilder
    {
        public ParametersDistributionChartBuilder(CharacteristicsDistributionRequest request) : base(request)
        {
        }

        protected override void CustomizeView()
        {
            ChartArea chartArea = Item.ChartAreas[0];
            chartArea.AxisX.Title = "Явка или голоса (%)";
            chartArea.AxisY.Title = Request.DistributionValue.GetDescription();
        }

        protected override void BuildPoints(ModelContext context)
        {
            var candidates = context.Candidates.ByElection(context, Request.ElectionId.Value);
            var results = context.ElectionResults.ByElection(Request.ElectionId.Value);
            if (Request.DistrictId != null)
            {
                results = results.ByDistrictOrHigherDistrict(context.ElectoralDistricts.GetById(Request.DistrictId.Value));
            }

            int colorId = 0;
            double yMax = 0;
            foreach (int distributionParameter in Request.DistributionParameters)
            {
                var series = new Series
                {
                    Color = Colors[colorId], 
                    Name = distributionParameter.ToString(), 
                    ChartType = SeriesChartType.Spline
                };
                colorId++;

                IQueryable<ChartData> values = null;
                if (Enum.IsDefined(typeof(DistributionParameter), distributionParameter))
                {
                    series.LegendText = ((DistributionParameter)distributionParameter).GetDescription(); 
                    switch ((DistributionParameter)distributionParameter)
                    {
                        case DistributionParameter.Attendance:
                            values = results.Select(result => new ChartData
                            {
                                VotersCount = result.VotersCount,
                                Value = ((double)(result.IssuedInsideBallotsCount + result.IssuedOutsideBallotsCount)) / result.VotersCount * 100
                            });
                            break;
                    }
                }
                else
                {
                    var candidate = candidates.GetById(distributionParameter);
                    series.LegendText = candidate.ShortName;
                }

                var grouppedValues = values
                    .GroupBy(data => Math.Ceiling(data.Value / Request.DistributionStepSize) * Request.DistributionStepSize);
                var points = Request.DistributionValue == DistributionValue.VotersCount
                    ? grouppedValues.Select(groupping => new { X = groupping.Sum(data => data.VotersCount), Y = groupping.Key })
                    : grouppedValues.Select(groupping => new { X = groupping.Count(), Y = groupping.Key });

                double previousX = 0;
                foreach (var point in points)
                {
                    while (point.X - previousX > Request.DistributionStepSize)
                    {
                        series.Points.Add(new DataPoint(previousX, 0));
                        previousX += Request.DistributionStepSize;
                    }

                    if (point.Y > yMax)
                    {
                        yMax = point.Y;
                    }
                    series.Points.Add(new DataPoint(point.X, point.Y));
                    previousX = point.X + Request.DistributionStepSize;
                }

                previousX += Request.DistributionStepSize;
                while (previousX <= 100)
                {
                    series.Points.Add(new DataPoint(previousX, 0));
                    previousX += Request.DistributionStepSize;
                }

                Item.Series.Add(series);
            }
            Item.ChartAreas[0].AxisY.Maximum = yMax;
        }

        private class ChartData
        {
            public int VotersCount { get; set; }
            public double Value { get; set; }
        }
    }
}