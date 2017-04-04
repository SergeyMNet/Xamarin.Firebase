using System.Linq;
using Alice.Services;
using Alice.ViewModels;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Alice.Pages
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ChatPage : ContentPage
    {
        public ChatPage()
        {
            var viewModel = ViewModelLocator.Instance.Resolve(typeof(ChatVM));
            this.BindingContext = viewModel;
            
            InitializeComponent();
            
            Subscribe();
        }

        private void Subscribe()
        {
            MessagingCenter.Subscribe<ChatVM>(this, "ScrollToEnd", (sender) =>
            {
                ScrollToEnd();
            });

        }

        private void ScrollToEnd()
        {
            var v = ChatList.ItemsSource.Cast<object>().LastOrDefault();
            ChatList.ScrollTo(v, ScrollToPosition.End, true);
        }
    }
}
