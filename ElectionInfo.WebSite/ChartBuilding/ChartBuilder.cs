using System;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Web.UI.WebControls;
using System.Windows.Forms.DataVisualization.Charting;
using ElectionInfo.Model;

namespace ElectionInfo.WebSite
{
    public abstract class ChartBuilder
    {
        protected static readonly Color[] Colors = new[]
    	{
			Color.BlueViolet,
			Color.Brown,
			Color.YellowGreen,
    		Color.Black,
			Color.White,
    		Color.Purple,
    		Color.Aqua,
    		Color.Yellow,
    		Color.Red,
    		Color.Green,
    		Color.Blue,
    		Color.Pink
    	};

        protected ChartBuilder(CharacteristicsDistributionRequest request)
        {
            Request = request;
        }

        protected CharacteristicsDistributionRequest Request { get; private set; }
        protected Chart Item { get; private set; }

        public Stream Image { get; private set; }

        public void Build(ModelContext context)
        {
            Item = new Chart();
            BuildView();
            CustomizeView();
            BuildPoints(context);

            Image = new MemoryStream();
            Item.SaveImage(Image, ChartImageFormat.Png);
        }

        protected virtual void BuildView()
        {
            Item.Height = Request.ChartHeight;
            Item.Width = Request.ChartWidth;
            Item.ChartAreas.Add(
                new ChartArea
                {
                    AxisX = new Axis { Maximum = 100, Minimum = 0 },
                    AxisY = new Axis { Maximum = 100, Minimum = 0 },
                    Position = new ElementPosition(0, 10, 100, 90)
                });
            Item.Legends.Add(new Legend { Docking = Docking.Top });
        }

        protected abstract void CustomizeView();

        protected abstract void BuildPoints(ModelContext context);

        protected struct SeriesInfo
        {
            public readonly string FieldName;

            public readonly string LegendText;

            public readonly Color Color;

            public SeriesInfo(string fieldName, string legendText, Color color)
            {
                FieldName = fieldName;
                LegendText = legendText;
                Color = color;
            }
        }
    }
}