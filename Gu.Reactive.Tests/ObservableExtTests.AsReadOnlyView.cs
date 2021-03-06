﻿namespace Gu.Reactive.Tests
{
    using System;
    using System.Collections.Generic;
    using System.Reactive.Linq;
    using System.Reactive.Subjects;

    using Gu.Reactive.Tests.Helpers;

    using NUnit.Framework;

    public static partial class ObservableExtTests
    {
        public static class AsReadOnlyView
        {
            [Test]
            public static void OnNextAddsOne()
            {
                using var subject = new Subject<IEnumerable<int>>();
                using var view = subject.AsReadOnlyView();
                using var actual = view.SubscribeAll();
                subject.OnNext(new[] { 1 });
                CollectionAssert.AreEqual(new[] { 1 }, view);
                var expected = new List<EventArgs>
                {
                    CachedEventArgs.CountPropertyChanged,
                    CachedEventArgs.IndexerPropertyChanged,
                    Diff.CreateAddEventArgs(1, 0),
                    CachedEventArgs.GetOrCreatePropertyChangedEventArgs("Source"),
                };
                CollectionAssert.AreEqual(expected, actual, EventArgsComparer.Default);

                subject.OnNext(new[] { 1, 2 });
                CollectionAssert.AreEqual(new[] { 1, 2 }, view);
                expected.AddRange(
                    new EventArgs[]
                    {
                        CachedEventArgs.CountPropertyChanged,
                        CachedEventArgs.IndexerPropertyChanged,
                        Diff.CreateAddEventArgs(2, 1),
                        CachedEventArgs.GetOrCreatePropertyChangedEventArgs("Source"),
                    });
                CollectionAssert.AreEqual(expected, actual, EventArgsComparer.Default);
            }

            [Test]
            public static void ObserveValueAsReadOnlyViewWhenIEnumerableOfT()
            {
                var with = new With<IEnumerable<int>>();
                using var view = with.ObserveValue(x => x.Value)
                                     .AsReadOnlyView();
                using var actual = view.SubscribeAll();
                with.Value = new[] { 1 };
                CollectionAssert.AreEqual(new[] { 1 }, view);
                var expected = new List<EventArgs>
                {
                    CachedEventArgs.CountPropertyChanged,
                    CachedEventArgs.IndexerPropertyChanged,
                    Diff.CreateAddEventArgs(1, 0),
                    CachedEventArgs.GetOrCreatePropertyChangedEventArgs("Source"),
                };
                CollectionAssert.AreEqual(expected, actual, EventArgsComparer.Default);

                with.Value = new[] { 1, 2 };
                CollectionAssert.AreEqual(new[] { 1, 2 }, view);
                expected.AddRange(
                    new EventArgs[]
                    {
                        CachedEventArgs.CountPropertyChanged,
                        CachedEventArgs.IndexerPropertyChanged,
                        Diff.CreateAddEventArgs(2, 1),
                        CachedEventArgs.GetOrCreatePropertyChangedEventArgs("Source"),
                    });
                CollectionAssert.AreEqual(expected, actual, EventArgsComparer.Default);
            }

            [Test]
            public static void ObserveValueAsReadOnlyViewWhenArrayOfTWhenCast()
            {
                var with = new With<int[]>();
                using var view = with.ObserveValue(x => x.Value).Select(x => (IMaybe<int[]>)x).AsReadOnlyView();
                using var actual = view.SubscribeAll();
                with.Value = new[] { 1 };
                CollectionAssert.AreEqual(new[] { 1 }, view);
                var expected = new List<EventArgs>
                {
                    CachedEventArgs.CountPropertyChanged,
                    CachedEventArgs.IndexerPropertyChanged,
                    Diff.CreateAddEventArgs(1, 0),
                    CachedEventArgs.GetOrCreatePropertyChangedEventArgs("Source"),
                };
                CollectionAssert.AreEqual(expected, actual, EventArgsComparer.Default);

                with.Value = new[] { 1, 2 };
                CollectionAssert.AreEqual(new[] { 1, 2 }, view);
                expected.AddRange(
                    new EventArgs[]
                    {
                        CachedEventArgs.CountPropertyChanged,
                        CachedEventArgs.IndexerPropertyChanged,
                        Diff.CreateAddEventArgs(2, 1),
                        CachedEventArgs.GetOrCreatePropertyChangedEventArgs("Source"),
                    });
                CollectionAssert.AreEqual(expected, actual, EventArgsComparer.Default);
            }

            [Test]
            public static void ObserveValueAsReadOnlyViewWhenArrayOfT()
            {
                var with = new With<int[]>();
                using var view = with.ObserveValue(x => x.Value)
                                     .Select(x => x.GetValueOrDefault())
                                     .AsReadOnlyView();
                using var actual = view.SubscribeAll();
                with.Value = new[] { 1 };
                CollectionAssert.AreEqual(new[] { 1 }, view);
                var expected = new List<EventArgs>
                {
                    CachedEventArgs.CountPropertyChanged,
                    CachedEventArgs.IndexerPropertyChanged,
                    Diff.CreateAddEventArgs(1, 0),
                    CachedEventArgs.GetOrCreatePropertyChangedEventArgs("Source"),
                };
                CollectionAssert.AreEqual(expected, actual, EventArgsComparer.Default);

                with.Value = new[] { 1, 2 };
                CollectionAssert.AreEqual(new[] { 1, 2 }, view);
                expected.AddRange(
                    new EventArgs[]
                    {
                        CachedEventArgs.CountPropertyChanged,
                        CachedEventArgs.IndexerPropertyChanged,
                        Diff.CreateAddEventArgs(2, 1),
                        CachedEventArgs.GetOrCreatePropertyChangedEventArgs("Source"),
                    });
                CollectionAssert.AreEqual(expected, actual, EventArgsComparer.Default);
            }
        }
    }
}
