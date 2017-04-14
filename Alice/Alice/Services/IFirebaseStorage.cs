using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Alice.Services
{
    public interface IFirebaseStorage
    {
        Task<string> UploadFiles();

        Task<string> GetFileUrl(string filename);
    }
}
