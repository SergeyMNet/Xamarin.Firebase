using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Alice.Services;
using Xamarin.Forms;

namespace Alice.ViewModels
{
    public class TestVM : BaseVM
    {
        public readonly IFirebaseStorage _firebaseStorage;


        public TestVM()
        {
            _firebaseStorage = DependencyService.Get<IFirebaseStorage>();
        }



        private string _imgTest = "close";

        public string ImgTest
        {
            get { return _imgTest; }
            set { _imgTest = value; OnPropertyChanged(); }
        }






        public ICommand StartTestCommand => new Command(StartTest);

        private async void StartTest()
        {
            IsBusy = true;

            var url = await _firebaseStorage.GetFileUrl("cute_pink_chik_by_rukusucherry.jpg");
            ImgTest = url;

            IsBusy = false;
        }


        public ICommand StartTestCommand2 => new Command(StartTest2);

        private async void StartTest2()
        {
            IsBusy = true;

            var file = await _firebaseStorage.UploadFiles();
            var url = await _firebaseStorage.GetFileUrl(file);
            ImgTest = url;

            System.Diagnostics.Debug.WriteLine("---> url " + url);

            IsBusy = false;
        }
    }
}
