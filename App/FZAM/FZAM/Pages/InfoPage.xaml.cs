using FZAM.lang;
using Xamarin.Essentials;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace FZAM.Pages
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public sealed partial class InfoPage : ContentPage
    {
        public InfoPage()
        {
            this.InitializeComponent();
            NavigationPage.SetHasBackButton(this, true);
            ((NavigationPage)Application.Current.MainPage).BarBackgroundColor = LayoutElements.BarBackgroundColor();
            ((NavigationPage)Application.Current.MainPage).BarTextColor = LayoutElements.BarTextColor();

            this.Title = l10n.InfoPageTitle;
            this.PageLabel.Text =
                "Stage:Cool  – die Auftritts - App für MusikerInnen –  stellt Ihnen Fragen zur Verfügung, " +
                "die Sie im Anschluss an jeden Auftritt ausfüllen können. \r\n\r\nMit Hilfe der Auftritts-App können" +
                " Sie mehr dazu erfahren, wie Sie mit Lampenfieber umgehen, wieviel Sie sich zutrauen und wie " +
                "Sie Ihre musikalische Qualität beim Auftritt beurteilen. Dabei können Sie Ihr Erleben vor, " +
                "während und nach dem Auftritt vergleichen und analysieren. \r\n\r\nSie können sowohl einzelne Auftritte" +
                " durch die App auswerten lassen als auch Durchschnittswerte über mehrere Auftritte erhalten. Diese " +
                "Rückmeldungen sollen Ihnen helfen, eine realistische Selbsteinschätzung zu entwickeln, wo Ihre " +
                "Stärken und Schwächen beim Auftreten liegen. \r\n";

            // Getting the Screen Height/Width
            var width = DeviceDisplay.MainDisplayInfo.Width;
            var height = DeviceDisplay.MainDisplayInfo.Height;
            //Scaling up for Big Screens (Ipads)
            if (Device.Idiom == TargetIdiom.Tablet)
            {
                this.PageLabel.FontSize = 27;
                this.HyperlinkLeadingSpan.FontSize = 27;
                this.HyperlinkSpanFIM.FontSize = 27;
                this.HyperlinkMiddleSpan.FontSize = 27;
                this.HyperlinkSpanDataProtection.FontSize = 27;
                this.HyperlinkClosingSpan.FontSize = 27;
                // LogoImage.HeightRequest = 974;
                // LogoImage.WidthRequest = 375;

            }
            //ImageScaling for every Screen (but the smallest)
            if (height > 1400 && width > 600)
            {

                PageContentWrapper.VerticalOptions = LayoutOptions.StartAndExpand;
                // LogoImage.HeightRequest = 377;
                // LogoImage.WidthRequest = 180;
                LogoImage.Margin = new Thickness(0, 50, 0, 0);
            }


        }

    }
}