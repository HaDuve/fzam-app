using FZAM.Questions;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Globalization;
using System.Text;
using Xamarin.Forms.Internals;

namespace FZAM.Results
{
    /// <summary>
    ///     Will evaluate numeric Expressions in prefix notation (wip)
    /// </summary>
    public abstract class EvaluatingExpression
    {
        protected readonly List<string> operands;
        protected Dictionary<string, IQuestion> environment;

        [JsonConstructor]
        protected EvaluatingExpression(List<string> operands)
        {
            this.operands = operands;
        }

        public void SetEnvironment(Dictionary<string, IQuestion> env)
        {
            this.environment = env;
        }

        public abstract float GetNumericResult();

        public abstract string GetResult();
    }

    public class EvaluatingAverageExpression : EvaluatingExpression
    {
        [Preserve(AllMembers = true)]
        [JsonConstructor]
        public EvaluatingAverageExpression(List<string> operands)
            : base(operands)
        { }

        public override string GetResult()
        {
            return this.GetNumericResult().ToString(CultureInfo.CurrentCulture);
        }

        public override float GetNumericResult()
        {
            float result = 0;
            int notApplicable = 0;
            foreach (var kvp in this.operands)
            {
                if (this.environment[kvp] is INumericalQuestion question)
                {
                    if (question.Value == 7)
                    {
                        notApplicable++;
                    }
                    else
                    {
                        result += question.Value;
                    }
                }
            }
            return result / (this.operands.Count - notApplicable);
        }
    }

    public class EvaluatingSingleValueExpression : EvaluatingExpression
    {
        [Preserve(AllMembers = true)]
        [JsonConstructor]
        public EvaluatingSingleValueExpression(List<string> operands)
            : base(operands)
        { }

        public override string GetResult()
        {
            StringBuilder sb = new StringBuilder();
            foreach (var op in this.operands)
            {
                sb.Append(this.environment[op].GetAnswer());
                sb.Append("\n");
            }

            return sb.ToString();
        }

        public override float GetNumericResult()
        {
            if (this.environment[this.operands[0]] is INumericalQuestion question)
            {
                return question.Value;
            }

            return float.NaN;
        }
    }
}