using System.Linq;
using Microcharts;
using SkiaSharp;

namespace FZAM.Microcharts.Charts
{
    public class CopingBarChart : BarChart
    {
        public bool DrawBackground { get; set; }

        public override void DrawContent(SKCanvas canvas, int width, int height)
        {
            if (this.DrawBackground)
            {
                width = MeasureHelper.CalculateYAxis(this.ShowYAxisText, this.ShowYAxisLines, this.entries,
                    this.YAxisMaxTicks, this.YAxisTextPaint, this.YAxisPosition, width, out var yAxisXShift,
                    out var yAxisIntervalLabels);
                var firstSerie = this.Series.FirstOrDefault();
                var labels = firstSerie.Entries.Select(x => x.Label).ToArray();
                var nbItems = labels.Length;

                var seriesNames = this.Series.Select(s => s.Name).ToArray();
                var seriesSizes = MeasureHelper.MeasureTexts(seriesNames, this.SerieLabelTextSize);
                var legendHeight = this.CalculateLegendSize(seriesSizes, this.SerieLabelTextSize, width);

                var labelSizes = MeasureHelper.MeasureTexts(labels, this.LabelTextSize);
                var footerHeight =
                    MeasureHelper.CalculateFooterHeaderHeight(this.Margin, this.LabelTextSize, labelSizes,
                        this.LabelOrientation);

                var valueLabelSizes = this.MeasureValueLabels();
                var headerHeight = this.CalculateHeaderHeight(valueLabelSizes);
                var headerWithLegendHeight =
                    headerHeight + (this.LegendOption == SeriesLegendOption.Top ? legendHeight : 0);

                var itemSize =
                    this.CalculateItemSize(nbItems, width, height, footerHeight + headerHeight + legendHeight);

                this.DrawAxisLines(canvas, itemSize, headerWithLegendHeight, width);
            }

            base.DrawContent(canvas, width, height);
        }


        protected void DrawAxisLines(SKCanvas canvas, SKSize itemSize, float headerHeight, int width)
        {
            var xStart = 0;
            var yStart = headerHeight + itemSize.Height;

            var xEnd = width;
            var yEnd = headerHeight;

            // large coloured background boxes
            for (var i = 0; i < this.ValueRange + 1; i++)
            {
                var colorGradientBackground = i * 10 + 10;
                var backgroundPaint = new SKPaint
                {
                    Style = SKPaintStyle.Fill,
                    Color = new SKColor((byte)(252 - colorGradientBackground), (byte)(213 - colorGradientBackground),
                        (byte)(230 - colorGradientBackground)),
                    StrokeWidth = 1,
                    IsAntialias = true
                };

                var step = (yStart - yEnd) / this.ValueRange + 1;
                var yPos = yStart - step * i;// + step / 2;
                var yHeight = -step;
                /*if (i <= 0)
                {
                    yPos = yPos - step / 2;
                    yHeight = -step / 2;
                }*/

                if (i >= this.ValueRange)
                    continue;
                //yPos = yPos + step / 2;
                //yHeight = -step / 2;

                canvas.DrawRect(xStart, yPos, xEnd, yHeight, backgroundPaint);
            }

            for (var i = 0; i < this.ValueRange; i++)
            {
                const int TEXT_SIZE = 30;
                var colorGradientScaleText = i * 5 - 5;
                var labelPaint = new SKPaint
                {
                    Style = SKPaintStyle.Fill,
                    Color = new SKColor((byte)(250 - colorGradientScaleText),
                        (byte)(235 - colorGradientScaleText), (byte)(242 - colorGradientScaleText)),
                    IsAntialias = true,
                    TextSize = TEXT_SIZE,
                    FakeBoldText = true,
                    TextAlign = SKTextAlign.Center
                };

                var step = (yStart - yEnd) / this.ValueRange + 1;
                var yPos = yStart - step * i;// + step / 2;
                /*if (i <= 0)
                    yPos = yPos - TEXT_SIZE / 2 + 10;
                else if (i >= this.ValueRange)
                    yPos = yPos + TEXT_SIZE;
                else*/
                yPos = yPos + TEXT_SIZE / 2;
                canvas.DrawText((i + 1).ToString(), xStart + 10, yPos - step / 2 - 2, labelPaint);
            }
        }
    }
}