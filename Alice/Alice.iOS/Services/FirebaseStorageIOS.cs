using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Alice.iOS.Services;
using Alice.Services;
using Xamarin.Forms;

[assembly: Dependency(typeof(FirebaseStorageIOS))]
namespace Alice.iOS.Services
{
    public class FirebaseStorageIOS : IFirebaseStorage
    {
        public void UploadFiles()
        {
            throw new NotImplementedException();
        }

        public void DownloadFiles()
        {
            throw new NotImplementedException();
        }

        public Task<string> GetFileUrl(string filename)
        {
            throw new NotImplementedException();
        }
    }
}
