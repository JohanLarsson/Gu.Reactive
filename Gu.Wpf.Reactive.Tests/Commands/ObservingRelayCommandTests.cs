﻿namespace Gu.Wpf.Reactive.Tests
{
    using System;
    using System.Reactive.Subjects;

    using Gu.Reactive;
    using Gu.Reactive.Tests.Helpers;

    using NUnit.Framework;

    public class ObservingRelayCommandTests
    {
        [Test]
        public void NotifiesOnConditionChanged()
        {
            var fake = new Fake { IsTrueOrNull = false };
            int count;
            using (var command = new ObservingRelayCommand(() => { }, () => false, fake.ObservePropertyChanged(x => x.IsTrueOrNull)))
            {
                count = 0;
                command.CanExecuteChanged += (sender, args) => count++;
                fake.IsTrueOrNull = true;
                Assert.AreEqual(1, count);
            }
        }

        [TestCase(true)]
        [TestCase(false)]
        public void CanExecuteCondition(bool expected)
        {
            using (var observable = new Subject<object>())
            {
                using (var command = new ObservingRelayCommand(() => { }, () => expected, observable))
                {
                    Assert.AreEqual(expected, command.CanExecute());
                }
            }
        }

        [TestCase(true)]
        [TestCase(false)]
        public void CanExecuteConditionParameter(bool expected)
        {
            using (var observable = new Subject<object>())
            {
                using (var command = new ObservingRelayCommand<int>(_ => { }, _ => expected, observable))
                {
                    Assert.AreEqual(expected, command.CanExecute(0));
                }
            }
        }

        [Test]
        public void ExecuteNotifies()
        {
            var invokeCount = 0;
            var isExecutingCount = 0;
            using (var command = new ObservingRelayCommand(() => invokeCount++, () => true, new Subject<object>()))
            {
                using (command.ObservePropertyChangedSlim(nameof(command.IsExecuting), false)
                              .Subscribe(_ => isExecutingCount++))
                {
                    Assert.IsFalse(command.IsExecuting);
                    Assert.True(command.CanExecute());
                    command.Execute();
                    Assert.IsFalse(command.IsExecuting);
                    Assert.True(command.CanExecute());
                    Assert.AreEqual(1, invokeCount);
                    Assert.AreEqual(2, isExecutingCount);
                }
            }
        }
    }
}