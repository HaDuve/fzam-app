using System;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace FZAM.Pages.Controls
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class DescriptionButton : ContentView
    {
        public DescriptionButton()
        {
            InitializeComponent();
        }

        public string Title
        {
            get { return this.LabelTitle.Text; }
            set { this.LabelTitle.Text = value; }
        }

        public string Text
        {
            get { return this.LabelText.Text; }
            set { this.LabelText.Text = value; }
        }

        public string KeyWords
        {
            get { return this.LabelKeywords.Text; }
            set { this.LabelKeywords.Text = value; }
        }

        public void addButtonKlick(EventHandler v)
        {
            this.OkButton.Clicked += v;
        }
    }
}