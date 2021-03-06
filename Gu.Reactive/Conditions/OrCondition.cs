﻿namespace Gu.Reactive
{
    using System.Collections.Generic;

    /// <summary>
    /// Creates an <see cref="ICondition"/> from a collection of conditions.
    /// It is Satisfied when any of the prerequisites is satisfied.
    /// If no prerequisite IsSatisfied IsSatisfied returns false.
    /// If no prerequisite is IsSatisfied == true and any prerequisite is null the result is null.
    /// </summary>
    public class OrCondition : CollectionCondition
    {
        private readonly OrConditionCollection prerequisites;

        /// <summary>
        /// Initializes a new instance of the <see cref="OrCondition"/> class.
        /// </summary>
        /// <param name="prerequisite1">The first child.</param>
        /// <param name="prerequisite2">The second child.</param>
        /// <param name="prerequisites">The children.</param>
        public OrCondition(ICondition prerequisite1, ICondition prerequisite2, params ICondition[] prerequisites)
            : this(new OrConditionCollection(ConditionCollection.Prepend(prerequisite1, prerequisite2, prerequisites), leaveOpen: true))
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="OrCondition"/> class.
        /// </summary>
        /// <param name="prerequisites">The children.</param>
        /// <param name="leaveOpen">True to not dispose <paramref name="prerequisites"/> when this instance is disposed.</param>
        public OrCondition(IReadOnlyList<ICondition> prerequisites, bool leaveOpen)
            : this(new OrConditionCollection(prerequisites, leaveOpen))
        {
        }

        private OrCondition(OrConditionCollection prerequisites)
            : base(prerequisites)
        {
            this.prerequisites = prerequisites;
        }

        /// <summary>
        /// Disposes of a <see cref="OrCondition"/>.
        /// </summary>
        /// <remarks>
        /// Called from Dispose() with disposing=true.
        /// Guidelines:
        /// 1. We may be called more than once: do nothing after the first call.
        /// 2. Avoid throwing exceptions if disposing is false, i.e. if we're being finalized.
        /// </remarks>
        /// <param name="disposing">True if called from Dispose(), false if called from the finalizer.</param>
        protected override void Dispose(bool disposing)
        {
            if (this.IsDisposed)
            {
                return;
            }

            if (disposing)
            {
                this.prerequisites.Dispose();
            }

            base.Dispose(disposing);
        }
    }
}
