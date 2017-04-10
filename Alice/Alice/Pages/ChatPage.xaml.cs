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

            if (Device.OS == TargetPlatform.iOS)
            {
                EntryChat.Focused += EntryChat_Focused;
                EntryChat.Unfocused += EntryChat_Completed;
            }
        }


        private void EntryChat_Completed(object sender, System.EventArgs e)
        {
            InputGrid.TranslateTo(0, 0, 250, Easing.Linear);
        }

        private void EntryChat_Focused(object sender, FocusEventArgs e)
        {
            InputGrid.TranslateTo(0, -300, 250, Easing.Linear);
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
