using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Alice.Services
{
    public interface IFirebaseStorage
    {
        void UploadFiles();
        void DownloadFiles();


        Task<string> GetFileUrl(string filename);
    }
}
