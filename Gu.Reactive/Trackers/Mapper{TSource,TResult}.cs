﻿namespace Gu.Reactive
{
    using System;
    using System.ComponentModel;
    using System.Runtime.CompilerServices;

    /// <summary>
    /// A reactive mapper from <typeparamref name="TSource"/> to <typeparamref name="TResult"/>.
    /// </summary>
    public sealed class Mapper<TSource, TResult> : ITracker<TResult>
    {
        private readonly IDisposable subscription;

        private bool disposed;
        private TResult value;

        /// <summary>
        /// Initializes a new instance of the <see cref="Mapper{TSource, TResult}"/> class.
        /// </summary>
#pragma warning disable CS8618 // Non-nullable field is uninitialized. Consider declaring as nullable.
        public Mapper(ITracker<TSource> source, Func<TSource, TResult> selector)
#pragma warning restore CS8618 // Non-nullable field is uninitialized. Consider declaring as nullable.
        {
            if (source is null)
            {
                throw new ArgumentNullException(nameof(source));
            }

            if (selector is null)
            {
                throw new ArgumentNullException(nameof(selector));
            }

            this.subscription = source.ObservePropertyChangedSlim(nameof(source.Value))
                                      .Subscribe(_ => this.Value = selector(source.Value));
        }

        /// <inheritdoc/>
        public event PropertyChangedEventHandler? PropertyChanged;

        /// <summary>
        /// Gets the mapped value.
        /// </summary>
        public TResult Value
        {
            get
            {
                this.ThrowIfDisposed();
                return this.value;
            }

            private set
            {
                if (Equals(value, this.value))
                {
                    return;
                }

                this.value = value;
                this.OnPropertyChanged();
            }
        }

        /// <inheritdoc/>
        public void Dispose()
        {
            if (this.disposed)
            {
                return;
            }

            this.disposed = true;
            this.subscription.Dispose();
        }

        private void ThrowIfDisposed()
        {
            if (this.disposed)
            {
                throw new ObjectDisposedException(typeof(Mapper<TSource, TResult>).FullName);
            }
        }

        private void OnPropertyChanged([CallerMemberName] string? propertyName = null)
        {
            this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
