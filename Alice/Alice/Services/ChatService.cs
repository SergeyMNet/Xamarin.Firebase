using Alice.DataServices;
using System;
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
        public void OnMessageReceived(string name, string text)
        {
            EventHandler eh = NewMessageReceived;
            if (eh != null)
                eh(this, new BodyEventArgs(name, text));
        }

        public void SendMessage(string name, string text)
        {
            string url = "https://fcm.googleapis.com/fcm/send";
            MessageModel model = new MessageModel(name, text);
            _requestProvider.PostAsync(url, model);
        }
        
    }


    public class BodyEventArgs : EventArgs
    {
        public string Name { get; set; }
        public string Text { get; set; }

        public BodyEventArgs(string name, string text)
        {
            Name = name;
            Text = text;
        }
    }
}
