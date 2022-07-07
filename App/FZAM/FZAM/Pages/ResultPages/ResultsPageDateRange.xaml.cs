using System;
using System.Collections.Generic;
using FZAM.Config;
using FZAM.lang;
using FZAM.Microcharts.Charts;
using Microcharts;
using Microcharts.Forms;
using SkiaSharp;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace FZAM.Pages.ResultPages
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ResultsPageDateRange : ContentPage, IResultsPage
    {
        private readonly DateTime endDate;
        private readonly List<Configuration> selectedResults;
        private readonly DateTime startDate;

        public ResultsPageDateRange(List<Configuration> configurations, Database dataBase, DateTime startDate,
            DateTime endDate)
        {
            NavigationPage.SetHasBackButton(this, true);
            ((NavigationPage) Application.Current.MainPage).BarBackgroundColor = LayoutElements.BarBackgroundColor();
            ((NavigationPage) Application.Current.MainPage).BarTextColor = LayoutElements.BarTextColor();
            this.startDate = startDate;
            this.endDate = endDate;

            this.selectedResults = configurations;
            this.InitializeComponent();
            this.Title = "";
            this.Render();
        }

        protected void Render()
        {
            this.PageTitel.Text = "Vergangene Auftritte";
            this.AverageOverText.Text = String.Format("Mittelwert über {0} ausgewählte Auftritte:",this.selectedResults.Count);
            this.PageDate.Text = string.Format(l10n.QuestionnaireDateRange, this.startDate.ToString("dd.MM.yyyy"),
                this.endDate.ToString("dd.MM.yyyy"));
            this.RenderMeanValues();
        }

        private void RenderMeanValues()
        {
            this.RenderAverageDiagram();
            this.RenderSingleValueDiagrams();
        }

        private void RenderAverageDiagram()
        {
            foreach (var areaKey in new List<string>
                {"Positives Coping", "Dysfunktionales Lampenfieber", "Selbstwirksamkeit"})
            {
                var entries = new List<ChartEntry>();
                foreach (var questionKey in new List<string> {"Vor ", "Während ", "Nach "})
                {
                    var value = this.AverageValueQuestion(areaKey, questionKey);
                    value.Label = questionKey;
                    entries.Add(value);
                }

                var chart = new CopingLineChart
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
                this.DataPanel.AddChart(chart);
            }
        }


        private ChartEntry AverageValueQuestion(string eaKey, string questionKey)
        {
            var sum = 0f;
            var count = 0;
            var color = SKColor.Empty;
            foreach (var conf in this.selectedResults)
            {
                var evaluationArea = conf.EvaluationHandler.GetEvaluationAreaByTitle(eaKey);
                var evaluation = evaluationArea.GetEvaluationObjectByName(questionKey);
                sum += evaluation.NumericValue;
                count++;
                color = evaluationArea.ChartColor;
            }

            return new ChartEntry(sum / count) {Color = color};
        }


        private void RenderSingleValueDiagrams()
        {
            foreach (var areaKey in new List<string>
                {"Positives Coping", "Dysfunktionales Lampenfieber", "Selbstwirksamkeit"})
            {
                foreach (var questionKey in new List<string> {"Vor ", "Während ", "Nach "})
                {
                    this.barChartContainer.Children.Add(new Label
                    {
                        Text = String.Format("{0} ({1} dem Auftritt)", areaKey, questionKey.Trim()),
                        FontSize = 14,
                        HorizontalOptions = LayoutOptions.Start
                    });
                    var chartView = new ChartView
                    {
                        HeightRequest = 300,
                        HorizontalOptions = LayoutOptions.FillAndExpand,
                    };
                    chartView.Chart = this.GetHistoryChatPoints(areaKey, questionKey, SKColor.Empty);
                    this.barChartContainer.Children.Add(chartView);
                }
            }
        }

        private BarChart GetHistoryChatPoints(string eaKey, string questionKey, SKColor color)
        {
            var entries = new List<ChartEntry>();
            foreach (var result in this.selectedResults)
            {
                var evalHandler = result.EvaluationHandler;
                var evalArea = evalHandler.GetEvaluationAreaByTitle(eaKey);
                var chartColor = evalArea.ChartColor;
                var e = evalArea.GetEvaluationObjectByName(questionKey).NumericValue;
                entries.Add(new ChartEntry(e)
                    {
                        Color = chartColor,
                        ValueLabel =  result.SurveyTime.ToString("dd.MM.yy"),
                    }
                );
            }

            return new CopingBarChart
            {
                Entries = entries,
                BackgroundColor = SKColors.Transparent,
                AnimationDuration = TimeSpan.Zero,
                IsAnimated = false,
                BarAreaAlpha = 0,
                MaxValue = 5.5f, //keep the max and min value as this to render full boxes in result view
                MinValue = .5f,
                DrawBackground = true,
                ValueLabelOption = ValueLabelOption.TopOfChart
            };
        }

        private async void OnShowInfoPopupButton(object sender, EventArgs e)
        {
            await this.DisplayAlert(l10n.DialogResultPageInfoTitel, l10n.DialogResultPageInfo, l10n.ok);
        }

        private async void OnShowFunctionalesCopingLabel(object sender, EventArgs e)
        {
            await this.DisplayAlert(l10n.PositiveCopingTitle, l10n.PositiveCopingText, l10n.ok);
        }

        private async void OnShowDysfunktionalesLampenfieber(object sender, EventArgs e)
        {
            await this.DisplayAlert(l10n.DysfunctionalStageFrightTitle, l10n.DysfunctionalStageFrightText, l10n.ok);
        }

        private async void OnShowSelbstwirksamkeit(object sender, EventArgs e)
        {
            await this.DisplayAlert(l10n.SelfAssesmentTitle, l10n.SelfAssesmentText, l10n.ok);
        }

        private void BackButton_OnClicked(object sender, EventArgs e)
        {
            this.Navigation.PopAsync();
        }
    }
}