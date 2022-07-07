using System.IO;
using System.Reflection;

namespace FZAM.Types
{
    public class ConfigFile
    {
        private const string DEFAULT_CONFIG_LANGUAGE = "de";

        public string Language { get; private set; }

        public string GetConfigStream(string version, string lang)
        {
            return this.TryGetConfigFile(lang, version);
        }

        private string TryGetConfigFile(string lang, string version)
        {
            try
            {
                var fileName = "FZAM.Config." + lang + ".config." + version + ".json";
                if (!File.Exists(fileName))
                {
                    fileName = "FZAM.Config." + DEFAULT_CONFIG_LANGUAGE + ".config." + version + ".json";
                }
                using (var configAsJsonStream = Assembly.GetExecutingAssembly()
                    .GetManifestResourceStream(fileName))
                {
                    this.Language = lang;
                    return new StreamReader(configAsJsonStream).ReadToEnd();
                }
            }
            catch
            {
                lang = DEFAULT_CONFIG_LANGUAGE;
                using (var configAsJsonStream = Assembly.GetExecutingAssembly()
                    .GetManifestResourceStream("FZAM.Config." + lang + ".config." + version + ".json"))
                {
                    this.Language = lang;
                    return new StreamReader(configAsJsonStream).ReadToEnd();
                }
            }
        }
    }
}