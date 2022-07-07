using System.Collections.Generic;
using FZAM.Questions;
using Newtonsoft.Json;
using Xamarin.Forms.Internals;

namespace FZAM.Results
{
    public class Evaluation
    {
        protected EvaluatingExpression Expression;

        [Preserve(AllMembers = true)]
        [JsonConstructor]
        public Evaluation(string title, EvaluatingExpression expression)
        {
            this.Title = title;
            this.Expression = expression;
        }

        public string Value => this.Expression.GetResult();
        public float NumericValue => this.Expression.GetNumericResult();

        public string Title { get; protected set; }

        public void EvaluateExpression(Dictionary<string, IQuestion> env)
        {
            this.Expression.SetEnvironment(env);
        }
    }
}