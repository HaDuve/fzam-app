using Xamarin.Essentials;

namespace FZAM.Utils
{
    /**
     * 
     * A collection of boolean setters/getters to check  if various parts of the app are being launched for the first time.
     * 
     */
    public static class Settings
    {
        public static bool FirstLaunch
        {
            get => Preferences.Get(nameof(FirstLaunch), true);
            set => Preferences.Set(nameof(FirstLaunch), value);
        }
        public static bool FirstQuestionnaire
        {
            get => Preferences.Get(nameof(FirstQuestionnaire), true);
            set => Preferences.Set(nameof(FirstQuestionnaire), value);
        }

        public static bool FirstResults
        {
            get => Preferences.Get(nameof(FirstResults), true);
            set => Preferences.Set(nameof(FirstResults), value);
        }

    }
}
