using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Input;
using Alice.Models;
using Alice.Services;
using Xamarin.Forms;

namespace Alice.ViewModels
{
    public class ChatVM : BaseVM
    {
        public IChatService ChatService;
        public ObservableCollection<ChatMessage> ChatMessages { get; set; } = new ObservableCollection<ChatMessage>();


        private string _newMessageText;
        public string NewMessageText
        {
            get { return _newMessageText; }
            set { _newMessageText = value; OnPropertyChanged(); }
        }


        public ICommand AddMessageCommand => new Command(AddMessage);

        private void AddMessage()
        {
            ChatMessages.Add(new ChatMessage()
            {
                IsYourMessage = true,
                Text = NewMessageText,
                UserName = "Admin"
            });

            NewMessageText = "";

            MessagingCenter.Send<ChatVM>(this, "ScrollToEnd");
        }

        public ChatVM()
        {
            for (int i = 0; i < 5; i++)
            {
                ChatMessages.Add(new ChatMessage()
                {
                    IsYourMessage = (i%2 == 0),
                    Text = "some mesage " + i,
                    UserName = (i % 2 == 0) ? "Admin" : "some friend"
                });
            }

            ChatService = DependencyService.Get<IChatService>();
            ChatService.NewMessageReceived += ChatVM_NewMessageReceived;

        }

        private void ChatVM_NewMessageReceived(object sender, System.EventArgs e)
        {
            var body = e as BodyEventArgs;

            System.Diagnostics.Debug.WriteLine("---> new message = " + body.Text);

            ChatMessages.Add(new ChatMessage()
            {
                Text = body.Text,
                UserName = "someone"
            });

            MessagingCenter.Send<ChatVM>(this, "ScrollToEnd");
        }
    }
}
