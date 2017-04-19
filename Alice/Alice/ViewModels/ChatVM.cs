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
                try
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
                            foreach (var message in messages.OrderBy(m => m.Value.DateMessageTimeSpan))
                            {
                                message.Value.IsYourMessage = UserCurent.Name == message.Value.UserName;

                                if (ChatMessages.All(m => m.Id != message.Value.Id))
                                {
                                    ChatMessages.Add(message.Value);
                                    System.Diagnostics.Debug.WriteLine("---> add new -> " + message.Value.Id);
                                }
                            }
                            
                            MessagingCenter.Send<ChatVM>(this, "ScrollToEnd");
                        }
                    }
                }
                catch (Exception ex)
                {

                    System.Diagnostics.Debug.WriteLine("---> error GetDataFromFirebase " + ex.Message);
                    throw;
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

        private void Logout()
        {
            _firebaseAuth.Logout();
            App.Current.MainPage = new MainPage();
        }


        private async void AttachFile()
        {
            IsBusy = true;
            
            var file = await _firebaseStorage.UploadFiles();
            var url = await _firebaseStorage.GetFileUrl(file);
            
            SendMessage(url);
            IsBusy = false;
        }
        
        private void AddMessage()
        {
            IsBusy = true;

            if (!String.IsNullOrEmpty(NewMessageText))
            {
                SendMessage();
            }

            IsBusy = false;
        }

        private void SendMessage(string attach = "")
        {
            var message = new ChatMessage()
            {
                IsYourMessage = true,
                Text = NewMessageText,
                AttachImg = attach,
                UserName = UserCurent.Name,
                UrlPhoto = UserCurent.UrlPhoto
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
