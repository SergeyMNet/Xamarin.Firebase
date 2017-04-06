using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Alice.iOS.Services;
using Alice.Services;
using Xamarin.Forms;

[assembly: Dependency(typeof(FirebaseAuthIOS))]
namespace Alice.iOS.Services
{
    public class FirebaseAuthIOS : IFirebaseAuth
    {
        public Task<string> LoginAsync(string email, string pwd)
        {
            throw new NotImplementedException();
        }

        public Task<bool> CreateUserAsync(string email, string pwd)
        {
            throw new NotImplementedException();
        }

        public void Logout()
        {
            throw new NotImplementedException();
        }
    }
}
