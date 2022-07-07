using FZAM.Questions;
using FZAM.Results;
using FZAM.Types;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Globalization;

namespace FZAM.Config
{
    /// <summary>
    ///     A Configuration object encapsulates a set of questionnaires from one complete question sheet
    ///     together with its Evaluation object, version and langauge.
    ///     TODO: remove database connection from this! or move Questionnairs and Evaluation into a storage object below.
    /// </summary>
    public class Configuration
    {
        private const string DEFAULT_VERSION = "0.6";

        private static readonly Dictionary<int, Configuration> cache = new Dictionary<int, Configuration>();
        private readonly Database dataBase;
        private readonly bool loadingFailed;
        private readonly List<Questionnaire> questionnaires = new List<Questionnaire>();

        /// <summary>
        ///     Gets you a fresh questionnaire (no questions filled in)  with default langauge.
        /// </summary>
        /// <param name="dataBase"></param>
        public Configuration(Database dataBase)
        {
            this.Version = DEFAULT_VERSION;
            this.dataBase = dataBase;
            this.Lang = CultureInfo.CurrentCulture.TwoLetterISOLanguageName;
            this.SurveyTime = DateTime.Now;
            this.SurveyId = -1; //hence invalid db id
            try
            {
                //As this Configuration object will be thrown away anyways before calculating anything, only load the result things
                this.questionnaires = this.JsonLoad<List<Questionnaire>>("questionnaires", this.Lang);
            }
            catch (Exception e)
            {
                //TODO: give some information to the outside world
                this.loadingFailed = true;
                this.ErrorMessage = e.Message;
            }
        }

        /// <summary>
        /// Load questionnair with given properties from database.
        /// <note>Use GetConfiguration method as it uses caching for database access and deserialisation.</note>
        /// </summary>
        private Configuration(Database dataBase, int surveyId, string lang, string version, DateTime time) :
            this(dataBase)
        {
            this.Version = version;
            this.Lang = lang;
            this.SurveyId = surveyId;
            this.SurveyTime = time;
            this.dataBase.LoadResultValues(this);
            try
            {
                //Here we have results so, also load the result areas
                var evaluationAreas = this.JsonLoad<List<EvaluationArea>>("ResultAreas", this.Lang);
                this.EvaluationHandler = new EvaluationHandler(evaluationAreas, this.GetQuestionnaires());
            }
            catch (Exception e)
            {
                //TODO: give some information to the outside world
                this.loadingFailed = true;
                this.ErrorMessage = e.Message;
            }
        }

        public string ErrorMessage { get; } = "";

        public string Lang { get; }
        public string Version { get; }

        public EvaluationHandler EvaluationHandler { get; }
        public DateTime SurveyTime { get; }

        public int SurveyId { get; }

        public static Configuration GetConfiguration(Database dataBase, int surveyId, string lang, string version,
            DateTime time)
        {
            if (cache.ContainsKey(surveyId))
            {
                return cache[surveyId];
            }

            var tmp = new Configuration(dataBase, surveyId, lang, version, time);
            cache.Add(surveyId, tmp);
            return tmp;
        }


        public T JsonLoad<T>(string key, string lang)
        {
            var configFile = new ConfigFile();
            var cs = configFile.GetConfigStream(this.Version, lang);
            var jo = JObject.Parse(cs);
            //TODO: this is not nice but will suffice to test new configuration handling: pull objects from json nicer 
            var kes = jo[key].ToString();
            // create config object
            var configuration = JsonConvert.DeserializeObject<T>(kes, new JsonSerializerSettings
            {
                TypeNameHandling = TypeNameHandling.Auto
            });
            return configuration;
        }

        public bool IsValid()
        {
            return !this.loadingFailed;
        }

        public Questionnaire GetQuestionnaire(int i)
        {
            return this.questionnaires[i];
        }

        public int QuestionnaireCount()
        {
            return this.questionnaires.Count;
        }

        public List<Questionnaire> GetQuestionnaires()
        {
            return this.questionnaires;
        }

        public override int GetHashCode()
        {
            return this.SurveyId ^ (int) this.SurveyTime.Ticks;
        }
    }
}