﻿namespace Gu.Wpf.Reactive
{
    using System;
    using System.ComponentModel;
    using System.Runtime.CompilerServices;
    using System.Threading.Tasks;

    /// <summary>
    /// A notifying view of a task execution.
    /// </summary>
    /// <typeparam name="T">The type of the task.</typeparam>
    public abstract class NotifyTaskCompletionBase<T> : INotifyPropertyChanged
        where T : Task
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="NotifyTaskCompletionBase{T}"/> class.
        /// </summary>
        /// <param name="task">The task to run and notify status for.</param>
        protected NotifyTaskCompletionBase(T task)
        {
            this.Task = task ?? throw new ArgumentNullException(nameof(task));
            if (!task.IsCompleted)
            {
                this.AwaitTask(task);
            }
        }

        /// <inheritdoc/>
        public event PropertyChangedEventHandler? PropertyChanged;

        /// <summary>
        /// Gets the current task.
        /// </summary>
        public T Task { get; }

        /// <summary>
        /// Gets the current status of the <see cref="Task"/>.
        /// </summary>
        public TaskStatus Status => this.Task.Status;

        /// <summary>
        /// Gets a value indicating whether the current status of the <see cref="Task"/>.
        /// </summary>
        public bool IsCompleted => this.Task.IsCompleted;

        /// <summary>
        /// Gets a value indicating whether the current status of the <see cref="Task"/>.
        /// </summary>
        public bool IsNotCompleted => !this.Task.IsCompleted;

        /// <summary>
        /// Gets a value indicating whether the current status of the <see cref="Task"/>.
        /// </summary>
        public bool IsSuccessfullyCompleted => this.Task.Status == TaskStatus.RanToCompletion;

        /// <summary>
        /// Gets a value indicating whether the current status of the <see cref="Task"/>.
        /// </summary>
        public bool IsCanceled => this.Task.IsCanceled;

        /// <summary>
        /// Gets a value indicating whether the current status of the <see cref="Task"/>.
        /// </summary>
        public bool IsFaulted => this.Task.IsFaulted;

        /// <summary>
        /// Gets the exception produced by the run if any.
        /// </summary>
        public AggregateException? Exception => this.Task.Exception;

        /// <summary>
        /// Gets the inner exception produced by the run if any.
        /// </summary>
        public Exception? InnerException => this.Exception?.InnerException;

        /// <summary>
        /// Gets the exception message produced by the run if any.
        /// </summary>
        public string? ErrorMessage => this.InnerException?.Message;

        /// <summary>
        /// Gets null if the run is not completed.
        /// </summary>
        public T? Completed => this.Task.IsCompleted
            ? this.Task
            : null;

        /// <summary>
        /// Called after awaiting the task.
        /// </summary>
        protected virtual void OnCompleted()
        {
            this.OnPropertyChanged(nameof(this.Completed));
            this.OnPropertyChanged(nameof(this.Status));
            this.OnPropertyChanged(nameof(this.IsCompleted));
            this.OnPropertyChanged(nameof(this.IsSuccessfullyCompleted));
            this.OnPropertyChanged(nameof(this.IsNotCompleted));
        }

        /// <summary>
        /// Notifies that <paramref name="propertyName"/> changed.
        /// </summary>
        /// <param name="propertyName">The name of the property.</param>
        protected virtual void OnPropertyChanged([CallerMemberName] string? propertyName = null)
        {
            this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

#pragma warning disable VSTHRD100 // Avoid async void
        private async void AwaitTask(T task)
#pragma warning restore VSTHRD100 // Avoid async void
        {
            try
            {
                await task.ConfigureAwait(false);
            }

            // ReSharper disable once EmptyGeneralCatchClause We don't want to propagate errors here. Just make them bindable.
#pragma warning disable CA1031 // Do not catch general exception types
            catch
            {
            }
#pragma warning restore CA1031 // Do not catch general exception types

            if (task.Status == TaskStatus.RanToCompletion)
            {
                this.OnCompleted();
            }

            var handler = this.PropertyChanged;
            if (handler is null)
            {
                return;
            }

            handler(this, new PropertyChangedEventArgs(nameof(this.Status)));
            handler(this, new PropertyChangedEventArgs(nameof(this.IsCompleted)));
            handler(this, new PropertyChangedEventArgs(nameof(this.IsNotCompleted)));
            if (task.IsCanceled)
            {
                handler(this, new PropertyChangedEventArgs(nameof(this.IsCanceled)));
            }
            else if (task.IsFaulted)
            {
                handler(this, new PropertyChangedEventArgs(nameof(this.IsFaulted)));
                handler(this, new PropertyChangedEventArgs(nameof(this.Exception)));
                handler(this, new PropertyChangedEventArgs(nameof(this.InnerException)));
                handler(this, new PropertyChangedEventArgs(nameof(this.ErrorMessage)));
            }
        }
    }
}
