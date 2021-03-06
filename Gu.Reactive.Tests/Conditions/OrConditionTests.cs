﻿namespace Gu.Reactive.Tests.Conditions
{
    using System;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.ComponentModel;
    using Gu.Reactive.Tests.Helpers;

    using Moq;
    using NUnit.Framework;

    public static class OrConditionTests
    {
        [TestCase(true, true, true, true)]
        [TestCase(true, true, null, true)]
        [TestCase(true, null, true, true)]
        [TestCase(null, true, true, true)]
        [TestCase(null, null, true, true)]
        [TestCase(false, null, true, true)]
        [TestCase(true, true, false, true)]
        [TestCase(true, false, null, true)]
        [TestCase(false, false, false, false)]
        [TestCase(false, null, null, null)]
        [TestCase(null, false, null, null)]
        [TestCase(null, null, false, null)]
        [TestCase(null, null, null, null)]
        public static void IsSatisfied(bool? first, bool? second, bool? third, bool? expected)
        {
            using var collection = new OrCondition(
                Mock.Of<ICondition>(x => x.IsSatisfied == first),
                Mock.Of<ICondition>(x => x.IsSatisfied == second),
                Mock.Of<ICondition>(x => x.IsSatisfied == third));
            Assert.AreEqual(expected, collection.IsSatisfied);
        }

        [Test]
        public static void Notifies()
        {
            var count = 0;
            var fake1 = new Fake { IsTrue = false };
            var fake2 = new Fake { IsTrue = false };
            var fake3 = new Fake { IsTrue = false };
            using var condition1 = new Condition(fake1.ObservePropertyChanged(x => x.IsTrue), () => fake1.IsTrue);
            using var condition2 = new Condition(fake2.ObservePropertyChanged(x => x.IsTrue), () => fake2.IsTrue);
            using var condition3 = new Condition(fake3.ObservePropertyChanged(x => x.IsTrue), () => fake3.IsTrue);
            using var collection = new OrCondition(condition1, condition2, condition3);
            using (collection.ObserveIsSatisfiedChanged()
                             .Subscribe(_ => count++))
            {
                Assert.AreEqual(false, collection.IsSatisfied);
                fake1.IsTrue = !fake1.IsTrue;
                Assert.AreEqual(true, collection.IsSatisfied);
                Assert.AreEqual(1, count);

                fake2.IsTrue = !fake2.IsTrue;
                Assert.AreEqual(true, collection.IsSatisfied);
                Assert.AreEqual(1, count);

                fake3.IsTrue = !fake3.IsTrue;
                Assert.AreEqual(true, collection.IsSatisfied);
                Assert.AreEqual(1, count);

                fake1.IsTrue = !fake1.IsTrue;
                Assert.AreEqual(true, collection.IsSatisfied);
                Assert.AreEqual(1, count);

                fake2.IsTrue = !fake2.IsTrue;
                Assert.AreEqual(true, collection.IsSatisfied);
                Assert.AreEqual(1, count);

                fake3.IsTrue = !fake3.IsTrue;
                Assert.AreEqual(false, collection.IsSatisfied);
                Assert.AreEqual(2, count);
            }
        }

        [Test]
        public static void ThrowsIfPrerequisiteIsNull()
        {
            var mock = Mock.Of<ICondition>();
            _ = Assert.Throws<ArgumentNullException>(() => new OrCondition(mock, null!));
            _ = Assert.Throws<ArgumentNullException>(() => new OrCondition(null!, mock));
        }

        [Test]
        public static void Prerequisites2()
        {
            var mock1 = Mock.Of<ICondition>();
            var mock2 = Mock.Of<ICondition>();
            using var condition = new OrCondition(mock1, mock2);
            CollectionAssert.AreEqual(new[] { mock1, mock2 }, condition.Prerequisites);
        }

        [Test]
        public static void Prerequisites3()
        {
            var mock1 = Mock.Of<ICondition>();
            var mock2 = Mock.Of<ICondition>();
            var mock3 = Mock.Of<ICondition>();
            using var condition = new OrCondition(mock1, mock2, mock3);
            CollectionAssert.AreEqual(new[] { mock1, mock2, mock3 }, condition.Prerequisites);
        }

        [Test]
        public static void Prerequisites4()
        {
            var mock1 = Mock.Of<ICondition>();
            var mock2 = Mock.Of<ICondition>();
            var mock3 = Mock.Of<ICondition>();
            var mock4 = Mock.Of<ICondition>();
            using var condition = new OrCondition(mock1, mock2, mock3, mock4);
            CollectionAssert.AreEqual(new[] { mock1, mock2, mock3, mock4 }, condition.Prerequisites);
        }

        [Test]
        public static void DisposeDoesNotDisposeInjected()
        {
            var mock1 = new Mock<ICondition>(MockBehavior.Strict);
            mock1.SetupGet(x => x.IsSatisfied)
                .Returns(false);
            var mock2 = new Mock<ICondition>(MockBehavior.Strict);
            mock2.SetupGet(x => x.IsSatisfied)
                .Returns(false);
            using (new OrCondition(mock1.Object, mock2.Object))
            {
            }

            mock1.Verify(x => x.Dispose(), Times.Never);
        }

        [Test]
        public static void DynamicList()
        {
            var conditions = new ObservableCollection<ICondition>();
            using var condition = new OrCondition(conditions, leaveOpen: true);
            var actuals = new List<bool?>();
            using (condition.ObserveIsSatisfiedChanged()
                            .Subscribe(c => actuals.Add(c.IsSatisfied)))
            {
                var preRequisiteChanges = 0;
                using (condition.ObservePropertyChangedSlim(x => x.Prerequisites, signalInitial: false)
                                .Subscribe(x => preRequisiteChanges++))
                {
                    Assert.AreEqual(0, preRequisiteChanges);
                    CollectionAssert.IsEmpty(condition.Prerequisites);
                    Assert.AreSame(condition.Prerequisites, condition.Prerequisites);
                    Assert.AreEqual(null, condition.IsSatisfied);
                    CollectionAssert.IsEmpty(actuals);
                    Assert.AreEqual(null, condition.IsSatisfied);

                    conditions.Add(Mock.Of<ICondition>(x => x.IsSatisfied == true));
                    Assert.AreEqual(true, condition.IsSatisfied);
                    CollectionAssert.AreEqual(new[] { true }, actuals);
                    Assert.AreEqual(1, preRequisiteChanges);
                    CollectionAssert.AreEqual(conditions, condition.Prerequisites);
                    Assert.AreSame(condition.Prerequisites, condition.Prerequisites);

                    Mock.Get(conditions[0]).SetupGet(x => x.IsSatisfied).Returns(false);
                    Mock.Get(conditions[0]).Raise(x => x.PropertyChanged += null, new PropertyChangedEventArgs("IsSatisfied"));
                    Assert.AreEqual(false, condition.IsSatisfied);
                    CollectionAssert.AreEqual(new[] { true, false }, actuals);
                    Assert.AreEqual(1, preRequisiteChanges);
                    CollectionAssert.AreEqual(conditions, condition.Prerequisites);
                    Assert.AreSame(condition.Prerequisites, condition.Prerequisites);
                }
            }
        }

        [Test]
        public static void DisposeTwice()
        {
            var mock1 = new Mock<ICondition>(MockBehavior.Strict);
            mock1.SetupGet(x => x.IsSatisfied)
                .Returns(false);
            var mock2 = new Mock<ICondition>(MockBehavior.Strict);
            mock2.SetupGet(x => x.IsSatisfied)
                .Returns(false);
#pragma warning disable IDISP016 // Don't use disposed instance.
            using (var condition = new OrCondition(mock1.Object, mock2.Object))
            {
                condition.Dispose();
                condition.Dispose();
            }
#pragma warning restore IDISP016 // Don't use disposed instance.

            mock1.Verify(x => x.Dispose(), Times.Never);
        }
    }
}
