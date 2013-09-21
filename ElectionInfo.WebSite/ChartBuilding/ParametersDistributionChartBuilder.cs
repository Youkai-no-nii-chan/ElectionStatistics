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
                results = results.ByHigherDistrict(context.ElectoralDistricts.GetById(Request.DistrictId.Value));
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

                IQueryable<ChartData> values;
                if (Enum.IsDefined(typeof(DistributionParameter), distributionParameter))
                {
                    series.LegendText = ((DistributionParameter)distributionParameter).GetDescription(); 
                    switch ((DistributionParameter)distributionParameter)
                    {
                        case DistributionParameter.Attendance:
                            values = results.Select(result => new ChartData
                            {
                                VotersCount = result.VotersCount,
                                Value = result.InsideBallotsCount + result.OutsideBallotsCount == 0
                                    ? 0
                                    : ((double)(result.InsideBallotsCount + result.OutsideBallotsCount)) / result.VotersCount * 100
                            });
                            break;
                        case DistributionParameter.AbsenteeCertificateVotersCount:
                            values = results.Select(result => new ChartData
                            {
                                VotersCount = result.InsideBallotsCount + result.OutsideBallotsCount,
                                Value = result.InsideBallotsCount + result.OutsideBallotsCount == 0
                                    ? 0
                                    : ((double)result.AbsenteeCertificateVotersCount) / (result.InsideBallotsCount + result.OutsideBallotsCount) * 100
                            });
                            break;
                        case DistributionParameter.OutsideBallotsCount:
                            values = results.Select(result => new ChartData
                            {
                                VotersCount = result.InsideBallotsCount + result.OutsideBallotsCount,
                                Value = result.InsideBallotsCount + result.OutsideBallotsCount == 0
                                    ? 0
                                    : ((double)result.OutsideBallotsCount) / (result.InsideBallotsCount + result.OutsideBallotsCount) * 100
                            });
                            break;
                        default:
                            throw new ArgumentOutOfRangeException();
                    }
                }
                else
                {
                    var candidate = candidates.GetById(distributionParameter);
                    series.LegendText = candidate.ShortName;

                    values = results
                        .Join(
                            context.ElectionCandidatesVotes.ByCandidate(candidate),
                            result => result.Id,
                            votes => votes.ElectionResultId,
                            (result, votes) => new ChartData
                            {
                                VotersCount = result.InsideBallotsCount + result.OutsideBallotsCount,
                                Value = result.InsideBallotsCount + result.OutsideBallotsCount == 0
                                    ? 0
                                    : ((double)votes.Count) / (result.InsideBallotsCount + result.OutsideBallotsCount) * 100
                            });
                }

                var grouppedValues = values
                    .GroupBy(data => Math.Ceiling(data.Value / Request.DistributionStepSize) * Request.DistributionStepSize);
                var points = Request.DistributionValue == DistributionValue.VotersCount
                    ? grouppedValues.Select(groupping => new { X = groupping.Key, Y = groupping.Sum(data => data.VotersCount) })
                    : grouppedValues.Select(groupping => new { X = groupping.Key, Y = groupping.Count() });

                double previousX = 0;
                foreach (var point in points.OrderBy(arg => arg.X))
                {
                    while (point.X - previousX >= Request.DistributionStepSize)
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
            Item.ChartAreas[0].AxisX.MajorGrid.Interval = 10;
            Item.ChartAreas[0].AxisX.Interval = 10;
            Item.ChartAreas[0].AxisY.MajorGrid.Interval = yMax / 10;
            Item.ChartAreas[0].AxisY.Interval = yMax / 10;
            Item.ChartAreas[0].AxisY.Maximum = yMax;
        }

        private class ChartData
        {
            public int VotersCount { get; set; }
            public double Value { get; set; }
        }
    }
}