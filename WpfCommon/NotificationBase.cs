using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace WpfCommon
{
    public class NotificationBase : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        protected bool SetProperty<T>(ref T value, T newValue, [CallerMemberName] string propertyName = null)
        {
            if (EqualityComparer<T>.Default.Equals(value, newValue))
            {
                return false;
            }

            value = newValue;
            OnPropertyChanged(propertyName);

            return true;
        }

        protected IDisposable ObservePropertyChanged(string propertyName, Action action)
        {
            void @event(object sender, PropertyChangedEventArgs args)
            {
                if (args.PropertyName == propertyName)
                {
                    action();
                }
            }

            this.PropertyChanged += @event;
            return new AnonymousDisposable(() => this.PropertyChanged -= @event);
        }

        private class AnonymousDisposable : IDisposable
        {
            private readonly Action _action;

            internal AnonymousDisposable(Action action)
            {
                _action = action;
            }

            public void Dispose()
            {
                _action();
            }
        }
    }
}
