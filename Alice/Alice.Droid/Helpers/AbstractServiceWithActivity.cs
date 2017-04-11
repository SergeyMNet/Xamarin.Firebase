//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using Alice.Helpers;
//using Android.App;
//using Android.Content;
//using Android.OS;
//using Android.Runtime;
//using Android.Views;
//using Android.Widget;
//using Xamarin.Forms;

//namespace Alice.Droid.Helpers
//{
//    public interface IActivityService
//    {
//        event EventHandler<ValueChangingEventArgs<Activity>> ActivityChanging;

//        event EventHandler<ValueChangedEventArgs<Activity>> ActivityChanged;

//        Activity CurrentActivity { get; set; }

//        /// <summary>
//        /// Method to start an activity and then get back the result
//        /// </summary>
//        /// <param name="intent">The intent to start</param>
//        /// <param name="resultCallback">A callback with two parameters to get the result. First is result code, second one is Intent with data</param>
//        void StartActivityForResult(Intent intent, Action<Result, Intent> resultCallback);

//        void ProcessActivityResult(int requestCode, Result resultCode, Intent data);
//    }

//    public abstract class AbstractServiceWithActivity
//    {
//        protected IActivityService ActivityService
//        {
//            get { return LazyResolver<IActivityService>.Service; }
//        }

//        protected Activity CurrentActivity
//        {
//            get { return ActivityService.CurrentActivity; }
//        }
//    }
//}