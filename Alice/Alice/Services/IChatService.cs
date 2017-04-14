using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Alice.Models;

namespace Alice.Services
{
    public interface IChatService
    {
        event EventHandler NewMessageReceived;
        void OnMessageReceived(ChatMessage message);
        
        void SendMessage(ChatMessage message);
    }


}
