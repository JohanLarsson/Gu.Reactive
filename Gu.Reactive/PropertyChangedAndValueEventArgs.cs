﻿namespace Gu.Reactive
{
    using System.ComponentModel;

    public class PropertyChangedAndValueEventArgs<TProperty> : PropertyChangedEventArgs, IMaybe<TProperty>
    {
        public PropertyChangedAndValueEventArgs(string propertyName, TProperty value, bool hasValue)
            : base(propertyName)
        {
            Value = value;
            HasValue = hasValue;
        }

        /// <summary>
        /// Use this to check if the returned value is a default value or read from source.
        /// Example: if subscribing to x => x.Next.Name and Next is null then IsDefaultValue will be true.
        /// If Name is null IsDefaultValue will be false because the value is read from source.
        /// </summary>
        public bool HasValue { get; private set; }

        public TProperty Value { get; private set; }
    }
}