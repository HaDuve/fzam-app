using System;
using System.Threading.Tasks;

namespace FZAM.Pages
{
    class Debounce
    {

        private static bool isInCall = false;

        public static async void create(Action<object, EventArgs> a, object sender, EventArgs e)
        {
            if (isInCall) return;
            isInCall = true;

            a(sender, e);

            await Task.Run(async () =>
            {
                await Task.Delay(200);
                isInCall = false;
            });
        }

        public static async void create(Action a)
        {
            if (isInCall) return;
            isInCall = true;

            a();

            await Task.Run(async () =>
            {
                await Task.Delay(200);
                isInCall = false;
            });
        }

    }
}
