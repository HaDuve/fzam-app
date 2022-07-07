using FZAM.Config;
using FZAM.lang;
using System;
using System.Collections.Generic;
using System.Globalization;
using FZAM.Pages.Controls;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace FZAM.Pages.ResultPages
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ResultsPageSingleInfo : ContentPage, IResultsPage
    {
        private readonly List<Configuration> availableResults;
        private readonly Database dataBase;
        private readonly Configuration selectedResult;

        public ResultsPageSingleInfo(Configuration selectedResult, Database dataBase)
        {
            NavigationPage.SetHasBackButton(this, true);
            ((NavigationPage)Application.Current.MainPage).BarBackgroundColor = LayoutElements.BarBackgroundColor();
            ((NavigationPage)Application.Current.MainPage).BarTextColor = LayoutElements.BarTextColor();

            this.dataBase = dataBase;
            this.selectedResult = selectedResult;
            this.availableResults = this.dataBase.GetSurveys();

            this.InitializeComponent();
            this.Render();
        }

        protected void Render()
        {
            this.PageTitel.Text = this.dataBase.GetResultTitle(this.selectedResult.SurveyId);
            this.PageDate.Text = string.Format(l10n.FilleOnDate, this.selectedResult.SurveyTime.ToString("dd.MM.yyyy"));

            this.RenderDataSection();
        }

        private string FormatFloat(float x)
        {
            return Math.Round(x, 1).ToString(CultureInfo.CurrentCulture);
        }

        public void RenderDataSection()
        {
            //TODO: ok, so for readabitlity I destroyed the original solution of generating everything form the jaml. This should be reintrduced in a clean way.
            var evalHandler = this.selectedResult.EvaluationHandler;
            var positivCoping = evalHandler.GetEvaluationAreaByTitle("Positives Coping");
            this.copingBefore.Text = this.FormatFloat(positivCoping.GetEvaluationObjectByName("Vor ").NumericValue);
            this.copingWhile.Text = this.FormatFloat(positivCoping.GetEvaluationObjectByName("Während ").NumericValue);
            this.copingAfter.Text = this.FormatFloat(positivCoping.GetEvaluationObjectByName("Nach ").NumericValue);

            var shakes = evalHandler.GetEvaluationAreaByTitle("Dysfunktionales Lampenfieber");
            this.shakesBefore.Text = this.FormatFloat(shakes.GetEvaluationObjectByName("Vor ").NumericValue);
            this.shakesWhile.Text = this.FormatFloat(shakes.GetEvaluationObjectByName("Während ").NumericValue);
            this.shakesAfter.Text = this.FormatFloat(shakes.GetEvaluationObjectByName("Nach ").NumericValue);

            var efficacy = evalHandler.GetEvaluationAreaByTitle("Selbstwirksamkeit");
            this.efficacyBefore.Text = this.FormatFloat(efficacy.GetEvaluationObjectByName("Vor ").NumericValue);
            this.efficacyWhile.Text = this.FormatFloat(efficacy.GetEvaluationObjectByName("Während ").NumericValue);
            this.efficacyAfter.Text = this.FormatFloat(efficacy.GetEvaluationObjectByName("Nach ").NumericValue);

            var urteilsSkala = evalHandler.GetEvaluationAreaByTitle("Urteilsskala");
            var judgement = urteilsSkala.GetEvaluationObjects()[0].NumericValue;
            this.overallResult.Text = RangeQuestionControl.GetResultToLabel(
                RangeQuestionControl.LabelType.Bad_to_Good, (int)Math.Round(judgement));

            var area = evalHandler.GetEvaluationAreaByTitle("Informationen zum Auftritt");
            this.personalRelevance.Text =
                RangeQuestionControl.GetResultToLabel(RangeQuestionControl.LabelType.Unimportant_to_Important,
                    (int)area.GetEvaluationObjectByName("Der Auftritt war für mich...").NumericValue);
            this.perfomanceComplexity.Text = RangeQuestionControl.GetResultToLabel(RangeQuestionControl.LabelType.Easy_to_Hard,
                (int)area.GetEvaluationObjectByName("Verglichen mit anderen Auftritten war dieser Auftritt...").NumericValue);
            this.performanceRequirements.Text = RangeQuestionControl.GetResultToLabel(RangeQuestionControl.LabelType.Low_to_High,
                (int)area.GetEvaluationObjectByName("Für mich persönlich waren die Anforderungen...").NumericValue);
            this.performerCount.Text = area.GetEvaluationObjectByName("Anzahl Musizierende").Value.Trim();
            this.viewerCount.Text = area.GetEvaluationObjectByName("Publikumsgröße").Value.Trim();
            this.keyWords.Text = area.GetEvaluationObjectByName("Stichwörter zum Auftritt").Value;
        }


        private async void InfoButton_Clicked(object sender, EventArgs e)
        {
            await Navigation.PopAsync();
        }

        public void DescriptionPopupTapRecognizer(object sender, EventArgs e)
        {
            this.DisplayAlert(l10n.DialogJudgmentalScaleTitle, l10n.DialogJudgmentalScale, l10n.ok);
        }

        private void BackButton_OnClicked(object sender, EventArgs e)
        {
            Navigation.PopAsync();
        }
    }
}