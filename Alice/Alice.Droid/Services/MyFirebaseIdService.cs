using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Firebase.Iid;

namespace Alice.Droid.Services
{
    [Service]
    [IntentFilter(new[] { "com.google.firebase.INSTANCE_ID_EVENT" })]
    class MyFirebaseIdService : FirebaseInstanceIdService
    {
        public override void OnTokenRefresh()
        {
            base.OnTokenRefresh();

            System.Diagnostics.Debug.WriteLine("---> Token = " + FirebaseInstanceId.Instance.Token);
        }
    }
}