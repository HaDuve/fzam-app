using System;
using FZAM.lang;
using FZAM.Pages.Controls;
using Newtonsoft.Json;
using Xamarin.Forms;
using Xamarin.Forms.Internals;

namespace FZAM.Questions
{
    internal class TextareaQuestion : Question, ITextQuestion
    {
        private TextQuestionControl textBox;

        [Preserve(AllMembers = true)]
        [JsonConstructor]
        public TextareaQuestion(int id, string title, bool isKeyword = false, bool isTitle = false)
            : base(id, title)
        {
            this.IsKeyword = isKeyword;
            this.IsTitle = isTitle;
        }

        public string Value { get; set; }

        public override void Render(EventHandler nextPageCallback, StackLayout parentLayoutElement)
        {
            this.textBox = new TextQuestionControl(l10n.AnswerPlaceholder, this.Value);
            parentLayoutElement.Children.Add(this.textBox);
            this.textBox.OkButtonClicked += nextPageCallback;
            this.textBox.OkButtonClicked += (sender, e) => this.Value = this.textBox.Text;
        }

        public override string GetAnswer()
        {
            return this.Value;
        }
    }
}