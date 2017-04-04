using System;
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
        public readonly IChatService _chatService;
        public ObservableCollection<ChatMessage> ChatMessages { get; set; } = new ObservableCollection<ChatMessage>();

        public readonly string YouName;

        public ChatVM(IChatService chatService)
        {
            _chatService = chatService;
            
            //FakeData();

            //FakeName
            YouName = Guid.NewGuid().ToString();


            _chatService.NewMessageReceived += ChatVM_NewMessageReceived;
        }
        

        private void FakeData()
        {
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


        public ICommand AddMessageCommand => new Command(AddMessage);

        private void AddMessage()
        {
            ChatMessages.Add(new ChatMessage()
            {
                IsYourMessage = true,
                Text = NewMessageText,
                UserName = YouName
            });
            
            MessagingCenter.Send<ChatVM>(this, "ScrollToEnd");
            _chatService.SendMessage(YouName, NewMessageText);

            NewMessageText = "";
        }
        
        private void ChatVM_NewMessageReceived(object sender, System.EventArgs e)
        {
            var body = e as BodyEventArgs;
            System.Diagnostics.Debug.WriteLine("---> new message = " + body.Text);

            if (YouName != body.Name)
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
