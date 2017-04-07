using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using Alice.Facebook.Models;
using Alice.Facebook.Services;
using Alice.Pages;
using Alice.Services;
using Alice.ViewModels;
using Xamarin.Forms;

namespace Alice.Facebook.ViewModels
{
    public class FacebookViewModel : BaseVM
    {
        public IFirebaseAuth _firebaseAuth = DependencyService.Get<IFirebaseAuth>();
        
        private FacebookProfile _facebookProfile;

        public FacebookProfile FacebookProfile
        {
            get { return _facebookProfile; }
            set
            {
                _facebookProfile = value;
                OnPropertyChanged();
            }
        }

        public async Task SetFacebookUserProfileAsync(string accessToken)
        {
            //var facebookServices = new FacebookServices();
            //FacebookProfile = await facebookServices.GetFacebookProfileAsync(accessToken);

            var user = await _firebaseAuth.LoginFacebookAsync(accessToken);
            System.Diagnostics.Debug.WriteLine("---> user " + user);
            if (user != "")
            {
                App.Current.MainPage = new ChatPage();
            }
            else
            {
                await App.Current.MainPage.Navigation.PopModalAsync();
            }
        }
        
    }
}
