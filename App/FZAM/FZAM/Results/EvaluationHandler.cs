using FZAM.Questions;
using System.Collections.Generic;

namespace FZAM.Results
{
    /**
     * 
     *  The handler takes a list of ResultArea Objects and a list of questionnaire objects.
     *  GetExpressionEnvironment(surveys) builds the env variable by iterating over the surveys, generating a prefix corresponding to the survey and mapping it to the questions.
     *  Env is populated by the method in this way as a Dictionary of kvps in the following form: {survey_id: question}
     * 
     */
    public class EvaluationHandler
    {
        private readonly List<EvaluationArea> ResultAreas = new List<EvaluationArea>();

        public EvaluationHandler(List<EvaluationArea> resultAreas, List<Questionnaire> surveys)
        {
            this.ResultAreas = resultAreas;
            var env = this.GetExpressionEnvironment(surveys);
            this.CalculateResults(env);
        }

        protected void CalculateResults(Dictionary<string, IQuestion> env)
        {
            foreach (var resultArea in this.ResultAreas)
            {
                resultArea.CalculateResults(env);
            }
        }

        private Dictionary<string, IQuestion> GetExpressionEnvironment(List<Questionnaire> surveys)
        {
            //TODO: for performance reasons, this thing could be moved up into the configuration
            var env = new Dictionary<string, IQuestion>();
            for (var sid = 0; sid < surveys.Count; sid++)
            {
                var variablePrefix = "q" + sid + "_";
                var questions = surveys[sid].GetQuestions();

                for (var quid = 0; quid < questions.Count; quid++)
                {
                    var variableName = variablePrefix + quid;

                    env[variableName] = questions[quid];

                }
            }

            return env;
        }

        public List<EvaluationArea> GetEvaluationAreas()
        {
            return this.ResultAreas;
        }

        public EvaluationArea GetEvaluationAreaByTitle(string searchTitle)
        {
            return this.ResultAreas.Find((x) => x.Title == searchTitle);
        }
    }
}