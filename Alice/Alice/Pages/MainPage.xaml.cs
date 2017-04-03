using Alice.ViewModels;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Alice.Pages
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class MainPage : ContentPage
    {
        public MainPage()
        {
            InitializeComponent();
            BindingContext = new MainVM();
        }
    }
}
