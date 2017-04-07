using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using System.Windows.Input;
using Alice.Models;
using Alice.Pages;
using Alice.Services;
using Xamarin.Forms;

namespace Alice.ViewModels
{
    public class ChatVM : BaseVM
    {
        public readonly IChatService _chatService;
        public readonly IFirebaseAuth _firebaseAuth;

        public ObservableCollection<ChatMessage> ChatMessages { get; set; } = new ObservableCollection<ChatMessage>();
        

        private UserModel _user = new UserModel();
        public UserModel UserCurent
        {
            get { return _user; }
            set { _user = value; OnPropertyChanged();
            }
        }



        public ChatVM(IChatService chatService)
        {
            _chatService = chatService;
            _firebaseAuth = DependencyService.Get<IFirebaseAuth>();

            //FakeData();
            
            _chatService.NewMessageReceived += ChatVM_NewMessageReceived;

            GetUser();
        }


        private async Task GetUser()
        {
            await Task.Delay(1000);

            Device.BeginInvokeOnMainThread(()=> {
                UserCurent = _firebaseAuth.GetUser();
                OnPropertyChanged("UserCurent");
            });
        }

        private void FakeData()
        {
            ChatMessages.Add(new ChatMessage()
            {
                IsYourMessage = true,
                Text = "some mesage  asdasdasd asdklj lj jiopjop jopj opjo pj ioph ioh uiohuio hioio",
                UserName = "Admin"
            });

            for (int i = 0; i < 5; i++)
            {
                ChatMessages.Add(new ChatMessage()
                {
                    IsYourMessage = (i % 2 == 0),
                    Text = "some mesage " + i,
                    UserName = (i % 2 == 0) ? "Admin" : "some friend"
                });
            }
        }


        private string _newMessageText;
        public string NewMessageText
        {
            get { return _newMessageText; }
            set { _newMessageText = value; OnPropertyChanged(); }
        }


        public ICommand LogoutCommand => new Command(Logout);

        private void Logout()
        {
            _firebaseAuth.Logout();
            App.Current.MainPage = new MainPage();
        }

        public ICommand AddMessageCommand => new Command(AddMessage);

        private void AddMessage()
        {
            ChatMessages.Add(new ChatMessage()
            {
                IsYourMessage = true,
                Text = NewMessageText,
                UserName = UserCurent.Name
            });
            
            MessagingCenter.Send<ChatVM>(this, "ScrollToEnd");
            _chatService.SendMessage(UserCurent.Name, NewMessageText, UserCurent.UrlPhoto);

            NewMessageText = "";
        }
        
        private void ChatVM_NewMessageReceived(object sender, System.EventArgs e)
        {
            var body = e as BodyEventArgs;
            System.Diagnostics.Debug.WriteLine("---> new message = " + body.Text);

            if (UserCurent.Name != body.Name)
            {
                ChatMessages.Add(new ChatMessage()
                {
                    Text = body.Text,
                    UserName = body.Name
                });

                MessagingCenter.Send<ChatVM>(this, "ScrollToEnd");
            }
        }
    }
}
