using System;
using Xamarin.Forms;

namespace FZAM.Questions
{
    public abstract class Question
    {
        public int Id { get; protected set; }
        public string Title { get; protected set; }

        public bool IsKeyword { get; protected set; } = false;
        public bool IsTitle { get; protected set; } = false; 
        public abstract void Render(EventHandler nextPageCallback, StackLayout parentLayoutElement);

        public abstract string GetAnswer();

        public Question(int id, string title)
        {
            this.Id = id;
            this.Title = title;
        }
    }
}