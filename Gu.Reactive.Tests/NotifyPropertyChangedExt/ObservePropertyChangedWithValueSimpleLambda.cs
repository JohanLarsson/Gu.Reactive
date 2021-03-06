﻿// ReSharper disable RedundantArgumentDefaultValue
namespace Gu.Reactive.Tests.NotifyPropertyChangedExt
{
    using System;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.Linq;
    using System.Reactive;

    using Gu.Reactive.Tests.Helpers;

    using NUnit.Framework;

    [Obsolete("Testing obsolete API.")]
    public class ObservePropertyChangedWithValueSimpleLambda
    {
        [TestCase(true)]
        [TestCase(false)]
        public void SignalInitialWhenValueIsNull(bool signalInitial)
        {
            var changes = new List<EventPattern<PropertyChangedAndValueEventArgs<string?>>>();
            var fake = new Fake();
            using (fake.ObservePropertyChangedWithValue(x => x.Name, signalInitial)
                       .Subscribe(changes.Add))
            {
                if (signalInitial)
                {
                    EventPatternAssert.AreEqual(fake, string.Empty, Maybe<string?>.Some(null), changes.Single());

                    fake.Name = "Johan";
                    Assert.AreEqual(2, changes.Count);
                    EventPatternAssert.AreEqual(fake, "Name", Maybe<string?>.Some("Johan"), changes.Last());

                    using (fake.ObservePropertyChangedWithValue(x => x.Name, signalInitial: true)
                               .Subscribe(changes.Add))
                    {
                        Assert.AreEqual(3, changes.Count);
                        EventPatternAssert.AreEqual(fake, string.Empty, Maybe<string?>.Some("Johan"), changes.Last());
                    }
                }
                else
                {
                    CollectionAssert.IsEmpty(changes);
                }
            }
        }

        [TestCase(true)]
        [TestCase(false)]
        public void TwoLevelsSignalInitialWhenHasValue(bool signalInitial)
        {
            var changes = new List<EventPattern<PropertyChangedAndValueEventArgs<string?>>>();
            var fake = new Fake { Level1 = new Level1 { Name = "Johan" } };
            using (fake.ObservePropertyChangedWithValue(x => x.Level1.Name, signalInitial)
                       .Subscribe(changes.Add))
            {
                if (signalInitial)
                {
                    EventPatternAssert.AreEqual(fake.Level1, string.Empty, Maybe<string?>.Some("Johan"), changes.Single());

                    fake.Level1.Name = "Erik";
                    Assert.AreEqual(2, changes.Count);
                    EventPatternAssert.AreEqual(fake.Level1, "Name", Maybe<string?>.Some("Erik"), changes.Last());

                    using (fake.ObservePropertyChangedWithValue(x => x.Level1.Name, signalInitial: true)
                               .Subscribe(changes.Add))
                    {
                        Assert.AreEqual(3, changes.Count);
                        EventPatternAssert.AreEqual(fake.Level1, string.Empty, Maybe<string?>.Some("Erik"), changes.Last());
                    }
                }
                else
                {
                    CollectionAssert.IsEmpty(changes);
                }
            }
        }

        [TestCase(true)]
        [TestCase(false)]
        public void TwoLevelsSignalInitialWhenMissing(bool signalInitial)
        {
            var changes = new List<EventPattern<PropertyChangedAndValueEventArgs<string?>>>();
            var fake = new Fake();
            using (fake.ObservePropertyChangedWithValue(x => x.Level1.Name, signalInitial)
                       .Subscribe(changes.Add))
            {
                if (signalInitial)
                {
                    EventPatternAssert.AreEqual(fake, string.Empty, Maybe<string?>.None, changes.Single());

                    fake.Level1 = new Level1 { Name = "Johan" };
                    Assert.AreEqual(2, changes.Count);
                    EventPatternAssert.AreEqual(fake, "Level1", Maybe<string?>.Some("Johan"), changes.Last());

                    fake.Level1.Name = "Erik";
                    Assert.AreEqual(3, changes.Count);
                    EventPatternAssert.AreEqual(fake.Level1, "Name", Maybe<string?>.Some("Erik"), changes.Last());

                    using (fake.ObservePropertyChangedWithValue(x => x.Level1.Name, signalInitial: true)
                               .Subscribe(changes.Add))
                    {
                        Assert.AreEqual(4, changes.Count);
                        EventPatternAssert.AreEqual(fake.Level1, string.Empty, Maybe<string?>.Some("Erik"), changes.Last());
                    }
                }
                else
                {
                    CollectionAssert.IsEmpty(changes);
                }
            }
        }

        [TestCase(true)]
        [TestCase(false)]
        public void TwoLevelsSignalInitialWhenValueIsNull(bool signalInitial)
        {
            var changes = new List<EventPattern<PropertyChangedAndValueEventArgs<string?>>>();
            var fake = new Fake { Level1 = new Level1() };
            using (fake.ObservePropertyChangedWithValue(x => x.Level1.Name, signalInitial)
                       .Subscribe(changes.Add))
            {
                if (signalInitial)
                {
                    EventPatternAssert.AreEqual(fake.Level1, string.Empty, Maybe<string?>.Some(null), changes.Single());

                    fake.Level1.Name = "Johan";
                    Assert.AreEqual(2, changes.Count);
                    EventPatternAssert.AreEqual(fake.Level1, "Name", Maybe<string?>.Some("Johan"), changes.Last());

                    using (fake.ObservePropertyChangedWithValue(x => x.Level1.Name, signalInitial: true)
                               .Subscribe(changes.Add))
                    {
                        Assert.AreEqual(3, changes.Count);
                        EventPatternAssert.AreEqual(fake.Level1, string.Empty, Maybe<string?>.Some("Johan"), changes.Last());
                    }
                }
                else
                {
                    CollectionAssert.IsEmpty(changes);
                }
            }
        }

        [TestCase(true)]
        [TestCase(false)]
        public void SignalInitialWhenHasValue(bool signalInitial)
        {
            var changes = new List<EventPattern<PropertyChangedAndValueEventArgs<string?>>>();
            var fake = new Fake { Name = "Johan" };
            using (fake.ObservePropertyChangedWithValue(x => x.Name, signalInitial)
                       .Subscribe(changes.Add))
            {
                if (signalInitial)
                {
                    EventPatternAssert.AreEqual(fake, string.Empty, Maybe<string?>.Some("Johan"), changes.Single());

                    fake.Name = "Erik";
                    Assert.AreEqual(2, changes.Count);
                    EventPatternAssert.AreEqual(fake, "Name", Maybe<string?>.Some("Erik"), changes.Last());

                    using (fake.ObservePropertyChangedWithValue(x => x.Name, signalInitial: true)
                               .Subscribe(changes.Add))
                    {
                        Assert.AreEqual(3, changes.Count);
                        EventPatternAssert.AreEqual(fake, string.Empty, Maybe<string?>.Some("Erik"), changes.Last());
                    }
                }
                else
                {
                    CollectionAssert.IsEmpty(changes);
                }
            }
        }

        [Test]
        public void ReadOnlyObservableCollectionCount()
        {
            var ints = new ObservableCollection<int>();
            var source = new ReadOnlyObservableCollection<int>(ints);
            var changes = new List<EventPattern<PropertyChangedAndValueEventArgs<int>>>();
            using (source.ObservePropertyChangedWithValue(x => x.Count, signalInitial: false)
                         .Subscribe(x => changes.Add(x)))
            {
                CollectionAssert.IsEmpty(changes);

                ints.Add(1);
                EventPatternAssert.AreEqual(source, "Count", Maybe<int>.Some(1), changes.Single());

                ints.Add(2);
                Assert.AreEqual(2, changes.Count);
                EventPatternAssert.AreEqual(source, "Count", Maybe<int>.Some(2), changes.Last());
            }
        }

        [Test]
        public void DoesNotSignalOnSubscribe()
        {
            var changes = new List<EventPattern<PropertyChangedAndValueEventArgs<string?>>>();
            var fake = new Fake { Name = "Johan" };
            using (fake.ObservePropertyChangedWithValue(x => x.Name, signalInitial: false)
                       .Subscribe(changes.Add))
            {
                CollectionAssert.IsEmpty(changes);
            }
        }

        [Test]
        public void SignalsOnSourceChanges()
        {
            var changes = new List<EventPattern<PropertyChangedAndValueEventArgs<string?>>>();
            var fake = new Fake();
            using (fake.ObservePropertyChangedWithValue(x => x.Name, signalInitial: false)
                       .Subscribe(changes.Add))
            {
                CollectionAssert.IsEmpty(changes);

                fake.Name = "Johan";
                EventPatternAssert.AreEqual(fake, "Name", Maybe.Some<string?>("Johan"), changes.Single());
            }
        }

        [Test]
        public void SignalsOnDerivedSourceChanges()
        {
            var changes = new List<EventPattern<PropertyChangedAndValueEventArgs<string?>>>();
            var fake = new DerivedFake();
            using (fake.ObservePropertyChangedWithValue(x => x.Name, signalInitial: false)
                       .Subscribe(changes.Add))
            {
                CollectionAssert.IsEmpty(changes);

                fake.Name = "Johan";
                EventPatternAssert.AreEqual(fake, "Name", Maybe.Some<string?>("Johan"), changes.Single());
            }
        }

        [Test]
        public void MemoryLeakNoDisposeTest()
        {
#if DEBUG
            Assert.Inconclusive("Debugger keeps things alive for the scope of the method.");
#endif

            var fake = new Fake();
            var wr = new WeakReference(fake);
            Assert.IsTrue(wr.IsAlive);
            var observable = fake.ObservePropertyChangedWithValue(x => x.Name, signalInitial: false);
#pragma warning disable IDISP001  // Dispose created.
            var subscription = observable.Subscribe();
#pragma warning restore IDISP001  // Dispose created.
            GC.KeepAlive(observable);
            GC.KeepAlive(subscription);

            // ReSharper disable once RedundantAssignment
            fake = null!;
            GC.Collect();
            Assert.IsFalse(wr.IsAlive);
        }

        [Test]
        public void MemoryLeakDisposeTest()
        {
#if DEBUG
            Assert.Inconclusive("Debugger keeps things alive for the scope of the method.");
#endif

            var fake = new Fake();
            var wr = new WeakReference(fake);
            Assert.IsTrue(wr.IsAlive);
            var observable = fake.ObservePropertyChangedWithValue(x => x.Name, signalInitial: false);
            using (var subscription = observable.Subscribe())
            {
                GC.KeepAlive(observable);
                GC.KeepAlive(subscription);

                // ReSharper disable once RedundantAssignment
                fake = null!;
            }

            GC.Collect();
            Assert.IsFalse(wr.IsAlive);
        }
    }
}
