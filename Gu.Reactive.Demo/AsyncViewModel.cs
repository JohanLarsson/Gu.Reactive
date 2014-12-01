﻿namespace Gu.Reactive.Demo
{
    using System;
    using System.ComponentModel;
    using System.Runtime.CompilerServices;
    using System.Threading.Tasks;
    using Gu.Reactive.Demo.Annotations;
    using Gu.Wpf.Reactive;

    public class AsyncViewModel : INotifyPropertyChanged
    {
        private int _delay = 2000;

        public AsyncViewModel()
        {
            AsyncCommand = new AsyncCommand(() => VoidTaskMethod(() => { }))
                               {
                                   ToolTipText = "AsyncCommand"
                               };

            AsyncResultCommand = new AsyncResultCommand<int>(() => ResultTaskMethod(() => 5))
            {
                ToolTipText = "AsyncResultCommand"
            };

            AsyncThrowCommand = new AsyncCommand(() => VoidTaskMethod(() => { throw new Exception("message"); }))
                                    {
                                        ToolTipText = "AsyncThrowCommand"
                                    };

            AsyncResultThrowCommand = new AsyncResultCommand<int>(() => ResultTaskMethod<int>(() => { throw new Exception("message"); }))
            {
                ToolTipText = "AsyncResultThrowCommand"
            };
        }

        public event PropertyChangedEventHandler PropertyChanged;

        public AsyncCommand AsyncCommand { get; private set; }
        
        public AsyncResultCommand<int> AsyncResultCommand { get; private set; }

        public AsyncCommand AsyncThrowCommand { get; private set; }
        
        public AsyncResultCommand<int> AsyncResultThrowCommand { get; private set; }

        public int Delay
        {
            get
            {
                return _delay;
            }
            set
            {
                if (value == _delay)
                {
                    return;
                }
                _delay = value;
                OnPropertyChanged();
            }
        }

        public async Task VoidTaskMethod(Action action)
        {
            await Task.Delay(Delay);
            action();
        }

        public async Task<T> ResultTaskMethod<T>(Func<T> action)
        {
            await Task.Delay(Delay);
            return action();
        }

        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            var handler = PropertyChanged;
            if (handler != null)
            {
                handler(this, new PropertyChangedEventArgs(propertyName));
            }
        }
    }
}
