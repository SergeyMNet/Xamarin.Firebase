using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Alice.ViewModels;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Alice.Pages
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class PageTest : ContentPage
    {
        public PageTest()
        {
            this.BindingContext = new TestVM();
            InitializeComponent();
        }
    }
}
