using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Alice.Services;
using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Xamarin.Forms;

[assembly: Dependency(typeof(ChatService))]
namespace Alice.Droid.Services
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

        public void SendMessage(string text)
        {
            

        }
    }
}