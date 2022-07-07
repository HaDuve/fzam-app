// Copyright (c) Aloïs DENIEL. All rights reserved.
// Licensed under the MIT License. See LICENSE in the project root for license information.

using System;
using System.Collections.Generic;
using System.Linq;
using SkiaSharp;

namespace Microcharts
{
    /// <summary>
    /// Base chart for Series chart based on Axis work
    /// </summary>
    public abstract class AxisBasedChart : SeriesChart
    {
        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="T:Microcharts.AxisBasedChart"/> class.
        /// </summary>
        public AxisBasedChart()
        {
            LabelOrientation = Orientation.Default;
            ValueLabelOrientation = Orientation.Default;

            YAxisTextPaint = new SKPaint
            {
                Color = SKColors.Black,
                IsAntialias = true,
                Style = SKPaintStyle.StrokeAndFill,
            };

            YAxisLinesPaint = new SKPaint
            {
                Color = SKColors.Black.WithAlpha(0x50),
                IsAntialias = true,
                Style = SKPaintStyle.Stroke
            };
        }

        #endregion

        /// <inheritdoc />
        protected override void GenerateDefaultSerie(IEnumerable<ChartEntry> value)
        {
            //To maintain a continuity with the previous barchart version,
            //we set properties accordingly to match the previous behaviors
            //OnLabelOrientationChanged();
            OnLabelTextSizeChanged();

            base.GenerateDefaultSerie(value);
        }

        /// <inheritdoc />
        protected override void OnLabelTextSizeChanged()
        {
            if (IsSeriesAutoGenerated && valueLabelTextSizeDefaultValue)
                ValueLabelTextSize = LabelTextSize;
        }

        #region Properties

        private Orientation labelOrientation;
        private Orientation valueLabelOrientation;
        private float valueLabelTextSize = 16;
        private float serieLabelTextSize = 16;
        private bool valueLabelTextSizeDefaultValue = true;

        /// <summary>
        /// Get or sets the legend option for the chart
        /// </summary>
        /// <value>The legend option</value>
        public SeriesLegendOption LegendOption { get; set; } = SeriesLegendOption.None;


        /// <summary>
        /// Gets or sets the text orientation of labels.
        /// </summary>
        /// <value>The label orientation.</value>
        public Orientation LabelOrientation
        {
            get => labelOrientation;
            set => labelOrientation = (value == Orientation.Default) ? Orientation.Vertical : value;
        }

        /// <summary>
        /// Gets or sets the text orientation of value labels.
        /// </summary>
        /// <value>The label orientation.</value>
        public Orientation ValueLabelOrientation
        {
            get => valueLabelOrientation;
            set => valueLabelOrientation = (value == Orientation.Default) ? Orientation.Vertical : value;
        }

        /// <summary>
        /// Gets or sets the text size of the value labels.
        /// </summary>
        /// <value>The size of the value label text.</value>
        public float ValueLabelTextSize
        {
            get => valueLabelTextSize;
            set
            {
                Set(ref valueLabelTextSize, value);
                valueLabelTextSizeDefaultValue = false;
            }
        }

        /// <summary>
        /// Gets or sets the value label layout option
        /// </summary>
        /// <remarks>Default is <seealso cref="T:Microcharts.ValueLabelOption.TopOfChart"/></remarks>
        /// <value>The layout option of value labels</value>
        public ValueLabelOption ValueLabelOption { get; set; } = ValueLabelOption.TopOfChart;

        /// <summary>
        /// Gets or sets the text size of the serie labels.
        /// </summary>
        /// <value>The size of the serie label text.</value>
        public float SerieLabelTextSize
        {
            get => serieLabelTextSize;
            set => Set(ref serieLabelTextSize, value);
        }

        /// <summary>
        /// Show Y Axis Text?
        /// </summary>
        public bool ShowYAxisText { get; set; } = false;

        /// <summary>
        /// Show Y Axis Lines?
        /// </summary>
        public bool ShowYAxisLines { get; set; } = false;

        //TODO : calculate this automatically, based on available area height and text height
        /// <summary>
        /// Y Axis Max Ticks
        /// </summary>
        public int YAxisMaxTicks { get; set; } = 5;

        /// <summary>
        /// Y Axis Position
        /// </summary>
        public Position YAxisPosition { get; set; } = Position.Right;

        /// <summary>
        /// Y Axis Paint
        /// </summary>
        public SKPaint YAxisTextPaint { get; set; }

        /// <summary>
        /// Y Axis Paint
        /// </summary>
        public SKPaint YAxisLinesPaint { get; set; }

        #endregion

        #region Methods

        /// <summary>
        /// Draws the content of the chart onto the specified canvas.
        /// </summary>
        /// <param name="canvas">The output canvas.</param>
        /// <param name="width">The width of the chart.</param>
        /// <param name="height">The height of the chart.</param>
        public override void DrawContent(SKCanvas canvas, int width, int height)
        {
            if (Series != null && entries != null)
            {
                width = MeasureHelper.CalculateYAxis(ShowYAxisText, ShowYAxisLines, entries, YAxisMaxTicks, YAxisTextPaint, YAxisPosition, width, out float yAxisXShift, out List<float> yAxisIntervalLabels);
                var firstSerie = Series.FirstOrDefault();
                var labels = firstSerie.Entries.Select(x => x.Label).ToArray();
                int nbItems = labels.Length;

                var groupedEntries = entries.GroupBy(x => x.Label);

                int barPerItems = groupedEntries.Max(g => g.Count());

                var seriesNames = Series.Select(s => s.Name).ToArray();
                var seriesSizes = MeasureHelper.MeasureTexts(seriesNames, SerieLabelTextSize);
                float legendHeight = CalculateLegendSize(seriesSizes, SerieLabelTextSize, width);

                var labelSizes = MeasureHelper.MeasureTexts(labels, LabelTextSize);
                var footerHeight = MeasureHelper.CalculateFooterHeaderHeight(Margin, LabelTextSize, labelSizes, LabelOrientation);
                var footerWithLegendHeight = footerHeight + (LegendOption == SeriesLegendOption.Bottom ? legendHeight : 0);

                var valueLabelSizes = MeasureValueLabels();
                float headerHeight = CalculateHeaderHeight(valueLabelSizes);
                var headerWithLegendHeight = headerHeight + (LegendOption == SeriesLegendOption.Top ? legendHeight : 0);

                var itemSize = CalculateItemSize(nbItems, width, height, footerHeight + headerHeight + legendHeight);
                var barSize = CalculateBarSize(itemSize, Series.Count());
                var origin = CalculateYOrigin(itemSize.Height, headerWithLegendHeight);
                DrawHelper.DrawYAxis(ShowYAxisText, ShowYAxisLines, YAxisPosition, YAxisTextPaint, YAxisLinesPaint, Margin, AnimationProgress, MaxValue, ValueRange, canvas, width, yAxisXShift, yAxisIntervalLabels, headerHeight, itemSize, origin);

                int nbSeries = series.Count();
                for (int i = 0; i < labels.Length; i++)
                {
                    string label = labels[i];
                    SKRect labelSize = labelSizes[i];

                    var itemX = Margin + (itemSize.Width / 2) + (i * (itemSize.Width + Margin));

                    for (int serieIndex = 0; serieIndex < nbSeries; serieIndex++)
                    {
                        ChartSerie serie = Series.ElementAt(serieIndex);
                        ChartEntry entry = serie.Entries.ElementAt(i);
                        float value = entry?.Value ?? 0;
                        float marge = serieIndex < nbSeries ? Margin / 2 : 0;
                        float totalBarMarge = serieIndex * Margin / 2;
                        float barX = itemX + serieIndex * barSize.Width + totalBarMarge;
                        float barY = headerWithLegendHeight + ((1 - AnimationProgress) * (origin - headerWithLegendHeight) + (((MaxValue - value) / ValueRange) * itemSize.Height) * AnimationProgress);

                        DrawBarArea(canvas, headerWithLegendHeight, itemSize, barSize, serie.Color ?? entry.Color, origin, value, barX, barY);
                        DrawBar(serie, canvas, headerWithLegendHeight, itemX, itemSize, barSize, origin, barX, barY, serie.Color ?? entry.Color);
                        DrawValueLabel(canvas, valueLabelSizes, headerWithLegendHeight, itemSize, barSize, entry, barX, barY, itemX, origin);
                    }

                    if(!string.IsNullOrEmpty(label))
                        DrawHelper.DrawLabel(canvas, LabelOrientation, YPositionBehavior.None, itemSize, new SKPoint(itemX, height - footerWithLegendHeight + Margin), LabelColor, labelSize, label, LabelTextSize, Typeface);
                }

                DrawLegend(canvas, seriesSizes, legendHeight, height, width);
                OnDrawContentEnd(canvas, itemSize, origin, valueLabelSizes);
            }
        }

        /// <summary>
        /// Calculate the header height to take care of the value label size display
        /// </summary>
        /// <param name="valueLabelSizes"></param>
        /// <returns>The calculated header height</returns>
        protected virtual float CalculateHeaderHeight(Dictionary<ChartEntry, SKRect> valueLabelSizes)
        {
            return MeasureHelper.CalculateFooterHeaderHeight(Margin, ValueLabelTextSize, valueLabelSizes.Values.ToArray(), ValueLabelOrientation);
        }

        /// <summary>
        /// Draw the value label of the corresponding entry on the canvas
        /// </summary>
        /// <param name="canvas"></param>
        /// <param name="valueLabelSizes"></param>
        /// <param name="headerWithLegendHeight"></param>
        /// <param name="itemSize"></param>
        /// <param name="barSize"></param>
        /// <param name="entry"></param>
        /// <param name="barX"></param>
        /// <param name="barY"></param>
        /// <param name="itemX"></param>
        protected virtual void DrawValueLabel(SKCanvas canvas, Dictionary<ChartEntry, SKRect> valueLabelSizes, float headerWithLegendHeight, SKSize itemSize, SKSize barSize, ChartEntry entry, float barX, float barY, float itemX, float origin)
        {
            if (!string.IsNullOrEmpty(entry?.ValueLabel))
                DrawHelper.DrawLabel(canvas, ValueLabelOrientation, YPositionBehavior.UpToElementHeight, barSize, new SKPoint(barX - (itemSize.Width / 2) + (barSize.Width / 2), headerWithLegendHeight - Margin), entry.ValueLabelColor.WithAlpha((byte)(255 * AnimationProgress)), valueLabelSizes[entry], entry.ValueLabel, ValueLabelTextSize, Typeface);
        }

        /// <summary>
        /// Method executed after the DrawContent parts
        /// </summary>
        /// <remarks>Usable by children to draw others contents on the canvas</remarks>
        /// <param name="canvas">canvas of the chart</param>
        /// <param name="itemSize">size of an item (per label)</param>
        /// <param name="origin">the calculated y origin</param>
        /// <param name="valueLabelSizes">Value label size by entry</param>
        protected virtual void OnDrawContentEnd(SKCanvas canvas, SKSize itemSize, float origin, Dictionary<ChartEntry, SKRect> valueLabelSizes)
        {
        }

        private void DrawLegend(SKCanvas canvas, SKRect[] seriesNameSize, float legendHeight, float height, float width)
        {
            if (LegendOption == SeriesLegendOption.None)
                return;

            if (Series.Any(s => !s.Color.HasValue))
                throw new ArgumentNullException(nameof(ChartSerie.Color), "Unable to draw legend without set a color in ChartSerie");

            float lineHeight = Math.Max(seriesNameSize.Where(b => !b.IsEmpty).Select(b => b.Height).FirstOrDefault(), SerieLabelTextSize);

            float origin = Margin;
            if (LegendOption == SeriesLegendOption.Bottom)
                origin += height - legendHeight;

            int nbLine = 1;
            float currentWidthUsed = 0;
            var series = Series.ToArray();
            for (int i = 0; i < series.Length; i++)
            {
                var serie = series[i];
                var serieBound = seriesNameSize[i];
            
                float legentItemWidth = Margin + SerieLabelTextSize + Margin + serieBound.Width;
                if (legentItemWidth > width)
                {
                    if (currentWidthUsed != 0)
                    {
                        nbLine++;
                        currentWidthUsed = 0;
                    }

                    currentWidthUsed = GenerateSerieLegend(canvas, lineHeight, origin, nbLine, currentWidthUsed, serie);
                }
                else if (legentItemWidth + currentWidthUsed > width)
                {
                    nbLine++;
                    currentWidthUsed = 0;
                    currentWidthUsed = GenerateSerieLegend(canvas, lineHeight, origin, nbLine, currentWidthUsed, serie);
                }
                else
                {
                    currentWidthUsed = GenerateSerieLegend(canvas, lineHeight, origin, nbLine, currentWidthUsed, serie);
                }
            }

        }

        private float GenerateSerieLegend(SKCanvas canvas, float lineHeight, float origin, int nbLine, float currentWidthUsed, ChartSerie serie)
        {
            var legendColor = serie.Color.Value.WithAlpha((byte)(serie.Color.Value.Alpha * AnimationProgress));
            var lblColor = LabelColor.WithAlpha((byte)(LabelColor.Alpha * AnimationProgress));
            var yPosition = origin + (nbLine - 1) * (lineHeight + Margin);
            var rect = SKRect.Create(currentWidthUsed + Margin, yPosition, SerieLabelTextSize, SerieLabelTextSize);
            using (var paint = new SKPaint
            {
                Style = SKPaintStyle.Fill,
                Color = legendColor
            })
            {
                canvas.DrawRect(rect, paint);
            }

            currentWidthUsed += Margin + SerieLabelTextSize + Margin;
            using (var paint = new SKPaint())
            {
                paint.TextSize = SerieLabelTextSize;
                paint.IsAntialias = true;
                paint.Color = lblColor;
                paint.IsStroke = false;
                paint.Typeface = Typeface;

                var bounds = new SKRect();
                paint.MeasureText(serie.Name, ref bounds);
                //Vertical center align the text to the legend color box
                float textYPosition = rect.Bottom - ((rect.Bottom - rect.Top) / 2) + (bounds.Height / 2);
                canvas.DrawText(serie.Name, currentWidthUsed, textYPosition, paint);
                currentWidthUsed += bounds.Width;
            }

            return currentWidthUsed;
        }

        protected float CalculateLegendSize(SKRect[] seriesSizes, float serieLabelTextSize, int width)
        {
            if (LegendOption == SeriesLegendOption.None)
                return 0;

            int nbLine = 1;
            float currentWidthUsed = 0;
            foreach(var rect in seriesSizes)
            {
                float legentItemWidth = Margin + serieLabelTextSize + Margin + rect.Width;
                if (legentItemWidth > width)
                {
                    if (currentWidthUsed != 0)
                        nbLine++;
                    currentWidthUsed = width;
                }
                else if (legentItemWidth + currentWidthUsed > width)
                {
                    nbLine++;
                    currentWidthUsed = legentItemWidth;
                }
            }

            float height = Math.Max(seriesSizes.Where(b => !b.IsEmpty).Select(b => b.Height).FirstOrDefault(), serieLabelTextSize);

            return nbLine * height + nbLine * Margin;
        }

        protected float CalculateYOrigin(float itemHeight, float headerHeight)
        {
            if (MaxValue <= 0)
            {
                return headerHeight;
            }

            if (MinValue > 0)
            {
                return headerHeight + itemHeight;
            }

            return headerHeight + ((MaxValue / ValueRange) * itemHeight);
        }

        protected Dictionary<ChartEntry, SKRect> MeasureValueLabels()
        {
            var dict = new Dictionary<ChartEntry, SKRect>();
            using (var paint = new SKPaint())
            {
                paint.TextSize = ValueLabelTextSize;
                foreach (var e in entries)
                {
                    SKRect bounds;
                    if (string.IsNullOrEmpty(e.ValueLabel))
                    {
                        bounds = SKRect.Empty;
                    }
                    else
                    {
                        bounds = new SKRect();
                        paint.MeasureText(e.ValueLabel, ref bounds);
                    }

                    dict.Add(e, bounds);
                }
            }

            return dict;
        }

        /// <summary>
        /// Draw bar (or point) item of an entry
        /// </summary>
        /// <param name="serie"></param>
        /// <param name="canvas"></param>
        /// <param name="headerHeight"></param>
        /// <param name="itemX"></param>
        /// <param name="itemSize"></param>
        /// <param name="barSize"></param>
        /// <param name="origin"></param>
        /// <param name="barX"></param>
        /// <param name="barY"></param>
        /// <param name="color"></param>
        protected abstract void DrawBar(ChartSerie serie, SKCanvas canvas, float headerHeight, float itemX, SKSize itemSize, SKSize barSize, float origin, float barX, float barY, SKColor color);

        /// <summary>
        /// Draw bar (or point) area of an entry
        /// </summary>
        /// <param name="canvas"></param>
        /// <param name="headerHeight"></param>
        /// <param name="itemSize"></param>
        /// <param name="barSize"></param>
        /// <param name="color"></param>
        /// <param name="origin"></param>
        /// <param name="value"></param>
        /// <param name="barX"></param>
        /// <param name="barY"></param>
        protected abstract void DrawBarArea(SKCanvas canvas, float headerHeight, SKSize itemSize, SKSize barSize, SKColor color, float origin, float value, float barX, float barY);

        protected SKSize CalculateBarSize(SKSize itemSize, int barPerItems)
        {
            var w = (itemSize.Width - ((barPerItems - 1) * Margin / 2)) / barPerItems;
            return new SKSize(w, itemSize.Height);
        }

        protected  SKSize CalculateItemSize(int items, int width, int height, float reservedSpace)
        {
            var w = (width - ((items + 1) * Margin)) / items;
            var h = height - Margin - reservedSpace;
            return new SKSize(w, h);
        }

        #endregion
    }
}