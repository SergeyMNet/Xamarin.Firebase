using System.Threading.Tasks;
using Alice.Droid.Services;
using Alice.Models;
using Alice.Services;
using Firebase.Auth;
using Xamarin.Forms;

[assembly: Dependency(typeof(FirebaseAuthAndroid))]
namespace Alice.Droid.Services
{
    public class FirebaseAuthAndroid : IFirebaseAuth
    {
        private static FirebaseAuth _auth;

        public FirebaseAuthAndroid()
        {
            _auth = FirebaseAuth.Instance;
        }


        public async Task<string> LoginAsync(string email, string pwd)
        {
            var result = await _auth
                .SignInWithEmailAndPasswordAsync(email, pwd);
            
            return result.User.Email;
        }

        public async Task<bool> CreateUserAsync(string email, string pwd)
        {
            return _auth
                .CreateUserWithEmailAndPassword(email, pwd).IsComplete;
        }


        public async Task<string> LoginFacebookAsync(string token)
        {
            try
            {
                AuthCredential credential = FacebookAuthProvider.GetCredential(token);
                await _auth.SignInWithCredentialAsync(credential);
                
                return _auth.CurrentUser.DisplayName;
            }
            catch (System.Exception ex)
            {
                System.Diagnostics.Debug.WriteLine("---> Error LoginFacebookAsync " + ex.Message);
            }

            return "";
        }


        public UserModel GetUser()
        {
            try
            {
                var user = _auth.CurrentUser;

                var name = "";
                var url = "user";

                if (!string.IsNullOrEmpty(user.DisplayName))
                {
                    name = user.DisplayName;
                }
                else if (!string.IsNullOrEmpty(user.Email))
                {
                    name = user.Email;
                }

                if (user.PhotoUrl != null)
                {
                    url = user.PhotoUrl.ToString();
                }

                return new UserModel(
                    id: "001",
                    name: name,
                    photo: url);
            }
            catch (System.Exception ex)
            {
                System.Diagnostics.Debug.WriteLine("---> error GetUser " + ex.Message);
            }

            return new UserModel(
                    id: "001",
                    name: "",
                    photo: "");
        }



        public void Logout()
        {
            _auth.SignOut();
        }
    }
}