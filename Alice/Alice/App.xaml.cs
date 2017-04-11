using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Alice.Models;
using Alice.Pages;
using Xamarin.Forms;

namespace Alice
{
    public partial class App : Application
    {
        public static bool IsActive = true;
        
        public App()
        {
            InitializeComponent();
            MainPage = new PageTest(); // new MainPage();
        }

        protected override void OnStart()
        {
            // Handle when your app starts
        }

        protected override void OnSleep()
        {
            // Handle when your app sleeps
        }

        protected override void OnResume()
        {
            // Handle when your app resumes
        }
    }
}
