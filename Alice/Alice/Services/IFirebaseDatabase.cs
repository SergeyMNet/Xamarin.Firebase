using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Alice.Models;

namespace Alice.Services
{
    public interface IFirebaseDatabase
    {
        void AddChildEvent<T>(string nodeKey, Action<string, T> OnChildAdded = null,
            Action<string, T> OnChildRemoved = null, Action<string, T> OnChildChanged = null);

        void AddValueEvent<T>(string nodeKey, Action<T> OnValueEvent = null);
        void AddSingleValueEvent<T>(string nodeKey, Action<T> OnValueEvent = null);
        void RemoveValueEvent(string nodeKey);
        void RemoveChildEvent(string nodeKey);
        void SetValue(string nodeKey, object obj, Action onSuccess = null, Action<string> onError = null);

        void BatchSetChildValues(string nodeKey, Dictionary<string, object> dict, Action onSuccess = null,
            Action<string> onError = null);

        string SetChildValueByAutoId(string nodePath, object obj, Action onSuccess = null, Action<string> onError = null);
        void RemoveValue(string nodeKey, Action onSuccess = null, Action<string> onError = null);

        void Search<T>(string nodeKey, Action<List<T>> action);
        void Search<T>(string nodeKey, Action<List<T>> action, string orderByChildKey);
        void Search<T>(string nodeKey, Action<List<T>> action, string orderByChildKey, string startAt, string endAt);
        void SearchOrderedByFirstValues<T>(string nodeKey, Action<List<T>> action, uint limitToFirst);
        void SearchOrderedByLastValues<T>(string nodeKey, Action<List<T>> action, uint limitToLast);
    }
}
