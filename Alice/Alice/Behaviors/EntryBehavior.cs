using System;
using Xamarin.Forms;
using System.Windows.Input;



namespace Alice.Behaviors
{
    public class OnComplatedBehavior : Behavior<Entry>
    {
        private VisualElement _element;

        public static readonly BindableProperty CompleteCommandProperty =
                BindableProperty.Create("CompleteCommand", typeof(ICommand),
                    typeof(OnComplatedBehavior), default(ICommand),
                    BindingMode.OneWay, null);

        public ICommand CompleteCommand
        {
            get { return (ICommand)GetValue(CompleteCommandProperty); }
            set { SetValue(CompleteCommandProperty, value); }
        }

        protected override void OnAttachedTo(Entry bindable)
        {
            _element = bindable;
            bindable.Completed += Bindable_Completed;
            bindable.BindingContextChanged += OnBindingContextChanged;
        }

        protected override void OnDetachingFrom(Entry bindable)
        {
            _element = null;
            BindingContext = null;
            bindable.Completed -= Bindable_Completed;
            bindable.BindingContextChanged -= OnBindingContextChanged;
        }

        private void Bindable_Completed(object sender, EventArgs e)
        {
            if (CompleteCommand != null && CompleteCommand.CanExecute(null))
            {
                CompleteCommand.Execute(null);
            }
        }
        

        private void OnBindingContextChanged(object sender, System.EventArgs e)
        {
            BindingContext = _element?.BindingContext;
        }
               
    }
}
