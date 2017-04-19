using System;
using Newtonsoft.Json;

namespace Alice.Models
{
    public class ChatMessage
    {
        [JsonProperty("id")]
        public string Id { get; set; } = Guid.NewGuid().ToString();

        [JsonProperty("username")]
        public string UserName { get; set; } = "";

        [JsonProperty("message")]
        public string Text { get; set; } = "";
        
        [JsonProperty("photo")]
        public string UrlPhoto { get; set; } = "";

        [JsonProperty("attach")]
        public string AttachImg { get; set; } = "";

        [JsonProperty("date_message")]
        public long DateMessageTimeSpan { get; set; } = DateTime.Now.Ticks;

        [JsonIgnore]
        public bool IsYourMessage { get; set; }

        [JsonIgnore]
        public string DateMessageDate {
            get
            {
                var date = DateTime.FromBinary(DateMessageTimeSpan);
                return date.ToString("f");
            }
        }
    }
}
