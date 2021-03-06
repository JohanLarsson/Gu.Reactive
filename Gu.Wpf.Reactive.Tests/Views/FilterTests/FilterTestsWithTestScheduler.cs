#pragma warning disable CS0618 // Type or member is obsolete
namespace Gu.Wpf.Reactive.Tests.Views.FilterTests
{
    using System;
    using Gu.Wpf.Reactive.Tests.Collections.Views.FilterTests;
    using Microsoft.Reactive.Testing;
    using NUnit.Framework;

    public class FilterTestsWithTestScheduler : AbstractFilterTests
    {
        [SetUp]
        public override void SetUp()
        {
            base.SetUp();
            this.Scheduler = new TestScheduler();
#pragma warning disable IDISP007 // Don't dispose injected.
            this.View?.Dispose();
#pragma warning restore IDISP007 // Don't dispose injected.
            this.View = new FilteredView<int>(this.Source, x => true, TimeSpan.Zero, this.Scheduler, leaveOpen: true);
        }
    }
}
