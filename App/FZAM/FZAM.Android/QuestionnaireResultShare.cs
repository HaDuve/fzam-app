using System;
using System.IO;
using System.Threading.Tasks;
using Android.Content;
using Android.Content.PM;
using Android.Support.V4.Content;
using FZAM.Droid;
using FZAM.Menu;
using FZAM.Types;

[assembly: Xamarin.Forms.Dependency(typeof(QuestionnaireResultShare))]
namespace FZAM.Droid
{
    class QuestionnaireResultShare : IQuestionnaireResultShare
    {
        private readonly Context Context;

        public QuestionnaireResultShare()
        {
            Context = Android.App.Application.Context;
        }

        public Task Show(string resultAsJson, string lang, string version)
        {
            var path = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);
            var filename = Path.Combine(path, "config_share_" + lang.ToString() + "_" + version.ToString() + ".txt");

            // delete the file if it already exists
            File.Delete(filename);

            // write the file content
            using (var streamWriter = new StreamWriter(filename, true))
            {
                streamWriter.Write(resultAsJson);
            }

            // TODO: delete file after it has been shared
            var file = new Java.IO.File(filename);


            // create new Intent
            var intent = new Intent(Intent.ActionSend);

            // set flag to give temporary permission to external app to use your FileProvider
            intent.SetFlags(ActivityFlags.GrantReadUriPermission);

            // generate URI
            var uri = FileProvider.GetUriForFile(Context, Context.ApplicationContext.PackageName, file);

            // put file
            intent.PutExtra(Intent.ExtraStream, uri);
            intent.SetType("*/*");

            // set email parameters
            intent.PutExtra(Intent.ExtraEmail, new String[] { "alexanderhornig@outlook.com" });
            intent.PutExtra(Intent.ExtraSubject, "FZAM - Evaluation Feedback");
            intent.PutExtra(Intent.ExtraText, "Dear FZAM Team,\n\n here are my last results.");

            // create chooser
            var chooserIntent = Intent.CreateChooser(intent, "Share Results");
            chooserIntent.SetFlags(ActivityFlags.ClearTop);
            chooserIntent.SetFlags(ActivityFlags.NewTask);
            Context.StartActivity(chooserIntent);

            return Task.FromResult(true);
        }

        
    }
}
