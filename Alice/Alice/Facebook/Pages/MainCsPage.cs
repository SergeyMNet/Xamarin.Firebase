using System;
using Xamarin.Forms;

namespace Alice.Facebook.Pages
{
    public class MainCsPage : ContentPage
    {

        public MainCsPage()
        {
            Title = "Facebook Login";
            BackgroundColor = Color.White;

            var loginButton = new Button
            {
                Text = "Login with Facebook",
                TextColor = Color.White,
                BackgroundColor = Color.FromHex("#01579B"),
                Font = Font.BoldSystemFontOfSize(26),
                FontSize = 26
            };

            loginButton.Clicked += LoginWithFacebook_Clicked;

            Content = new StackLayout
            {
                Orientation = StackOrientation.Vertical,
                Children =
                {
                    new Label
                    {
                        Text = "Login with Facebook API",
                        FontSize = 66,
                        TextColor = Color.Black
                    },
                    loginButton
                }
            };
        }

        private async void LoginWithFacebook_Clicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new FacebookProfileCsPage());
        }
    }
}


