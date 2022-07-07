using System;
using System.Collections.Generic;
using FZAM.lang;
using Xamarin.Forms;
using Xamarin.Forms.Internals;
using Xamarin.Forms.Xaml;

namespace FZAM.Pages.Controls
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class RangeQuestionControl : ContentView
    {
        public enum LabelType
        {
            Default = 0,
            Low_to_High = 1,
            Easy_to_Hard = 2,
            Unimportant_to_Important = 3,
            Not_to_VeryMuch = 4,
            Bad_to_Good = 5
        }

        private static readonly Dictionary<LabelType, List<string>> ButtonLabels =
            new Dictionary<LabelType, List<string>>
            {
                // Default (just numbers)
                {
                    LabelType.Default, new List<string> {"0", "1", "2", "3", "4", "5", "6", "7"}
                },
                // Low_to_High
                {
                    LabelType.Low_to_High, new List<string>
                    {
                        l10n.WayTooLow,
                        l10n.TooLow,
                        l10n.LittleTooLow,
                        l10n.JustRight,
                        l10n.LittleTooHigh,
                        l10n.TooHigh,
                        l10n.WayTooHigh
                    }
                },
                // Easy_to_Hard
                {
                    LabelType.Easy_to_Hard, new List<string>
                    {
                        l10n.WayTooEasy,
                        l10n.TooEasy,
                        l10n.LittleTooEasy,
                        l10n.JustRightEasy,
                        l10n.LittleTooHard,
                        l10n.TooHard,
                        l10n.WayTooHard
                    }
                },
                // Unimportant_to_Important
                {
                    LabelType.Unimportant_to_Important, new List<string>
                    {
                        l10n.NotAtAllImportant,
                        l10n.Unimportant,
                        l10n.NotThatImportant,
                        l10n.JustRightImportant,
                        l10n.LittleImportant,
                        l10n.Important,
                        l10n.VeryImportant
                    }
                },
                // Not_to_VeryMuch
                {
                    LabelType.Not_to_VeryMuch, new List<string>
                    {
                        l10n.NotAtAll,
                        l10n.VeryLittle,
                        l10n.Little,
                        l10n.Allright,
                        l10n.Much,
                        l10n.VeryMuch,
                        l10n.Absolutely
                    }
                },
                // Bad_to_Good
                {
                    LabelType.Bad_to_Good, new List<string>
                    {
                        l10n.Desasterous,
                        l10n.VeryBad,
                        l10n.Bad,
                        l10n.LessGood,
                        l10n.Good,
                        l10n.VeryGood,
                        l10n.Excellent
                    }
                }
            };

        private readonly Button[] ChoiceButtons;
        private readonly bool showSkipButton;
        private readonly int Start;

        private readonly int Steps;
        private int selectedOption = -1;


        public RangeQuestionControl(LabelType labelSet, int start, int steps, int selected, bool showSkipButton)
        {
            this.InitializeComponent();
            this.Steps = steps;
            this.Start = start;
            this.showSkipButton = showSkipButton;
            this.selectedOption = selected;
            this.ChoiceButtons = new[]
                {this.Button0, this.Button1, this.Button2, this.Button3, this.Button4, this.Button5, this.Button6};
            // set offset
            for (var i = 0; i < start; i++)
            {
                this.ChoiceButtons[i].IsVisible = false;
                this.ChoiceButtons[i].IsEnabled = false;
            }

            // set ButtonLabels
            for (var i = start; i < steps + start; i++) this.ChoiceButtons[i].Text = ButtonLabels[labelSet][i];
            // decativate what is not in range
            for (var i = steps + start; i < 7; i++)
            {
                this.ChoiceButtons[i].IsVisible = false;
                this.ChoiceButtons[i].IsEnabled = false;
            }

            this.SelectedButton(selected);
            this.NotApplicableButton.IsVisible = this.showSkipButton;
        }

        public int Result
        {
            get => this.selectedOption;
            set
            {
                this.selectedOption = value;
                this.SelectedButton(value);
            }
        }

        public static string GetResultToLabel(LabelType type, int result)
        {
            try
            {
                return ButtonLabels[type][result];
            }
            catch
            {
                return result.ToString();
            }
        }

        private void SelectedButton(int selected)
        {
            if ((selected >= this.Start) & (selected < this.Start + this.Steps))
                this.ChoiceButtons[selected].BackgroundColor = Color.FromHex("#d61726");
            if (selected == 7) this.NotApplicableButton.BackgroundColor = Color.FromHex("#d61726");
        }

        public event EventHandler ResultSelected;

        public void ChoiceButtonAccepted(object sender, EventArgs e)
        {
            this.Result = this.ChoiceButtons.IndexOf(sender);
            if (this.ResultSelected != null)
                this.ResultSelected(sender, e);
        }

        private void NotApplicableButton_OnClicked(object sender, EventArgs e)
        {
            this.Result = 7;
            if (this.ResultSelected != null)
                this.ResultSelected(sender, e);
        }
    }
}