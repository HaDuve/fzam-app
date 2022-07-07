using System;
using FZAM.Pages.Controls;
using Newtonsoft.Json;
using Xamarin.Forms;
using Xamarin.Forms.Internals;

namespace FZAM.Questions
{
    internal class RangeQuestion : Question, INumericalQuestion
    {
        private readonly bool MaySkip;
        private readonly int Steps;
        private readonly int StartStep;

        private readonly RangeQuestionControl.LabelType usedLabel;

        [Preserve(AllMembers = true)]
        [JsonConstructor]
        public RangeQuestion(int id, string title, int start, int steps, int usedLabel, bool maySkip = false)
            : base(id, title)
        {
            this.usedLabel = (RangeQuestionControl.LabelType) usedLabel;
            this.Steps = steps;
            this.StartStep = start;
            this.MaySkip = maySkip;
        }

        [JsonIgnore] public int Value { get; set; } = -1;

        public override void Render(EventHandler nextPageCallback, StackLayout parentLayoutElement)
        {
            var buttonSet = new RangeQuestionControl(this.usedLabel, this.StartStep, this.Steps, this.Value, this.MaySkip);
            buttonSet.VerticalOptions = LayoutOptions.FillAndExpand;
            parentLayoutElement.Children.Add(buttonSet);
            buttonSet.ResultSelected += (sender, e) => this.Value = buttonSet.Result;
            buttonSet.ResultSelected += nextPageCallback;
        }

        public override string GetAnswer()
        {
            return this.Value.ToString();
        }

        public float GetAnswerNumerical()
        {
            return this.Value;
        }
    }
}