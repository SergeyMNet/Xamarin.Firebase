using Alice.DataServices;
using System;
using Alice.Models;
using Alice.Models.FirebaseModels;

namespace Alice.Services
{
    public class ChatService : IChatService
    {
        private readonly IRequestProvider _requestProvider;
        public ChatService(IRequestProvider requestProvider)
        {
            _requestProvider = requestProvider;
        }

        public event EventHandler NewMessageReceived;
        public void OnMessageReceived(ChatMessage message)
        {
            EventHandler eh = NewMessageReceived;
            if (eh != null)
                eh(this, new BodyEventArgs(message));
        }

        public void SendMessage(ChatMessage message)
        {
            string url = "https://fcm.googleapis.com/fcm/send";

            var data = new MessageModel(message);
            _requestProvider.PostAsync(url, data);
        }
        
    }


    public class BodyEventArgs : EventArgs
    {
        public ChatMessage Message { get; set; }
        
        public BodyEventArgs(ChatMessage message)
        {
            Message = message;
        }
    }
}
