using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Alice.Services;
using Android.App;
using Android.Content;
using Android.Media;
using Android.OS;
using Android.Runtime;
using Android.Support.V7.App;
using Android.Views;
using Android.Widget;
using Firebase.Messaging;
using Xamarin.Forms;

namespace Alice.Droid.Services
{
    [Service]
    [IntentFilter(new[] { "com.google.firebase.MESSAGING_EVENT" })]
    class MyFirebaseMessagingService : FirebaseMessagingService
    {
        // id message, if you remove messages will be replaced by new ones
        private static int idPush = 1;

        public override void OnMessageReceived(RemoteMessage message)
        {
            base.OnMessageReceived(message);

            try
            {
                if (message.GetNotification() != null)
                    SendNotification(message.GetNotification().Body);

                if (message.Data.Count > 0)
                {
                    SendNotification(message.Data["message"], message.Data["username"], message.Data["photo"]);
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }

        private void SendNotification(string body, string name = "admin", string photo = "")
        {
            var intent = new Intent(this, typeof(MainActivity));
            intent.AddFlags(ActivityFlags.ClearTop | ActivityFlags.SingleTop);
            var pendingIntent = PendingIntent.GetActivity(this, 0, intent, PendingIntentFlags.OneShot);

            var defaultSoundUri = RingtoneManager.GetDefaultUri(RingtoneType.Notification);
            var notificationBuilder = new NotificationCompat.Builder(this)
                .SetSmallIcon(Resource.Drawable.icon)   // Display this icon
                .SetContentTitle(name)                  // Set its title
                .SetContentText(body)                   // The message to display.
                .SetAutoCancel(true)                    // Dismiss from the notif. area when clicked
                .SetSound(defaultSoundUri)              // Sound of message
                .SetContentIntent(pendingIntent);


            if (!App.IsActive)
            {
                var notificationManager = NotificationManager.FromContext(this);
                notificationManager.Notify(idPush++, notificationBuilder.Build());
            }
            else
            {
                var chatService = ViewModelLocator.Instance.Resolve(typeof(ChatService)) as IChatService;
                chatService.OnMessageReceived(name, body, photo);
            }

            
        }
    }
}