using System;
using System.Collections.Generic;
using Alice.Droid.Services;
using Alice.Models;
using Alice.Services;
using Android.Runtime;
using Firebase.Database;
using Java.Util;
using GoogleGson;
using Newtonsoft.Json;
using Xamarin.Forms;

[assembly: Dependency(typeof(FirebaseDatabaseServiceAndroid))]
namespace Alice.Droid.Services
{
    public class FirebaseDatabaseServiceAndroid : IFirebaseDatabase
    {
        Dictionary<string, DatabaseReference> DatabaseReferences;
        Dictionary<string, IValueEventListener> ValueEventListeners;
        Dictionary<string, IChildEventListener> ChildEventListeners;

        public FirebaseDatabaseServiceAndroid()
        {
            DatabaseReferences = new Dictionary<string, DatabaseReference>();
            ValueEventListeners = new Dictionary<string, IValueEventListener>();
            ChildEventListeners = new Dictionary<string, IChildEventListener>();
        }

        private DatabaseReference GetDatabaseReference(string nodeKey)
        {
            if (DatabaseReferences.ContainsKey(nodeKey))
            {
                return DatabaseReferences[nodeKey];
            }
            else
            {
                DatabaseReference dr = FirebaseDatabase.Instance.GetReference(nodeKey);
                DatabaseReferences[nodeKey] = dr;
                return dr;
            }
        }

        public void AddValueEvent<T>(string nodeKey, Action<T> action)
        {
            DatabaseReference dr = GetDatabaseReference(nodeKey);

            if (dr != null)
            {
                ValueEventListener<T> listener = new ValueEventListener<T>(action);
                dr.AddValueEventListener(listener);

                ValueEventListeners.Add(nodeKey, listener);
            }
        }

        public void AddSingleValueEvent<T>(string nodeKey, Action<T> action)
        {
            DatabaseReference dr = GetDatabaseReference(nodeKey);

            if (dr != null)
            {
                ValueEventListener<T> listener = new ValueEventListener<T>(action);
                dr.AddListenerForSingleValueEvent(listener);


                ValueEventListeners.Add(nodeKey, listener);
            }

        }

        public void AddChildEvent<T>(string nodeKey, Action<string, T> OnChildAdded, Action<string, T> OnChildRemoved, Action<string, T> OnChildUpdated)
        {
            DatabaseReference dr = GetDatabaseReference(nodeKey);

            if (dr != null)
            {
                ChildEventListener<T> listener = new ChildEventListener<T>(OnChildAdded);
                dr.AddChildEventListener(listener);

                ChildEventListeners.Add(nodeKey, listener);
            }
        }

        public void FirebaseObserveEventChildRemoved<T>(string nodeKey, Action<T> action)
        {
            DatabaseReference dr = FirebaseDatabase.Instance.GetReference(nodeKey);
            ValueEventListener<T> listener = new ValueEventListener<T>(action);
            dr.AddValueEventListener(listener);

            DatabaseReferences.Add(nodeKey, dr);
            ValueEventListeners.Add(nodeKey, listener);
        }

        public string SetChildValueByAutoId(string nodeKey, object obj, Action onSuccess = null, Action<string> onError = null)
        {
            DatabaseReference dr = FirebaseDatabase.Instance.GetReference(nodeKey);

            if (dr != null)
            {
                string objJsonString = JsonConvert.SerializeObject(obj);

                Gson gson = new GsonBuilder().SetPrettyPrinting().Create();
                HashMap dataHashMap = new HashMap();
                Java.Lang.Object jsonObj = gson.FromJson(objJsonString, dataHashMap.Class);
                dataHashMap = jsonObj.JavaCast<HashMap>();
                DatabaseReference newChildRef = dr.Push();
                newChildRef.SetValue(dataHashMap);
                return newChildRef.Key;
            }

            return null;
        }

        public void SetValue(string nodeKey, object obj, Action onSuccess = null, Action<string> onError = null)
        {
            DatabaseReference dr = FirebaseDatabase.Instance.GetReference(nodeKey);

            if (dr != null)
            {
                string objJsonString = JsonConvert.SerializeObject(obj);

                Gson gson = new GsonBuilder().SetPrettyPrinting().Create();
                HashMap dataHashMap = new HashMap();
                Java.Lang.Object jsonObj = gson.FromJson(objJsonString, dataHashMap.Class);
                dataHashMap = jsonObj.JavaCast<HashMap>();
                dr.SetValue(dataHashMap);
            }
        }

        public void BatchSetChildValues(string nodeKey, Dictionary<string, object> dict, Action onSuccess = null, Action<string> onError = null)
        {
            throw new NotImplementedException();
        }

        public void RemoveValueEvent(string nodeKey)
        {
            DatabaseReference dr = DatabaseReferences[nodeKey];

            if (dr != null)
            {
                dr.RemoveEventListener(ValueEventListeners[nodeKey]);
                ValueEventListeners.Remove(nodeKey);
            }
        }

        public void RemoveChildEvent(string nodeKey)
        {
            DatabaseReference dr = DatabaseReferences[nodeKey];

            if (dr != null)
            {
                dr.RemoveEventListener(ChildEventListeners[nodeKey]);
                ChildEventListeners.Remove(nodeKey);
            }
        }

        public void RemoveValue(string nodeKey, Action onSuccess = null, Action<string> onError = null)
        {
            DatabaseReference dr = FirebaseDatabase.Instance.GetReference(nodeKey);

            if (dr != null)
            {
                dr.RemoveValue();
            }
        }

     



        //public void SaveMessage(ChatMessage message)
        //{
        //    // Write a message to the database
        //    var database = FirebaseDatabase.Instance;
        //    DatabaseReference myRef = database.GetReferenceFromUrl("https://alice-1d9df.firebaseio.com/");

        //    var main = "chats";
        //    var id = Guid.NewGuid().ToString();

        //    myRef.Child(main).Child("id").SetValue(id);
        //    myRef.Child(main).Child("username").SetValue(message.UserName);
        //    myRef.Child(main).Child("message").SetValue(message.Text);
        //    myRef.Child(main).Child("attach").SetValue(message.AttachImg);
        //    myRef.Child(main).Child("photo").SetValue(message.UrlPhoto);
        //}

        //public void GetMessages()
        //{
        //    throw new NotImplementedException();
        //}






        public void Search<T>(string nodeKey, Action<List<T>> action)
        {
            throw new NotImplementedException();
        }

        public void Search<T>(string nodeKey, Action<List<T>> action, string orderByChildKey)
        {
            throw new NotImplementedException();
        }

        public void Search<T>(string nodeKey, Action<List<T>> action, string orderByChildKey, string startAt, string endAt)
        {
            throw new NotImplementedException();
        }

        public void SearchOrderedByFirstValues<T>(string nodeKey, Action<List<T>> action, uint limitToFirst)
        {
            throw new NotImplementedException();
        }

        public void SearchOrderedByLastValues<T>(string nodeKey, Action<List<T>> action, uint limitToLast)
        {
            throw new NotImplementedException();
        }
    }



    public class ValueEventListener<T> : Java.Lang.Object, IValueEventListener
    {
        public Action<T> action;

        public ValueEventListener(Action<T> action)
        {
            this.action = action;
        }

        void IValueEventListener.OnCancelled(DatabaseError error)
        {
            //throw new NotImplementedException();
        }

        void IValueEventListener.OnDataChange(DataSnapshot snapshot)
        {
            if (snapshot.Exists() && snapshot.HasChildren)
            {
                HashMap dataHashMap = snapshot.Value.JavaCast<HashMap>();
                Gson gson = new GsonBuilder().Create();
                string chatItemDaataString = gson.ToJson(dataHashMap);

                // Try to deserialize :
                try
                {
                    T chatItems = JsonConvert.DeserializeObject<T>(chatItemDaataString);
                    action(chatItems);
                }
                catch
                {

                }
            }
            else
            {
                T item = default(T);
                action(item);
            }
        }
    }


    public class ChildEventListener<T> : Java.Lang.Object, IChildEventListener
    {
        public Action<string, T> action;

        public ChildEventListener(Action<string, T> action)
        {
            this.action = action;
        }

        void IChildEventListener.OnCancelled(DatabaseError error)
        {
            throw new NotImplementedException();
        }

        void IChildEventListener.OnChildAdded(DataSnapshot snapshot, string previousChildName)
        {
            HashMap dataHashMap = snapshot.Value.JavaCast<HashMap>();
            Gson gson = new GsonBuilder().Create();
            string chatItemDaataString = gson.ToJson(dataHashMap);

            // Try to deserialize :
            try
            {
                T chatItems = JsonConvert.DeserializeObject<T>(chatItemDaataString);
                action("temp", chatItems);
            }
            catch
            {

            }
        }

        void IChildEventListener.OnChildChanged(DataSnapshot snapshot, string previousChildName)
        {
            throw new NotImplementedException();
        }

        void IChildEventListener.OnChildMoved(DataSnapshot snapshot, string previousChildName)
        {
            throw new NotImplementedException();
        }

        void IChildEventListener.OnChildRemoved(DataSnapshot snapshot)
        {
            throw new NotImplementedException();
        }
    }
}