﻿#pragma warning disable SA1500 // Braces for multi-line statements should not share line
#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional
namespace Gu.Reactive.Tests
{
    using System;
    using System.Collections.Generic;
    using System.Globalization;
    using System.Linq;
    using System.Reactive;
    using System.Text;

    using Microsoft.Reactive.Testing;

    using NUnit.Framework;

    public static partial class ObservableExtTests
    {
        public static class Throttle
        {
            private const int DueTime = 50;

            private const int MaxTime = 100;

            private const int CompletedTime = 100000;

            [TestCaseSource(typeof(ThrottleSource))]
            public static void ThrottleWithMaxTime(ThrottleData data)
            {
                var pattern = data.Pattern;
                var completeEvent = data.CompletedEvent;
                var scheduler = new TestScheduler();
                var source = scheduler.CreateColdObservable(
                    pattern.Select(v => ReactiveTest.OnNext(v, v))
                           .Concat(completeEvent)
                           .ToArray());
                var throttled = source.Throttle(TimeSpan.FromTicks(DueTime), TimeSpan.FromTicks(MaxTime), scheduler);
                var observer = scheduler.CreateObserver<int>();
                using (throttled.Subscribe(observer))
                {
                    // start the clock
                    scheduler.Start();

                    // check the results
                    var expected = data.ExpectedMessages;
                    CollectionAssert.AreEqual(expected, observer.Messages);
                }
            }

            public class ThrottleSource : List<ThrottleData>
            {
                public ThrottleSource()
                {
                    this.Add(new[] { 1, 10 }, new[,] { { 10, 10 + DueTime } });
                    this.Add(new[] { 1, 10, 40, 60 }, new[,] { { 60, 1 + MaxTime } });
                    this.Add(
                        new[] { 1, 45, 1000, 1040, 1080, 1110 },
                        new[,] { { 45, 45 + DueTime }, { 1080, 1000 + MaxTime }, { 1110, 1110 + DueTime } });
                }

                public void Add(int[] pattern, int[,] expected)
                {
                    this.Add(new ThrottleData(pattern, expected));
                }
            }

            public class ThrottleData
            {
                public readonly IReadOnlyList<int> Pattern;

                public readonly IReadOnlyList<int> ExpectedPattern;

                public readonly IReadOnlyList<int> ExpectedTimes;

                public readonly IReadOnlyList<Recorded<Notification<int>>> ExpectedMessages;

                public readonly IReadOnlyList<Recorded<Notification<int>>> CompletedEvent = new[]
                {
                    ReactiveTest.OnCompleted(CompletedTime, CompletedTime),
                };

                public ThrottleData(int[] pattern, int[,] expected)
                {
                    this.Pattern = pattern;
                    var expectedPattern = new List<int>();
                    var expectedTimes = new List<int>();
                    var expectedMessages = new List<Recorded<Notification<int>>>();

                    for (var i = 0; i < expected.GetLength(0); i++)
                    {
                        var value = expected[i, 0];
                        expectedPattern.Add(value);
                        var time = expected[i, 1];
                        expectedTimes.Add(time);
                        expectedMessages.Add(ReactiveTest.OnNext(time, value));
                    }

                    expectedMessages.AddRange(this.CompletedEvent);
                    this.ExpectedPattern = expectedPattern;
                    this.ExpectedTimes = expectedTimes;
                    this.ExpectedMessages = expectedMessages;
                }

                public override string ToString()
                {
                    var pattern = $"{string.Join(", ", this.Pattern)}";
                    var expected = new StringBuilder();
                    expected.Append('{');
                    for (var i = 0; i < this.ExpectedTimes.Count; i++)
                    {
                        expected.AppendFormat(CultureInfo.InvariantCulture, @"({0}, {1})", this.ExpectedPattern[i], this.ExpectedTimes[i]);
                        if (i < this.ExpectedTimes.Count - 1)
                        {
                            expected.Append(", ");
                        }
                    }

                    expected.Append('}');

                    return $"{pattern} -> {expected}";
                }
            }
        }
    }
}
