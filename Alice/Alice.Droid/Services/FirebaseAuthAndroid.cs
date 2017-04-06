using System.Threading.Tasks;
using Alice.Droid.Services;
using Alice.Services;
using Android.Gms.Auth.Api.SignIn;
using Firebase.Auth;
using Xamarin.Forms;

[assembly: Dependency(typeof(FirebaseAuthAndroid))]
namespace Alice.Droid.Services
{
    public class FirebaseAuthAndroid : IFirebaseAuth
    {
        public async Task<string> LoginAsync(string email, string pwd)
        {
            var result = await FirebaseAuth.Instance
                .SignInWithEmailAndPasswordAsync(email, pwd);

            return result.User.Email;
        }

        public async Task<bool> CreateUserAsync(string email, string pwd)
        {
            return FirebaseAuth.Instance.CreateUserWithEmailAndPassword(email, pwd).IsComplete;
        }
        
     


        public void Logout()
        {
            FirebaseAuth.Instance.SignOut();
        }
    }
}