using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Alice.Models
{
    public class UserModel
    {
        public string Id { get; set; } = "";
        public string Name { get; set; } = "";
        public string UrlPhoto { get; set; } = "";

        public UserModel()
        {
            
        }

        public UserModel(string id, string name, string photo)
        {
            Id = id;
            Name = name;
            UrlPhoto = photo;
        }
    }
}
