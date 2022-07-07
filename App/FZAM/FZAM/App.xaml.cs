using FZAM.lang;
using System.Globalization;
using Xamarin.Essentials;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

[assembly: XamlCompilation(XamlCompilationOptions.Compile)]
namespace FZAM
{
    public partial class App
    {

        public App()
        {
            // TODO: keep german translation as only option for now, maybe change this later
            l10n.Culture = new CultureInfo("de-DE");

            this.InitializeComponent();

            var menu = new Pages.MainPage();
            this.MainPage = new NavigationPage(menu);

            VersionTracking.Track();
        }


        protected override void OnStart()
        {
            // Handle when your app starts
        }

        protected override void OnSleep()
        {
            // Handle when your app sleeps
        }

        protected override void OnResume()
        {
            // Handle when your app resumes
        }
    }
}