using System;
using System.IO;
using System.Threading.Tasks;
using Alice.Droid.Services;
using Alice.Services;
using Android.App;
using Android.Content;
using Android.Gms.Extensions;
using Firebase.Storage;
using Xamarin.Forms;
using Object = Java.Lang.Object;

[assembly: Dependency(typeof(FirebaseStorageAndroid))]
namespace Alice.Droid.Services
{
    public class FirebaseStorageAndroid : IFirebaseStorage
    {
        public async void UploadFiles()
        {
            try
            {
                var activity = Forms.Context as Activity;

                PickFileActivity.OnFinishAction = (path) =>
                {
                    try
                    {
                        // todo do some with path
                        System.Diagnostics.Debug.WriteLine("---> path " + path);


                        activity.StartActivity(new Intent(Forms.Context, typeof(MainActivity)));

                        SaveFileToStorage(path);
                    }
                    catch (Exception e)
                    {
                        //Getting ticket failed, possibly because YouTubeHook is null or
                        //its access is denied. Switch to main screen.
                        Console.WriteLine(e.Message);
                        activity.StartActivity(new Intent(Forms.Context, typeof(MainActivity)));
                    }
                };

                PickFileActivity.OnCancelAction = () =>
                    activity.StartActivity(new Intent(Forms.Context, typeof(MainActivity)));

                activity.StartActivity(new Intent(Forms.Context, typeof(PickFileActivity)));


            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine("---> Error UploadFiles " + ex.Message);
            }
        }

        public void DownloadFiles()
        {
            throw new NotImplementedException();
        }



        public async Task<string> GetFileUrl(string filename)
        {
            try
            {
                filename = "type_drive.png";
                
                var storage = FirebaseStorage.Instance;
                var storageRef = storage.GetReferenceFromUrl("gs://alice-1d9df.appspot.com");
                var spaceRef = storageRef.Child($"{filename}");
                var url = await spaceRef.DownloadUrl;

                filename = url.ToString();

                System.Diagnostics.Debug.WriteLine("---> result " + url.ToString());
                
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine("---> Error UploadFiles " + ex.Message);
            }

            return filename;
        }


        private void SaveFileToStorage(string localPath)
        {
            var storage = FirebaseStorage.Instance;
            var storageRef = storage.GetReferenceFromUrl("gs://alice-1d9df.appspot.com");
            
            var bytes = System.IO.File.ReadAllBytes(localPath);
            var metadata = new StorageMetadata.Builder()
                .SetContentType("image/jpeg")
                .Build();

            var child = storageRef.Child("images/" + Path.GetFileName(localPath));
            var uploadTask = child.PutBytes(bytes, metadata);

            var activity = Forms.Context as Activity;

            // Listen for state changes, errors, and completion of the upload.
            uploadTask.AddOnProgressListener(activity, new ProgressListener());

        }

      
    }


    public class ProgressListener : IOnProgressListener
    {
        public void Dispose()
        {
            //this.Dispose();
        }

        public IntPtr Handle { get; }
        public void OnProgress(Object snapshot)
        {
            var taskSnapshot = snapshot as UploadTask.TaskSnapshot;
            double progress = (100.0 * taskSnapshot.BytesTransferred) / taskSnapshot.TotalByteCount;
            System.Diagnostics.Debug.WriteLine("---> " + progress);
        }
    }
}