#pragma warning disable CS0618 // Type or member is obsolete
namespace Gu.Wpf.Reactive.Tests.Views.FilterTests
{
    using System;
    using Gu.Wpf.Reactive.Tests.Collections.Views.FilterTests;
    using Gu.Wpf.Reactive.Tests.FakesAndHelpers;
    using NUnit.Framework;

    public class FilterTestsWithDispatcher : AbstractFilterTests
    {
        [SetUp]
        public override void SetUp()
        {
            App.Start();
            this.Scheduler = new TestDispatcherScheduler();
            base.SetUp();
#pragma warning disable IDISP007 // Don't dispose injected.
            this.View?.Dispose();
#pragma warning restore IDISP007 // Don't dispose injected.
            this.View = this.Source.AsFilteredView(x => true, TimeSpan.Zero);
        }
    }
}
