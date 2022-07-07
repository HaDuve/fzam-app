using System;
using System.Collections.Generic;
using FZAM.Config;
using FZAM.lang;
using FZAM.Pages.Controls;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace FZAM.Pages.ResultPages
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ResultsPageSingle : ContentPage, IResultsPage
    {
        private readonly List<Configuration> availableResults;

        private readonly Database dataBase;
        private Configuration selectedResult;


        public ResultsPageSingle(Configuration selectedResult, Database dataBase,
            bool comingFromQuestionnaire = false)
        {
            NavigationPage.SetHasBackButton(this, true);
            ((NavigationPage) Application.Current.MainPage).BarBackgroundColor = LayoutElements.BarBackgroundColor();
            ((NavigationPage) Application.Current.MainPage).BarTextColor = LayoutElements.BarTextColor();

            this.dataBase = dataBase;
            this.selectedResult = selectedResult;
            this.availableResults = this.dataBase.GetSurveys();
            this.ComeFromQuestionnaire = comingFromQuestionnaire;

            this.InitializeComponent();
            if (!this.ComeFromQuestionnaire) this.AddSwipeGesture();
            this.SetPopups();
            this.Title = "";
            this.Render();
        }

        public bool ComeFromQuestionnaire { get; }

        private int Pivot => this.availableResults.IndexOf(this.selectedResult);

        protected bool NextResultExists =>
            this.availableResults.Count > this.availableResults.IndexOf(this.selectedResult) + 1;

        protected bool PrevResultExists => this.availableResults.IndexOf(this.selectedResult) > 0;

        private void AddSwipeGesture()
        {
            var swipeLeft = new SwipeGestureRecognizer
            {
                Direction = SwipeDirection.Left
            };
            swipeLeft.Swiped += this.OnSwipedLeft;
            this.LayoutWrapper.GestureRecognizers.Add(swipeLeft);

            var swipeRight = new SwipeGestureRecognizer
            {
                Direction = SwipeDirection.Right
            };
            swipeRight.Swiped += this.OnSwipedRight;
            this.LayoutWrapper.GestureRecognizers.Add(swipeRight);
        }

        private void OnSwipedLeft(object sender, SwipedEventArgs e)
        {
            // weiter
            if (this.NextResultExists) this.ShowNextResult(sender, e);
        }

        private void OnSwipedRight(object sender, SwipedEventArgs e)
        {
            // zurück
            if (this.PrevResultExists) this.ShowPrevResult(sender, e);
        }

        private void ShowNextResult(object sender, EventArgs e)
        {
            Debounce.create(this.ShowNextResultAction, sender, e);
        }

        protected void ShowNextResultAction(object sender, EventArgs e)
        {
            this.selectedResult = this.availableResults[this.Pivot + 1];
            this.SetPopups();
            this.Render();
        }

        private void ShowPrevResult(object sender, EventArgs e)
        {
            Debounce.create(this.ShowPrevResultAction, sender, e);
        }

        protected void ShowPrevResultAction(object sender, EventArgs e)
        {
            this.selectedResult = this.availableResults[this.Pivot - 1];
            this.SetPopups();
            this.Render();
        }

        /// <summary>
        ///     Updates the dynamic means of scales presentation of the InfoPopups
        /// </summary>
        private void SetPopups()
        {
            var count = 0;
            foreach (var resultArea in this.selectedResult.EvaluationHandler.GetEvaluationAreas())
            {
                if (resultArea.Type != "chart") continue;

                var rowList = new List<float>();
                foreach (var result in resultArea.GetEvaluationObjects())
                {
                    count++;
                    rowList.Add((float) Math.Round(result.NumericValue, 1));
                    if (count == 3)
                    {
                        //TODO:this.mainMenu.SetAverageForPopup1(rowList);
                    }
                    else if (count == 6)
                    {
                        //TODO:this.mainMenu.SetAverageForPopup2(rowList);
                    }
                    else if (count == 9)
                    {
                        //TODO: this.mainMenu.SetAverageForPopup3(rowList);
                    }
                }
            }
        }

        protected async void Render()
        {
            if (this.availableResults.Count < 1)
            {
                await this.Navigation.PopAsync();
                await this.Navigation.PushAsync(new ErrorPage(ErrorPage.Type.QuestionnaireResultLoad));
                return;
            }

            this.PageTitel.Text = this.dataBase.GetResultTitle(this.selectedResult.SurveyId);
            this.PageDate.Text = string.Format(l10n.FilleOnDate, this.selectedResult.SurveyTime.ToString("dd.MM.yyyy"));
            this.EntryLabel.Text =
                string.Format(l10n.QuestionnaireNumbering, this.Pivot + 1, this.availableResults.Count);

            this.NextButton.IsEnabled = this.NextResultExists;
            this.PreviousButton.IsEnabled = this.PrevResultExists;
            this.NextButton.IsVisible = !this.ComeFromQuestionnaire;
            this.PreviousButton.IsVisible = !this.ComeFromQuestionnaire;
            this.SaveButton.IsVisible = this.ComeFromQuestionnaire;

            this.DataPanel.Children.Clear();
            var dataView = this.RenderDataSection();
            dataView.HorizontalOptions = LayoutOptions.FillAndExpand;
            dataView.VerticalOptions = LayoutOptions.FillAndExpand;
            this.DataPanel.Children.Add(dataView);
        }


        public View RenderDataSection()
        {
            var newMultiChartView = new ThreeValueChart();
            foreach (var resultArea in this.selectedResult.EvaluationHandler.GetEvaluationAreas())
            {
                if (resultArea.Type != "chart") continue;

                newMultiChartView.AddChart(resultArea.GetChart());
            }

            return newMultiChartView;
        }


        private void MoreInfoButtonClicked(object sender, EventArgs e)
        {
            Debounce.create(this.MoreInfoButtonAction, sender, e);
        }

        private async void MoreInfoButtonAction(object sender, EventArgs e)
        {
            await this.Navigation.PushAsync(new ResultsPageSingleInfo(this.selectedResult, this.dataBase));
        }

        private void saveReturnButton_Clicked(object sender, EventArgs e)
        {
            Debounce.create(this.SaveReturnButtonAction, sender, e);
        }

        private async void SaveReturnButtonAction(object sender, EventArgs e)
        {
            await this.Navigation.PopToRootAsync();
        }

        private void DeleteButton_Clicked(object sender, EventArgs e)
        {
            Debounce.create(this.DeleteButtonAction, sender, e);
        }

        private async void DeleteButtonAction(object sender, EventArgs e)
        {
            var answer = await this.DisplayAlert(l10n.ReallyDelete,
                l10n.DeleteInfoText + this.dataBase.GetResultTitle(this.selectedResult.SurveyId), l10n.Yes,
                l10n.No);
            if (!answer) return;

            this.dataBase.DeleteResultFromDb(this.selectedResult);
            await this.Navigation.PopToRootAsync();
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
            Navigation.PopAsync();
        }
    }
}