using System;
using Xamarin.Forms;

namespace FZAM.Questions
{
    public interface IQuestion
    {
        int Id { get; }
        bool IsKeyword { get; }
        bool IsTitle { get; }
        string Title { get; }
        string GetAnswer();
        void Render(EventHandler nextPageCallback, StackLayout parentLayoutElement);
    }

    public interface INumericalQuestion : IQuestion
    {
        int Value { get; set; }
        float GetAnswerNumerical();
    }

    public interface ITextQuestion : IQuestion
    {
        string Value { get; set; }
    }
}