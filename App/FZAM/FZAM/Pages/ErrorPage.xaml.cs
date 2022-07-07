using FZAM.lang;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace FZAM.Pages
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ErrorPage : ContentPage
    {
        public enum Type
        {
            General,
            ConfigLoad,
            QuestionnaireResultSave,
            QuestionnaireResultLoad
        }

        public ErrorPage() : this(Type.General)
        { }

        public ErrorPage(Type type, string details) : this(type)
        {
            this.message.Text += "\n" + details;
        }

        public ErrorPage(Type type)
        {
            this.Title = l10n.ErrorPageTitle;

            NavigationPage.SetHasBackButton(this, true);

            this.InitializeComponent();

            switch (type)
            {
                case Type.General:
                    this.message.Text = l10n.GenearlError;
                    break;
                case Type.ConfigLoad:
                    this.message.Text = l10n.ConfigLoadError;
                    // TODO: add language selection dialog
                    break;
                case Type.QuestionnaireResultSave:
                    this.message.Text = l10n.QuestionnaireResultSaveError;
                    break;
                case Type.QuestionnaireResultLoad:
                    this.message.Text = l10n.QuestionnaireResultLoadError;
                    break;
            }
        }
    }
}