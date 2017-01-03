namespace Gu.Reactive.Tests.Collections.CrudView
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.Collections.Specialized;
    using System.ComponentModel;
    using System.Linq;

    using Gu.Reactive;
    using Gu.Reactive.Tests.Helpers;

    using Microsoft.Reactive.Testing;

    using NUnit.Framework;

    public abstract class CrudViewTests
    {
        private List<EventArgs> expected;

        protected List<EventArgs> Actual { get; set; }

        protected TestScheduler Scheduler { get; set; }

        protected IFilteredView<int> View { get; set; }

        protected ObservableCollection<int> Ints { get; set; }

        [SetUp]
        public virtual void SetUp()
        {
            this.Ints = new ObservableCollection<int>(new[] { 1, 2, 3 });
            this.expected = this.SubscribeAll(this.Ints);
        }

        [TearDown]
        public void TearDown()
        {
            this.View.Dispose();
        }

        [Test]
        public void NoChangeNoEvent()
        {
            CollectionAssert.AreEqual(this.Ints, this.View);

            this.View.Refresh();
            this.Scheduler?.Start();
            CollectionAssert.AreEqual(this.Ints, this.View);
            CollectionAssert.IsEmpty(this.Actual);
        }

        [Test]
        public void UpdatesBeforeItNotifies()
        {
            int[] actual = null;
            int[] expected = null;
            this.View.ObserveCollectionChanged(false)
                 .Subscribe(_ => { actual = this.View.ToArray(); });
            this.Ints.ObserveCollectionChanged(false)
                 .Subscribe(_ => { expected = this.Ints.ToArray(); });
            this.View.Add(5);
            this.Scheduler?.Start();
            CollectionAssert.AreEqual(expected, actual);

            this.View.Clear();
            this.Scheduler?.Start();
            CollectionAssert.AreEqual(expected, actual);
        }

        [Test]
        public void Add()
        {
            this.View.Add(4);
            this.Scheduler?.Start();
            CollectionAssert.AreEqual(this.Ints, this.View);
            CollectionAssert.AreEqual(this.expected, this.Actual, EventArgsComparer.Default);
        }

        [Test]
        public void ListAdd()
        {
            // DataGrid adds items like this
            var index = ((IList)this.View).Add(4);
            this.Scheduler?.Start();
            Assert.AreEqual(3, index);
            CollectionAssert.AreEqual(this.Ints, this.View);
            CollectionAssert.IsNotEmpty(this.Actual);
            CollectionAssert.AreEqual(this.expected, this.Actual, EventArgsComparer.Default);

            var before = this.Actual.ToArray();
            this.Scheduler?.Start(); // Should not signal deferred

            CollectionAssert.AreEqual(before, this.Actual, EventArgsComparer.Default);
        }

        [TestCase(1)]
        [TestCase(2)]
        public void Remove(int toRemove)
        {
            this.View.Remove(toRemove);
            this.Scheduler?.Start();
            CollectionAssert.AreEqual(this.Ints, this.View);
            CollectionAssert.AreEqual(this.expected, this.Actual, EventArgsComparer.Default);
        }

        [TestCase(2, 5)]
        [TestCase(0, 5)]
        public void ReplaceIndexer(int index, int value)
        {
            this.View[index] = value;
            this.Scheduler?.Start();
            Assert.AreEqual(value, this.View[index]);
            CollectionAssert.AreEqual(this.Ints, this.View);
            CollectionAssert.AreEqual(this.expected, this.Actual, EventArgsComparer.Default);
        }

        [TestCase(0, 1)]
        public void Move(int fromIndex, int toIndex)
        {
            Assert.Inconclusive("Do we want move?");
            ////_view.Move(fromIndex, toIndex);
            this.Scheduler?.Start();
            CollectionAssert.AreEqual(this.Ints, this.View);
            CollectionAssert.AreEqual(this.expected, this.Actual, EventArgsComparer.Default);
        }

        [Test]
        public void Count()
        {
            Assert.AreEqual(3, this.View.Count);
        }

        [Test]
        public void ToArrayTest()
        {
            CollectionAssert.AreEqual(this.Ints, this.View.ToArray());
        }

        protected List<EventArgs> SubscribeAll<T>(T view)
            where T : IEnumerable, INotifyCollectionChanged, INotifyPropertyChanged
        {
            var changes = new List<EventArgs>();
            view.ObserveCollectionChanged(false)
                .Subscribe(x => changes.Add(x.EventArgs));
            view.ObservePropertyChanged()
                .Subscribe(x => changes.Add(x.EventArgs));
            return changes;
        }
    }
}