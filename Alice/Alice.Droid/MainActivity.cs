using System;
using Android.App;
using Android.Content;
using Android.Content.PM;
using Android.Gms.Auth.Api.SignIn;
using Android.Gms.Common;
using Android.Gms.Common.Apis;
using Android.Gms.Tasks;
using Android.Widget;
using Android.OS;
using Firebase.Iid;
using Firebase.Messaging;
using Android.Gms.Auth.Api;
using Firebase;
using Firebase.Auth;
using Xamarin.Auth;


namespace Alice.Droid
{
    [Activity(Label = "Alice", Icon = "@drawable/icon", Theme = "@style/MainTheme", MainLauncher = true, ConfigurationChanges = ConfigChanges.ScreenSize | ConfigChanges.Orientation)]
    public class MainActivity : global::Xamarin.Forms.Platform.Android.FormsAppCompatActivity, IOnCompleteListener, FirebaseAuth.IAuthStateListener
    {
        private FirebaseAuth mAuth;
        GoogleApiClient mGoogleApiClient;
        private static int RC_SIGN_IN = 9001;


        protected override void OnCreate(Bundle bundle)
        {
            TabLayoutResource = Resource.Layout.Tabbar;
            ToolbarResource = Resource.Layout.Toolbar;

            base.OnCreate(bundle);

            global::Xamarin.Forms.Forms.Init(this, bundle);
            LoadApplication(new App());

            

            if (!GetString(Resource.String.google_app_id).Equals("1:799235809725:android:d40b68b2638551f5"))
                throw new SystemException("Invalid Json file");
            
            

            // FirebaseMessagesInit
            GetTokenFcm();

            


            FacebookAuthorizeService();


            //System.Threading.Tasks.Task.Run(()=> {

            //});
        }

        private void FacebookAuthorizeService()
        {
            // Setup our firebase options then init
            FirebaseOptions o = new FirebaseOptions.Builder()
                .SetApiKey("AIzaSyCxbHQupMvQfsJtvtUAa1xvrFWxVpr7kWI")
                .SetApplicationId("1:799235809725:android:d40b68b2638551f5")
                .Build();
            FirebaseApp fa = FirebaseApp.InitializeApp(this, o, Application.PackageName);

            // Get the auth instance so we can add to it
            mAuth = FirebaseAuth.GetInstance(fa);

            LoginFacebook();
        }

        private void LoginFacebook()
        {
            bool allowCancel = true;

            var auth = new OAuth2Authenticator(
                clientId: "175293032980376",
                scope: "",
                authorizeUrl: new Uri("https://m.facebook.com/dialog/oauth/"),
                redirectUrl: new Uri("https://alice-1d9df.firebaseapp.com/__/auth/handler"))
            { AllowCancel = allowCancel };
            auth.Error += Auth_Error;
            auth.Completed += FacebookAuthComplete;

        }

        private void Auth_Error(object sender, AuthenticatorErrorEventArgs e)
        {
            System.Diagnostics.Debug.WriteLine("---> Auth_Error");
            System.Diagnostics.Debug.WriteLine("---> " + e.Exception);
            System.Diagnostics.Debug.WriteLine("---> " + e.Message);
            
        }

        private void FacebookAuthComplete(object sender, AuthenticatorCompletedEventArgs e)
        {
            System.Diagnostics.Debug.WriteLine("---> user -> " + e.ToString());

            var request = new OAuth2Request("GET", new Uri("https://graph.facebook.com/me"), null, e.Account);
            request.GetResponseAsync().ContinueWith(t => {
                if (t.IsFaulted)
                    Console.WriteLine("Error: " + t.Exception.InnerException.Message);
                else
                {
                    string json = t.Result.GetResponseText();
                    Console.WriteLine(json);
                }
            });



            //Log.Debug(Tag, "FacebookAuthComplete:" + e.IsAuthenticated);

            if (e.IsAuthenticated)
            {
                var token = e.Account.Properties["access_token"];
                AuthCredential credential = FacebookAuthProvider.GetCredential(token);
                mAuth.SignInWithCredential(credential);
            }


            System.Diagnostics.Debug.WriteLine("---> user -> " + mAuth?.CurrentUser?.Email);

            
        }

        

        //private void GoogleSignIn()
        //{
        //    var gso = new GoogleSignInOptions.Builder(GoogleSignInOptions.DefaultSignIn)
        //                .RequestEmail()
        //                .Build();

        //    mGoogleApiClient = new GoogleApiClient.Builder(this)
        //        //.EnableAutoManage(this, this)
        //        .AddApi(Auth.GOOGLE_SIGN_IN_API, gso)
        //        .AddScope(new Scope(Scopes.Profile))
        //        .Build();

        //    Intent signInIntent = Auth.GoogleSignInApi.GetSignInIntent(mGoogleApiClient);
        //    StartActivityForResult(signInIntent, RC_SIGN_IN);
        //}


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

        public void OnAuthStateChanged(FirebaseAuth auth)
        {
            var user = auth.CurrentUser;


            System.Diagnostics.Debug.WriteLine("---> OnAuthStateChanged user => " + user);
            
        }
    }
}

