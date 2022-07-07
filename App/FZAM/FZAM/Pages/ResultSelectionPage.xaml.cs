using System;
using System.Collections.Generic;
using System.Linq;
using FZAM.Config;
using FZAM.lang;
using FZAM.Pages.Controls;
using FZAM.Pages.ResultPages;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace FZAM.Pages
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public sealed partial class ResultSelectionPage : ContentPage
    {
        private readonly Database dataBase;

        private List<Configuration> availableResults;

        public ResultSelectionPage(Database dataBase)
        {
            this.InitializeComponent();
            this.dataBase = dataBase;
            this.availableResults = this.dataBase.GetSurveys();
            if(this.availableResults.Count > 0) this.StartDatePicker.Date = this.availableResults.Last().SurveyTime;
            this.RenderDataList(this.availableResults);
        }

        private void DatePicker_DateSpecified(object sender, DateChangedEventArgs e)
        {
            var availableRes = this.dataBase.GetSurveys(v =>
                v.Time >= this.StartDatePicker.Date &&
                v.Time <= this.EndDatePicker.Date.Add(TimeSpan.FromDays(1)));
            this.showDataRange.IsEnabled = availableRes.Count > 0;
            this.RenderDataList(availableRes);
        }

        private void RenderDataList(IEnumerable<Configuration> surveys)
        {
            this.ListWrapper.Children.Clear();
            foreach (var survey in surveys)
            {
                //Truncate long Strings
                var title = this.dataBase.GetResultTitle(survey.SurveyId);
                var date = survey.SurveyTime.ToString("dd.MM.yyyy");
                var keyWords = this.dataBase.GetResultKeywords(survey.SurveyId);

                var entry = new DescriptionButton {Title = date, Text = title, KeyWords = keyWords};
                entry.addButtonKlick((sender, e) => this.ShowResultsPageFromQuestionnaire(survey));
                this.ListWrapper.Children.Add(entry);
            }
        }

        public void ShowResultsPageFromQuestionnaire(Configuration resultConfig)
        {
            if (this.dataBase.LoadResultValues(resultConfig))
                this.Navigation.PushAsync(new ResultsPageSingle(resultConfig, this.dataBase));
            else
                this.Navigation.PushAsync(new ErrorPage(ErrorPage.Type.QuestionnaireResultLoad));
        }

        private void SearchBar_SearchButtonPressed(object sender, EventArgs e)
        {
            var keyword = ((SearchBar) sender).Text;
            this.availableResults = this.dataBase.GetSurveys(q => q.Keywords.Contains(keyword.ToLower()));
            this.RenderDataList(this.availableResults);
        }

        private async void SelectByDateButton_Clicked(object sender, EventArgs e)
        {
            var startDate = this.StartDatePicker.Date;
            var endDate = this.EndDatePicker.Date.Add(TimeSpan.FromDays(1));
            var dateSelectedResults = this.dataBase.GetSurveys(v => v.Time >= startDate && v.Time <= endDate);
            if (dateSelectedResults.Count > 1)
            {
                await this.Navigation.PushAsync(
                    new ResultsPageDateRange(dateSelectedResults, this.dataBase, startDate, endDate));
            } else if (dateSelectedResults.Count == 1)
            {
                await this.Navigation.PushAsync(
                    new ResultsPageSingle(dateSelectedResults.First(), this.dataBase));
            }
        }

        private void OnShowInfoPopupButton(object sender, EventArgs e)
        {
            this.DisplayAlert(l10n.DialogResultListPageTitle, l10n.DialogResultListPage, l10n.ok);
        }


        private void BackButton_OnClicked(object sender, EventArgs e)
        {
            this.Navigation.PopAsync();
        }
    }
}