using System;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace FZAM.Pages.Controls
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class LabeledLabel : ContentView
    {
        public LabeledLabel()
        {
            InitializeComponent();
        }

        public string Title
        {
            get => LabelTitle.Text;
            set => LabelTitle.Text = value;
        }

        public FontAttributes TitleAttributes
        {
            get => LabelTitle.FontAttributes;
            set => LabelTitle.FontAttributes = value;
        }

        public string Text
        {
            get => LabelText.Text;
            set => LabelText.Text = value;
        }

        public bool HasInfo
        {
            get => InfoButton.IsEnabled;
            set
            {
                InfoButton.IsEnabled = value;
                InfoButton.IsVisible = value;
            }
        }

        public event EventHandler InfoButtonClicked;

        private void InfoClick(object sender, EventArgs e)
        {
            if (InfoButtonClicked != null) InfoButtonClicked(this, EventArgs.Empty);
        }
    }
}