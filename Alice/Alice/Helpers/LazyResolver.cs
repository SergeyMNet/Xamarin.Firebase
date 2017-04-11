//using System;


//namespace Alice.Helpers
//{
//    public interface IContainer
//    {
//        void RegisterFactory<TClass>(Func<IContainer, TClass> factory);

//        void RegisterFactory<TInterface, TClass>(Func<IContainer, TClass> factory) where TClass : TInterface;

//        void RegisterInstance<TClass>(TClass _object);

//        void RegisterInstance<TInterface, TClass>(TClass _object) where TClass : TInterface;

//        void RegisterLazyInstance<TClass>(Func<IContainer, TClass> factory);

//        void RegisterLazyInstance<TInterface, TClass>(Func<IContainer, TClass> factory) where TClass : TInterface;

//        TClass Resolve<TClass>();
//    }

//    public static class DependencyService
//    {
//        public static IContainer Container { get; private set; }

//        public static void Attach(IContainer container)
//        {
//            Container = container;
//        }

//        public static void Detach(IContainer container)
//        {
//            if (Equals(container, Container))
//            {
//                Container = null;
//            }
//        }
//    }

//    public static class LazyResolver<T> where T : class
//    {
//        private static T _service;

//        public static T Service
//        {
//            get { return _service ?? (_service = DependencyService.Container.Resolve<T>()); }
//        }
//    }

//    public class ValueChangedEventArgs<T> : EventArgs
//    {
//        public T OldValue { get; private set; }

//        public T NewValue { get; private set; }

//        public ValueChangedEventArgs(T oldValue, T newValue)
//        {
//            OldValue = oldValue;
//            NewValue = newValue;
//        }
//    }

//    public class ValueChangingEventArgs<T> : EventArgs
//    {
//        public T OldValue { get; private set; }

//        public T NewValue { get; private set; }

//        public ValueChangingEventArgs(T oldValue, T newValue)
//        {
//            OldValue = oldValue;
//            NewValue = newValue;
//        }
//    }
//}
