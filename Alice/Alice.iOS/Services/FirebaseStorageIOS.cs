using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Alice.Controls;
using Alice.iOS.Services;
using Alice.Pages;
using Alice.Services;
using Firebase.Storage;
using Foundation;
using Microsoft.Practices.ObjectBuilder2;
using UIKit;
using Xamarin.Forms;

[assembly: Dependency(typeof(FirebaseStorageIOS))]
namespace Alice.iOS.Services
{
    public class FirebaseStorageIOS : IFirebaseStorage
    {
        private string _storage = "gs://alice-1d9df.appspot.com";

        //private UIImagePickerController controller;
        private UIImagePickerController imagePicker;
        


        public async Task<string> GetFileUrl(string filename)
        {
            var result = "";
            int timeWait = 250;
            int countWait = 0;
            int maxCountWait = 20;
            
            try
            {
                var storage = Storage.DefaultInstance;
                StorageReference spaceNode = storage.GetReferenceFromUrl($"{_storage}/images/{filename}");


                // Fetch the download Url
                spaceNode.GetDownloadUrl((url, error) => {
                    if (error != null)
                    {
                        // Handle any errors
                    }
                    else
                    {
                        // Get the download URL for 'images/stars.jpg'
                        result = url.ToString();
                    }
                });
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine("---> Error UploadFiles " + ex.Message);
                countWait = maxCountWait;
            }

            do
            {
                countWait++;
                await System.Threading.Tasks.Task.Delay(timeWait);
            }
            while (result == "" || countWait < maxCountWait);


            System.Diagnostics.Debug.WriteLine("---> result =  " + result);

            return result;
        }

        public async Task<string> UploadFiles()
        {
            imagePicker = new UIImagePickerController();

            imagePicker.SourceType = UIImagePickerControllerSourceType.PhotoLibrary;
            imagePicker.MediaTypes = UIImagePickerController.AvailableMediaTypes(UIImagePickerControllerSourceType.PhotoLibrary);

            imagePicker.FinishedPickingMedia += Handle_FinishedPickingMedia;
            imagePicker.Canceled += Handle_Canceled;
            
            UIApplication.SharedApplication.KeyWindow.RootViewController.PresentModalViewController(imagePicker, true);

            return "";


         
        }

        private void Handle_Canceled(object sender, EventArgs e)
        {
            UIApplication.SharedApplication.KeyWindow.RootViewController.DismissViewControllerAsync(true);
            //imagePicker.DismissModalViewControllerAnimated(true);
        }

        private void Handle_FinishedPickingMedia(object sender, UIImagePickerMediaPickedEventArgs e)
        {
            // determine what was selected, video or image
            bool isImage = false;
            switch (e.Info[UIImagePickerController.MediaType].ToString())
            {
                case "public.image":
                    System.Diagnostics.Debug.WriteLine("--> Image selected");
                    isImage = true;
                    break;
                case "public.video":
                    System.Diagnostics.Debug.WriteLine("--> Video selected");
                    break;
            }

            // get common info (shared between images and video)
            NSUrl referenceURL = e.Info[new NSString("UIImagePickerControllerReferenceUrl")] as NSUrl;


            System.Diagnostics.Debug.WriteLine("--> got url " + referenceURL);

            if (referenceURL != null)
                System.Diagnostics.Debug.WriteLine("--> Url:" + referenceURL.ToString());

            // if it was an image, get the other image info
            if (isImage)
            {
                // get the original image
                UIImage originalImage = e.Info[UIImagePickerController.OriginalImage] as UIImage;
                if (originalImage != null)
                {
                    

                    // todo do something with the image
                    System.Diagnostics.Debug.WriteLine("--> got the original image");
                    System.Diagnostics.Debug.WriteLine("--> PATH "+ originalImage.AccessibilityPath);
                    
                    //imageView.Image = originalImage; // display
                }
            }
            else
            { // if it's a video
              // get video url
                NSUrl mediaURL = e.Info[UIImagePickerController.MediaURL] as NSUrl;
                if (mediaURL != null)
                {
                    System.Diagnostics.Debug.WriteLine("--> " + mediaURL.ToString());
                }
            }


            // dismiss the picker
            //imagePicker.DismissModalViewControllerAnimated(true);
            UIApplication.SharedApplication.KeyWindow.RootViewController.DismissViewControllerAsync(true);
        }
    }




    
}
