namespace Gu.Reactive.Tests.Collections
{
    using System;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;

    using Gu.Reactive.Tests.Helpers;

    using Microsoft.Reactive.Testing;

    using NUnit.Framework;

    public class ReadOnlyFilteredViewTests
    {
        [Test]
        public void InitializeFiltered()
        {
            var source = new ObservableCollection<int> { 1, 2 };
            var scheduler = new TestScheduler();
            using (var view = source.AsReadOnlyFilteredView(x => x < 2, scheduler))
            {
                CollectionAssert.AreEqual(new[] { 1 }, view);
            }
        }

        [Test]
        public void InitializeFilteredBuffered()
        {
            var source = new ObservableCollection<int> { 1, 2 };
            var scheduler = new TestScheduler();
            using (var view = source.AsReadOnlyFilteredView(x => x < 2, TimeSpan.FromMilliseconds(100), scheduler))
            {
                CollectionAssert.AreEqual(new[] { 1 }, view);
            }
        }

        [Test]
        public void NotifiesAdd()
        {
            var source = new ObservableCollection<int>();
            var scheduler = new TestScheduler();
            using (var view = source.AsReadOnlyFilteredView(x => true, scheduler))
            {
                using (var expected = source.SubscribeAll())
                {
                    using (var actual = view.SubscribeAll())
                    {
                        source.Add(1);
                        scheduler.Start();
                        CollectionAssert.AreEqual(expected, actual, EventArgsComparer.Default);
                    }
                }
            }
        }

        [Test]
        public void NotifiesAddBuffered()
        {
            var source = new ObservableCollection<int>();
            var scheduler = new TestScheduler();
            using (var view = source.AsReadOnlyFilteredView(x => true, TimeSpan.FromMilliseconds(100), scheduler))
            {
                using (var expected = source.SubscribeAll())
                {
                    using (var actual = view.SubscribeAll())
                    {
                        source.Add(1);
                        scheduler.Start();
                        CollectionAssert.AreEqual(expected, actual, EventArgsComparer.Default);
                    }
                }
            }
        }

        [Test]
        public void AddFiltered()
        {
            var source = new ObservableCollection<int>();
            var scheduler = new TestScheduler();
            using (var view = source.AsReadOnlyFilteredView(x => x % 2 == 0, scheduler))
            {
                using (var changes = view.SubscribeAll())
                {
                    source.Add(1);
                    scheduler.Start();
                    CollectionAssert.IsEmpty(view);
                    CollectionAssert.IsEmpty(changes);
                }
            }
        }

        [Test]
        public void AddFilteredBuffered()
        {
            var source = new ObservableCollection<int>();
            var scheduler = new TestScheduler();
            using (var view = source.AsReadOnlyFilteredView(x => x % 2 == 0, TimeSpan.FromMilliseconds(100), scheduler))
            {
                using (var changes = view.SubscribeAll())
                {
                    source.Add(1);
                    scheduler.Start();
                    CollectionAssert.IsEmpty(view);
                    CollectionAssert.IsEmpty(changes);
                }
            }
        }

        [Test]
        public void AddManyFiltered()
        {
            var source = new ObservableCollection<int>();
            var scheduler = new TestScheduler();
            using (var view = source.AsReadOnlyFilteredView(x => x % 2 == 0, scheduler))
            {
                using (var changes = view.SubscribeAll())
                {
                    source.Add(1);
                    source.Add(3);
                    source.Add(5);
                    scheduler.Start();
                    CollectionAssert.IsEmpty(view);
                    CollectionAssert.IsEmpty(changes);
                }
            }
        }

        [Test]
        public void AddManyFilteredBuffered()
        {
            var source = new ObservableCollection<int>();
            var scheduler = new TestScheduler();
            using (var view = source.AsReadOnlyFilteredView(x => x % 2 == 0, TimeSpan.FromMilliseconds(100), scheduler))
            {
                using (var changes = view.SubscribeAll())
                {
                    source.Add(1);
                    source.Add(3);
                    source.Add(5);
                    scheduler.Start();
                    CollectionAssert.IsEmpty(view);
                    CollectionAssert.IsEmpty(changes);
                }
            }
        }

        [Test]
        public void AddVisibleWhenFiltered()
        {
            var source = new ObservableCollection<int>();
            var scheduler = new TestScheduler();
            using (var view = source.AsReadOnlyFilteredView(x => x % 2 == 0, scheduler))
            {
                scheduler.Start();
                using (var actual = view.SubscribeAll())
                {
                    CollectionAssert.IsEmpty(view);
                    CollectionAssert.IsEmpty(actual);

                    source.Add(2);
                    scheduler.Start();
                    CollectionAssert.AreEqual(new[] { 2 }, view);
                    var expected = new EventArgs[]
                                       {
                                           CachedEventArgs.CountPropertyChanged,
                                           CachedEventArgs.IndexerPropertyChanged,
                                           Diff.CreateAddEventArgs(2, 0)
                                       };
                    CollectionAssert.AreEqual(expected, actual, EventArgsComparer.Default);
                }
            }
        }

        [Test]
        public void AddVisibleWhenFilteredBuffered()
        {
            var source = new ObservableCollection<int>();
            var scheduler = new TestScheduler();
            using (var view = source.AsReadOnlyFilteredView(x => x % 2 == 0, TimeSpan.FromMilliseconds(100), scheduler))
            {
                scheduler.Start();
                using (var actual = view.SubscribeAll())
                {
                    CollectionAssert.IsEmpty(view);
                    CollectionAssert.IsEmpty(actual);

                    source.Add(2);
                    scheduler.Start();
                    CollectionAssert.AreEqual(new[] { 2 }, view);
                    var expected = new EventArgs[]
                                       {
                                               CachedEventArgs.CountPropertyChanged,
                                               CachedEventArgs.IndexerPropertyChanged,
                                               Diff.CreateAddEventArgs(2, 0)
                                       };
                    CollectionAssert.AreEqual(expected, actual, EventArgsComparer.Default);
                }
            }
        }

        [Test]
        public void AddManyVisibleWhenFiltered()
        {
            var source = new ObservableCollection<int>();
            var scheduler = new TestScheduler();
            using (var view = source.AsReadOnlyFilteredView(x => x % 2 == 0, scheduler))
            {
                scheduler.Start();
                CollectionAssert.IsEmpty(view);
                using (var actual = view.SubscribeAll())
                {
                    source.Add(1);
                    scheduler.Start();
                    CollectionAssert.IsEmpty(view);
                    var expected = new List<EventArgs>();
                    CollectionAssert.AreEqual(expected, actual, EventArgsComparer.Default);

                    source.Add(2);
                    scheduler.Start();
                    CollectionAssert.AreEqual(new[] { 2 }, view);
                    expected.AddRange(
                        new EventArgs[]
                            {
                                CachedEventArgs.CountPropertyChanged,
                                CachedEventArgs.IndexerPropertyChanged,
                                Diff.CreateAddEventArgs(2, 0),
                            });
                    CollectionAssert.AreEqual(expected, actual, EventArgsComparer.Default);

                    source.Add(3);
                    scheduler.Start();
                    CollectionAssert.AreEqual(new[] { 2 }, view);
                    CollectionAssert.AreEqual(expected, actual, EventArgsComparer.Default);

                    source.Add(4);
                    scheduler.Start();
                    CollectionAssert.AreEqual(new[] { 2, 4 }, view);
                    expected.AddRange(
                        new EventArgs[]
                            {
                                CachedEventArgs.CountPropertyChanged,
                                CachedEventArgs.IndexerPropertyChanged,
                                Diff.CreateAddEventArgs(4, 1),
                            });
                    CollectionAssert.AreEqual(expected, actual, EventArgsComparer.Default);

                    source.Add(5);
                    scheduler.Start();
                    CollectionAssert.AreEqual(new[] { 2, 4 }, view);
                    CollectionAssert.AreEqual(expected, actual, EventArgsComparer.Default);

                    source.Add(6);
                    scheduler.Start();
                    CollectionAssert.AreEqual(new[] { 2, 4, 6 }, view);
                    expected.AddRange(
                        new EventArgs[]
                            {
                                CachedEventArgs.CountPropertyChanged,
                                CachedEventArgs.IndexerPropertyChanged,
                                Diff.CreateAddEventArgs(6, 2),
                            });
                    CollectionAssert.AreEqual(expected, actual, EventArgsComparer.Default);
                }
            }
        }

        [Test]
        public void AddManyVisibleWhenFilteredBuffered()
        {
            var source = new ObservableCollection<int>();
            var scheduler = new TestScheduler();
            using (var view = source.AsReadOnlyFilteredView(x => x % 2 == 0, TimeSpan.FromMilliseconds(100), scheduler))
            {
                using (var changes = view.SubscribeAll())
                {
                    source.Add(1);
                    source.Add(2);
                    source.Add(3);
                    source.Add(4);
                    source.Add(5);
                    source.Add(6);
                    scheduler.Start();
                    CollectionAssert.AreEqual(new[] { 2, 4, 6 }, view);
                    var expected = new EventArgs[]
                                       {
                                               CachedEventArgs.CountPropertyChanged,
                                               CachedEventArgs.IndexerPropertyChanged,
                                               CachedEventArgs.NotifyCollectionReset
                                       };
                    CollectionAssert.AreEqual(expected, changes, EventArgsComparer.Default);
                }
            }
        }

        [Test]
        public void RemoveFiltered()
        {
            var source = new ObservableCollection<int> { 1 };
            var scheduler = new TestScheduler();
            using (var view = source.AsReadOnlyFilteredView(x => x % 2 == 0, scheduler))
            {
                using (var changes = view.SubscribeAll())
                {
                    source.Remove(1);
                    scheduler.Start();
                    CollectionAssert.IsEmpty(view);
                    CollectionAssert.IsEmpty(changes);
                }
            }
        }

        [Test]
        public void RemoveFilteredBuffered()
        {
            var source = new ObservableCollection<int> { 1 };
            var scheduler = new TestScheduler();
            using (var view = source.AsReadOnlyFilteredView(x => x % 2 == 0, TimeSpan.FromMilliseconds(100), scheduler))
            {
                using (var changes = view.SubscribeAll())
                {
                    source.Remove(1);
                    scheduler.Start();
                    CollectionAssert.IsEmpty(view);
                    CollectionAssert.IsEmpty(changes);
                }
            }
        }

        [Test]
        public void RemoveVisible()
        {
            var source = new ObservableCollection<int> { 1, 2, 3 };
            var scheduler = new TestScheduler();
            using (var view = source.AsReadOnlyFilteredView(x => x % 2 == 0, scheduler))
            {
                scheduler.Start();
                using (var actual = view.SubscribeAll())
                {
                    source.Remove(2);
                    scheduler.Start();
                    CollectionAssert.IsEmpty(view);
                    var expected = new EventArgs[]
                                       {
                                           CachedEventArgs.CountPropertyChanged,
                                           CachedEventArgs.IndexerPropertyChanged,
                                           Diff.CreateRemoveEventArgs(2, 0)
                                       };
                    CollectionAssert.AreEqual(expected, actual, EventArgsComparer.Default);
                }
            }
        }

        [Test]
        public void RemoveVisibleBuffered()
        {
            var source = new ObservableCollection<int> { 1, 2, 3 };
            var scheduler = new TestScheduler();
            using (var view = source.AsReadOnlyFilteredView(x => x % 2 == 0, TimeSpan.FromMilliseconds(100), scheduler))
            {
                scheduler.Start();
                using (var actual = view.SubscribeAll())
                {
                    CollectionAssert.AreEqual(new[] { 2 }, view);
                    CollectionAssert.IsEmpty(actual);

                    source.Remove(2);
                    scheduler.Start();
                    CollectionAssert.IsEmpty(view);
                    var expected = new EventArgs[]
                                       {
                                           CachedEventArgs.CountPropertyChanged,
                                           CachedEventArgs.IndexerPropertyChanged,
                                           Diff.CreateRemoveEventArgs(2, 0)
                                       };
                    CollectionAssert.AreEqual(expected, actual, EventArgsComparer.Default);
                }
            }
        }

        [Test]
        public void ReplaceFiltered()
        {
            var source = new ObservableCollection<int> { 1 };
            var scheduler = new TestScheduler();
            using (var view = source.AsReadOnlyFilteredView(x => x % 2 == 0, scheduler))
            {
                using (var changes = view.SubscribeAll())
                {
                    source[0] = 3;
                    scheduler.Start();
                    CollectionAssert.IsEmpty(view);
                    CollectionAssert.IsEmpty(changes);
                }
            }
        }

        [Test]
        public void ReplaceFilteredBuffered()
        {
            var source = new ObservableCollection<int> { 1 };
            var scheduler = new TestScheduler();
            using (var view = source.AsReadOnlyFilteredView(x => x % 2 == 0, TimeSpan.FromMilliseconds(100), scheduler))
            {
                using (var changes = view.SubscribeAll())
                {
                    source[0] = 3;
                    scheduler.Start();
                    CollectionAssert.IsEmpty(view);
                    CollectionAssert.IsEmpty(changes);
                }
            }
        }

        [Test]
        public void ReplaceVisible()
        {
            var source = new ObservableCollection<int> { 1, 2, 3 };
            var scheduler = new TestScheduler();
            using (var view = source.AsReadOnlyFilteredView(x => x % 2 == 0, scheduler))
            {
                scheduler.Start();
                using (var actual = view.SubscribeAll())
                {
                    CollectionAssert.AreEqual(new[] { 2 }, view);
                    CollectionAssert.IsEmpty(actual);

                    source[1] = 4;
                    scheduler.Start();
                    CollectionAssert.AreEqual(new[] { 4 }, view);
                    var expected = new EventArgs[]
                                       {
                                           CachedEventArgs.IndexerPropertyChanged,
                                           Diff.CreateReplaceEventArgs(4, 2, 0)
                                       };
                    CollectionAssert.AreEqual(expected, actual, EventArgsComparer.Default);
                }
            }
        }

        [Test]
        public void ReplaceVisibleBuffered()
        {
            var source = new ObservableCollection<int> { 1, 2, 3 };
            var scheduler = new TestScheduler();
            using (var view = source.AsReadOnlyFilteredView(x => x % 2 == 0, TimeSpan.FromMilliseconds(100), scheduler))
            {
                scheduler.Start();
                scheduler.Start();
                using (var actual = view.SubscribeAll())
                {
                    CollectionAssert.AreEqual(new[] { 2 }, view);
                    CollectionAssert.IsEmpty(actual);

                    source[1] = 4;
                    scheduler.Start();
                    CollectionAssert.AreEqual(new[] { 4 }, view);
                    var expected = new EventArgs[]
                                       {
                                           CachedEventArgs.IndexerPropertyChanged,
                                           Diff.CreateReplaceEventArgs(4, 2, 0)
                                       };
                    CollectionAssert.AreEqual(expected, actual, EventArgsComparer.Default);
                }
            }
        }

        [Test]
        public void ReplaceFilteredWithVisible()
        {
            var source = new ObservableCollection<int> { 1 };
            var scheduler = new TestScheduler();
            using (var view = source.AsReadOnlyFilteredView(x => x % 2 == 0, scheduler))
            {
                scheduler.Start();
                using (var changes = view.SubscribeAll())
                {
                    source[0] = 2;
                    scheduler.Start();
                    CollectionAssert.AreEqual(new[] { 2 }, view);
                    var expected = new EventArgs[]
                                       {
                                           CachedEventArgs.CountPropertyChanged,
                                           CachedEventArgs.IndexerPropertyChanged,
                                           Diff.CreateAddEventArgs(2, 0)
                                       };
                    CollectionAssert.AreEqual(expected, changes, EventArgsComparer.Default);
                }
            }
        }

        [Test]
        public void ReplaceFilteredWithVisibleBuffered()
        {
            var source = new ObservableCollection<int> { 1 };
            var scheduler = new TestScheduler();
            using (var view = source.AsReadOnlyFilteredView(x => x % 2 == 0, TimeSpan.FromMilliseconds(100), scheduler))
            {
                using (var changes = view.SubscribeAll())
                {
                    source[0] = 2;
                    scheduler.Start();
                    CollectionAssert.AreEqual(new[] { 2 }, view);
                    var expected = new EventArgs[]
                                       {
                                           CachedEventArgs.CountPropertyChanged,
                                           CachedEventArgs.IndexerPropertyChanged,
                                           Diff.CreateAddEventArgs(2, 0)
                                       };
                    CollectionAssert.AreEqual(expected, changes, EventArgsComparer.Default);
                }
            }
        }
    }
}