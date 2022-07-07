using Xamarin.Forms;

namespace FZAM.Behaviors
{
    public class NumericValidationBehavior : Behavior<Entry>
    {
        protected override void OnAttachedTo(Entry entry)
        {
            entry.TextChanged += this.OnEntryTextChanged;
            base.OnAttachedTo(entry);
        }

        protected override void OnDetachingFrom(Entry entry)
        {
            entry.TextChanged -= this.OnEntryTextChanged;
            base.OnDetachingFrom(entry);
        }

        protected virtual void OnEntryTextChanged(object sender, TextChangedEventArgs e)
        {
            if (string.IsNullOrEmpty(e.NewTextValue))
            {
                ((Entry)sender).Text = 0.ToString();
                return;
            }

            var isValid = double.TryParse(e.NewTextValue, out _);
            ((Entry)sender).TextColor = isValid ? Color.Default : Color.Red;
            if (!isValid)
            {
                ((Entry)sender).Text = e.OldTextValue;
            }
        }
    }
}