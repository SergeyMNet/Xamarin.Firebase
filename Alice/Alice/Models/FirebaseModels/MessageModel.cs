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
        public DataModel Data { get; set; }

        public MessageModel(string name, string text, string photo)
        {
            Data = new DataModel() {UserName = name, Message = text, UrlPhoto = photo};
        }
    }

    public class DataModel
    {
        [JsonProperty("username")]
        public string UserName { get; set; }

        [JsonProperty("message")]
        public string Message { get; set; }

        [JsonProperty("photo")]
        public string UrlPhoto { get; set; }
    }
}
