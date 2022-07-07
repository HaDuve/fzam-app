using FZAM.Questions;
using SQLite;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;

namespace FZAM.Config
{
    public class Database
    {
        private const string DB_FILE_NAME = "FZAM.db";

        private readonly SQLiteConnection databaseConnection;

        public Database()
        {
            //TODO: error handling
            var dbFolder = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData);
            var dbFilePath = Path.Combine(dbFolder, DB_FILE_NAME);
            this.databaseConnection = new SQLiteConnection(dbFilePath);

            this.CreateTables();
        }

        /// <summary>
        ///     Creates Database Tables for the Questionnairs and single Answers
        ///     <note>The SQLite Library does an implicit "if exists" check so calling this code every time is not problematic</note>
        /// </summary>
        private void CreateTables()
        {
            //this.databaseConnection.DropTable<QuestionGroupTable>();
            //this.databaseConnection.DropTable<Answer>();
            this.databaseConnection.CreateTable<QuestionGroupTable>();
            this.databaseConnection.CreateTable<Answer>();
        }

        private void Insert(object table)
        {
            if (this.databaseConnection.Insert(table) == 0)
            //TODO: custom exception, catch at appropriate place
            {
                throw new Exception("Database Insert did not work");
            }
        }

        public Configuration SaveValues(Configuration config)
        {
            var title = "No Title";
            StringBuilder keywords = new StringBuilder();
            foreach (var questionnaire in config.GetQuestionnaires())
            {
                foreach (var question in questionnaire.GetQuestions())
                {
                    if (question.IsTitle)
                    {
                        title = question.GetAnswer();
                    }
                    if (question.IsKeyword)
                    {
                        keywords.Append(question.GetAnswer());
                    }
                }
            }
            var resultsTable = new QuestionGroupTable(config.Lang, config.Version, title, keywords.ToString().ToLower());
            this.Insert(resultsTable);
            var surveys = config.GetQuestionnaires();
            for (var sid = 0; sid < surveys.Count; sid++)
            {
                var questions = surveys[sid].GetQuestions();
                for (var quid = 0; quid < questions.Count; quid++)
                {
                    this.Insert(new Answer(resultsTable, sid, quid, questions[quid]));
                }
            }

            //this is the easiest way to get a clean Configuration object
            return Configuration.GetConfiguration(this, resultsTable.ResultId, config.Lang, config.Version,
                config.SurveyTime);
        }

        public List<Configuration> GetSurveys()
        {
            var results = new List<Configuration>();
            var resultTables = this.databaseConnection.Table<QuestionGroupTable>().OrderByDescending(v => v.Time);
            foreach (var resultTable in resultTables)
            {
                var result = Configuration.GetConfiguration(this, resultTable.ResultId, resultTable.Lang,
                    resultTable.Version, resultTable.Time);
                results.Add(result);
            }

            return results;
        }

        public List<Configuration> GetSurveys(Func<QuestionGroupTable, bool> condition)
        {
            var results = new List<Configuration>();
            var resultTables = this.databaseConnection.Table<QuestionGroupTable>().Where(condition)
                .OrderByDescending(v => v.Time);
            foreach (var resultTable in resultTables)
            {
                var result = Configuration.GetConfiguration(this, resultTable.ResultId, resultTable.Lang,
                    resultTable.Version, resultTable.Time);
                results.Add(result);
            }

            return results;
        }

        protected TableQuery<Answer> GetQuestionsTables()
        {
            return this.databaseConnection.Table<Answer>();
        }

        public bool LoadResultValues(Configuration config)
        {
            var answersPerQuestionGroup = this.databaseConnection.Table<Answer>()
                .Where(qt => qt.ResultId.Equals(config.SurveyId)).GroupBy(qt => qt.QuestionnaireId);
            //TODO: seperate queriing of the database vs. filling of a config... this entanlgement is not good for error handling
            //TODO: add error handling
            foreach (var answersPerGroup in answersPerQuestionGroup)
            {
                var questionnaire = config.GetQuestionnaire(answersPerGroup.Key);
                if (!questionnaire.IsValid())
                {
                    continue;
                }

                foreach (var answer in answersPerGroup)
                {
                    var question = questionnaire.GetQuestion(answer.QuestionId);
                    if (question is ITextQuestion)
                    {
                        ((ITextQuestion)question).Value = answer.Value;
                    }
                    else if (question is INumericalQuestion)
                    {
                        try
                        {
                            ((INumericalQuestion)question).Value = int.Parse(answer.Value);
                        }
                        catch (Exception e)
                        {
                            ((INumericalQuestion)question).Value = 0;
                            Debug.WriteLine("Exception: " + e);
                        }
                    }
                    else
                    {
                        throw new Exception("Type not supported");
                    }
                }
            }

            return true;
        }

        public string GetResultTitle(int resultId)
        {
            //TODO: this is ... unsuspected... we should find a generic solution, this is against the whole idea of "independend" questionnaire definition files
            var resultQuery = this.GetQuestionsTables()
                .Where(v => v.ResultId == resultId && v.QuestionnaireId == 4 && v.QuestionId == 2);
            return resultQuery.FirstOrDefault()?.Value ?? "Description not found!";
        }

        public string GetResultKeywords(int resultId)
        {
            //TODO: this is ... unsuspected... we should find a generic solution, this is against the whole idea of "independend" questionnaire definition files
            var resultQuery = this.GetQuestionsTables()
                .Where(v => v.ResultId == resultId && v.QuestionnaireId == 4 && v.QuestionId == 3);
            return resultQuery.FirstOrDefault()?.Value ?? "Description not found!";
        }

        public void DeleteResultFromDb(Configuration config)
        {
            this.databaseConnection.Table<QuestionGroupTable>().Where(v => v.ResultId == config.SurveyId).Delete();
        }

        [Table("Results")]
        public class QuestionGroupTable
        {
            public QuestionGroupTable()
            { }

            public QuestionGroupTable(string lang, string version, string title, string keywords)
            {
                this.Time = DateTime.Now;
                this.Lang = lang;
                this.Version = version;
                this.Keywords = keywords;
                this.Title = title;
            }

            [PrimaryKey] [AutoIncrement] public int ResultId { get; set; }

            public DateTime Time { get; set; }

            public string Title { get; set; }

            public string Keywords { get; set; }

            [MaxLength(5)] public string Lang { get; set; }

            [MaxLength(5)] public string Version { get; set; }
        }

        [Table("Questions")]
        public class Answer
        {
            public Answer()
            { }

            public Answer(QuestionGroupTable questionGroupTable, int questionnaireId, int questionId,
                IQuestion question)
            {
                this.ResultId = questionGroupTable.ResultId;
                this.QuestionnaireId = questionnaireId;
                this.QuestionId = questionId;
                this.Value = question.GetAnswer();
            }

            public int ResultId { get; set; }
            public int QuestionnaireId { get; set; }
            public int QuestionId { get; set; }

            [MaxLength(256)] public string Value { get; set; }
        }
    }
}