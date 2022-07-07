using System.Text.RegularExpressions;

namespace FZAM.Types
{
    public class Lang
    {
        private readonly string lang = "de";

        // empty constructor for use of default language
        public Lang()
        { }

        public Lang(string givenLang)
        {
            var regex = new Regex("^[a-z]{2}(_[A-Z]{2})?$");

            if (regex.IsMatch(givenLang))
            {
                this.lang = givenLang;
            }
        }

        public static string operator +(Lang lang, string s)
        {
            return lang.ToString() + s;
        }

        public static string operator +(string s, Lang lang)
        {
            return s + lang.ToString();
        }

        public override string ToString()
        {
            return this.lang;
        }
    }
}