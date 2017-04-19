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

        [JsonProperty("priority")]
        public string Priority { get; set; } = "high";

        [JsonProperty("notification")]
        public NotificationModel NotificationModel { get; set; }

        [JsonProperty("data")]
        public ChatMessage Message { get; set; }

        public MessageModel(ChatMessage message)
        {
            Message = message;

            NotificationModel = new NotificationModel()
            {
                Title = "Alice new message",
                Body = message.Text,
                Icon = "mail"
            };
        }
    }


    public class NotificationModel
    {
        [JsonProperty("title")]
        public string Title { get; set; }

        [JsonProperty("body")]
        public string Body { get; set; }

        [JsonProperty("icon")]
        public string Icon { get; set; }
    }

   
}
