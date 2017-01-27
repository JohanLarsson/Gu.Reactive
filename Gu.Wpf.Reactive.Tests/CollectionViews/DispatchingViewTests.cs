﻿namespace Gu.Wpf.Reactive.Tests.CollectionViews
{
    using System;
    using System.Collections.ObjectModel;
    using System.Threading;
    using System.Threading.Tasks;
    using System.Windows;

    using FakesAndHelpers;

    using Gu.Reactive;

    using NUnit.Framework;

    [Apartment(ApartmentState.STA)]
    public class DispatchingViewTests
    {
        [OneTimeSetUp]
        public void SetUp()
        {
            App.Start();
        }

        [Test]
        public async Task WhenAddToSource()
        {
            var source = new ObservableCollection<int>();
            var ourceChanges = source.SubscribeAll();

            var view = source.AsDispatchingView();
            var viewChanges = view.SubscribeAll();

            source.Add(1);
            await Application.Current.Dispatcher.SimulateYield();

            CollectionAssert.AreEqual(source, view);
            CollectionAssert.AreEqual(ourceChanges, viewChanges, EventArgsComparer.Default);
        }

        [Test]
        public async Task WhenAddToSourceExplicitZero()
        {
            var source = new ObservableCollection<int>();
            var ourceChanges = source.SubscribeAll();

            var view = source.AsDispatchingView(TimeSpan.Zero);
            var viewChanges = view.SubscribeAll();

            source.Add(1);
            await Application.Current.Dispatcher.SimulateYield();

            CollectionAssert.AreEqual(source, view);
            CollectionAssert.AreEqual(ourceChanges, viewChanges, EventArgsComparer.Default);
        }

        [Test]
        public async Task WhenAddToSourceWithBufferTime()
        {
            var source = new ObservableCollection<int>();
            var sourceChanges = source.SubscribeAll();

            var bufferTime = TimeSpan.FromMilliseconds(20);
            var view = source.AsDispatchingView(bufferTime);
            var viewChanges = view.SubscribeAll();

            source.Add(1);
            await Application.Current.Dispatcher.SimulateYield();
            CollectionAssert.IsEmpty(view);
            CollectionAssert.IsEmpty(viewChanges);

            await Task.Delay(bufferTime);
            await Application.Current.Dispatcher.SimulateYield();

            CollectionAssert.AreEqual(source, view);
            CollectionAssert.AreEqual(sourceChanges, viewChanges, EventArgsComparer.Default);
        }

        [Test]
        public async Task WhenManyAddsToSourceWithBufferTime()
        {
            var source = new ObservableCollection<int>();
            var bufferTime = TimeSpan.FromMilliseconds(20);
            var view = source.AsDispatchingView(bufferTime);
            var viewChanges = view.SubscribeAll();

            source.Add(1);
            source.Add(1);
            await Application.Current.Dispatcher.SimulateYield();
            CollectionAssert.IsEmpty(view);
            CollectionAssert.IsEmpty(viewChanges);

            await Task.Delay(bufferTime);
            await Task.Delay(bufferTime);
            await Application.Current.Dispatcher.SimulateYield();

            CollectionAssert.AreEqual(source, view);
            CollectionAssert.AreEqual(CachedEventArgs.ResetEventArgsCollection, viewChanges, EventArgsComparer.Default);
        }

        [Test]
        public async Task WhenAddToView()
        {
            var source = new ObservableCollection<int>();
            var sourceChanges = source.SubscribeAll();

            var view = source.AsDispatchingView();
            var viewChanges = view.SubscribeAll();

            view.Add(1);
            await Application.Current.Dispatcher.SimulateYield();

            CollectionAssert.AreEqual(source, view);
            CollectionAssert.AreEqual(sourceChanges, viewChanges, EventArgsComparer.Default);
        }

        [Test]
        public async Task WhenAddToViewExplicitZero()
        {
            var source = new ObservableCollection<int>();
            var sourceChanges = source.SubscribeAll();

            var view = source.AsDispatchingView(TimeSpan.Zero);
            var viewChanges = view.SubscribeAll();

            view.Add(1);
            await Application.Current.Dispatcher.SimulateYield();

            CollectionAssert.AreEqual(source, view);
            CollectionAssert.AreEqual(sourceChanges, viewChanges, EventArgsComparer.Default);
        }
    }
}
