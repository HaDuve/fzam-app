using System.Collections.Generic;
using FZAM.Microcharts.Charts;
using FZAM.Questions;
using Microcharts;
using Newtonsoft.Json;
using SkiaSharp;
using Xamarin.Forms.Internals;

namespace FZAM.Results
{
    public class EvaluationArea
    {
        private readonly string chartColorString;
        private readonly List<Evaluation> results;

        [Preserve(AllMembers = true)]
        [JsonConstructor]
        public EvaluationArea(string title, string type,
            string chartColorString, List<Evaluation> results)
        {
            this.Title = title;
            this.Type = type;
            this.chartColorString = chartColorString;
            this.results = results;
        }

        public string Title { get; protected set; }
        public string Type { get; }
        public SKColor ChartColor => SKColor.Parse(this.chartColorString);

        public void CalculateResults(Dictionary<string, IQuestion> env)
        {
            foreach (var result in this.results) result.EvaluateExpression(env);
        }

        public List<Evaluation> GetEvaluationObjects()
        {
            return this.results;
        }

        public Evaluation GetEvaluationObjectByName(string searchName)
        {
            return this.results.Find(x => x.Title == searchName);
        }

        public CopingLineChart GetChart()
        {
            var entries = new List<ChartEntry>();

            foreach (var result in this.results)
            {
                var chartEntry = new ChartEntry(result.NumericValue)
                {
                    Label = result.Title,
                    Color = this.ChartColor
                };
                entries.Add(chartEntry);
            }

            return new CopingLineChart
            {
                Entries = entries,
                LineMode = LineMode.Straight,
                BackgroundColor = SKColors.Transparent,
                //DottedLine = true,
                PointSize = 20f,
                LegendOption = SeriesLegendOption.None,
                LabelOrientation = Orientation.Horizontal,
                ValueLabelOption = ValueLabelOption.OverElement,
                MaxValue = 5.5f, //keep the max and min value as this to render full boxes in result view
                MinValue = .5f,
                LabelTextSize = 20,
                LineAreaAlpha = 0,
                DottedLine = true
            };
        }
    }
}