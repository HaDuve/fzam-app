using Xamarin.Essentials;
using Xamarin.Forms;

namespace FZAM.Utils
/**
 * 
 * A Util for building Hyperlinks into app text without having to repeat boilerplate code each time.
 * 
 */

{
    public class HyperlinkSpan : Span
    {

        public static readonly BindableProperty UrlProperty =
            BindableProperty.Create(nameof(Url), typeof(string), typeof(HyperlinkSpan), null);

        public string Url
        {
            get => (string)GetValue(UrlProperty);
            set => SetValue(UrlProperty, value);
        }


        public HyperlinkSpan()
        {

            TextDecorations = TextDecorations.Underline;
            TextColor = Color.Blue;
            GestureRecognizers.Add(new TapGestureRecognizer
            {
                Command = new Command(async () =>
                    await Browser.OpenAsync(Url, BrowserLaunchMode.SystemPreferred))

            });

        }
    }
}
