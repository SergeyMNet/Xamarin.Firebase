using System;
using System.Threading.Tasks;
using Android.App;
using Android.Content.PM;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Android.OS;
using Firebase.Iid;

namespace Alice.Droid
{
    [Activity(Label = "Alice", Icon = "@drawable/icon", Theme = "@style/MainTheme", MainLauncher = true, ConfigurationChanges = ConfigChanges.ScreenSize | ConfigChanges.Orientation)]
    public class MainActivity : global::Xamarin.Forms.Platform.Android.FormsAppCompatActivity
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

            Task.Run(() =>
            {
                var instancedId = FirebaseInstanceId.Instance;
                instancedId.DeleteInstanceId();

                System.Diagnostics.Debug.WriteLine($"---> t1= {instancedId.Token}");
                System.Diagnostics.Debug.WriteLine($"---> t2= {instancedId.GetToken(GetString(Resource.String.gcm_defaultSenderId), Firebase.Messaging.FirebaseMessaging.InstanceIdScope)}");
            });
        }
    }
}

