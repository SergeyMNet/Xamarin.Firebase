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
        public void OnMessageReceived(string name, string text, string photo = "")
        {
            EventHandler eh = NewMessageReceived;
            if (eh != null)
                eh(this, new BodyEventArgs(name, text, photo));
        }

        public void SendMessage(string name, string text, string photo)
        {
            string url = "https://fcm.googleapis.com/fcm/send";
            MessageModel model = new MessageModel(name, text, photo);
            _requestProvider.PostAsync(url, model);
        }
        
    }


    public class BodyEventArgs : EventArgs
    {
        public string Name { get; set; }
        public string Text { get; set; }
        public string UrlPhoto { get; set; }

        public BodyEventArgs(string name, string text, string photo)
        {
            Name = name;
            Text = text;
            UrlPhoto = photo;
        }
    }
}
