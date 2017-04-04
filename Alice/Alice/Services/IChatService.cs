using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Alice.Services
{
    public interface IChatService
    {
        event EventHandler NewMessageReceived;
        void OnMessageReceived(string name, string text);


        void SendMessage(string name, string text);
    }


}
