using System;
using FZAM.lang;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace FZAM.Pages.Controls
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class TextAreaQuestionControl : ContentView
    {
        public TextAreaQuestionControl(string placeHolder)
        {
            this.InitializeComponent();
            this.TextField.Placeholder = placeHolder;
            this.TextField.Focus();
            this.OkButton.Text = l10n.NextButton;
        }

        public string Text
        {
            get => this.TextField.Text;
            set => this.TextField.Text = value;
        }

        public event EventHandler OkButtonClicked;

        public void acceptButtonClicked(object sender, EventArgs e)
        {
            if (this.OkButtonClicked != null) this.OkButtonClicked(sender, e);
        }
    }
}