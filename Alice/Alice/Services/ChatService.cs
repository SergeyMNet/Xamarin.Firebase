using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Alice.Services
{
    public class ChatService : IChatService
    {
        public event EventHandler NewMessageReceived; 
        public void OnMessageReceived(string text)
        {
            EventHandler eh = NewMessageReceived;
            if (eh != null)
                eh(this, new BodyEventArgs(text));
        }
    }


    public class BodyEventArgs : EventArgs
    {
        public string Text { get; set; }
        public BodyEventArgs(string text)
        {
            Text = text;
        }
    }
}
