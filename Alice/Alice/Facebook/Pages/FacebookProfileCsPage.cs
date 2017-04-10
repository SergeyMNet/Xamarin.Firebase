using System.Linq;
using Alice.Facebook.Models;
using Alice.Facebook.ViewModels;
using Alice.Services;
using Xamarin.Forms;
using Device = Xamarin.Forms.Device;

namespace Alice.Facebook.Pages
{
    public class FacebookProfileCsPage : ContentPage
    {
        /// <summary>
        /// Make sure to get a new ClientId from:
        /// https://developers.facebook.com/apps/
        /// </summary>
        private string ClientId = "175293032980376";


        /// <summary>
        /// From firebase console
        /// </summary>
        private string _redirectUri = "https://alice-1d9df.firebaseapp.com/__/auth/handler"; 

        public FacebookProfileCsPage()
        {
            var viewModel = ViewModelLocator.Instance.Resolve(typeof(FacebookViewModel));
            BindingContext = viewModel;

            Title = "Facebook Profile";
            BackgroundColor = Color.White;

            var apiRequest =
                "https://www.facebook.com/dialog/oauth?client_id="
                + ClientId
                + "&display=popup&response_type=token&redirect_uri="
                + _redirectUri;

            var webView = new WebView
            {
                Source = apiRequest,
                HeightRequest = 1
            };

            webView.Navigated += WebViewOnNavigated;

            var grigMain = new Grid();
            var imgClose = new Image();
            imgClose.Source = "close";
            imgClose.HeightRequest = 50;
            imgClose.HorizontalOptions = LayoutOptions.EndAndExpand;
            imgClose.VerticalOptions = LayoutOptions.StartAndExpand;
            imgClose.Margin = new Thickness(0, 30, 20, 0);
            var tap = new TapGestureRecognizer();
            tap.Tapped += FacebookProfileCsPage_Tapped;
            imgClose.GestureRecognizers.Add(tap);

            grigMain.Children.Add(webView);
            grigMain.Children.Add(imgClose);
            
            Content = grigMain;
        }

        private async void FacebookProfileCsPage_Tapped(object sender, System.EventArgs e)
        {
            await App.Current.MainPage.Navigation.PopModalAsync();
        }

        private async void WebViewOnNavigated(object sender, WebNavigatedEventArgs e)
        {
            var accessToken = ExtractAccessTokenFromUrl(e.Url);

            System.Diagnostics.Debug.WriteLine("---> accessToken = " + accessToken);

            if (accessToken != "")
            {
                var vm = BindingContext as FacebookViewModel;
                await vm.SetFacebookUserProfileAsync(accessToken);
            //    SetPageContent(vm.FacebookProfile);
            }
        }

        private void SetPageContent(FacebookProfile facebookProfile)
        {
            Content = new StackLayout
            {
                Orientation = StackOrientation.Vertical,
                Padding = new Thickness(8, 30),
                Children =
                {
                    new Label
                    {
                        Text = facebookProfile.Name,
                        TextColor = Color.Black,
                        FontSize = 22,
                    },
                    new Label
                    {
                        Text = facebookProfile.Id,
                        TextColor = Color.Black,
                        FontSize = 22,
                    },
                    new Label
                    {
                        Text = facebookProfile.IsVerified.ToString(),
                        TextColor = Color.Black,
                        FontSize = 22,
                    },
                    new Label
                    {
                        Text = facebookProfile.Devices.FirstOrDefault().Os,
                        TextColor = Color.Black,
                        FontSize = 22,
                    },
                    new Label
                    {
                        Text = facebookProfile.Gender,
                        TextColor = Color.Black,
                        FontSize = 22,
                    },
                    new Label
                    {
                        Text = facebookProfile.AgeRange.Min.ToString(),
                        TextColor = Color.Black,
                        FontSize = 22,
                    },
                    new Label
                    {
                        Text = facebookProfile.Picture.Data.Url,
                        TextColor = Color.Black,
                        FontSize = 22,
                    },
                    new Label
                    {
                        Text = facebookProfile.Cover.Source,
                        TextColor = Color.Black,
                        FontSize = 22,
                    },
                }
            };
        }

        private string ExtractAccessTokenFromUrl(string url)
        {
            if (url.Contains("access_token") && url.Contains("&expires_in="))
            {
                var at = url.Replace("https://alice-1d9df.firebaseapp.com/__/auth/handler?#access_token=", "");
                
                var accessToken = at.Remove(at.IndexOf("&expires_in="));

                return accessToken;
            }

            return string.Empty;
        }
    }
}
