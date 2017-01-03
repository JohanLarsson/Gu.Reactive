﻿namespace Gu.Wpf.Reactive
{
    using System;
    using System.ComponentModel;
    using System.Reactive.Linq;
    using System.Runtime.CompilerServices;
    using System.Threading.Tasks;

    using Gu.Reactive;
    using JetBrains.Annotations;

    public abstract class TaskRunnerBase : INotifyPropertyChanged, IDisposable
    {
        private NotifyTaskCompletion taskCompletion;

        private bool disposed;

        /// <inheritdoc/>
        public virtual event PropertyChangedEventHandler PropertyChanged;

        protected TaskRunnerBase()
        {
            var observable = this.ObservePropertyChanged(x => x.TaskCompletion.Status);
            this.CanRunCondition = new Condition(observable, this.CanRun) { Name = "CanRun" };
            this.CanCancelCondition = new Condition(Observable.Empty<object>(), () => false) { Name = "CanCancel" };
        }

        public NotifyTaskCompletion TaskCompletion
        {
            get
            {
                return this.taskCompletion;
            }

            protected set
            {
                if (Equals(value, this.taskCompletion))
                {
                    return;
                }
                this.taskCompletion = value;
                this.OnPropertyChanged();
            }
        }

        public virtual ICondition CanCancelCondition { get; }

        public ICondition CanRunCondition { get; }

        public bool? CanRun()
        {
            var completion = this.TaskCompletion;
            if (completion == null)
            {
                return true;
            }

            switch (completion.Task.Status)
            {
                case TaskStatus.Created:
                case TaskStatus.WaitingForActivation:
                case TaskStatus.WaitingToRun:
                case TaskStatus.Running:
                case TaskStatus.WaitingForChildrenToComplete:
                    return false;
                case TaskStatus.RanToCompletion:
                case TaskStatus.Canceled:
                case TaskStatus.Faulted:
                    return true;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        public virtual void Cancel()
        {
            // intentional NOP
        }

        /// <inheritdoc/>
        public void Dispose()
        {
            this.Dispose(true);
        }

        /// <summary>
        /// Protected implementation of Dispose pattern.
        /// </summary>
        /// <param name="disposing">true: safe to free managed resources</param>
        protected virtual void Dispose(bool disposing)
        {
            if (this.disposed)
            {
                return;
            }

            this.disposed = true;
            if (disposing)
            {
                this.TaskCompletion?.Task.Dispose();
            }
        }

        protected void VerifyDisposed()
        {
            if (this.disposed)
            {
                throw new ObjectDisposedException(
                    this.GetType()
                        .FullName);
            }
        }

        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}