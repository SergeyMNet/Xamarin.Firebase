using Android.App;
using Android.Content;
using Android.OS;
using Android.Database;
using Uri = Android.Net.Uri;
using System;

namespace Alice.Droid
{
    [Activity(Label = "PickFileActivity")]
    public class PickFileActivity : Activity
    {
        public static readonly int PickImageId = 1000;
        public static Action<string> OnFinishAction = null;
        public static Action OnCancelAction = null;
        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);

            Intent = new Intent(Intent.ActionPick, Android.Provider.MediaStore.Images.Media.ExternalContentUri);
            Intent.SetAction(Intent.ActionPick);
            StartActivityForResult(Intent, PickImageId);
        }

        protected override void OnActivityResult(int requestCode, Result resultCode, Intent data)
        {
            
            if ((requestCode == PickImageId) && (resultCode == Result.Ok) && (data != null))
            {
                Uri uri = data.Data;
                string path = GetPathToImage(uri);
                if (path != null && System.IO.File.Exists(path))
                {
                    Console.WriteLine(string.Format("PickFileActivity terminated with {0}", path));

                    if (OnFinishAction != null)
                        OnFinishAction(path);

                    Finish();
                }
                else
                {
                    Console.WriteLine("PickFileActivity failed with null or non-existant path.");
                }
            }
            if (OnCancelAction != null) OnCancelAction();

            Finish();
        }

        private string GetPathToImage(Uri uri)
        {
            string path = null;
            // The projection contains the columns we want to return in our query.
            string[] projection = new[] { Android.Provider.MediaStore.Images.Media.InterfaceConsts.Data };
            using (ICursor cursor = ManagedQuery(uri, projection, null, null, null))
            {
                if (cursor != null)
                {
                    int columnIndex = cursor.GetColumnIndexOrThrow(Android.Provider.MediaStore.Images.Media.InterfaceConsts.Data);
                    cursor.MoveToFirst();
                    path = cursor.GetString(columnIndex);
                }
            }
            return path;
        }
    }
}