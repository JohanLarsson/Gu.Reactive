﻿namespace Gu.Reactive
{
    using System;
    using System.Collections.Generic;
    using System.Collections.Specialized;

    internal sealed class SimpleChanges<TCollection, TValue> : IChanges<TValue>
        where TCollection : IEnumerable<TValue>, INotifyCollectionChanged
        where TValue : struct, IComparable<TValue>
    {
        private readonly TCollection source;
        private readonly IDisposable subscription;
        private bool disposed;

        internal SimpleChanges(TCollection source)
        {
            this.source = source;
            this.subscription = source.ObserveCollectionChangedSlim(signalInitial: false)
                                      .Subscribe(this.OnSourceChanged);
        }

        public event Action<TValue>? Add;

        public event Action<TValue>? Remove;

        public event Action<IEnumerable<TValue>>? Reset;

#pragma warning disable INPC017 // Backing field name must match.
        public IEnumerable<TValue> Values => this.source;
#pragma warning restore INPC017 // Backing field name must match.

        public void Dispose()
        {
            if (this.disposed)
            {
                return;
            }

            this.disposed = true;
            this.subscription?.Dispose();
        }

        private void OnSourceChanged(NotifyCollectionChangedEventArgs e)
        {
            switch (e.Action)
            {
                case NotifyCollectionChangedAction.Add:
                    {
                        if (e.TryGetSingleNewItem(out TValue item))
                        {
                            this.Add?.Invoke(item);
                        }
                        else
                        {
                            this.Reset?.Invoke(this.source);
                        }

                        break;
                    }

                case NotifyCollectionChangedAction.Remove:
                    {
                        if (e.TryGetSingleOldItem(out TValue item))
                        {
                            this.Remove?.Invoke(item);
                        }
                        else
                        {
                            this.Reset?.Invoke(this.source);
                        }

                        break;
                    }

                case NotifyCollectionChangedAction.Replace:
                    {
                        if (e.TryGetSingleNewItem(out TValue newValue) &&
                            e.TryGetSingleOldItem(out TValue oldValue))
                        {
                            this.Add?.Invoke(newValue);
                            this.Remove?.Invoke(oldValue);
                        }
                        else
                        {
                            this.Reset?.Invoke(this.source);
                        }

                        break;
                    }

                case NotifyCollectionChangedAction.Move:
                    break;
                case NotifyCollectionChangedAction.Reset:
                    {
                        this.Reset?.Invoke(this.source);
                        break;
                    }

                default:
                    throw new ArgumentOutOfRangeException(nameof(e));
            }
        }
    }
}
