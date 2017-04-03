using System;
using System.Windows.Input;
using Alice.Pages;
using Alice.Services;
using Xamarin.Forms;

namespace Alice.ViewModels
{
    public class MainVM : BaseVM
    {
        public IFirebaseAuth _firebaseAuth = DependencyService.Get<IFirebaseAuth>();
        

        public MainVM()
        {
            
        }

        private string _login = "admin@aa.aa";

        public string Login
        {
            get { return _login; }
            set { _login = value; OnPropertyChanged(); }
        }

        private string _password = "123456";

        public string Password
        {
            get { return _password; }
            set { _password = value; OnPropertyChanged(); }
        }

        private string _resultText;

        public string ResultText
        {
            get { return _resultText; }
            set { _resultText = value; OnPropertyChanged(); }
        }



        public ICommand LoginByPassCommand => new Command(LoginByPassword);


        private async void LoginByPassword()
        {
            try
            {
                var result = await _firebaseAuth.LoginAsync(Login, Password);
                ResultText = result.ToString();
                if(result)
                    App.Current.MainPage = new ChatPage();
            }
            catch (Exception ex)
            {

                System.Diagnostics.Debug.WriteLine("---> " + ex.Message);
                ResultText = ex.Message;
            }

        }

        
    }
}
