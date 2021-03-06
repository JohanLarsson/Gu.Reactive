﻿// ReSharper disable HeuristicUnreachableCode
// ReSharper disable InconsistentNaming
namespace Gu.Reactive.Tests.NotifyPropertyChangedExt
{
    using System;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.ComponentModel;
    using System.Linq;
    using System.Reactive;

    using Gu.Reactive.Tests.Helpers;

    using Moq;

    using NUnit.Framework;

    public static class OnPropertyChangedSimpleLambda
    {
        [Test]
        public static void ReactsOnMock()
        {
            var changes = new List<EventPattern<PropertyChangedEventArgs>>();
            var mock = new Mock<IReadOnlyObservableCollection<int>>();
            using (mock.Object.ObservePropertyChanged(x => x.Count, signalInitial: false)
                       .Subscribe(changes.Add))
            {
                Assert.AreEqual(0, changes.Count);

                mock.Raise(x => x.PropertyChanged += null, new PropertyChangedEventArgs("Count"));
                Assert.AreEqual(1, changes.Count);
            }
        }

        [TestCase("")]
        [TestCase(null)]
        [TestCase("Name")]
        public static void ReactsOnStringEmptyOrNullWhenNull(string propertyName)
        {
            var changes = new List<EventPattern<PropertyChangedEventArgs>>();
            var fake = new Fake { Name = null };
            using (fake.ObservePropertyChanged(x => x.Name, signalInitial: false)
                       .Subscribe(changes.Add))
            {
                Assert.AreEqual(0, changes.Count);
                fake.OnPropertyChanged(propertyName); // This means all properties changed according to wpf convention
                CollectionAssert.AreEqual(new[] { propertyName }, changes.Select(x => x.EventArgs.PropertyName));
                CollectionAssert.AreEqual(new[] { fake }, changes.Select(x => x.Sender));
            }
        }

        [Test]
        public static void ReadOnlyObservableCollectionCount()
        {
            var ints = new ObservableCollection<int>();
            var source = new ReadOnlyObservableCollection<int>(ints);
            var changes = new List<EventPattern<PropertyChangedEventArgs>>();
            using (source.ObservePropertyChanged(x => x.Count, signalInitial: false)
                         .Subscribe(x => changes.Add(x)))
            {
                CollectionAssert.IsEmpty(changes);

                ints.Add(1);
                Assert.AreEqual(1, changes.Count);
                Assert.AreEqual("Count", changes.Single().EventArgs.PropertyName);
                Assert.AreSame(source, changes.Single().Sender);

                ints.Add(2);
                Assert.AreEqual(2, changes.Count);
                Assert.AreEqual("Count", changes.Last().EventArgs.PropertyName);
                Assert.AreSame(source, changes.Last().Sender);
            }
        }

        [Test]
        public static void IFakeValue()
        {
            var fake = (IFake)new Fake();
            var changes = new List<EventPattern<PropertyChangedEventArgs>>();
            using (fake.ObservePropertyChanged(x => x.Value, signalInitial: false)
                         .Subscribe(x => changes.Add(x)))
            {
                CollectionAssert.IsEmpty(changes);

                fake.Value++;
                Assert.AreEqual(1, changes.Count);
                Assert.AreEqual("Value", changes.Single().EventArgs.PropertyName);
                Assert.AreSame(fake, changes.Single().Sender);

                fake.Value++;
                Assert.AreEqual(2, changes.Count);
                Assert.AreEqual("Value", changes.Last().EventArgs.PropertyName);
                Assert.AreSame(fake, changes.Last().Sender);
            }
        }

        [Test]
        public static void HandlesNull()
        {
            var changes = new List<EventPattern<PropertyChangedEventArgs>>();
            var fake = new Fake { Name = "1" };
            using (fake.ObservePropertyChanged(x => x.Name, signalInitial: false)
                       .Subscribe(changes.Add))
            {
                Assert.AreEqual(0, changes.Count);

                fake.Name = null;
                Assert.AreEqual(1, changes.Count);
                EventPatternAssert.AreEqual(fake, "Name", changes.Last());

                fake.Name = "1";
                Assert.AreEqual(2, changes.Count);
                EventPatternAssert.AreEqual(fake, "Name", changes.Last());
            }
        }

        [Test]
        public static void ReactsTwoPropertiesSameInstance()
        {
            var changes = new List<EventPattern<PropertyChangedEventArgs>>();
            var fake = new Fake { Value = 1 };
            using (fake.ObservePropertyChanged(x => x.Value, signalInitial: false)
                       .Subscribe(changes.Add))
            {
                using (fake.ObservePropertyChanged(x => x.IsTrue, signalInitial: false)
                           .Subscribe(changes.Add))
                {
                    Assert.AreEqual(0, changes.Count);

                    fake.Value++;
                    Assert.AreEqual(1, changes.Count);
                    EventPatternAssert.AreEqual(fake, "Value", changes.Last());

                    fake.IsTrue = !fake.IsTrue;
                    Assert.AreEqual(2, changes.Count);
                    EventPatternAssert.AreEqual(fake, "IsTrue", changes.Last());
                }
            }
        }

        [Test]
        public static void ReactsTwoInstances()
        {
            var changes = new List<EventPattern<PropertyChangedEventArgs>>();
            var fake1 = new Fake { Value = 1 };
            using (fake1.ObservePropertyChanged(x => x.Value, signalInitial: false)
                        .Subscribe(changes.Add))
            {
                var fake2 = new Fake { Value = 1 };
                using (fake2.ObservePropertyChanged(x => x.Value, signalInitial: false)
                            .Subscribe(changes.Add))
                {
                    Assert.AreEqual(0, changes.Count);

                    fake1.Value++;
                    Assert.AreEqual(1, changes.Count);
                    EventPatternAssert.AreEqual(fake1, "Value", changes.Last());

                    fake2.Value++;
                    Assert.AreEqual(2, changes.Count);
                    EventPatternAssert.AreEqual(fake2, "Value", changes.Last());
                }
            }
        }

        [Test]
        public static void TwoSubscriptionsOneObservable()
        {
            var changes1 = new List<EventPattern<PropertyChangedEventArgs>>();
            var changes2 = new List<EventPattern<PropertyChangedEventArgs>>();
            var fake = new Fake { Value = 1 };
            var observable = fake.ObservePropertyChanged(x => x.IsTrue, signalInitial: false);
            using (observable.Subscribe(changes1.Add))
            {
                using (observable.Subscribe(changes2.Add))
                {
                    Assert.AreEqual(0, changes1.Count);
                    Assert.AreEqual(0, changes2.Count);

                    fake.IsTrue = !fake.IsTrue;
                    Assert.AreEqual(1, changes1.Count);
                    Assert.AreEqual(1, changes2.Count);
                    EventPatternAssert.AreEqual(fake, "IsTrue", changes1.Last());
                    EventPatternAssert.AreEqual(fake, "IsTrue", changes2.Last());

                    fake.IsTrue = !fake.IsTrue;
                    Assert.AreEqual(2, changes1.Count);
                    Assert.AreEqual(2, changes2.Count);
                    EventPatternAssert.AreEqual(fake, "IsTrue", changes1.Last());
                    EventPatternAssert.AreEqual(fake, "IsTrue", changes2.Last());
                }
            }
        }

        [Test]
        public static void ReactsWhenValueChanges()
        {
            var changes = new List<EventPattern<PropertyChangedEventArgs>>();
            var fake = new Fake { Value = 1 };
            using (fake.ObservePropertyChanged(x => x.Value, signalInitial: false)
                       .Subscribe(changes.Add))
            {
                Assert.AreEqual(0, changes.Count);

                fake.Value++;

                Assert.AreEqual(1, changes.Count);
                EventPatternAssert.AreEqual(fake, "Value", changes.Last());
            }
        }

        [Test]
        public static void ReactsWhenValueChangesGeneric()
        {
            var changes = new List<EventPattern<PropertyChangedEventArgs>>();
            var fake = new Fake<int> { Value = 1 };
            using (fake.ObservePropertyChanged(x => x.Value, signalInitial: false)
                       .Subscribe(changes.Add))
            {
                Assert.AreEqual(0, changes.Count);

                fake.Value++;

                Assert.AreEqual(1, changes.Count);
                EventPatternAssert.AreEqual(fake, "Value", changes.Last());
            }
        }

        [Test]
        public static void DoesNotReactWhenOtherPropertyChanges()
        {
            var changes = new List<EventPattern<PropertyChangedEventArgs>>();
            var fake = new Fake { Value = 1 };
            using (fake.ObservePropertyChanged(x => x.Value, signalInitial: false)
                       .Subscribe(changes.Add))
            {
                Assert.AreEqual(0, changes.Count);
                fake.Value++;
                Assert.AreEqual(1, changes.Count);
                EventPatternAssert.AreEqual(fake, "Value", changes.Last());

                fake.IsTrue = !fake.IsTrue;

                Assert.AreEqual(1, changes.Count); // No notification when changing other property
            }
        }

        [TestCase("")]
        [TestCase(null)]
        [TestCase("Value")]
        public static void ReactsOnStringEmptyOrNull(string propertyName)
        {
            var changes = new List<EventPattern<PropertyChangedEventArgs>>();
            var fake = new Fake { Value = 1 };
            using (fake.ObservePropertyChanged(x => x.Value, signalInitial: false)
                       .Subscribe(changes.Add))
            {
                Assert.AreEqual(0, changes.Count);

                fake.OnPropertyChanged(propertyName); // This means all properties changed according to wpf convention

                Assert.AreEqual(1, changes.Count);
                EventPatternAssert.AreEqual(fake, propertyName, changes.Last());
            }
        }

        [TestCase(true)]
        [TestCase(false)]
        public static void SignalsInitial(bool signalInitial)
        {
            var changes = new List<EventPattern<PropertyChangedEventArgs>>();
            var fake = new Fake { Value = 1 };

            using (fake.ObservePropertyChanged(x => x.Value, signalInitial)
                       .Subscribe(changes.Add))
            {
                if (signalInitial)
                {
                    EventPatternAssert.AreEqual(fake, string.Empty, changes.Single());
                }

                fake.Value++;
                Assert.AreEqual(signalInitial ? 2 : 1, changes.Count);
                EventPatternAssert.AreEqual(fake, "Value", changes.Last());
            }
        }

        [Test]
        public static void MemoryLeakDisposeTest()
        {
#if DEBUG
            Assert.Inconclusive("Debugger keeps things alive for the scope of the method.");
#endif
            var fake = new Fake();
            var wr = new WeakReference(fake);
            using (fake.ObservePropertyChanged(x => x.IsTrueOrNull).Subscribe())
            {
            }

            GC.Collect();
            Assert.IsFalse(wr.IsAlive);
        }

        [Test]
        public static void MemoryLeakNoDisposeTest()
        {
#if DEBUG
            Assert.Inconclusive("Debugger keeps things alive for the scope of the method.");
#endif
            var fake = new Fake();
            var wr = new WeakReference(fake);
            var observable = fake.ObservePropertyChanged(x => x.IsTrueOrNull);
#pragma warning disable IDISP001  // Dispose created.
            //// ReSharper disable once UnusedVariable
            var subscribe = observable.Subscribe();
#pragma warning restore IDISP001  // Dispose created.
            GC.Collect();
            Assert.IsFalse(wr.IsAlive);
        }

        [TestCase(true)]
        [TestCase(false)]
        public static void ThrowsOnStructInPath(bool signalInitial)
        {
            var fake = new Fake();
#pragma warning disable GUREA03 // Path must notify.
            var exception = Assert.Throws<ArgumentException>(() => fake.ObservePropertyChanged(x => x.StructLevel.Name, signalInitial));
#pragma warning restore GUREA03 // Path must notify.
            var expected = "Error found in x => x.StructLevel.Name\r\n" +
                           "Property path cannot have structs in it. Copy by value will make subscribing error prone. Also mutable struct much?\r\n" +
                           "The type StructLevel is a value type not so StructLevel.Name will not notify when it changes.\r\n" +
                           "The path is: x => x.StructLevel.Name\r\n\r\n" +
                           "Parameter name: property";
            Assert.AreEqual(expected, exception!.Message);
        }

        [TestCase(true)]
        [TestCase(false)]
        public static void ThrowsOnNotNotifyingPathOneLevel(bool signalInitial)
        {
            var fake = new Fake();
#pragma warning disable GUREA03 // Path must notify.
            var exception = Assert.Throws<ArgumentException>(() => fake.ObservePropertyChanged(x => x.Name.Length, signalInitial));
#pragma warning restore GUREA03 // Path must notify.
            var expected = "Error found in x => x.Name.Length\r\n" +
                           "All levels in the path must implement INotifyPropertyChanged.\r\n" +
                           "The type string does not so Name.Length will not notify when it changes.\r\n" +
                           "The path is: x => x.Name.Length\r\n\r\n" +
                           "Parameter name: property";
            Assert.AreEqual(expected, exception!.Message);
        }
    }
}
