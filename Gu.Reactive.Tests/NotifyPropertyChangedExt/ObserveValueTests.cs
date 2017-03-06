namespace Gu.Reactive.Tests.NotifyPropertyChangedExt
{
    using System;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.Linq;

    using Gu.Reactive.Tests.Helpers;
    using NUnit.Framework;

    public class ObserveValueTests
    {
        [Test]
        public void Simple()
        {
            var fake = new Fake();
            var values = new List<Maybe<int>>();
            using (fake.ObserveValue(x => x.Value, false)
                       .Subscribe(values.Add))
            {
                CollectionAssert.IsEmpty(values);

                fake.Value++;
                CollectionAssert.AreEqual(new[] { Maybe.Some(1) }, values);

                fake.Value++;
                CollectionAssert.AreEqual(new[] { Maybe.Some(1), Maybe.Some(2) }, values);

                fake.OnPropertyChanged("Value");
                CollectionAssert.AreEqual(new[] { Maybe.Some(1), Maybe.Some(2), Maybe.Some(2) }, values);
            }
        }

        [TestCase(true, new[] { 1 })]
        [TestCase(false, new int[0])]
        public void SimpleSignalInitial(bool signalInitial, int[] start)
        {
            var expected = start.Select(Maybe.Some).ToList();
            var values = new List<Maybe<int>>();
            var fake = new Fake { Value = 1 };
            using (fake.ObserveValue(x => x.Value, signalInitial)
                       .Subscribe(values.Add))
            {
                CollectionAssert.AreEqual(expected, values);

                fake.Value++;
                expected.Add(Maybe.Some(fake.Value));
                CollectionAssert.AreEqual(expected, values);

                fake.Value++;
                expected.Add(Maybe.Some(fake.Value));
                CollectionAssert.AreEqual(expected, values);
            }
        }

        [TestCase(true, new[] { 1 })]
        [TestCase(false, new int[0])]
        public void SimpleSignalInitialDifferent(bool signalInitial, int[] start)
        {
            var expected = start.Select(Maybe.Some).ToList();
            var actuals1 = new List<Maybe<int>>();
            var actuals2 = new List<Maybe<int>>();
            var fake = new Fake { Value = 1 };
            var observable = fake.ObserveValue(x => x.Value, signalInitial);
            using (observable.Subscribe(actuals1.Add))
            {
                CollectionAssert.AreEqual(expected, actuals1);

                fake.Value++;
                expected.Add(Maybe.Some(fake.Value));
                CollectionAssert.AreEqual(expected, actuals1);

                using (observable.Subscribe(actuals2.Add))
                {
                    CollectionAssert.AreEqual(expected, actuals1);
                    CollectionAssert.AreEqual(expected.Skip(1), actuals2);

                    fake.Value++;
                    expected.Add(Maybe.Some(fake.Value));
                    CollectionAssert.AreEqual(expected, actuals1);
                    CollectionAssert.AreEqual(expected.Skip(1), actuals2);
                }
            }
        }

        [TestCase(true, new[] { 1 })]
        [TestCase(false, new int[0])]
        public void NestedSignalInitial(bool signalInitial, int[] start)
        {
            var expected = start.Select(Maybe.Some).ToList();
            var values = new List<Maybe<int>>();
            var fake = new Fake { Next = new Level { Value = 1 } };
            using (fake.ObserveValue(x => x.Next.Value, signalInitial)
                       .Subscribe(values.Add))
            {
                CollectionAssert.AreEqual(expected, values);

                fake.Next.Value++;
                expected.Add(Maybe.Some(fake.Next.Value));
                CollectionAssert.AreEqual(expected, values);

                fake.Next.OnPropertyChanged("Value");
                expected.Add(Maybe.Some(fake.Next.Value));
                CollectionAssert.AreEqual(expected, values);

                fake.Next.OnPropertyChanged("Next");
                CollectionAssert.AreEqual(expected, values);

                fake.Next = null;
                expected.Add(Maybe<int>.None);
                CollectionAssert.AreEqual(expected, values);
            }
        }

        [TestCase(true, new[] { 1 })]
        [TestCase(false, new int[0])]
        public void NestedSignalInitialDifferent(bool signalInitial, int[] start)
        {
            var expected = start.Select(Maybe.Some).ToList();
            var actuals1 = new List<Maybe<int>>();
            var actuals2 = new List<Maybe<int>>();
            var fake = new Fake { Next = new Level { Value = 1 } };
            var observable = fake.ObserveValue(x => x.Next.Value, signalInitial);
            using (observable.Subscribe(actuals1.Add))
            {
                CollectionAssert.AreEqual(expected, actuals1);

                fake.Next.Value++;
                expected.Add(Maybe.Some(fake.Next.Value));
                CollectionAssert.AreEqual(expected, actuals1);
                using (observable.Subscribe(actuals2.Add))
                {
                    CollectionAssert.AreEqual(expected.Skip(1), actuals2);

                    fake.Next.OnPropertyChanged("Value");
                    expected.Add(Maybe.Some(fake.Next.Value));
                    CollectionAssert.AreEqual(expected, actuals1);
                    CollectionAssert.AreEqual(expected.Skip(1), actuals2);

                    fake.Next.OnPropertyChanged("Next");
                    CollectionAssert.AreEqual(expected, actuals1);
                    CollectionAssert.AreEqual(expected.Skip(1), actuals2);

                    fake.Next = null;
                    expected.Add(Maybe<int>.None);
                    CollectionAssert.AreEqual(expected, actuals1);
                    CollectionAssert.AreEqual(expected.Skip(1), actuals2);
                }
            }
        }

        [Test]
        public void ReadOnlyObservableCollectionCount()
        {
            var ints = new ObservableCollection<int>();
            var source = new ReadOnlyObservableCollection<int>(ints);
            var values = new List<Maybe<int>>();
            using (source.ObserveValue(x => x.Count, false)
                       .Subscribe(values.Add))
            {
                CollectionAssert.IsEmpty(values);

                ints.Add(1);
                CollectionAssert.AreEqual(new[] { Maybe.Some(1) }, values);

                ints.Add(2);
                CollectionAssert.AreEqual(new[] { Maybe.Some(1), Maybe.Some(2) }, values);
            }
        }

        [Test]
        public void MemoryLeakSimpleDisposeTest()
        {
#if DEBUG
            Assert.Inconclusive("Debugger keeps things alive for the scope of the method.");
#endif
            var fake = new Fake();
            var wr = new WeakReference(fake);
            using (fake.ObserveValue(x => x.IsTrueOrNull).Subscribe())
            {
            }

            GC.Collect();
            Assert.IsFalse(wr.IsAlive);
        }

        [Test]
        public void MemoryLeakNestedDisposeTest()
        {
#if DEBUG
            Assert.Inconclusive("Debugger keeps things alive for the scope of the method.");
#endif
            var fake = new Fake();
            var wr = new WeakReference(fake);
            using (fake.ObserveValue(x => x.Next.Next.Value).Subscribe())
            {
            }

            GC.Collect();
            Assert.IsFalse(wr.IsAlive);
        }

        [Test]
        public void MemoryLeakSimpleNoDisposeTest()
        {
#if DEBUG
            Assert.Inconclusive("Debugger keeps things alive for the scope of the method.");
#endif
            var fake = new Fake();
            var wr = new WeakReference(fake);
            var observable = fake.ObserveValue(x => x.IsTrueOrNull);
#pragma warning disable GU0030 // Use using.
            //// ReSharper disable once UnusedVariable
            var subscribe = observable.Subscribe();
#pragma warning restore GU0030 // Use using.
            GC.Collect();
            Assert.IsFalse(wr.IsAlive);
        }

        [Test]
        public void MemoryLeakNestedNoDisposeTest()
        {
#if DEBUG
            Assert.Inconclusive("Debugger keeps things alive for the scope of the method.");
#endif
            var fake = new Fake();
            var wr = new WeakReference(fake);
            var observable = fake.ObserveValue(x => x.Next.Next.Value);
#pragma warning disable GU0030 // Use using.
            //// ReSharper disable once UnusedVariable
            var subscribe = observable.Subscribe();
#pragma warning restore GU0030 // Use using.
            GC.Collect();
            Assert.IsFalse(wr.IsAlive);
        }
    }
}