using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
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
        #region Init

        public readonly IFirebaseDatabase _firebaseDatabase;
        public readonly IFirebaseStorage _firebaseStorage;
        public readonly IChatService _chatService;
        public readonly IFirebaseAuth _firebaseAuth;
        
        string nodePath = "chats";

        public ChatVM(IChatService chatService)
        {
            _chatService = chatService;
            _firebaseAuth = DependencyService.Get<IFirebaseAuth>();
            _firebaseStorage = DependencyService.Get<IFirebaseStorage>();
            _firebaseDatabase = DependencyService.Get<IFirebaseDatabase>();

            //FakeData();
            
            _chatService.NewMessageReceived += ChatVM_NewMessageReceived;

            GetUser().ContinueWith(x => GetDataFromFirebase());
        }

      

        private void GetDataFromFirebase()
        {

            Action<Dictionary<string, ChatMessage>> onValueEvent = (Dictionary<string, ChatMessage> messages) =>
            {

                System.Diagnostics.Debug.WriteLine("---> EVENT GetDataFromFirebase ");

                Action onSetValueSuccess = () =>
                {
                
                };

                Action<string> onSetValueError = (string errorDesc) =>
                {
                    
                };

                if (messages == null)
                {
                    
                }
                else
                {
                    if (messages.Count != 0 && ChatMessages.Count != messages.Count)
                    {
                        ChatMessages.Clear();
                        foreach (var message in messages.OrderBy(m => m.Value.DateMessageTimeSpan))
                        {
                            message.Value.IsYourMessage = UserCurent.Name == message.Value.UserName;
                            ChatMessages.Add(message.Value);
                        }
                        MessagingCenter.Send<ChatVM>(this, "ScrollToEnd");
                    }
                }
            };
            
            _firebaseDatabase.AddValueEvent("chats", onValueEvent);
        }


        private async Task GetUser()
        {
            await Task.Delay(1000);

            Device.BeginInvokeOnMainThread(() => {
                UserCurent = _firebaseAuth.GetUser();
                OnPropertyChanged("UserCurent");
            });
        }

        private void FakeData()
        {
            ChatMessages.Add(new ChatMessage()
            {
                IsYourMessage = true,
                //Text = "some mesage  asdasdasd asdklj lj jiopjop jopj opjo pj ioph ioh uiohuio hioio",
                UserName = "Admin",
                AttachImg = "alice"
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

        #endregion


        public ICommand AttachFileCommand => new Command(AttachFile);
        public ICommand LogoutCommand => new Command(Logout);
        public ICommand AddMessageCommand => new Command(AddMessage);



        #region Properties

        public ObservableCollection<ChatMessage> ChatMessages { get; set; } = new ObservableCollection<ChatMessage>();


        private UserModel _user = new UserModel();
        public UserModel UserCurent
        {
            get { return _user; }
            set
            {
                _user = value; OnPropertyChanged();
            }
        }


        private string _newMessageText;
        public string NewMessageText
        {
            get { return _newMessageText; }
            set { _newMessageText = value; OnPropertyChanged(); }
        }

        #endregion


        #region Commands
        
        private async void AttachFile()
        {
            IsBusy = true;
            
            var file = await _firebaseStorage.UploadFiles();
            var url = await _firebaseStorage.GetFileUrl(file);

            System.Diagnostics.Debug.WriteLine("---> url " + url);

            var message = new ChatMessage()
            {
                IsYourMessage = true,
                AttachImg = url,
                UserName = UserCurent.Name
            };
            
            ChatMessages.Add(message);

            MessagingCenter.Send<ChatVM>(this, "ScrollToEnd");
            _chatService.SendMessage(message);

            _firebaseDatabase.SetValue(nodePath + "/" + message.Id, message);

            NewMessageText = "";
            IsBusy = false;
        }

        private void Logout()
        {
            _firebaseAuth.Logout();
            App.Current.MainPage = new MainPage();
        }

        private void AddMessage()
        {
            var message = new ChatMessage()
            {
                IsYourMessage = true,
                Text = NewMessageText,
                UserName = UserCurent.Name
            };

            ChatMessages.Add(message);

            MessagingCenter.Send<ChatVM>(this, "ScrollToEnd");
            _chatService.SendMessage(message);
            
            _firebaseDatabase.SetValue(nodePath + "/" + message.Id, message);

            NewMessageText = "";
        }
        
        #endregion


        #region Events

        private void ChatVM_NewMessageReceived(object sender, System.EventArgs e)
        {
            var body = e as BodyEventArgs;

            if (UserCurent.Name != body.Message.UserName)
            {
                // todo hide - because database has listner
                //ChatMessages.Add(body.Message);
                
                MessagingCenter.Send<ChatVM>(this, "ScrollToEnd");
            }
        } 

        #endregion

    }
}
