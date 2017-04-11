//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;
//using Android.App;
//using Android.Content;
//using Android.OS;
//using Android.Runtime;
//using Android.Views;
//using Android.Widget;

//namespace Alice.Droid.Helpers
//{
//    public static class AsyncHelper
//    {
//        public static Task<T> CreateAsyncFromCallback<T>(Action<Action<T>> asyncStarter)
//        {
//            TaskCompletionSource<T> taskSource = new TaskCompletionSource<T>();
//            Task<T> t = taskSource.Task;

//            Task.Factory.StartNew(() =>
//            {
//                asyncStarter(taskSource.SetResult);
//            });

//            return t;
//        }

//        public static Task<TU> CreateAsyncFromCallback<T, TU>(Action<Action<T>> asyncStarter, Func<T, TU> resultHandler)
//        {
//            TaskCompletionSource<TU> taskSource = new TaskCompletionSource<TU>();
//            Task<TU> t = taskSource.Task;

//            Task.Factory.StartNew(() =>
//            {
//                asyncStarter(res => taskSource.SetResult(resultHandler(res)));
//            });

//            return t;
//        }
//    }
//}