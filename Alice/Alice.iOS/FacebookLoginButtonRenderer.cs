using System;
using System.Collections.Generic;
using System.Text;
using Alice.Controls;
using Alice.iOS;
using Facebook.LoginKit;
using Foundation;
using UIKit;
using Xamarin.Forms;
using Xamarin.Forms.Platform.iOS;

[assembly: ExportRenderer(typeof(FacebookLoginButton), typeof(FacebookLoginButtonRenderer))]
namespace Alice.iOS
{
    class FacebookLoginButtonRenderer : ButtonRenderer
    {
        protected override void OnElementChanged(ElementChangedEventArgs<Button> e)
        {
            base.OnElementChanged(e);
            if (Control != null)
            {
                UIButton button = Control;

                button.TouchUpInside += delegate
                {
                    HandleFacebookLoginClicked();
                };
            }
        }
        List<string> readPermissions = new List<string> { "public_profile" };

        private void HandleFacebookLoginClicked()
        {
            LoginManager login = new LoginManager();
            login.LogInWithReadPermissions(readPermissions.ToArray(), delegate (LoginManagerLoginResult result, NSError error)
            {
                if (error != null)
                {
                    App.OnFacebookAuthFailed();
                }
                else if (result.IsCancelled)
                {
                    App.OnFacebookAuthFailed();
                }
                else
                {
                    var accessToken = result.Token;
                    App.OnFacebookAuthSuccess(accessToken.TokenString);
                }
            });
        }


    }
}
