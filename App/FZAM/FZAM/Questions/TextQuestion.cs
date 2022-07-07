using System;
using FZAM.lang;
using FZAM.Pages.Controls;
using Newtonsoft.Json;
using Xamarin.Forms;
using Xamarin.Forms.Internals;

namespace FZAM.Questions
{
    internal class TextQuestion : Question, ITextQuestion
    {
        public enum InputType
        {
            Text = 0,
            Int = 1
        }

        private readonly InputType inputType;
        private TextQuestionControl textBox;

        [Preserve(AllMembers = true)]
        [JsonConstructor]
        public TextQuestion(int id, string title, int inputType)
            : base(id, title)
        {
            this.inputType = (InputType) inputType;
        }

        public string Value { get; set; }

        public override void Render(EventHandler nextPageCallback, StackLayout parentLayoutElement)
        {
            this.textBox = new TextQuestionControl(l10n.AnswerPlaceholder, this.Value, this.inputType == InputType.Int);
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