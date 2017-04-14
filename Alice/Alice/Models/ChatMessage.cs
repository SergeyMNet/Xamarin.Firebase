using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace Alice.Models
{
    public class ChatMessage
    {
        [JsonProperty("message")]
        public string Text { get; set; } = "";

        [JsonProperty("username")]
        public string UserName { get; set; } = "";

        [JsonProperty("photo")]
        public string UrlPhoto { get; set; } = "";

        [JsonProperty("attach")]
        public string AttachImg { get; set; } = "";


        public bool IsYourMessage { get; set; }
    }
}
