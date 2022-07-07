using System;
using System.Threading.Tasks;
using Android.App;
using Xamarin.Forms;
using AppCompToolbar = Android.Support.V7.Widget.Toolbar;
using FZAM;
using Xamarin.Forms.Platform.Android.AppCompat;

[assembly: ExportRenderer(typeof(CustomNavigationPage), typeof(NavigationPageRenderer))]
namespace FZAM.Droid.CustomRenderers
{
    [Obsolete]
    public class NavigationPageRenderer : Xamarin.Forms.Platform.Android.AppCompat.NavigationPageRenderer
    {
        public AppCompToolbar toolbar;
        public Activity context;

        protected override Task<bool> OnPushAsync(Page view, bool animated)
        {
            var retVal = base.OnPushAsync(view, animated);

            context = (Activity)Forms.Context;
            toolbar = context.FindViewById<AppCompToolbar>(Droid.Resource.Id.toolbar);

            if (toolbar != null)
            {
                if (toolbar.NavigationIcon != null)
                {
                    //TODO: add Resource.Drawable.BackButton
                    toolbar.NavigationIcon = Android.Support.V7.Content.Res.AppCompatResources.GetDrawable(context, Resource.Drawable.arrow_left);
                    toolbar.Title = " ";
                }
            }

            return retVal;
        }
    }
}