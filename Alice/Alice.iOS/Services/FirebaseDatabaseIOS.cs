using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using Alice.iOS.Services;
using Alice.Services;
using Firebase.Database;
using Foundation;
using Newtonsoft.Json;
using UIKit;
using Xamarin.Forms;

[assembly: Dependency(typeof(FirebaseDatabaseIOS))]
namespace Alice.iOS.Services
{
    public class FirebaseDatabaseIOS : IFirebaseDatabase
    {
        Dictionary<string, nuint> ChildRemovedEventHandles;
        Dictionary<string, nuint> ChildChangedEventHandles;
        Dictionary<string, nuint> ChildAddedEventHandles;
        Dictionary<string, nuint> ValueEventHandles;

        public FirebaseDatabaseIOS()
        {
            ChildChangedEventHandles = new Dictionary<string, nuint>();
            ChildRemovedEventHandles = new Dictionary<string, nuint>();
            ChildAddedEventHandles = new Dictionary<string, nuint>();
            ValueEventHandles = new Dictionary<string, nuint>();
        }

        string IFirebaseDatabase.SetChildValueByAutoId(string nodeKey, object obj, Action onSuccess, Action<string> onError)
        {
            DatabaseReference rootRef = Database.DefaultInstance.GetRootReference();
            DatabaseReference nodeRef = rootRef.GetChild(nodeKey);
            string objectJsonString = JsonConvert.SerializeObject(obj);
            NSError jsonError = null;
            NSData nsData = NSData.FromString(objectJsonString);
            NSObject nsObj = NSJsonSerialization.Deserialize(nsData, NSJsonReadingOptions.AllowFragments, out jsonError);

            DatabaseReference newChildRef = nodeRef.GetChildByAutoId();

            newChildRef.SetValue(nsObj, (NSError error, DatabaseReference reference) =>
            {
                if (error == null)
                {
                    if (onSuccess != null)
                    {
                        onSuccess();
                    }
                }
                else if (onError != null)
                {
                    onError(error.Description);
                }
            });

            return newChildRef.Key;
        }

        void IFirebaseDatabase.SetValue(string nodeKey, object obj, Action onSuccess, Action<string> onError)
        {
            DatabaseReference rootRef = Database.DefaultInstance.GetRootReference();

            DatabaseReference nodeRef = rootRef.GetChild(nodeKey);

            NSObject nsObj = null;

            if (obj != null)
            {
                string objectJsonString = JsonConvert.SerializeObject(obj);
                NSError jsonError = null;
                NSData nsData = NSData.FromString(objectJsonString);

                nsObj = NSJsonSerialization.Deserialize(nsData, NSJsonReadingOptions.AllowFragments, out jsonError);
            }

            nodeRef.SetValue(nsObj, (NSError error, DatabaseReference reference) =>
            {
                if (error == null)
                {
                    if (onSuccess != null)
                    {
                        onSuccess();
                    }
                }
                else if (onError != null)
                {
                    onError(error.Description);
                }
            });
        }

        void IFirebaseDatabase.BatchSetChildValues(string nodeKey, Dictionary<string, object> dict, Action onSuccess, Action<string> onError)
        {
            DatabaseReference rootRef = Database.DefaultInstance.GetRootReference();

            DatabaseReference nodeRef = rootRef.GetChild(nodeKey);

            NSDictionary nsDict = null;

            if (dict != null)
            {
                string objectJsonString = JsonConvert.SerializeObject(dict);
                NSData nsData = NSData.FromString(objectJsonString);

                NSError jsonError = null;
                nsDict = NSJsonSerialization.Deserialize(nsData, NSJsonReadingOptions.AllowFragments, out jsonError) as NSDictionary;
            }

            //nodeRef.SetValue(nsObj, (NSError error, DatabaseReference reference) =>
            nodeRef.UpdateChildValues(nsDict, (NSError error, DatabaseReference reference) =>
            {
                if (error == null)
                {
                    if (onSuccess != null)
                    {
                        onSuccess();
                    }
                }
                else if (onError != null)
                {
                    onError(error.Description);
                }
            });
        }

        void IFirebaseDatabase.RemoveValue(string nodeKey, Action onSuccess, Action<string> onError)
        {
            DatabaseReference rootRef = Database.DefaultInstance.GetRootReference();

            DatabaseReference nodeRef = rootRef.GetChild(nodeKey);

            nodeRef.RemoveValue((NSError error, DatabaseReference reference) =>
            {
                if (error == null)
                {
                    if (onSuccess != null)
                    {
                        onSuccess();
                    }
                }
                else if (onError != null)
                {
                    onError(error.Description);
                }
            });
        }

        void IFirebaseDatabase.AddChildEvent<T>(string nodeKey, Action<string, T> OnChildAdded, Action<string, T> OnChildRemoved, Action<string, T> OnChildChanged)
        {
            DatabaseReference rootRef = Database.DefaultInstance.GetRootReference();
            DatabaseReference nodeRef = rootRef.GetChild(nodeKey);

            nuint handleReference = AddChildEvent(nodeRef, DataEventType.ChildAdded, OnChildAdded);
            ChildAddedEventHandles[nodeKey] = handleReference;

            handleReference = AddChildEvent(nodeRef, DataEventType.ChildRemoved, OnChildRemoved);
            ChildRemovedEventHandles[nodeKey] = handleReference;

            handleReference = AddChildEvent(nodeRef, DataEventType.ChildChanged, OnChildChanged);
            ChildChangedEventHandles[nodeKey] = handleReference;
        }

        nuint AddChildEvent<T>(DatabaseReference nodeRef, DataEventType type, Action<string, T> eventAction)
        {
            nuint handleReference = nodeRef.ObserveEvent(type, (snapshot) =>
            {
                if (snapshot.HasChildren && eventAction != null)
                {
                    NSDictionary itemDict = snapshot.GetValue<NSDictionary>();
                    NSError error = null;
                    string itemDictString = NSJsonSerialization.Serialize(itemDict, NSJsonWritingOptions.PrettyPrinted, out error).ToString();

                    T item = JsonConvert.DeserializeObject<T>(itemDictString);
                    eventAction(snapshot.Key, item);
                }
            });

            return handleReference;
        }

        void IFirebaseDatabase.AddSingleValueEvent<T>(string nodeKey, Action<T> action)
        {
            DatabaseReference rootRef = Database.DefaultInstance.GetRootReference();

            DatabaseReference nodeRef = rootRef.GetChild(nodeKey);

            nodeRef.ObserveSingleEvent(DataEventType.Value, (snapshot) =>
            {
                if (snapshot.Exists && snapshot.HasChildren && action != null)
                {
                    NSDictionary itemDict = snapshot.GetValue<NSDictionary>();
                    NSError error = null;
                    string itemDictString = NSJsonSerialization.Serialize(itemDict, NSJsonWritingOptions.PrettyPrinted, out error).ToString();

                    T item = JsonConvert.DeserializeObject<T>(itemDictString);
                    action(item);
                }
                else
                {
                    T item = default(T);
                    action(item);
                }
            });
        }

        void IFirebaseDatabase.AddValueEvent<T>(string nodeKey, Action<T> action)
        {
            DatabaseReference rootRef = Database.DefaultInstance.GetRootReference();

            DatabaseReference nodeRef = rootRef.GetChild(nodeKey);

            nuint handleReference = nodeRef.ObserveEvent(DataEventType.Value, (snapshot) =>
            {
                if (snapshot.Exists && snapshot.HasChildren && action != null)
                {
                    NSDictionary itemDict = snapshot.GetValue<NSDictionary>();
                    NSError error = null;
                    string itemDictString = NSJsonSerialization.Serialize(itemDict, NSJsonWritingOptions.PrettyPrinted, out error).ToString();

                    T item = JsonConvert.DeserializeObject<T>(itemDictString);
                    action(item);
                }
                else
                {
                    T item = default(T);
                    action(item);
                }
            });

            ValueEventHandles[nodeKey] = handleReference;
        }

        void IFirebaseDatabase.RemoveValueEvent(string nodeKey)
        {
            if (ValueEventHandles.ContainsKey(nodeKey))
            {
                DatabaseReference rootRef = Database.DefaultInstance.GetRootReference();

                DatabaseReference nodeRef = rootRef.GetChild(nodeKey);
                nodeRef.RemoveObserver(ValueEventHandles[nodeKey]);
            }

        }

        void IFirebaseDatabase.RemoveChildEvent(string nodeKey)
        {
            if (ChildAddedEventHandles.ContainsKey(nodeKey))
            {
                DatabaseReference rootRef = Database.DefaultInstance.GetRootReference();

                DatabaseReference nodeRef = rootRef.GetChild(nodeKey);
                nodeRef.RemoveObserver(ChildAddedEventHandles[nodeKey]);
                nodeRef.RemoveObserver(ChildRemovedEventHandles[nodeKey]);
                nodeRef.RemoveObserver(ChildChangedEventHandles[nodeKey]);
            }
        }

        void IFirebaseDatabase.Search<T>(string nodeKey, Action<List<T>> action)
        {
            DatabaseReference nodeRef = Database.DefaultInstance.GetRootReference().GetChild(nodeKey);
            Stopwatch watch = Stopwatch.StartNew();
            nodeRef.ObserveSingleEvent(DataEventType.Value, (snapshot) => OnSearchReturn(action, snapshot, watch));
        }

        void IFirebaseDatabase.Search<T>(string nodeKey, Action<List<T>> action, string orderByChildKey)
        {
            DatabaseReference nodeRef = Database.DefaultInstance.GetRootReference().GetChild(nodeKey);
            Stopwatch watch = Stopwatch.StartNew();
            nodeRef.GetQueryOrderedByChild(orderByChildKey).ObserveSingleEvent(DataEventType.Value, (snapshot) => OnSearchReturn(action, snapshot, watch));
        }

        void IFirebaseDatabase.Search<T>(string nodeKey, Action<List<T>> action, string orderByChildKey, string startAt, string endAt)
        {
            DatabaseReference nodeRef = Database.DefaultInstance.GetRootReference().GetChild(nodeKey);
            Stopwatch watch = Stopwatch.StartNew();
            nodeRef.GetQueryOrderedByChild(orderByChildKey)
                 .GetQueryStartingAtValue(NSObject.FromObject(startAt))
                 .GetQueryEndingAtValue(NSObject.FromObject(endAt))
                .ObserveSingleEvent(DataEventType.Value, (snapshot) => OnSearchReturn(action, snapshot, watch));
        }

        void IFirebaseDatabase.SearchOrderedByFirstValues<T>(string nodeKey, Action<List<T>> action, uint limitToFirst)
        {
            DatabaseReference nodeRef = Database.DefaultInstance.GetRootReference().GetChild(nodeKey);
            Stopwatch watch = Stopwatch.StartNew();
            nodeRef.GetQueryOrderedByValue()
               .GetQueryLimitedToFirst(limitToFirst)
               .ObserveSingleEvent(DataEventType.Value, (snapshot) => OnSearchReturn(action, snapshot, watch));
        }

        void IFirebaseDatabase.SearchOrderedByLastValues<T>(string nodeKey, Action<List<T>> action, uint limitToLast)
        {
            DatabaseReference nodeRef = Database.DefaultInstance.GetRootReference().GetChild(nodeKey);
            Stopwatch watch = Stopwatch.StartNew();
            nodeRef.GetQueryOrderedByValue()
                  .GetQueryLimitedToLast(limitToLast)
               .ObserveSingleEvent(DataEventType.Value, (snapshot) => OnSearchReturn(action, snapshot, watch));
        }

        void OnSearchReturn<T>(Action<List<T>> callback, DataSnapshot snapshot, Stopwatch watch)
        {
            if (snapshot.Exists && snapshot.HasChildren && callback != null)
            {
                watch.Stop();
                Debug.WriteLine("search time: " + watch.ElapsedMilliseconds);
                watch.Restart();

                NSEnumerator e = snapshot.Children;
                NSObject o = e.NextObject();
                DataSnapshot snap;
                NSMutableArray array = new NSMutableArray();

                while (o != null)
                {
                    //Debug.WriteLine(o.ToString());
                    snap = o as DataSnapshot;
                    array.Add(snap.GetValue());
                    o = e.NextObject();
                }

                NSError error = null;
                string itemArrayStr = NSJsonSerialization.Serialize(array, NSJsonWritingOptions.PrettyPrinted, out error).ToString();
                List<T> itemArr = JsonConvert.DeserializeObject<List<T>>(itemArrayStr);

                watch.Stop();
                Debug.WriteLine("finish search process time: " + watch.ElapsedMilliseconds);

                callback(itemArr);
            }
            else
            {
                List<T> item = new List<T>();
                callback(item);
            }
        }


        //void FirebaseDatabaseService.ChildExists<T>(string nodeKey, Action<T> onNodeFound, Action onNodeMissing)
        //{
        //    DatabaseReference rootRef = Database.DefaultInstance.GetRootReference();

        //    DatabaseReference nodeRef = rootRef.GetChild(nodeKey);
        //    nodeRef.ObserveSingleEvent(DataEventType.Value, (snapshot) =>
        //    {
        //        if (snapshot.Exists && onNodeFound != null)
        //        {
        //            NSDictionary itemDict = snapshot.GetValue<NSDictionary>();
        //            NSError error = null;
        //            string itemDictString = NSJsonSerialization.Serialize(itemDict, NSJsonWritingOptions.PrettyPrinted, out error).ToString();

        //            T item = JsonConvert.DeserializeObject<T>(itemDictString);
        //            onNodeFound(item);
        //        }
        //        else if(onNodeMissing != null)
        //        {
        //            onNodeMissing();
        //        }
        //    });
        //}

    }
}