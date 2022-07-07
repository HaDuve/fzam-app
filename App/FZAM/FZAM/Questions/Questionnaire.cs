using Newtonsoft.Json;
using System.Collections.Generic;
using Xamarin.Forms.Internals;

namespace FZAM.Questions
{
    public class Questionnaire
    {
        private readonly List<IQuestion> Questions;

        [Preserve(AllMembers = true)]
        [JsonConstructor]
        public Questionnaire(string TextBefore, string ButtonTextBefore, List<IQuestion> Questions)
        {
            this.TextBefore = TextBefore;
            this.ButtonTextBefore = ButtonTextBefore;
            this.Questions = Questions;
        }

        public string TextBefore { get; }
        public string ButtonTextBefore { get; }
        public int Id { get; protected set; }

        public IQuestion GetQuestion(int i)
        {
            return this.Questions[i];
        }

        public int QuestionCount()
        {
            return this.Questions.Count;
        }

        public List<IQuestion> GetQuestions()
        {
            return this.Questions;
        }

        public bool IsValid()
        {
            return this.Questions.Count > 0;
        }
    }
}