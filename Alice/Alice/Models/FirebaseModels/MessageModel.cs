using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace Alice.Models.FirebaseModels
{
    public class MessageModel
    {
        [JsonProperty("to")]
        public string To { get; set; } = "/topics/chat";

        [JsonProperty("data")]
        public ChatMessage Message { get; set; }

        public MessageModel(ChatMessage message)
        {
            Message = message;
        }
    }
}
