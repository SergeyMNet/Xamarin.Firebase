using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Alice.iOS.Services;
using Alice.Models;
using Alice.Services;
using Firebase.Auth;
using Foundation;
using Xamarin.Forms;

[assembly: Dependency(typeof(FirebaseAuthIOS))]
namespace Alice.iOS.Services
{
    public class FirebaseAuthIOS : IFirebaseAuth
    {
        public async Task<string> LoginAsync(string email, string pwd)
        {
            string user_name = "";
            int count = 0;
            int max = 10; // 5s max

            Auth.DefaultInstance.SignIn(email, pwd, (user, error) => {
                if (error != null)
                {
                    AuthErrorCode errorCode;
                    if (IntPtr.Size == 8) // 64 bits devices
                        errorCode = (AuthErrorCode)((long)error.Code);
                    else // 32 bits devices
                        errorCode = (AuthErrorCode)((int)error.Code);

                    // Posible error codes that SignIn method with email and password could throw
                    // Visit https://firebase.google.com/docs/auth/ios/errors for more information
                    switch (errorCode)
                    {
                        case AuthErrorCode.OperationNotAllowed:
                        case AuthErrorCode.InvalidEmail:
                        case AuthErrorCode.UserDisabled:
                        case AuthErrorCode.WrongPassword:
                        default:
                            // Print error
                            break;
                    }

                    // stop waiting
                    count = max;
                }
                else
                {
                    // Do your magic to handle authentication result
                    user_name = user.Email;
                }
            });


            do
            {
                await Task.Delay(500);
                count++;
            } while (user_name == "" || count < max);

            return user_name;
        }

        public async Task<bool> CreateUserAsync(string email, string pwd)
        {
            string user_name = "";
            int count = 0;
            int max = 10; // 5s max


            Auth.DefaultInstance.CreateUser(email, pwd, (user, error) => {
                if (error != null)
                {
                    AuthErrorCode errorCode;
                    if (IntPtr.Size == 8) // 64 bits devices
                        errorCode = (AuthErrorCode)((long)error.Code);
                    else // 32 bits devices
                        errorCode = (AuthErrorCode)((int)error.Code);

                    // Posible error codes that CreateUser method could throw
                    switch (errorCode)
                    {
                        case AuthErrorCode.InvalidEmail:
                        case AuthErrorCode.EmailAlreadyInUse:
                        case AuthErrorCode.OperationNotAllowed:
                        case AuthErrorCode.WeakPassword:
                        default:
                            // Print error
                            break;
                    }

                    // stop waiting
                    count = max;
                }
                else
                {
                    // Do your magic to handle authentication result
                    user_name = user.Email;
                }
            });
            
            do
            {
                await Task.Delay(500);
                count++;
            } while (user_name == "" || count < max);

            return user_name != "";
        }

        public void Logout()
        {
            NSError error;
            Auth.DefaultInstance.SignOut(out error);
        }
        

        public async Task<string> LoginFacebookAsync(string token)
        {
            string user_name = "";
            int count = 0;
            int max = 10; // 5s max

            // Get access token for the signed-in user and exchange it for a Firebase credential
            var credential = FacebookAuthProvider.GetCredential(token);

            // Authenticate with Firebase using the credential
            Auth.DefaultInstance.SignIn(credential, (user, error) => {
                if (error != null)
                {
                    AuthErrorCode errorCode;
                    if (IntPtr.Size == 8) // 64 bits devices
                        errorCode = (AuthErrorCode)((long)error.Code);
                    else // 32 bits devices
                        errorCode = (AuthErrorCode)((int)error.Code);

                    // Posible error codes that SignIn method with credentials could throw
                    // Visit https://firebase.google.com/docs/auth/ios/errors for more information
                    switch (errorCode)
                    {
                        case AuthErrorCode.InvalidCredential:
                        case AuthErrorCode.InvalidEmail:
                        case AuthErrorCode.OperationNotAllowed:
                        case AuthErrorCode.EmailAlreadyInUse:
                        case AuthErrorCode.UserDisabled:
                        case AuthErrorCode.WrongPassword:
                        default:
                            // Print error
                            break;
                    }

                    // stop waiting
                    count = max;
                }
                else
                {
                    // Do your magic to handle authentication result
                    user_name = user.Email;
                }
            });


            do
            {
                await Task.Delay(500);
                count++;
            } while (user_name == "" || count < max);

            return user_name;
        }

        public UserModel GetUser()
        {
            var user = Auth.DefaultInstance.CurrentUser;

            var name = "";
            var url = "user";

            if (!string.IsNullOrEmpty(user?.DisplayName))
            {
                name = user.DisplayName;
            }
            else if (!string.IsNullOrEmpty(user?.Email))
            {
                name = user.Email;
            }

            if (user?.PhotoUrl != null)
            {
                url = user.PhotoUrl.ToString();
            }

            return new UserModel(
                id: "001",
                name: name,
                photo: url);
        }
    }
}
