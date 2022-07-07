using FZAM.Config;
using FZAM.lang;
using FZAM.Utils;
using System;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace FZAM.Pages
{
    public sealed partial class MainPage : ContentPage
    {
        private readonly Database dataBase;
        private QuestionnairePage activeQuestionnairePage;

        public MainPage()
        {
            //TODO: do something if configuration is faulty
            /* var contentPage = this.Config.IsValid()
                ? (ContentPage)(this.mainPage = new MainPage(this.Config, this))
                : new ErrorPage(ErrorPage.Type.ConfigLoad, this.Config.ErrorMessage);

            return new NavigationPage(contentPage);*/
            this.dataBase = new Database();
            this.Config = new Configuration(this.dataBase);
            this.activeQuestionnairePage = new QuestionnairePage(this.Config, this.dataBase);

            this.InitializeComponent();
            this.CurtainAnimation();
        }

        private Configuration Config;

        private async void FirstLaunchDialog()
        {
            if (Settings.FirstLaunch)
            {
                Settings.FirstLaunch = false;
                await this.DisplayAlert(l10n.WelcomeMessageDialogeTitle, l10n.WelcomeMessageDialoge,
                    l10n.LiftTheCurtain);
            }
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
            if (!this.activeQuestionnairePage.InProgress) this.ResetButton.IsVisible = false;
        }

        public async void CurtainAnimation()
        {
            this.Vorhang.InputTransparent = false;
            // delay animation to prevent crashing
            await Task.Delay(1000);

            this.FirstLaunchDialog();

            for (var i = 0; i < 100; i++)
            {
                await Task.Delay(15);
                this.Vorhang.TranslationY -= (double)i / 4;
            }

            this.Vorhang.IsVisible = false;
        }

        private async void OnStartQuestionnaireButton(object sender, EventArgs e)
        {
            if (!this.activeQuestionnairePage.InProgress)
            {
                this.Config = new Configuration(this.dataBase);
                this.activeQuestionnairePage = new QuestionnairePage(this.Config, this.dataBase);
            }
            await this.Navigation.PushAsync(this.activeQuestionnairePage);
            this.ResetButton.IsVisible = true;
        }

        private void OnShowResultSelectionButton(object sender, EventArgs e)
        {
            Navigation.PushAsync(new ResultSelectionPage(this.dataBase));
        }

        private void OnShowInfoButton(object sender, EventArgs e)
        {
            Navigation.PushAsync(new InfoPage());
        }


        private async void OnResetQuestionnaireButton(object sender, EventArgs e)
        {
            if (await this.DisplayAlert(l10n.ReallyResetQuestionnaire, l10n.AllAnswersWillBeLost, l10n.Yes, l10n.No))
            {
                this.Config = new Configuration(this.dataBase);
                this.activeQuestionnairePage = new QuestionnairePage(this.Config, this.dataBase);
                this.ResetButton.IsVisible = false;
            }

        }
    }
}