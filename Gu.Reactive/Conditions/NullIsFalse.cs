﻿namespace Gu.Reactive
{
    using System.Collections.Generic;
    using System.Reactive.Linq;

    /// <summary>
    /// Wraps a <see cref="ICondition"/> and returns false if the wrapped condition returns null.
    /// </summary>
    /// <typeparam name="TCondition">The source condition type.</typeparam>
    public class NullIsFalse<TCondition> : Condition
        where TCondition : class, ICondition
    {
        private readonly TCondition condition;

        /// <summary>
        /// Initializes a new instance of the <see cref="NullIsFalse{TCondition}"/> class.
        /// </summary>
        /// <param name="condition">The condition.</param>
        public NullIsFalse(TCondition condition)
            : base(
                condition.ObserveValue(x => x.IsSatisfied)
                         .Select(x => (object?)x.GetValueOrDefault(false))
                         .DistinctUntilChanged(),
                () => condition.IsSatisfied ?? false)
        {
            this.condition = condition;
        }

        /// <inheritdoc />
        public override IReadOnlyList<ICondition> Prerequisites => this.condition.Prerequisites;
    }
}
