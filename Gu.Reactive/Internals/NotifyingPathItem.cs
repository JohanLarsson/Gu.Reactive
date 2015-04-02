namespace Gu.Reactive.Internals
{
    using System;
    using System.ComponentModel;
    using System.Linq;
    using System.Reactive;
    using System.Reflection;

    internal sealed class NotifyingPathItem : INotifyingPathItem
    {
        private readonly PropertyChangedEventArgs _propertyChangedEventArgs;
        private readonly WeakReference _sourceRef = new WeakReference(null);
        private bool _disposed;
        private IDisposable _subscription;
        private readonly Action<EventPattern<PropertyChangedEventArgs>> _onNext;

        private readonly Action<Exception> _onError;

        private NotifyingPathItem()
        {
        }

        public NotifyingPathItem(INotifyingPathItem previous, PathProperty pathProperty)
        {
            var declaringType = pathProperty.PropertyInfo.DeclaringType;
            if (declaringType.IsValueType)
            {
                throw new ArgumentException("Cannot listen to changes for structs. Copy by value...");
            }
            if (!declaringType.GetInterfaces().Any(i => i == typeof(INotifyPropertyChanged)))
            {
                throw new ArgumentException("Type must be INotifyPropertyChanged");
            }
            PathProperty = pathProperty;
            _onNext = x => OnPropertyChanged(x.Sender, x.EventArgs);
            _onError = OnError;
            _propertyChangedEventArgs = new PropertyChangedEventArgs(PathProperty.PropertyInfo.Name);
            Previous = previous;
            var notifyingPathItem = previous as NotifyingPathItem;
            if (notifyingPathItem != null)
            {
                notifyingPathItem.Next = this;
            }
            if (previous != null)
            {
                Source = (INotifyPropertyChanged)previous.Value;
            }

        }

        public event PropertyChangedEventHandler PropertyChanged;

        public PathProperty PathProperty { get; private set; }

        public INotifyingPathItem Previous { get; private set; }

        public NotifyingPathItem Next { get; private set; }

        public PropertyChangedEventArgs PropertyChangedEventArgs
        {
            get { return _propertyChangedEventArgs; }
        }

        public bool IsLast
        {
            get { return PathProperty.IsLast; }
        }

        public object Value
        {
            get { return PathProperty.Value; }
        }

        /// <summary>
        /// Gets or sets the source.
        /// </summary>
        public INotifyPropertyChanged Source
        {
            get
            {
                return (INotifyPropertyChanged)_sourceRef.Target;
            }
            set
            {
                if (value != null)
                {
                    if (value.GetType() != PathProperty.PropertyInfo.DeclaringType)
                    {
                        throw new InvalidOperationException(
                            string.Format(
                                "Trying to set source to illegal type. Was: {0} expected {1}",
                                value.GetType().FullName,
                               PathProperty.PropertyInfo.DeclaringType.FullName));
                    }
                }

                var oldSource = _sourceRef.Target;
                _sourceRef.Target = value;
                var inpc = value;

                var isNullToNull = IsNullToNull(oldSource, value);
                if (inpc != null)
                {
                    if (!ReferenceEquals(oldSource, value))
                    {
                        Subscription = inpc.ObservePropertyChanged(PathProperty.PropertyInfo.Name, !isNullToNull)
                                           .Subscribe(_onNext, _onError);
                    }
                }
                else
                {
                    Subscription = null;
                    if (!isNullToNull)
                    {
                        OnPropertyChanged(value, PropertyChangedEventArgs);
                    }
                }
            }
        }

        /// <summary>
        /// Gets or sets the subscription.
        /// </summary>
        public IDisposable Subscription
        {
            get { return _subscription; }
            private set
            {
                if (_subscription != null)
                {
                    _subscription.Dispose();
                }
                _subscription = value;
            }
        }

        /// <summary>
        /// The dispose.
        /// </summary>
        public void Dispose()
        {
            if (_disposed)
            {
                return;
            }
            _disposed = true;
            if (Subscription != null)
            {
                Subscription.Dispose();
            }
        }

        private void OnError(Exception obj)
        {
            throw new NotImplementedException();
        }

        private void OnPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            var next = Next;
            if (next != null)
            {
                var value = (INotifyPropertyChanged)PathProperty.Value;
                if (ReferenceEquals(value, next.Source) && value != null) // The source signaled event without changing value. We still bubble up since it is not our job to filter.
                {
                    next.OnPropertyChanged(next.Source, e);
                }
                else if (string.IsNullOrEmpty(e.PropertyName) && value != null) // We want eventArgs.PropertyName string.Empty to bubble up
                {
                    next.OnPropertyChanged(next.Source, e);
                }
                else
                {
                    next.Source = value; // Let event bubble up this way.
                }
            }
            var handler = PropertyChanged;
            if (handler != null)
            {
                handler(sender, e);
            }
        }

        private bool IsNullToNull(object oldSource, object newSource)
        {
            var oldValue = oldSource != null ? PathProperty.GetValue(oldSource) : null;
            var newValue = newSource != null ? PathProperty.GetValue(newSource) : null;
            return oldValue == null && newValue == null;
        }
    }
}