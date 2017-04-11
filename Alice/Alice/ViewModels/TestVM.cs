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
            var url = await _firebaseStorage.GetFileUrl("");
            ImgTest = url;
        }


        public ICommand StartTestCommand2 => new Command(StartTest2);

        private async void StartTest2()
        {
            _firebaseStorage.UploadFiles();
            //ImgTest = url;
        }
    }
}
