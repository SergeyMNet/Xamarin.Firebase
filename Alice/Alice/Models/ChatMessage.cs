using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Alice.Models
{
    public class ChatMessage
    {
        public string Text { get; set; }
        public string UserName { get; set; }

        public bool IsYourMessage { get; set; }
    }
}
