using System;
using FZAM.Behaviors;
using FZAM.lang;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace FZAM.Pages.Controls
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class TextQuestionControl : ContentView
    {
        public TextQuestionControl(string placeHolder, string givenAnswer, bool isNumeric = false)
        {
            this.InitializeComponent();
            this.TextField.Placeholder = placeHolder;
            this.TextField.Text = givenAnswer;
            this.OkButton.IsEnabled = givenAnswer?.Length > 0;
            if (isNumeric)
            {
                this.TextField.Keyboard = Keyboard.Numeric;
                this.TextField.Behaviors.Add(new NumericValidationBehavior());
            }

            this.OkButton.Text = l10n.NextButton;
            this.TextField.Focus();
        }

        public string Text
        {
            get => this.TextField.Text;
            set => this.TextField.Text = value;
        }

        public event EventHandler OkButtonClicked;

        public void acceptButtonClicked(object sender, EventArgs e) 
        {
            this.OkButtonClicked?.Invoke(sender, e);
        }

        private void TextField_OnTextChanged(object sender, TextChangedEventArgs e)
        {
            this.OkButton.IsEnabled = this.TextField.Text?.Length > 0;
        }
    }
}