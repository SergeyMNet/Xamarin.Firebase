using System;
using Android.App;
using Android.Content.PM;
using Android.Gms.Tasks;
using Android.Widget;
using Android.OS;
using Android.Runtime;
using Firebase.Iid;
using Firebase.Messaging;
using Firebase.Database;
using System.Collections.Generic;
using Alice.Models;
using FFImageLoading.Forms.Droid;
using GoogleGson;
using Java.Util;
using Newtonsoft.Json;

namespace Alice.Droid
{
    [Activity(Label = "Alice", Icon = "@drawable/icon", Theme = "@style/MainTheme", MainLauncher = true, ConfigurationChanges = ConfigChanges.ScreenSize | ConfigChanges.Orientation)]
    public class MainActivity : global::Xamarin.Forms.Platform.Android.FormsAppCompatActivity, IOnCompleteListener
    {
        
        protected override void OnCreate(Bundle bundle)
        {
            TabLayoutResource = Resource.Layout.Tabbar;
            ToolbarResource = Resource.Layout.Toolbar;

            base.OnCreate(bundle);

            global::Xamarin.Forms.Forms.Init(this, bundle);
            LoadApplication(new App());

            

            if (!GetString(Resource.String.google_app_id).Equals("1:799235809725:android:d40b68b2638551f5"))
                throw new SystemException("Invalid Json file");
            
            GetTokenFcm();

            CachedImageRenderer.Init();
        }

        
        private void GetTokenFcm()
        {
            System.Threading.Tasks.Task.Run(() =>
            {
                var instancedId = FirebaseInstanceId.Instance;
                instancedId.DeleteInstanceId();

                System.Diagnostics.Debug.WriteLine($"---> t1= {instancedId.Token}");
                System.Diagnostics.Debug.WriteLine(
                    $"---> t2= {instancedId.GetToken(GetString(Resource.String.gcm_defaultSenderId), Firebase.Messaging.FirebaseMessaging.InstanceIdScope)}");


                // FirebaseSubscribeToTopic
                FirebaseMessaging.Instance.SubscribeToTopic("chat");
            });
        }
        



        protected override void OnStart()
        {
            base.OnStart();
            App.IsActive = true;
        }
        
        protected override void OnStop()
        {
            base.OnStop();
            App.IsActive = false;
        }

        public void OnComplete(Android.Gms.Tasks.Task task)
        {
            if (task.IsSuccessful)
            {
                ShowToast("Login Success");
            }
            else
            {
                ShowToast($"Error {task.Exception} {task.Result}");
            }
        }



        private void ShowToast(string message)
        {
            Toast.MakeText(this, message, ToastLength.Long).Show();
        }
    }
}

