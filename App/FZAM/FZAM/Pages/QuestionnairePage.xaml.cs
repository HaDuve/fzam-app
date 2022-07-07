using System;
using System.Linq;
using System.Threading.Tasks;
using FZAM.Config;
using FZAM.lang;
using FZAM.Pages.ResultPages;
using FZAM.Questions;
using FZAM.Utils;
using Xamarin.Forms.Xaml;

namespace FZAM.Pages
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class QuestionnairePage
    {
        private readonly Database Database;
        protected Configuration Config;
        protected int CurrentQuestionId;
        protected bool IsFinished;
        protected int QuestionCounter = 1;

        public QuestionnairePage(Configuration givenConfig, Database database)
        {
            this.Config = givenConfig;
            this.Database = database;

            this.InitializeComponent();
            this.RenderCurrentPage();
        }

        private int CurrentQuestionnaireId { get; set; }

        public bool InProgress => (this.QuestionCounter > 1) & !this.IsFinished;

        protected Questionnaire Questionnaire => this.Config.GetQuestionnaire(this.CurrentQuestionnaireId);
        protected int QuestionnairesCount => this.Config.QuestionnaireCount();
        protected IQuestion Question => this.Questionnaire.GetQuestion(this.CurrentQuestionId);
        protected int QuestionnaireLength => this.Questionnaire.QuestionCount();

        protected int QuestionsCount =>
            this.Config.GetQuestionnaires().Sum(questionnaire => questionnaire.QuestionCount());

        protected override async void OnAppearing()
        {
            base.OnAppearing();
            if (Settings.FirstQuestionnaire)
            {
                Settings.FirstQuestionnaire = false;
                await this.DisplayFirstQuestionnairePopUp();
            }
        }

        protected void RenderCurrentPage()
        {
            this.contentWrapperLayout.Children.Clear();
            this.PageTitel.Text = this.Questionnaire.TextBefore;
            this.PageDate.Text = string.Format(l10n.QuestionnairePageTitleCount,
                this.QuestionCounter, this.QuestionsCount);
            this.QuestionTitle.Text = this.Question.Title;

            this.Question.Render(this.GotoNextPage, this.contentWrapperLayout);

            this.RenderBackgroundImages();
        }

        private void RenderBackgroundImages()
        {
            this.BackgroundImage0.IsVisible = false;
            this.BackgroundImage1.IsVisible = false;
            this.BackgroundImage2.IsVisible = false;
            this.BackgroundImage3.IsVisible = false;
            switch (this.CurrentQuestionnaireId)
            {
                case 0:
                    this.BackgroundImage0.IsVisible = true;
                    break;
                case 1:
                    this.BackgroundImage1.IsVisible = true;
                    break;
                case 2:
                    this.BackgroundImage2.IsVisible = true;
                    break;
                case 3:
                    this.BackgroundImage3.IsVisible = true;
                    break;
            }
        }

        private async void GotoNextPage(object sender, EventArgs e)
        {
            if (this.CurrentQuestionId < this.QuestionnaireLength - 1)
            {
                this.CurrentQuestionId++;
                this.QuestionCounter++;
            }
            else if (this.CurrentQuestionnaireId < this.QuestionnairesCount - 1)
            {
                this.CurrentQuestionnaireId++;
                this.CurrentQuestionId = 0;
                this.QuestionCounter++;
            }
            else
            {
                await this.SaveQuestionnaire();
                return;
            }

            this.RenderCurrentPage();
        }

        private void GotoPrevPage(object sender, EventArgs e)
        {
            if (this.CurrentQuestionId > 0)
            {
                this.QuestionCounter--;
                this.CurrentQuestionId--;
            }
            else if (this.CurrentQuestionnaireId > 0)
            {
                this.CurrentQuestionnaireId--;
                this.CurrentQuestionId = this.QuestionnaireLength - 1;
                this.QuestionCounter--;
            }
            else
            {
                this.Navigation.PopAsync();
            }

            this.RenderCurrentPage();
        }

        private async Task SaveQuestionnaire()
        {
            try
            {
                this.IsFinished = true;
                var resultsOfQuestionnaire = this.Database.SaveValues(this.Config);
                this.Navigation.InsertPageBefore(new ResultsPageSingle(resultsOfQuestionnaire, this.Database,
                        true),
                    this);
                await this.Navigation.PopAsync();
            }
            catch
            {
                await this.Navigation.PushAsync(new ErrorPage(ErrorPage.Type.QuestionnaireResultSave));
            }
        }

        private void PrevPageButton_Clicked(object sender, EventArgs e)
        {
            Debounce.create(this.GotoPrevPage, sender, e);
        }

        private async void OnShowInfoPopupButton(object sender, EventArgs e)
        {
            await this.DisplayFirstQuestionnairePopUp();
        }

        private async Task DisplayFirstQuestionnairePopUp()
        {
            await this.DisplayAlert(l10n.FirstQuestionnaireDialogeTitle, l10n.FirstQuestionnaireDialog, l10n.ok);
        }

        private void BackButton_OnClicked(object sender, EventArgs e)
        {
            Navigation.PopAsync();
        }
    }
}