﻿#nullable enable
#pragma warning disable RS0026 // Do not add multiple public overloads with optional parameters
#pragma warning disable RS0027 // Public API with optional parameter(s) should have the most parameters amongst its public overloads
namespace Gu.Reactive
{
    using System;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.Reactive.Concurrency;

    /// <summary>
    /// Factory methods for creating <see cref="MappingView{TSource, TResult}"/>
    /// </summary>
    public static partial class MappingView
    {
        /// <summary>
        /// Create a <see cref="MappingView{TSource, TResult}"/> for <paramref name="source"/>
        /// </summary>
        /// <typeparam name="TSource">The type of the elements in the collection.</typeparam>
        /// <typeparam name="TResult">The type of the elements in the resulting collection.</typeparam>
        /// <param name="source">The source collection</param>
        /// <param name="selector">The function mapping an element of type <typeparamref name="TSource"/> to <typeparamref name="TResult"/>.</param>
        /// <param name="triggers">Additional triggers for when mapping is updated.</param>
        /// <returns>A <see cref="MappingView{TSource, TResult}"/></returns>
        public static MappingView<TSource, TResult> AsMappingView<TSource, TResult>(
            this ObservableCollection<TSource> source,
            Func<TSource, TResult> selector,
            params IObservable<object?>[] triggers)
        {
            if (source is null)
            {
                throw new ArgumentNullException(nameof(source));
            }

            if (selector is null)
            {
                throw new ArgumentNullException(nameof(selector));
            }

            return new MappingView<TSource, TResult>(source, selector, null, false, triggers);
        }

        /// <summary>
        /// Create a <see cref="MappingView{TSource, TResult}"/> for <paramref name="source"/>
        /// </summary>
        /// <typeparam name="TSource">The type of the elements in the collection.</typeparam>
        /// <typeparam name="TResult">The type of the elements in the resulting collection.</typeparam>
        /// <param name="source">The source collection</param>
        /// <param name="selector">The function mapping an element of type <typeparamref name="TSource"/> to <typeparamref name="TResult"/>.</param>
        /// <param name="leaveOpen">True means that the <paramref name="source"/> is not disposed when this instance is disposed.</param>
        /// <param name="triggers">Additional triggers for when mapping is updated.</param>
        /// <returns>A <see cref="MappingView{TSource, TResult}"/></returns>
        public static MappingView<TSource, TResult> AsMappingView<TSource, TResult>(
            this ObservableCollection<TSource> source,
            Func<TSource, TResult> selector,
            bool leaveOpen,
            params IObservable<object?>[] triggers)
        {
            if (source is null)
            {
                throw new ArgumentNullException(nameof(source));
            }

            if (selector is null)
            {
                throw new ArgumentNullException(nameof(selector));
            }

            return new MappingView<TSource, TResult>(source, selector, null, leaveOpen, triggers);
        }

        /// <summary>
        /// Create a <see cref="MappingView{TSource, TResult}"/> for <paramref name="source"/>
        /// </summary>
        /// <typeparam name="TSource">The type of the elements in the collection.</typeparam>
        /// <typeparam name="TResult">The type of the elements in the resulting collection.</typeparam>
        /// <param name="source">The source collection</param>
        /// <param name="selector">The function mapping an element of type <typeparamref name="TSource"/> to <typeparamref name="TResult"/>.</param>
        /// <param name="scheduler">The scheduler to notify changes on.</param>
        /// <param name="leaveOpen">True means that the <paramref name="source"/> is not disposed when this instance is disposed.</param>
        /// <param name="triggers">Additional triggers for when mapping is updated.</param>
        /// <returns>A <see cref="MappingView{TSource, TResult}"/></returns>
        public static MappingView<TSource, TResult> AsMappingView<TSource, TResult>(
            this ObservableCollection<TSource> source,
            Func<TSource, TResult> selector,
            IScheduler? scheduler = null,
            bool leaveOpen = false,
            params IObservable<object?>[] triggers)
        {
            if (source is null)
            {
                throw new ArgumentNullException(nameof(source));
            }

            if (selector is null)
            {
                throw new ArgumentNullException(nameof(selector));
            }

            return new MappingView<TSource, TResult>(source, selector, TimeSpan.Zero, scheduler, leaveOpen, triggers);
        }

        /// <summary>
        /// Create a <see cref="MappingView{TSource, TResult}"/> for <paramref name="source"/>
        /// </summary>
        /// <typeparam name="TSource">The type of the elements in the collection.</typeparam>
        /// <typeparam name="TResult">The type of the elements in the resulting collection.</typeparam>
        /// <param name="source">The source collection</param>
        /// <param name="selector">The function mapping an element of type <typeparamref name="TSource"/> to <typeparamref name="TResult"/>.</param>
        /// <param name="bufferTime">The time to buffer changes before updating and notifying.</param>
        /// <param name="scheduler">The scheduler to notify changes on.</param>
        /// <param name="leaveOpen">True means that the <paramref name="source"/> is not disposed when this instance is disposed.</param>
        /// <param name="triggers">Additional triggers for when mapping is updated.</param>
        /// <returns>A <see cref="MappingView{TSource, TResult}"/></returns>
        public static MappingView<TSource, TResult> AsMappingView<TSource, TResult>(
            this ObservableCollection<TSource> source,
            Func<TSource, TResult> selector,
            TimeSpan bufferTime,
            IScheduler? scheduler = null,
            bool leaveOpen = false,
            params IObservable<object?>[] triggers)
        {
            if (source is null)
            {
                throw new ArgumentNullException(nameof(source));
            }

            if (selector is null)
            {
                throw new ArgumentNullException(nameof(selector));
            }

            return new MappingView<TSource, TResult>(source, selector, bufferTime, scheduler, leaveOpen, triggers);
        }

        /// <summary>
        /// Create a <see cref="MappingView{TSource, TResult}"/> for <paramref name="source"/>
        /// </summary>
        /// <typeparam name="TSource">The type of the elements in the collection.</typeparam>
        /// <typeparam name="TResult">The type of the elements in the resulting collection.</typeparam>
        /// <param name="source">The source collection</param>
        /// <param name="selector">The function mapping an element of type <typeparamref name="TSource"/> to <typeparamref name="TResult"/>.</param>
        /// <param name="onRemove">An action to perform when an item is removed from the collection. Typically it will be x => x.Dispose()</param>
        /// <param name="scheduler">The scheduler to notify changes on.</param>
        /// <param name="leaveOpen">True means that the <paramref name="source"/> is not disposed when this instance is disposed.</param>
        /// <param name="triggers">Additional triggers for when mapping is updated.</param>
        /// <returns>A <see cref="MappingView{TSource, TResult}"/></returns>
        public static MappingView<TSource, TResult> AsMappingView<TSource, TResult>(
            this ObservableCollection<TSource> source,
            Func<TSource, TResult> selector,
            Action<TResult> onRemove,
            IScheduler? scheduler = null,
            bool leaveOpen = false,
            params IObservable<object?>[] triggers)
            where TResult : class
        {
            if (source is null)
            {
                throw new ArgumentNullException(nameof(source));
            }

            if (selector is null)
            {
                throw new ArgumentNullException(nameof(selector));
            }

            if (onRemove is null)
            {
                 throw new ArgumentNullException(nameof(onRemove));
            }

            return new MappingView<TSource, TResult>(source, Mapper.Create(selector, onRemove), TimeSpan.Zero, scheduler, leaveOpen, triggers);
        }

        /// <summary>
        /// Create a <see cref="MappingView{TSource, TResult}"/> for <paramref name="source"/>
        /// </summary>
        /// <typeparam name="TSource">The type of the elements in the collection.</typeparam>
        /// <typeparam name="TResult">The type of the elements in the resulting collection.</typeparam>
        /// <param name="source">The source collection</param>
        /// <param name="selector">The function mapping an element of type <typeparamref name="TSource"/> to <typeparamref name="TResult"/>.</param>
        /// <param name="onRemove">An action to perform when an item is removed from the collection. Typically it will be x => x.Dispose()</param>
        /// <param name="bufferTime">The time to buffer changes before updating and notifying.</param>
        /// <param name="scheduler">The scheduler to notify changes on.</param>
        /// <param name="leaveOpen">True means that the <paramref name="source"/> is not disposed when this instance is disposed.</param>
        /// <param name="triggers">Additional triggers for when mapping is updated.</param>
        /// <returns>A <see cref="MappingView{TSource, TResult}"/></returns>
        public static MappingView<TSource, TResult> AsMappingView<TSource, TResult>(
            this ObservableCollection<TSource> source,
            Func<TSource, TResult> selector,
            Action<TResult> onRemove,
            TimeSpan bufferTime,
            IScheduler? scheduler = null,
            bool leaveOpen = false,
            params IObservable<object?>[] triggers)
            where TResult : class
        {
            if (source is null)
            {
                throw new ArgumentNullException(nameof(source));
            }

            if (selector is null)
            {
                throw new ArgumentNullException(nameof(selector));
            }

            if (onRemove is null)
            {
                 throw new ArgumentNullException(nameof(onRemove));
            }

            return new MappingView<TSource, TResult>(source, Mapper.Create(selector, onRemove), bufferTime, scheduler, leaveOpen, triggers);
        }

        /// <summary>
        /// Create a <see cref="MappingView{TSource, TResult}"/> for <paramref name="source"/>
        /// </summary>
        /// <typeparam name="TSource">The type of the elements in the collection.</typeparam>
        /// <typeparam name="TResult">The type of the elements in the resulting collection.</typeparam>
        /// <param name="source">The source collection</param>
        /// <param name="selector">
        /// The function mapping an element of type <typeparamref name="TSource"/> to <typeparamref name="TResult"/>.
        /// The second parameter is the index of the element.
        /// </param>
        /// <param name="updater">
        /// The function updating an element for which the index changed.
        /// The second parameter is the index of the element.
        /// </param>
        /// <param name="scheduler">The scheduler to notify changes on.</param>
        /// <param name="leaveOpen">True means that the <paramref name="source"/> is not disposed when this instance is disposed.</param>
        /// <param name="triggers">Additional triggers for when mapping is updated.</param>
        /// <returns>A <see cref="MappingView{TSource, TResult}"/></returns>
        public static MappingView<TSource, TResult> AsMappingView<TSource, TResult>(
            this ObservableCollection<TSource> source,
            Func<TSource, int, TResult> selector,
            Func<TResult, int, TResult> updater,
            IScheduler? scheduler = null,
            bool leaveOpen = false,
            params IObservable<object?>[] triggers)
        {
            if (source is null)
            {
                throw new ArgumentNullException(nameof(source));
            }

            if (selector is null)
            {
                throw new ArgumentNullException(nameof(selector));
            }

            if (updater is null)
            {
                 throw new ArgumentNullException(nameof(updater));
            }

            return new MappingView<TSource, TResult>(source, Mapper.Create(selector, updater), TimeSpan.Zero, scheduler, leaveOpen, triggers);
        }

        /// <summary>
        /// Create a <see cref="MappingView{TSource, TResult}"/> for <paramref name="source"/>
        /// </summary>
        /// <typeparam name="TSource">The type of the elements in the collection.</typeparam>
        /// <typeparam name="TResult">The type of the elements in the resulting collection.</typeparam>
        /// <param name="source">The source collection</param>
        /// <param name="selector">
        /// The function mapping an element of type <typeparamref name="TSource"/> to <typeparamref name="TResult"/>.
        /// The second parameter is the index of the element.
        /// </param>
        /// <param name="updater">
        /// The function updating an element for which the index changed.
        /// The second parameter is the index of the element.
        /// </param>
        /// <param name="bufferTime">The time to buffer changes before updating and notifying.</param>
        /// <param name="scheduler">The scheduler to notify changes on.</param>
        /// <param name="leaveOpen">True means that the <paramref name="source"/> is not disposed when this instance is disposed.</param>
        /// <param name="triggers">Additional triggers for when mapping is updated.</param>
        /// <returns>A <see cref="MappingView{TSource, TResult}"/></returns>
        public static MappingView<TSource, TResult> AsMappingView<TSource, TResult>(
            this ObservableCollection<TSource> source,
            Func<TSource, int, TResult> selector,
            Func<TResult, int, TResult> updater,
            TimeSpan bufferTime,
            IScheduler? scheduler = null,
            bool leaveOpen = false,
            params IObservable<object?>[] triggers)
        {
            if (source is null)
            {
                throw new ArgumentNullException(nameof(source));
            }

            if (selector is null)
            {
                throw new ArgumentNullException(nameof(selector));
            }

            if (updater is null)
            {
                 throw new ArgumentNullException(nameof(updater));
            }

            return new MappingView<TSource, TResult>(source, Mapper.Create(selector, updater), bufferTime, scheduler, leaveOpen, triggers);
        }

        /// <summary>
        /// Create a <see cref="MappingView{TSource, TResult}"/> for <paramref name="source"/>
        /// </summary>
        /// <typeparam name="TSource">The type of the elements in the collection.</typeparam>
        /// <typeparam name="TResult">The type of the elements in the resulting collection.</typeparam>
        /// <param name="source">The source collection</param>
        /// <param name="selector">
        /// The function mapping an element of type <typeparamref name="TSource"/> to <typeparamref name="TResult"/>.
        /// The second parameter is the index of the element.
        /// </param>
        /// <param name="updater">
        /// The function updating an element for which the index changed.
        /// The second parameter is the index of the element.
        /// </param>
        /// <param name="onRemove">An action to perform when an item is removed from the collection. Typically it will be x => x.Dispose()</param>
        /// <param name="scheduler">The scheduler to notify changes on.</param>
        /// <param name="leaveOpen">True means that the <paramref name="source"/> is not disposed when this instance is disposed.</param>
        /// <param name="triggers">Additional triggers for when mapping is updated.</param>
        /// <returns>A <see cref="MappingView{TSource, TResult}"/></returns>
        public static MappingView<TSource, TResult> AsMappingView<TSource, TResult>(
            this ObservableCollection<TSource> source,
            Func<TSource, int, TResult> selector,
            Func<TResult, int, TResult> updater,
            Action<TResult> onRemove,
            IScheduler? scheduler = null,
            bool leaveOpen = false,
            params IObservable<object?>[] triggers)
            where TResult : class
        {
            if (source is null)
            {
                throw new ArgumentNullException(nameof(source));
            }

            if (selector is null)
            {
                throw new ArgumentNullException(nameof(selector));
            }

            if (updater is null)
            {
                 throw new ArgumentNullException(nameof(updater));
            }

            return new MappingView<TSource, TResult>(source, Mapper.Create(selector, updater, onRemove), TimeSpan.Zero, scheduler, leaveOpen, triggers);
        }

        /// <summary>
        /// Create a <see cref="MappingView{TSource, TResult}"/> for <paramref name="source"/>
        /// </summary>
        /// <typeparam name="TSource">The type of the elements in the collection.</typeparam>
        /// <typeparam name="TResult">The type of the elements in the resulting collection.</typeparam>
        /// <param name="source">The source collection</param>
        /// <param name="selector">
        /// The function mapping an element of type <typeparamref name="TSource"/> to <typeparamref name="TResult"/>.
        /// The second parameter is the index of the element.
        /// </param>
        /// <param name="updater">
        /// The function updating an element for which the index changed.
        /// The second parameter is the index of the element.
        /// </param>
        /// <param name="onRemove">An action to perform when an item is removed from the collection. Typically it will be x => x.Dispose()</param>
        /// <param name="bufferTime">The time to buffer changes before updating and notifying.</param>
        /// <param name="scheduler">The scheduler to notify changes on.</param>
        /// <param name="leaveOpen">True means that the <paramref name="source"/> is not disposed when this instance is disposed.</param>
        /// <param name="triggers">Additional triggers for when mapping is updated.</param>
        /// <returns>A <see cref="MappingView{TSource, TResult}"/></returns>
        public static MappingView<TSource, TResult> AsMappingView<TSource, TResult>(
            this ObservableCollection<TSource> source,
            Func<TSource, int, TResult> selector,
            Func<TResult, int, TResult> updater,
            Action<TResult> onRemove,
            TimeSpan bufferTime,
            IScheduler? scheduler = null,
            bool leaveOpen = false,
            params IObservable<object?>[] triggers)
            where TResult : class
        {
            if (source is null)
            {
                throw new ArgumentNullException(nameof(source));
            }

            if (selector is null)
            {
                throw new ArgumentNullException(nameof(selector));
            }

            if (updater is null)
            {
                 throw new ArgumentNullException(nameof(updater));
            }

            return new MappingView<TSource, TResult>(source, Mapper.Create(selector, updater, onRemove), bufferTime, scheduler, leaveOpen, triggers);
        }

        /// <summary>
        /// Create a <see cref="MappingView{TSource, TResult}"/> for <paramref name="source"/>
        /// </summary>
        /// <typeparam name="TSource">The type of the elements in the collection.</typeparam>
        /// <typeparam name="TResult">The type of the elements in the resulting collection.</typeparam>
        /// <param name="source">The source collection</param>
        /// <param name="selector">The function mapping an element of type <typeparamref name="TSource"/> to <typeparamref name="TResult"/>.</param>
        /// <param name="triggers">Additional triggers for when mapping is updated.</param>
        /// <returns>A <see cref="MappingView{TSource, TResult}"/></returns>
        public static MappingView<TSource, TResult> AsMappingView<TSource, TResult>(
            this ReadOnlyObservableCollection<TSource> source,
            Func<TSource, TResult> selector,
            params IObservable<object?>[] triggers)
        {
            if (source is null)
            {
                throw new ArgumentNullException(nameof(source));
            }

            if (selector is null)
            {
                throw new ArgumentNullException(nameof(selector));
            }

            return new MappingView<TSource, TResult>(source, selector, null, false, triggers);
        }

        /// <summary>
        /// Create a <see cref="MappingView{TSource, TResult}"/> for <paramref name="source"/>
        /// </summary>
        /// <typeparam name="TSource">The type of the elements in the collection.</typeparam>
        /// <typeparam name="TResult">The type of the elements in the resulting collection.</typeparam>
        /// <param name="source">The source collection</param>
        /// <param name="selector">The function mapping an element of type <typeparamref name="TSource"/> to <typeparamref name="TResult"/>.</param>
        /// <param name="leaveOpen">True means that the <paramref name="source"/> is not disposed when this instance is disposed.</param>
        /// <param name="triggers">Additional triggers for when mapping is updated.</param>
        /// <returns>A <see cref="MappingView{TSource, TResult}"/></returns>
        public static MappingView<TSource, TResult> AsMappingView<TSource, TResult>(
            this ReadOnlyObservableCollection<TSource> source,
            Func<TSource, TResult> selector,
            bool leaveOpen,
            params IObservable<object?>[] triggers)
        {
            if (source is null)
            {
                throw new ArgumentNullException(nameof(source));
            }

            if (selector is null)
            {
                throw new ArgumentNullException(nameof(selector));
            }

            return new MappingView<TSource, TResult>(source, selector, null, leaveOpen, triggers);
        }

        /// <summary>
        /// Create a <see cref="MappingView{TSource, TResult}"/> for <paramref name="source"/>
        /// </summary>
        /// <typeparam name="TSource">The type of the elements in the collection.</typeparam>
        /// <typeparam name="TResult">The type of the elements in the resulting collection.</typeparam>
        /// <param name="source">The source collection</param>
        /// <param name="selector">The function mapping an element of type <typeparamref name="TSource"/> to <typeparamref name="TResult"/>.</param>
        /// <param name="scheduler">The scheduler to notify changes on.</param>
        /// <param name="leaveOpen">True means that the <paramref name="source"/> is not disposed when this instance is disposed.</param>
        /// <param name="triggers">Additional triggers for when mapping is updated.</param>
        /// <returns>A <see cref="MappingView{TSource, TResult}"/></returns>
        public static MappingView<TSource, TResult> AsMappingView<TSource, TResult>(
            this ReadOnlyObservableCollection<TSource> source,
            Func<TSource, TResult> selector,
            IScheduler? scheduler = null,
            bool leaveOpen = false,
            params IObservable<object?>[] triggers)
        {
            if (source is null)
            {
                throw new ArgumentNullException(nameof(source));
            }

            if (selector is null)
            {
                throw new ArgumentNullException(nameof(selector));
            }

            return new MappingView<TSource, TResult>(source, selector, TimeSpan.Zero, scheduler, leaveOpen, triggers);
        }

        /// <summary>
        /// Create a <see cref="MappingView{TSource, TResult}"/> for <paramref name="source"/>
        /// </summary>
        /// <typeparam name="TSource">The type of the elements in the collection.</typeparam>
        /// <typeparam name="TResult">The type of the elements in the resulting collection.</typeparam>
        /// <param name="source">The source collection</param>
        /// <param name="selector">The function mapping an element of type <typeparamref name="TSource"/> to <typeparamref name="TResult"/>.</param>
        /// <param name="bufferTime">The time to buffer changes before updating and notifying.</param>
        /// <param name="scheduler">The scheduler to notify changes on.</param>
        /// <param name="leaveOpen">True means that the <paramref name="source"/> is not disposed when this instance is disposed.</param>
        /// <param name="triggers">Additional triggers for when mapping is updated.</param>
        /// <returns>A <see cref="MappingView{TSource, TResult}"/></returns>
        public static MappingView<TSource, TResult> AsMappingView<TSource, TResult>(
            this ReadOnlyObservableCollection<TSource> source,
            Func<TSource, TResult> selector,
            TimeSpan bufferTime,
            IScheduler? scheduler = null,
            bool leaveOpen = false,
            params IObservable<object?>[] triggers)
        {
            if (source is null)
            {
                throw new ArgumentNullException(nameof(source));
            }

            if (selector is null)
            {
                throw new ArgumentNullException(nameof(selector));
            }

            return new MappingView<TSource, TResult>(source, selector, bufferTime, scheduler, leaveOpen, triggers);
        }

        /// <summary>
        /// Create a <see cref="MappingView{TSource, TResult}"/> for <paramref name="source"/>
        /// </summary>
        /// <typeparam name="TSource">The type of the elements in the collection.</typeparam>
        /// <typeparam name="TResult">The type of the elements in the resulting collection.</typeparam>
        /// <param name="source">The source collection</param>
        /// <param name="selector">The function mapping an element of type <typeparamref name="TSource"/> to <typeparamref name="TResult"/>.</param>
        /// <param name="onRemove">An action to perform when an item is removed from the collection. Typically it will be x => x.Dispose()</param>
        /// <param name="scheduler">The scheduler to notify changes on.</param>
        /// <param name="leaveOpen">True means that the <paramref name="source"/> is not disposed when this instance is disposed.</param>
        /// <param name="triggers">Additional triggers for when mapping is updated.</param>
        /// <returns>A <see cref="MappingView{TSource, TResult}"/></returns>
        public static MappingView<TSource, TResult> AsMappingView<TSource, TResult>(
            this ReadOnlyObservableCollection<TSource> source,
            Func<TSource, TResult> selector,
            Action<TResult> onRemove,
            IScheduler? scheduler = null,
            bool leaveOpen = false,
            params IObservable<object?>[] triggers)
            where TResult : class
        {
            if (source is null)
            {
                throw new ArgumentNullException(nameof(source));
            }

            if (selector is null)
            {
                throw new ArgumentNullException(nameof(selector));
            }

            if (onRemove is null)
            {
                 throw new ArgumentNullException(nameof(onRemove));
            }

            return new MappingView<TSource, TResult>(source, Mapper.Create(selector, onRemove), TimeSpan.Zero, scheduler, leaveOpen, triggers);
        }

        /// <summary>
        /// Create a <see cref="MappingView{TSource, TResult}"/> for <paramref name="source"/>
        /// </summary>
        /// <typeparam name="TSource">The type of the elements in the collection.</typeparam>
        /// <typeparam name="TResult">The type of the elements in the resulting collection.</typeparam>
        /// <param name="source">The source collection</param>
        /// <param name="selector">The function mapping an element of type <typeparamref name="TSource"/> to <typeparamref name="TResult"/>.</param>
        /// <param name="onRemove">An action to perform when an item is removed from the collection. Typically it will be x => x.Dispose()</param>
        /// <param name="bufferTime">The time to buffer changes before updating and notifying.</param>
        /// <param name="scheduler">The scheduler to notify changes on.</param>
        /// <param name="leaveOpen">True means that the <paramref name="source"/> is not disposed when this instance is disposed.</param>
        /// <param name="triggers">Additional triggers for when mapping is updated.</param>
        /// <returns>A <see cref="MappingView{TSource, TResult}"/></returns>
        public static MappingView<TSource, TResult> AsMappingView<TSource, TResult>(
            this ReadOnlyObservableCollection<TSource> source,
            Func<TSource, TResult> selector,
            Action<TResult> onRemove,
            TimeSpan bufferTime,
            IScheduler? scheduler = null,
            bool leaveOpen = false,
            params IObservable<object?>[] triggers)
            where TResult : class
        {
            if (source is null)
            {
                throw new ArgumentNullException(nameof(source));
            }

            if (selector is null)
            {
                throw new ArgumentNullException(nameof(selector));
            }

            if (onRemove is null)
            {
                 throw new ArgumentNullException(nameof(onRemove));
            }

            return new MappingView<TSource, TResult>(source, Mapper.Create(selector, onRemove), bufferTime, scheduler, leaveOpen, triggers);
        }

        /// <summary>
        /// Create a <see cref="MappingView{TSource, TResult}"/> for <paramref name="source"/>
        /// </summary>
        /// <typeparam name="TSource">The type of the elements in the collection.</typeparam>
        /// <typeparam name="TResult">The type of the elements in the resulting collection.</typeparam>
        /// <param name="source">The source collection</param>
        /// <param name="selector">
        /// The function mapping an element of type <typeparamref name="TSource"/> to <typeparamref name="TResult"/>.
        /// The second parameter is the index of the element.
        /// </param>
        /// <param name="updater">
        /// The function updating an element for which the index changed.
        /// The second parameter is the index of the element.
        /// </param>
        /// <param name="scheduler">The scheduler to notify changes on.</param>
        /// <param name="leaveOpen">True means that the <paramref name="source"/> is not disposed when this instance is disposed.</param>
        /// <param name="triggers">Additional triggers for when mapping is updated.</param>
        /// <returns>A <see cref="MappingView{TSource, TResult}"/></returns>
        public static MappingView<TSource, TResult> AsMappingView<TSource, TResult>(
            this ReadOnlyObservableCollection<TSource> source,
            Func<TSource, int, TResult> selector,
            Func<TResult, int, TResult> updater,
            IScheduler? scheduler = null,
            bool leaveOpen = false,
            params IObservable<object?>[] triggers)
        {
            if (source is null)
            {
                throw new ArgumentNullException(nameof(source));
            }

            if (selector is null)
            {
                throw new ArgumentNullException(nameof(selector));
            }

            if (updater is null)
            {
                 throw new ArgumentNullException(nameof(updater));
            }

            return new MappingView<TSource, TResult>(source, Mapper.Create(selector, updater), TimeSpan.Zero, scheduler, leaveOpen, triggers);
        }

        /// <summary>
        /// Create a <see cref="MappingView{TSource, TResult}"/> for <paramref name="source"/>
        /// </summary>
        /// <typeparam name="TSource">The type of the elements in the collection.</typeparam>
        /// <typeparam name="TResult">The type of the elements in the resulting collection.</typeparam>
        /// <param name="source">The source collection</param>
        /// <param name="selector">
        /// The function mapping an element of type <typeparamref name="TSource"/> to <typeparamref name="TResult"/>.
        /// The second parameter is the index of the element.
        /// </param>
        /// <param name="updater">
        /// The function updating an element for which the index changed.
        /// The second parameter is the index of the element.
        /// </param>
        /// <param name="bufferTime">The time to buffer changes before updating and notifying.</param>
        /// <param name="scheduler">The scheduler to notify changes on.</param>
        /// <param name="leaveOpen">True means that the <paramref name="source"/> is not disposed when this instance is disposed.</param>
        /// <param name="triggers">Additional triggers for when mapping is updated.</param>
        /// <returns>A <see cref="MappingView{TSource, TResult}"/></returns>
        public static MappingView<TSource, TResult> AsMappingView<TSource, TResult>(
            this ReadOnlyObservableCollection<TSource> source,
            Func<TSource, int, TResult> selector,
            Func<TResult, int, TResult> updater,
            TimeSpan bufferTime,
            IScheduler? scheduler = null,
            bool leaveOpen = false,
            params IObservable<object?>[] triggers)
        {
            if (source is null)
            {
                throw new ArgumentNullException(nameof(source));
            }

            if (selector is null)
            {
                throw new ArgumentNullException(nameof(selector));
            }

            if (updater is null)
            {
                 throw new ArgumentNullException(nameof(updater));
            }

            return new MappingView<TSource, TResult>(source, Mapper.Create(selector, updater), bufferTime, scheduler, leaveOpen, triggers);
        }

        /// <summary>
        /// Create a <see cref="MappingView{TSource, TResult}"/> for <paramref name="source"/>
        /// </summary>
        /// <typeparam name="TSource">The type of the elements in the collection.</typeparam>
        /// <typeparam name="TResult">The type of the elements in the resulting collection.</typeparam>
        /// <param name="source">The source collection</param>
        /// <param name="selector">
        /// The function mapping an element of type <typeparamref name="TSource"/> to <typeparamref name="TResult"/>.
        /// The second parameter is the index of the element.
        /// </param>
        /// <param name="updater">
        /// The function updating an element for which the index changed.
        /// The second parameter is the index of the element.
        /// </param>
        /// <param name="onRemove">An action to perform when an item is removed from the collection. Typically it will be x => x.Dispose()</param>
        /// <param name="scheduler">The scheduler to notify changes on.</param>
        /// <param name="leaveOpen">True means that the <paramref name="source"/> is not disposed when this instance is disposed.</param>
        /// <param name="triggers">Additional triggers for when mapping is updated.</param>
        /// <returns>A <see cref="MappingView{TSource, TResult}"/></returns>
        public static MappingView<TSource, TResult> AsMappingView<TSource, TResult>(
            this ReadOnlyObservableCollection<TSource> source,
            Func<TSource, int, TResult> selector,
            Func<TResult, int, TResult> updater,
            Action<TResult> onRemove,
            IScheduler? scheduler = null,
            bool leaveOpen = false,
            params IObservable<object?>[] triggers)
            where TResult : class
        {
            if (source is null)
            {
                throw new ArgumentNullException(nameof(source));
            }

            if (selector is null)
            {
                throw new ArgumentNullException(nameof(selector));
            }

            if (updater is null)
            {
                 throw new ArgumentNullException(nameof(updater));
            }

            return new MappingView<TSource, TResult>(source, Mapper.Create(selector, updater, onRemove), TimeSpan.Zero, scheduler, leaveOpen, triggers);
        }

        /// <summary>
        /// Create a <see cref="MappingView{TSource, TResult}"/> for <paramref name="source"/>
        /// </summary>
        /// <typeparam name="TSource">The type of the elements in the collection.</typeparam>
        /// <typeparam name="TResult">The type of the elements in the resulting collection.</typeparam>
        /// <param name="source">The source collection</param>
        /// <param name="selector">
        /// The function mapping an element of type <typeparamref name="TSource"/> to <typeparamref name="TResult"/>.
        /// The second parameter is the index of the element.
        /// </param>
        /// <param name="updater">
        /// The function updating an element for which the index changed.
        /// The second parameter is the index of the element.
        /// </param>
        /// <param name="onRemove">An action to perform when an item is removed from the collection. Typically it will be x => x.Dispose()</param>
        /// <param name="bufferTime">The time to buffer changes before updating and notifying.</param>
        /// <param name="scheduler">The scheduler to notify changes on.</param>
        /// <param name="leaveOpen">True means that the <paramref name="source"/> is not disposed when this instance is disposed.</param>
        /// <param name="triggers">Additional triggers for when mapping is updated.</param>
        /// <returns>A <see cref="MappingView{TSource, TResult}"/></returns>
        public static MappingView<TSource, TResult> AsMappingView<TSource, TResult>(
            this ReadOnlyObservableCollection<TSource> source,
            Func<TSource, int, TResult> selector,
            Func<TResult, int, TResult> updater,
            Action<TResult> onRemove,
            TimeSpan bufferTime,
            IScheduler? scheduler = null,
            bool leaveOpen = false,
            params IObservable<object?>[] triggers)
            where TResult : class
        {
            if (source is null)
            {
                throw new ArgumentNullException(nameof(source));
            }

            if (selector is null)
            {
                throw new ArgumentNullException(nameof(selector));
            }

            if (updater is null)
            {
                 throw new ArgumentNullException(nameof(updater));
            }

            return new MappingView<TSource, TResult>(source, Mapper.Create(selector, updater, onRemove), bufferTime, scheduler, leaveOpen, triggers);
        }

        /// <summary>
        /// Create a <see cref="MappingView{TSource, TResult}"/> for <paramref name="source"/>
        /// </summary>
        /// <typeparam name="TSource">The type of the elements in the collection.</typeparam>
        /// <typeparam name="TResult">The type of the elements in the resulting collection.</typeparam>
        /// <param name="source">The source collection</param>
        /// <param name="selector">The function mapping an element of type <typeparamref name="TSource"/> to <typeparamref name="TResult"/>.</param>
        /// <param name="triggers">Additional triggers for when mapping is updated.</param>
        /// <returns>A <see cref="MappingView{TSource, TResult}"/></returns>
        public static MappingView<TSource, TResult> AsMappingView<TSource, TResult>(
            this IReadOnlyObservableCollection<TSource> source,
            Func<TSource, TResult> selector,
            params IObservable<object?>[] triggers)
        {
            if (source is null)
            {
                throw new ArgumentNullException(nameof(source));
            }

            if (selector is null)
            {
                throw new ArgumentNullException(nameof(selector));
            }

            return new MappingView<TSource, TResult>(source, selector, null, false, triggers);
        }

        /// <summary>
        /// Create a <see cref="MappingView{TSource, TResult}"/> for <paramref name="source"/>
        /// </summary>
        /// <typeparam name="TSource">The type of the elements in the collection.</typeparam>
        /// <typeparam name="TResult">The type of the elements in the resulting collection.</typeparam>
        /// <param name="source">The source collection</param>
        /// <param name="selector">The function mapping an element of type <typeparamref name="TSource"/> to <typeparamref name="TResult"/>.</param>
        /// <param name="leaveOpen">True means that the <paramref name="source"/> is not disposed when this instance is disposed.</param>
        /// <param name="triggers">Additional triggers for when mapping is updated.</param>
        /// <returns>A <see cref="MappingView{TSource, TResult}"/></returns>
        public static MappingView<TSource, TResult> AsMappingView<TSource, TResult>(
            this IReadOnlyObservableCollection<TSource> source,
            Func<TSource, TResult> selector,
            bool leaveOpen,
            params IObservable<object?>[] triggers)
        {
            if (source is null)
            {
                throw new ArgumentNullException(nameof(source));
            }

            if (selector is null)
            {
                throw new ArgumentNullException(nameof(selector));
            }

            return new MappingView<TSource, TResult>(source, selector, null, leaveOpen, triggers);
        }

        /// <summary>
        /// Create a <see cref="MappingView{TSource, TResult}"/> for <paramref name="source"/>
        /// </summary>
        /// <typeparam name="TSource">The type of the elements in the collection.</typeparam>
        /// <typeparam name="TResult">The type of the elements in the resulting collection.</typeparam>
        /// <param name="source">The source collection</param>
        /// <param name="selector">The function mapping an element of type <typeparamref name="TSource"/> to <typeparamref name="TResult"/>.</param>
        /// <param name="scheduler">The scheduler to notify changes on.</param>
        /// <param name="leaveOpen">True means that the <paramref name="source"/> is not disposed when this instance is disposed.</param>
        /// <param name="triggers">Additional triggers for when mapping is updated.</param>
        /// <returns>A <see cref="MappingView{TSource, TResult}"/></returns>
        public static MappingView<TSource, TResult> AsMappingView<TSource, TResult>(
            this IReadOnlyObservableCollection<TSource> source,
            Func<TSource, TResult> selector,
            IScheduler? scheduler = null,
            bool leaveOpen = false,
            params IObservable<object?>[] triggers)
        {
            if (source is null)
            {
                throw new ArgumentNullException(nameof(source));
            }

            if (selector is null)
            {
                throw new ArgumentNullException(nameof(selector));
            }

            return new MappingView<TSource, TResult>(source, selector, TimeSpan.Zero, scheduler, leaveOpen, triggers);
        }

        /// <summary>
        /// Create a <see cref="MappingView{TSource, TResult}"/> for <paramref name="source"/>
        /// </summary>
        /// <typeparam name="TSource">The type of the elements in the collection.</typeparam>
        /// <typeparam name="TResult">The type of the elements in the resulting collection.</typeparam>
        /// <param name="source">The source collection</param>
        /// <param name="selector">The function mapping an element of type <typeparamref name="TSource"/> to <typeparamref name="TResult"/>.</param>
        /// <param name="bufferTime">The time to buffer changes before updating and notifying.</param>
        /// <param name="scheduler">The scheduler to notify changes on.</param>
        /// <param name="leaveOpen">True means that the <paramref name="source"/> is not disposed when this instance is disposed.</param>
        /// <param name="triggers">Additional triggers for when mapping is updated.</param>
        /// <returns>A <see cref="MappingView{TSource, TResult}"/></returns>
        public static MappingView<TSource, TResult> AsMappingView<TSource, TResult>(
            this IReadOnlyObservableCollection<TSource> source,
            Func<TSource, TResult> selector,
            TimeSpan bufferTime,
            IScheduler? scheduler = null,
            bool leaveOpen = false,
            params IObservable<object?>[] triggers)
        {
            if (source is null)
            {
                throw new ArgumentNullException(nameof(source));
            }

            if (selector is null)
            {
                throw new ArgumentNullException(nameof(selector));
            }

            return new MappingView<TSource, TResult>(source, selector, bufferTime, scheduler, leaveOpen, triggers);
        }

        /// <summary>
        /// Create a <see cref="MappingView{TSource, TResult}"/> for <paramref name="source"/>
        /// </summary>
        /// <typeparam name="TSource">The type of the elements in the collection.</typeparam>
        /// <typeparam name="TResult">The type of the elements in the resulting collection.</typeparam>
        /// <param name="source">The source collection</param>
        /// <param name="selector">The function mapping an element of type <typeparamref name="TSource"/> to <typeparamref name="TResult"/>.</param>
        /// <param name="onRemove">An action to perform when an item is removed from the collection. Typically it will be x => x.Dispose()</param>
        /// <param name="scheduler">The scheduler to notify changes on.</param>
        /// <param name="leaveOpen">True means that the <paramref name="source"/> is not disposed when this instance is disposed.</param>
        /// <param name="triggers">Additional triggers for when mapping is updated.</param>
        /// <returns>A <see cref="MappingView{TSource, TResult}"/></returns>
        public static MappingView<TSource, TResult> AsMappingView<TSource, TResult>(
            this IReadOnlyObservableCollection<TSource> source,
            Func<TSource, TResult> selector,
            Action<TResult> onRemove,
            IScheduler? scheduler = null,
            bool leaveOpen = false,
            params IObservable<object?>[] triggers)
            where TResult : class
        {
            if (source is null)
            {
                throw new ArgumentNullException(nameof(source));
            }

            if (selector is null)
            {
                throw new ArgumentNullException(nameof(selector));
            }

            if (onRemove is null)
            {
                 throw new ArgumentNullException(nameof(onRemove));
            }

            return new MappingView<TSource, TResult>(source, Mapper.Create(selector, onRemove), TimeSpan.Zero, scheduler, leaveOpen, triggers);
        }

        /// <summary>
        /// Create a <see cref="MappingView{TSource, TResult}"/> for <paramref name="source"/>
        /// </summary>
        /// <typeparam name="TSource">The type of the elements in the collection.</typeparam>
        /// <typeparam name="TResult">The type of the elements in the resulting collection.</typeparam>
        /// <param name="source">The source collection</param>
        /// <param name="selector">The function mapping an element of type <typeparamref name="TSource"/> to <typeparamref name="TResult"/>.</param>
        /// <param name="onRemove">An action to perform when an item is removed from the collection. Typically it will be x => x.Dispose()</param>
        /// <param name="bufferTime">The time to buffer changes before updating and notifying.</param>
        /// <param name="scheduler">The scheduler to notify changes on.</param>
        /// <param name="leaveOpen">True means that the <paramref name="source"/> is not disposed when this instance is disposed.</param>
        /// <param name="triggers">Additional triggers for when mapping is updated.</param>
        /// <returns>A <see cref="MappingView{TSource, TResult}"/></returns>
        public static MappingView<TSource, TResult> AsMappingView<TSource, TResult>(
            this IReadOnlyObservableCollection<TSource> source,
            Func<TSource, TResult> selector,
            Action<TResult> onRemove,
            TimeSpan bufferTime,
            IScheduler? scheduler = null,
            bool leaveOpen = false,
            params IObservable<object?>[] triggers)
            where TResult : class
        {
            if (source is null)
            {
                throw new ArgumentNullException(nameof(source));
            }

            if (selector is null)
            {
                throw new ArgumentNullException(nameof(selector));
            }

            if (onRemove is null)
            {
                 throw new ArgumentNullException(nameof(onRemove));
            }

            return new MappingView<TSource, TResult>(source, Mapper.Create(selector, onRemove), bufferTime, scheduler, leaveOpen, triggers);
        }

        /// <summary>
        /// Create a <see cref="MappingView{TSource, TResult}"/> for <paramref name="source"/>
        /// </summary>
        /// <typeparam name="TSource">The type of the elements in the collection.</typeparam>
        /// <typeparam name="TResult">The type of the elements in the resulting collection.</typeparam>
        /// <param name="source">The source collection</param>
        /// <param name="selector">
        /// The function mapping an element of type <typeparamref name="TSource"/> to <typeparamref name="TResult"/>.
        /// The second parameter is the index of the element.
        /// </param>
        /// <param name="updater">
        /// The function updating an element for which the index changed.
        /// The second parameter is the index of the element.
        /// </param>
        /// <param name="scheduler">The scheduler to notify changes on.</param>
        /// <param name="leaveOpen">True means that the <paramref name="source"/> is not disposed when this instance is disposed.</param>
        /// <param name="triggers">Additional triggers for when mapping is updated.</param>
        /// <returns>A <see cref="MappingView{TSource, TResult}"/></returns>
        public static MappingView<TSource, TResult> AsMappingView<TSource, TResult>(
            this IReadOnlyObservableCollection<TSource> source,
            Func<TSource, int, TResult> selector,
            Func<TResult, int, TResult> updater,
            IScheduler? scheduler = null,
            bool leaveOpen = false,
            params IObservable<object?>[] triggers)
        {
            if (source is null)
            {
                throw new ArgumentNullException(nameof(source));
            }

            if (selector is null)
            {
                throw new ArgumentNullException(nameof(selector));
            }

            if (updater is null)
            {
                 throw new ArgumentNullException(nameof(updater));
            }

            return new MappingView<TSource, TResult>(source, Mapper.Create(selector, updater), TimeSpan.Zero, scheduler, leaveOpen, triggers);
        }

        /// <summary>
        /// Create a <see cref="MappingView{TSource, TResult}"/> for <paramref name="source"/>
        /// </summary>
        /// <typeparam name="TSource">The type of the elements in the collection.</typeparam>
        /// <typeparam name="TResult">The type of the elements in the resulting collection.</typeparam>
        /// <param name="source">The source collection</param>
        /// <param name="selector">
        /// The function mapping an element of type <typeparamref name="TSource"/> to <typeparamref name="TResult"/>.
        /// The second parameter is the index of the element.
        /// </param>
        /// <param name="updater">
        /// The function updating an element for which the index changed.
        /// The second parameter is the index of the element.
        /// </param>
        /// <param name="bufferTime">The time to buffer changes before updating and notifying.</param>
        /// <param name="scheduler">The scheduler to notify changes on.</param>
        /// <param name="leaveOpen">True means that the <paramref name="source"/> is not disposed when this instance is disposed.</param>
        /// <param name="triggers">Additional triggers for when mapping is updated.</param>
        /// <returns>A <see cref="MappingView{TSource, TResult}"/></returns>
        public static MappingView<TSource, TResult> AsMappingView<TSource, TResult>(
            this IReadOnlyObservableCollection<TSource> source,
            Func<TSource, int, TResult> selector,
            Func<TResult, int, TResult> updater,
            TimeSpan bufferTime,
            IScheduler? scheduler = null,
            bool leaveOpen = false,
            params IObservable<object?>[] triggers)
        {
            if (source is null)
            {
                throw new ArgumentNullException(nameof(source));
            }

            if (selector is null)
            {
                throw new ArgumentNullException(nameof(selector));
            }

            if (updater is null)
            {
                 throw new ArgumentNullException(nameof(updater));
            }

            return new MappingView<TSource, TResult>(source, Mapper.Create(selector, updater), bufferTime, scheduler, leaveOpen, triggers);
        }

        /// <summary>
        /// Create a <see cref="MappingView{TSource, TResult}"/> for <paramref name="source"/>
        /// </summary>
        /// <typeparam name="TSource">The type of the elements in the collection.</typeparam>
        /// <typeparam name="TResult">The type of the elements in the resulting collection.</typeparam>
        /// <param name="source">The source collection</param>
        /// <param name="selector">
        /// The function mapping an element of type <typeparamref name="TSource"/> to <typeparamref name="TResult"/>.
        /// The second parameter is the index of the element.
        /// </param>
        /// <param name="updater">
        /// The function updating an element for which the index changed.
        /// The second parameter is the index of the element.
        /// </param>
        /// <param name="onRemove">An action to perform when an item is removed from the collection. Typically it will be x => x.Dispose()</param>
        /// <param name="scheduler">The scheduler to notify changes on.</param>
        /// <param name="leaveOpen">True means that the <paramref name="source"/> is not disposed when this instance is disposed.</param>
        /// <param name="triggers">Additional triggers for when mapping is updated.</param>
        /// <returns>A <see cref="MappingView{TSource, TResult}"/></returns>
        public static MappingView<TSource, TResult> AsMappingView<TSource, TResult>(
            this IReadOnlyObservableCollection<TSource> source,
            Func<TSource, int, TResult> selector,
            Func<TResult, int, TResult> updater,
            Action<TResult> onRemove,
            IScheduler? scheduler = null,
            bool leaveOpen = false,
            params IObservable<object?>[] triggers)
            where TResult : class
        {
            if (source is null)
            {
                throw new ArgumentNullException(nameof(source));
            }

            if (selector is null)
            {
                throw new ArgumentNullException(nameof(selector));
            }

            if (updater is null)
            {
                 throw new ArgumentNullException(nameof(updater));
            }

            return new MappingView<TSource, TResult>(source, Mapper.Create(selector, updater, onRemove), TimeSpan.Zero, scheduler, leaveOpen, triggers);
        }

        /// <summary>
        /// Create a <see cref="MappingView{TSource, TResult}"/> for <paramref name="source"/>
        /// </summary>
        /// <typeparam name="TSource">The type of the elements in the collection.</typeparam>
        /// <typeparam name="TResult">The type of the elements in the resulting collection.</typeparam>
        /// <param name="source">The source collection</param>
        /// <param name="selector">
        /// The function mapping an element of type <typeparamref name="TSource"/> to <typeparamref name="TResult"/>.
        /// The second parameter is the index of the element.
        /// </param>
        /// <param name="updater">
        /// The function updating an element for which the index changed.
        /// The second parameter is the index of the element.
        /// </param>
        /// <param name="onRemove">An action to perform when an item is removed from the collection. Typically it will be x => x.Dispose()</param>
        /// <param name="bufferTime">The time to buffer changes before updating and notifying.</param>
        /// <param name="scheduler">The scheduler to notify changes on.</param>
        /// <param name="leaveOpen">True means that the <paramref name="source"/> is not disposed when this instance is disposed.</param>
        /// <param name="triggers">Additional triggers for when mapping is updated.</param>
        /// <returns>A <see cref="MappingView{TSource, TResult}"/></returns>
        public static MappingView<TSource, TResult> AsMappingView<TSource, TResult>(
            this IReadOnlyObservableCollection<TSource> source,
            Func<TSource, int, TResult> selector,
            Func<TResult, int, TResult> updater,
            Action<TResult> onRemove,
            TimeSpan bufferTime,
            IScheduler? scheduler = null,
            bool leaveOpen = false,
            params IObservable<object?>[] triggers)
            where TResult : class
        {
            if (source is null)
            {
                throw new ArgumentNullException(nameof(source));
            }

            if (selector is null)
            {
                throw new ArgumentNullException(nameof(selector));
            }

            if (updater is null)
            {
                 throw new ArgumentNullException(nameof(updater));
            }

            return new MappingView<TSource, TResult>(source, Mapper.Create(selector, updater, onRemove), bufferTime, scheduler, leaveOpen, triggers);
        }

        /// <summary>
        /// Create a <see cref="MappingView{TSource, TResult}"/> for <paramref name="source"/>
        /// </summary>
        /// <typeparam name="TSource">The type of the elements in the collection.</typeparam>
        /// <typeparam name="TResult">The type of the elements in the resulting collection.</typeparam>
        /// <param name="source">The source collection</param>
        /// <param name="selector">The function mapping an element of type <typeparamref name="TSource"/> to <typeparamref name="TResult"/>.</param>
        /// <param name="triggers">Additional triggers for when mapping is updated.</param>
        /// <returns>A <see cref="MappingView{TSource, TResult}"/></returns>
        public static MappingView<TSource, TResult> AsMappingView<TSource, TResult>(
            this IObservable<IEnumerable<TSource>?> source,
            Func<TSource, TResult> selector,
            params IObservable<object?>[] triggers)
        {
            if (source is null)
            {
                throw new ArgumentNullException(nameof(source));
            }

            if (selector is null)
            {
                throw new ArgumentNullException(nameof(selector));
            }

            return source.AsReadOnlyView()
                         .AsMappingView<TSource, TResult>(selector, null, false, triggers);
        }

        /// <summary>
        /// Create a <see cref="MappingView{TSource, TResult}"/> for <paramref name="source"/>
        /// </summary>
        /// <typeparam name="TSource">The type of the elements in the collection.</typeparam>
        /// <typeparam name="TResult">The type of the elements in the resulting collection.</typeparam>
        /// <param name="source">The source collection</param>
        /// <param name="selector">The function mapping an element of type <typeparamref name="TSource"/> to <typeparamref name="TResult"/>.</param>
        /// <param name="scheduler">The scheduler to notify changes on.</param>
        /// <param name="triggers">Additional triggers for when mapping is updated.</param>
        /// <returns>A <see cref="MappingView{TSource, TResult}"/></returns>
        public static MappingView<TSource, TResult> AsMappingView<TSource, TResult>(
            this IObservable<IEnumerable<TSource>?> source,
            Func<TSource, TResult> selector,
            IScheduler? scheduler = null,
            params IObservable<object?>[] triggers)
        {
            if (source is null)
            {
                throw new ArgumentNullException(nameof(source));
            }

            if (selector is null)
            {
                throw new ArgumentNullException(nameof(selector));
            }

            return source.AsReadOnlyView()
                         .AsMappingView<TSource, TResult>(selector, TimeSpan.Zero, scheduler, leaveOpen: false, triggers: triggers);
        }

        /// <summary>
        /// Create a <see cref="MappingView{TSource, TResult}"/> for <paramref name="source"/>
        /// </summary>
        /// <typeparam name="TSource">The type of the elements in the collection.</typeparam>
        /// <typeparam name="TResult">The type of the elements in the resulting collection.</typeparam>
        /// <param name="source">The source collection</param>
        /// <param name="selector">The function mapping an element of type <typeparamref name="TSource"/> to <typeparamref name="TResult"/>.</param>
        /// <param name="bufferTime">The time to buffer changes before updating and notifying.</param>
        /// <param name="scheduler">The scheduler to notify changes on.</param>
        /// <param name="triggers">Additional triggers for when mapping is updated.</param>
        /// <returns>A <see cref="MappingView{TSource, TResult}"/></returns>
        public static MappingView<TSource, TResult> AsMappingView<TSource, TResult>(
            this IObservable<IEnumerable<TSource>?> source,
            Func<TSource, TResult> selector,
            TimeSpan bufferTime,
            IScheduler? scheduler = null,
            params IObservable<object?>[] triggers)
        {
            if (source is null)
            {
                throw new ArgumentNullException(nameof(source));
            }

            if (selector is null)
            {
                throw new ArgumentNullException(nameof(selector));
            }

            return source.AsReadOnlyView()
                         .AsMappingView<TSource, TResult>(selector, bufferTime, scheduler, leaveOpen: false, triggers: triggers);
        }

        /// <summary>
        /// Create a <see cref="MappingView{TSource, TResult}"/> for <paramref name="source"/>
        /// </summary>
        /// <typeparam name="TSource">The type of the elements in the collection.</typeparam>
        /// <typeparam name="TResult">The type of the elements in the resulting collection.</typeparam>
        /// <param name="source">The source collection</param>
        /// <param name="selector">The function mapping an element of type <typeparamref name="TSource"/> to <typeparamref name="TResult"/>.</param>
        /// <param name="onRemove">An action to perform when an item is removed from the collection. Typically it will be x => x.Dispose()</param>
        /// <param name="scheduler">The scheduler to notify changes on.</param>
        /// <param name="triggers">Additional triggers for when mapping is updated.</param>
        /// <returns>A <see cref="MappingView{TSource, TResult}"/></returns>
        public static MappingView<TSource, TResult> AsMappingView<TSource, TResult>(
            this IObservable<IEnumerable<TSource>?> source,
            Func<TSource, TResult> selector,
            Action<TResult> onRemove,
            IScheduler? scheduler = null,
            params IObservable<object?>[] triggers)
            where TResult : class
        {
            if (source is null)
            {
                throw new ArgumentNullException(nameof(source));
            }

            if (selector is null)
            {
                throw new ArgumentNullException(nameof(selector));
            }

            if (onRemove is null)
            {
                 throw new ArgumentNullException(nameof(onRemove));
            }

            return source.AsReadOnlyView()
                         .AsMappingView<TSource, TResult>(selector, onRemove, TimeSpan.Zero, scheduler, leaveOpen: false, triggers: triggers);
        }

        /// <summary>
        /// Create a <see cref="MappingView{TSource, TResult}"/> for <paramref name="source"/>
        /// </summary>
        /// <typeparam name="TSource">The type of the elements in the collection.</typeparam>
        /// <typeparam name="TResult">The type of the elements in the resulting collection.</typeparam>
        /// <param name="source">The source collection</param>
        /// <param name="selector">The function mapping an element of type <typeparamref name="TSource"/> to <typeparamref name="TResult"/>.</param>
        /// <param name="onRemove">An action to perform when an item is removed from the collection. Typically it will be x => x.Dispose()</param>
        /// <param name="bufferTime">The time to buffer changes before updating and notifying.</param>
        /// <param name="scheduler">The scheduler to notify changes on.</param>
        /// <param name="triggers">Additional triggers for when mapping is updated.</param>
        /// <returns>A <see cref="MappingView{TSource, TResult}"/></returns>
        public static MappingView<TSource, TResult> AsMappingView<TSource, TResult>(
            this IObservable<IEnumerable<TSource>?> source,
            Func<TSource, TResult> selector,
            Action<TResult> onRemove,
            TimeSpan bufferTime,
            IScheduler? scheduler = null,
            params IObservable<object?>[] triggers)
            where TResult : class
        {
            if (source is null)
            {
                throw new ArgumentNullException(nameof(source));
            }

            if (selector is null)
            {
                throw new ArgumentNullException(nameof(selector));
            }

            if (onRemove is null)
            {
                 throw new ArgumentNullException(nameof(onRemove));
            }

            return source.AsReadOnlyView()
                         .AsMappingView<TSource, TResult>(selector, onRemove, bufferTime, scheduler, leaveOpen: false, triggers: triggers);
        }

        /// <summary>
        /// Create a <see cref="MappingView{TSource, TResult}"/> for <paramref name="source"/>
        /// </summary>
        /// <typeparam name="TSource">The type of the elements in the collection.</typeparam>
        /// <typeparam name="TResult">The type of the elements in the resulting collection.</typeparam>
        /// <param name="source">The source collection</param>
        /// <param name="selector">
        /// The function mapping an element of type <typeparamref name="TSource"/> to <typeparamref name="TResult"/>.
        /// The second parameter is the index of the element.
        /// </param>
        /// <param name="updater">
        /// The function updating an element for which the index changed.
        /// The second parameter is the index of the element.
        /// </param>
        /// <param name="scheduler">The scheduler to notify changes on.</param>
        /// <param name="triggers">Additional triggers for when mapping is updated.</param>
        /// <returns>A <see cref="MappingView{TSource, TResult}"/></returns>
        public static MappingView<TSource, TResult> AsMappingView<TSource, TResult>(
            this IObservable<IEnumerable<TSource>?> source,
            Func<TSource, int, TResult> selector,
            Func<TResult, int, TResult> updater,
            IScheduler? scheduler = null,
            params IObservable<object?>[] triggers)
        {
            if (source is null)
            {
                throw new ArgumentNullException(nameof(source));
            }

            if (selector is null)
            {
                throw new ArgumentNullException(nameof(selector));
            }

            if (updater is null)
            {
                 throw new ArgumentNullException(nameof(updater));
            }

            return source.AsReadOnlyView()
                         .AsMappingView<TSource, TResult>(selector, updater, TimeSpan.Zero, scheduler, leaveOpen: false, triggers: triggers);
        }

        /// <summary>
        /// Create a <see cref="MappingView{TSource, TResult}"/> for <paramref name="source"/>
        /// </summary>
        /// <typeparam name="TSource">The type of the elements in the collection.</typeparam>
        /// <typeparam name="TResult">The type of the elements in the resulting collection.</typeparam>
        /// <param name="source">The source collection</param>
        /// <param name="selector">
        /// The function mapping an element of type <typeparamref name="TSource"/> to <typeparamref name="TResult"/>.
        /// The second parameter is the index of the element.
        /// </param>
        /// <param name="updater">
        /// The function updating an element for which the index changed.
        /// The second parameter is the index of the element.
        /// </param>
        /// <param name="bufferTime">The time to buffer changes before updating and notifying.</param>
        /// <param name="scheduler">The scheduler to notify changes on.</param>
        /// <param name="triggers">Additional triggers for when mapping is updated.</param>
        /// <returns>A <see cref="MappingView{TSource, TResult}"/></returns>
        public static MappingView<TSource, TResult> AsMappingView<TSource, TResult>(
            this IObservable<IEnumerable<TSource>?> source,
            Func<TSource, int, TResult> selector,
            Func<TResult, int, TResult> updater,
            TimeSpan bufferTime,
            IScheduler? scheduler = null,
            params IObservable<object?>[] triggers)
        {
            if (source is null)
            {
                throw new ArgumentNullException(nameof(source));
            }

            if (selector is null)
            {
                throw new ArgumentNullException(nameof(selector));
            }

            if (updater is null)
            {
                 throw new ArgumentNullException(nameof(updater));
            }

            return source.AsReadOnlyView()
                         .AsMappingView<TSource, TResult>(selector, updater, bufferTime, scheduler, leaveOpen: false, triggers: triggers);
        }

        /// <summary>
        /// Create a <see cref="MappingView{TSource, TResult}"/> for <paramref name="source"/>
        /// </summary>
        /// <typeparam name="TSource">The type of the elements in the collection.</typeparam>
        /// <typeparam name="TResult">The type of the elements in the resulting collection.</typeparam>
        /// <param name="source">The source collection</param>
        /// <param name="selector">
        /// The function mapping an element of type <typeparamref name="TSource"/> to <typeparamref name="TResult"/>.
        /// The second parameter is the index of the element.
        /// </param>
        /// <param name="updater">
        /// The function updating an element for which the index changed.
        /// The second parameter is the index of the element.
        /// </param>
        /// <param name="onRemove">An action to perform when an item is removed from the collection. Typically it will be x => x.Dispose()</param>
        /// <param name="scheduler">The scheduler to notify changes on.</param>
        /// <param name="triggers">Additional triggers for when mapping is updated.</param>
        /// <returns>A <see cref="MappingView{TSource, TResult}"/></returns>
        public static MappingView<TSource, TResult> AsMappingView<TSource, TResult>(
            this IObservable<IEnumerable<TSource>?> source,
            Func<TSource, int, TResult> selector,
            Func<TResult, int, TResult> updater,
            Action<TResult> onRemove,
            IScheduler? scheduler = null,
            params IObservable<object?>[] triggers)
            where TResult : class
        {
            if (source is null)
            {
                throw new ArgumentNullException(nameof(source));
            }

            if (selector is null)
            {
                throw new ArgumentNullException(nameof(selector));
            }

            if (updater is null)
            {
                 throw new ArgumentNullException(nameof(updater));
            }

            return source.AsReadOnlyView()
                         .AsMappingView<TSource, TResult>(selector, updater, onRemove, TimeSpan.Zero, scheduler, leaveOpen: false, triggers: triggers);
        }

        /// <summary>
        /// Create a <see cref="MappingView{TSource, TResult}"/> for <paramref name="source"/>
        /// </summary>
        /// <typeparam name="TSource">The type of the elements in the collection.</typeparam>
        /// <typeparam name="TResult">The type of the elements in the resulting collection.</typeparam>
        /// <param name="source">The source collection</param>
        /// <param name="selector">
        /// The function mapping an element of type <typeparamref name="TSource"/> to <typeparamref name="TResult"/>.
        /// The second parameter is the index of the element.
        /// </param>
        /// <param name="updater">
        /// The function updating an element for which the index changed.
        /// The second parameter is the index of the element.
        /// </param>
        /// <param name="onRemove">An action to perform when an item is removed from the collection. Typically it will be x => x.Dispose()</param>
        /// <param name="bufferTime">The time to buffer changes before updating and notifying.</param>
        /// <param name="scheduler">The scheduler to notify changes on.</param>
        /// <param name="triggers">Additional triggers for when mapping is updated.</param>
        /// <returns>A <see cref="MappingView{TSource, TResult}"/></returns>
        public static MappingView<TSource, TResult> AsMappingView<TSource, TResult>(
            this IObservable<IEnumerable<TSource>?> source,
            Func<TSource, int, TResult> selector,
            Func<TResult, int, TResult> updater,
            Action<TResult> onRemove,
            TimeSpan bufferTime,
            IScheduler? scheduler = null,
            params IObservable<object?>[] triggers)
            where TResult : class
        {
            if (source is null)
            {
                throw new ArgumentNullException(nameof(source));
            }

            if (selector is null)
            {
                throw new ArgumentNullException(nameof(selector));
            }

            if (updater is null)
            {
                 throw new ArgumentNullException(nameof(updater));
            }

            return source.AsReadOnlyView()
                         .AsMappingView<TSource, TResult>(selector, updater, onRemove, bufferTime, scheduler, leaveOpen: false, triggers: triggers);
        }

        /// <summary>
        /// Create a <see cref="MappingView{TSource, TResult}"/> for <paramref name="source"/>
        /// </summary>
        /// <typeparam name="TSource">The type of the elements in the collection.</typeparam>
        /// <typeparam name="TResult">The type of the elements in the resulting collection.</typeparam>
        /// <param name="source">The source collection</param>
        /// <param name="selector">The function mapping an element of type <typeparamref name="TSource"/> to <typeparamref name="TResult"/>.</param>
        /// <param name="triggers">Additional triggers for when mapping is updated.</param>
        /// <returns>A <see cref="MappingView{TSource, TResult}"/></returns>
        public static MappingView<TSource, TResult> AsMappingView<TSource, TResult>(
            this IObservable<IMaybe<IEnumerable<TSource>?>> source,
            Func<TSource, TResult> selector,
            params IObservable<object?>[] triggers)
        {
            if (source is null)
            {
                throw new ArgumentNullException(nameof(source));
            }

            if (selector is null)
            {
                throw new ArgumentNullException(nameof(selector));
            }

            return source.AsReadOnlyView()
                         .AsMappingView<TSource, TResult>(selector, null, false, triggers);
        }

        /// <summary>
        /// Create a <see cref="MappingView{TSource, TResult}"/> for <paramref name="source"/>
        /// </summary>
        /// <typeparam name="TSource">The type of the elements in the collection.</typeparam>
        /// <typeparam name="TResult">The type of the elements in the resulting collection.</typeparam>
        /// <param name="source">The source collection</param>
        /// <param name="selector">The function mapping an element of type <typeparamref name="TSource"/> to <typeparamref name="TResult"/>.</param>
        /// <param name="scheduler">The scheduler to notify changes on.</param>
        /// <param name="triggers">Additional triggers for when mapping is updated.</param>
        /// <returns>A <see cref="MappingView{TSource, TResult}"/></returns>
        public static MappingView<TSource, TResult> AsMappingView<TSource, TResult>(
            this IObservable<IMaybe<IEnumerable<TSource>?>> source,
            Func<TSource, TResult> selector,
            IScheduler? scheduler = null,
            params IObservable<object?>[] triggers)
        {
            if (source is null)
            {
                throw new ArgumentNullException(nameof(source));
            }

            if (selector is null)
            {
                throw new ArgumentNullException(nameof(selector));
            }

            return source.AsReadOnlyView()
                         .AsMappingView<TSource, TResult>(selector, TimeSpan.Zero, scheduler, leaveOpen: false, triggers: triggers);
        }

        /// <summary>
        /// Create a <see cref="MappingView{TSource, TResult}"/> for <paramref name="source"/>
        /// </summary>
        /// <typeparam name="TSource">The type of the elements in the collection.</typeparam>
        /// <typeparam name="TResult">The type of the elements in the resulting collection.</typeparam>
        /// <param name="source">The source collection</param>
        /// <param name="selector">The function mapping an element of type <typeparamref name="TSource"/> to <typeparamref name="TResult"/>.</param>
        /// <param name="bufferTime">The time to buffer changes before updating and notifying.</param>
        /// <param name="scheduler">The scheduler to notify changes on.</param>
        /// <param name="triggers">Additional triggers for when mapping is updated.</param>
        /// <returns>A <see cref="MappingView{TSource, TResult}"/></returns>
        public static MappingView<TSource, TResult> AsMappingView<TSource, TResult>(
            this IObservable<IMaybe<IEnumerable<TSource>?>> source,
            Func<TSource, TResult> selector,
            TimeSpan bufferTime,
            IScheduler? scheduler = null,
            params IObservable<object?>[] triggers)
        {
            if (source is null)
            {
                throw new ArgumentNullException(nameof(source));
            }

            if (selector is null)
            {
                throw new ArgumentNullException(nameof(selector));
            }

            return source.AsReadOnlyView()
                         .AsMappingView<TSource, TResult>(selector, bufferTime, scheduler, leaveOpen: false, triggers: triggers);
        }

        /// <summary>
        /// Create a <see cref="MappingView{TSource, TResult}"/> for <paramref name="source"/>
        /// </summary>
        /// <typeparam name="TSource">The type of the elements in the collection.</typeparam>
        /// <typeparam name="TResult">The type of the elements in the resulting collection.</typeparam>
        /// <param name="source">The source collection</param>
        /// <param name="selector">The function mapping an element of type <typeparamref name="TSource"/> to <typeparamref name="TResult"/>.</param>
        /// <param name="onRemove">An action to perform when an item is removed from the collection. Typically it will be x => x.Dispose()</param>
        /// <param name="scheduler">The scheduler to notify changes on.</param>
        /// <param name="triggers">Additional triggers for when mapping is updated.</param>
        /// <returns>A <see cref="MappingView{TSource, TResult}"/></returns>
        public static MappingView<TSource, TResult> AsMappingView<TSource, TResult>(
            this IObservable<IMaybe<IEnumerable<TSource>?>> source,
            Func<TSource, TResult> selector,
            Action<TResult> onRemove,
            IScheduler? scheduler = null,
            params IObservable<object?>[] triggers)
            where TResult : class
        {
            if (source is null)
            {
                throw new ArgumentNullException(nameof(source));
            }

            if (selector is null)
            {
                throw new ArgumentNullException(nameof(selector));
            }

            if (onRemove is null)
            {
                 throw new ArgumentNullException(nameof(onRemove));
            }

            return source.AsReadOnlyView()
                         .AsMappingView<TSource, TResult>(selector, onRemove, TimeSpan.Zero, scheduler, leaveOpen: false, triggers: triggers);
        }

        /// <summary>
        /// Create a <see cref="MappingView{TSource, TResult}"/> for <paramref name="source"/>
        /// </summary>
        /// <typeparam name="TSource">The type of the elements in the collection.</typeparam>
        /// <typeparam name="TResult">The type of the elements in the resulting collection.</typeparam>
        /// <param name="source">The source collection</param>
        /// <param name="selector">The function mapping an element of type <typeparamref name="TSource"/> to <typeparamref name="TResult"/>.</param>
        /// <param name="onRemove">An action to perform when an item is removed from the collection. Typically it will be x => x.Dispose()</param>
        /// <param name="bufferTime">The time to buffer changes before updating and notifying.</param>
        /// <param name="scheduler">The scheduler to notify changes on.</param>
        /// <param name="triggers">Additional triggers for when mapping is updated.</param>
        /// <returns>A <see cref="MappingView{TSource, TResult}"/></returns>
        public static MappingView<TSource, TResult> AsMappingView<TSource, TResult>(
            this IObservable<IMaybe<IEnumerable<TSource>?>> source,
            Func<TSource, TResult> selector,
            Action<TResult> onRemove,
            TimeSpan bufferTime,
            IScheduler? scheduler = null,
            params IObservable<object?>[] triggers)
            where TResult : class
        {
            if (source is null)
            {
                throw new ArgumentNullException(nameof(source));
            }

            if (selector is null)
            {
                throw new ArgumentNullException(nameof(selector));
            }

            if (onRemove is null)
            {
                 throw new ArgumentNullException(nameof(onRemove));
            }

            return source.AsReadOnlyView()
                         .AsMappingView<TSource, TResult>(selector, onRemove, bufferTime, scheduler, leaveOpen: false, triggers: triggers);
        }

        /// <summary>
        /// Create a <see cref="MappingView{TSource, TResult}"/> for <paramref name="source"/>
        /// </summary>
        /// <typeparam name="TSource">The type of the elements in the collection.</typeparam>
        /// <typeparam name="TResult">The type of the elements in the resulting collection.</typeparam>
        /// <param name="source">The source collection</param>
        /// <param name="selector">
        /// The function mapping an element of type <typeparamref name="TSource"/> to <typeparamref name="TResult"/>.
        /// The second parameter is the index of the element.
        /// </param>
        /// <param name="updater">
        /// The function updating an element for which the index changed.
        /// The second parameter is the index of the element.
        /// </param>
        /// <param name="scheduler">The scheduler to notify changes on.</param>
        /// <param name="triggers">Additional triggers for when mapping is updated.</param>
        /// <returns>A <see cref="MappingView{TSource, TResult}"/></returns>
        public static MappingView<TSource, TResult> AsMappingView<TSource, TResult>(
            this IObservable<IMaybe<IEnumerable<TSource>?>> source,
            Func<TSource, int, TResult> selector,
            Func<TResult, int, TResult> updater,
            IScheduler? scheduler = null,
            params IObservable<object?>[] triggers)
        {
            if (source is null)
            {
                throw new ArgumentNullException(nameof(source));
            }

            if (selector is null)
            {
                throw new ArgumentNullException(nameof(selector));
            }

            if (updater is null)
            {
                 throw new ArgumentNullException(nameof(updater));
            }

            return source.AsReadOnlyView()
                         .AsMappingView<TSource, TResult>(selector, updater, TimeSpan.Zero, scheduler, leaveOpen: false, triggers: triggers);
        }

        /// <summary>
        /// Create a <see cref="MappingView{TSource, TResult}"/> for <paramref name="source"/>
        /// </summary>
        /// <typeparam name="TSource">The type of the elements in the collection.</typeparam>
        /// <typeparam name="TResult">The type of the elements in the resulting collection.</typeparam>
        /// <param name="source">The source collection</param>
        /// <param name="selector">
        /// The function mapping an element of type <typeparamref name="TSource"/> to <typeparamref name="TResult"/>.
        /// The second parameter is the index of the element.
        /// </param>
        /// <param name="updater">
        /// The function updating an element for which the index changed.
        /// The second parameter is the index of the element.
        /// </param>
        /// <param name="bufferTime">The time to buffer changes before updating and notifying.</param>
        /// <param name="scheduler">The scheduler to notify changes on.</param>
        /// <param name="triggers">Additional triggers for when mapping is updated.</param>
        /// <returns>A <see cref="MappingView{TSource, TResult}"/></returns>
        public static MappingView<TSource, TResult> AsMappingView<TSource, TResult>(
            this IObservable<IMaybe<IEnumerable<TSource>?>> source,
            Func<TSource, int, TResult> selector,
            Func<TResult, int, TResult> updater,
            TimeSpan bufferTime,
            IScheduler? scheduler = null,
            params IObservable<object?>[] triggers)
        {
            if (source is null)
            {
                throw new ArgumentNullException(nameof(source));
            }

            if (selector is null)
            {
                throw new ArgumentNullException(nameof(selector));
            }

            if (updater is null)
            {
                 throw new ArgumentNullException(nameof(updater));
            }

            return source.AsReadOnlyView()
                         .AsMappingView<TSource, TResult>(selector, updater, bufferTime, scheduler, leaveOpen: false, triggers: triggers);
        }

        /// <summary>
        /// Create a <see cref="MappingView{TSource, TResult}"/> for <paramref name="source"/>
        /// </summary>
        /// <typeparam name="TSource">The type of the elements in the collection.</typeparam>
        /// <typeparam name="TResult">The type of the elements in the resulting collection.</typeparam>
        /// <param name="source">The source collection</param>
        /// <param name="selector">
        /// The function mapping an element of type <typeparamref name="TSource"/> to <typeparamref name="TResult"/>.
        /// The second parameter is the index of the element.
        /// </param>
        /// <param name="updater">
        /// The function updating an element for which the index changed.
        /// The second parameter is the index of the element.
        /// </param>
        /// <param name="onRemove">An action to perform when an item is removed from the collection. Typically it will be x => x.Dispose()</param>
        /// <param name="scheduler">The scheduler to notify changes on.</param>
        /// <param name="triggers">Additional triggers for when mapping is updated.</param>
        /// <returns>A <see cref="MappingView{TSource, TResult}"/></returns>
        public static MappingView<TSource, TResult> AsMappingView<TSource, TResult>(
            this IObservable<IMaybe<IEnumerable<TSource>?>> source,
            Func<TSource, int, TResult> selector,
            Func<TResult, int, TResult> updater,
            Action<TResult> onRemove,
            IScheduler? scheduler = null,
            params IObservable<object?>[] triggers)
            where TResult : class
        {
            if (source is null)
            {
                throw new ArgumentNullException(nameof(source));
            }

            if (selector is null)
            {
                throw new ArgumentNullException(nameof(selector));
            }

            if (updater is null)
            {
                 throw new ArgumentNullException(nameof(updater));
            }

            return source.AsReadOnlyView()
                         .AsMappingView<TSource, TResult>(selector, updater, onRemove, TimeSpan.Zero, scheduler, leaveOpen: false, triggers: triggers);
        }

        /// <summary>
        /// Create a <see cref="MappingView{TSource, TResult}"/> for <paramref name="source"/>
        /// </summary>
        /// <typeparam name="TSource">The type of the elements in the collection.</typeparam>
        /// <typeparam name="TResult">The type of the elements in the resulting collection.</typeparam>
        /// <param name="source">The source collection</param>
        /// <param name="selector">
        /// The function mapping an element of type <typeparamref name="TSource"/> to <typeparamref name="TResult"/>.
        /// The second parameter is the index of the element.
        /// </param>
        /// <param name="updater">
        /// The function updating an element for which the index changed.
        /// The second parameter is the index of the element.
        /// </param>
        /// <param name="onRemove">An action to perform when an item is removed from the collection. Typically it will be x => x.Dispose()</param>
        /// <param name="bufferTime">The time to buffer changes before updating and notifying.</param>
        /// <param name="scheduler">The scheduler to notify changes on.</param>
        /// <param name="triggers">Additional triggers for when mapping is updated.</param>
        /// <returns>A <see cref="MappingView{TSource, TResult}"/></returns>
        public static MappingView<TSource, TResult> AsMappingView<TSource, TResult>(
            this IObservable<IMaybe<IEnumerable<TSource>?>> source,
            Func<TSource, int, TResult> selector,
            Func<TResult, int, TResult> updater,
            Action<TResult> onRemove,
            TimeSpan bufferTime,
            IScheduler? scheduler = null,
            params IObservable<object?>[] triggers)
            where TResult : class
        {
            if (source is null)
            {
                throw new ArgumentNullException(nameof(source));
            }

            if (selector is null)
            {
                throw new ArgumentNullException(nameof(selector));
            }

            if (updater is null)
            {
                 throw new ArgumentNullException(nameof(updater));
            }

            return source.AsReadOnlyView()
                         .AsMappingView<TSource, TResult>(selector, updater, onRemove, bufferTime, scheduler, leaveOpen: false, triggers: triggers);
        }

        /// <summary>
        /// Create a <see cref="MappingView{TSource, TResult}"/> for <paramref name="source"/>
        /// </summary>
        /// <typeparam name="TSource">The type of the elements in the collection.</typeparam>
        /// <typeparam name="TResult">The type of the elements in the resulting collection.</typeparam>
        /// <param name="source">The source collection</param>
        /// <param name="selector">The function mapping an element of type <typeparamref name="TSource"/> to <typeparamref name="TResult"/>.</param>
        /// <param name="triggers">Additional triggers for when mapping is updated.</param>
        /// <returns>A <see cref="MappingView{TSource, TResult}"/></returns>
        public static MappingView<TSource, TResult> AsMappingView<TSource, TResult>(
            this IObservable<Maybe<IEnumerable<TSource>?>> source,
            Func<TSource, TResult> selector,
            params IObservable<object?>[] triggers)
        {
            if (source is null)
            {
                throw new ArgumentNullException(nameof(source));
            }

            if (selector is null)
            {
                throw new ArgumentNullException(nameof(selector));
            }

            return source.AsReadOnlyView()
                         .AsMappingView<TSource, TResult>(selector, null, false, triggers);
        }

        /// <summary>
        /// Create a <see cref="MappingView{TSource, TResult}"/> for <paramref name="source"/>
        /// </summary>
        /// <typeparam name="TSource">The type of the elements in the collection.</typeparam>
        /// <typeparam name="TResult">The type of the elements in the resulting collection.</typeparam>
        /// <param name="source">The source collection</param>
        /// <param name="selector">The function mapping an element of type <typeparamref name="TSource"/> to <typeparamref name="TResult"/>.</param>
        /// <param name="scheduler">The scheduler to notify changes on.</param>
        /// <param name="triggers">Additional triggers for when mapping is updated.</param>
        /// <returns>A <see cref="MappingView{TSource, TResult}"/></returns>
        public static MappingView<TSource, TResult> AsMappingView<TSource, TResult>(
            this IObservable<Maybe<IEnumerable<TSource>?>> source,
            Func<TSource, TResult> selector,
            IScheduler? scheduler = null,
            params IObservable<object?>[] triggers)
        {
            if (source is null)
            {
                throw new ArgumentNullException(nameof(source));
            }

            if (selector is null)
            {
                throw new ArgumentNullException(nameof(selector));
            }

            return source.AsReadOnlyView()
                         .AsMappingView<TSource, TResult>(selector, TimeSpan.Zero, scheduler, leaveOpen: false, triggers: triggers);
        }

        /// <summary>
        /// Create a <see cref="MappingView{TSource, TResult}"/> for <paramref name="source"/>
        /// </summary>
        /// <typeparam name="TSource">The type of the elements in the collection.</typeparam>
        /// <typeparam name="TResult">The type of the elements in the resulting collection.</typeparam>
        /// <param name="source">The source collection</param>
        /// <param name="selector">The function mapping an element of type <typeparamref name="TSource"/> to <typeparamref name="TResult"/>.</param>
        /// <param name="bufferTime">The time to buffer changes before updating and notifying.</param>
        /// <param name="scheduler">The scheduler to notify changes on.</param>
        /// <param name="triggers">Additional triggers for when mapping is updated.</param>
        /// <returns>A <see cref="MappingView{TSource, TResult}"/></returns>
        public static MappingView<TSource, TResult> AsMappingView<TSource, TResult>(
            this IObservable<Maybe<IEnumerable<TSource>?>> source,
            Func<TSource, TResult> selector,
            TimeSpan bufferTime,
            IScheduler? scheduler = null,
            params IObservable<object?>[] triggers)
        {
            if (source is null)
            {
                throw new ArgumentNullException(nameof(source));
            }

            if (selector is null)
            {
                throw new ArgumentNullException(nameof(selector));
            }

            return source.AsReadOnlyView()
                         .AsMappingView<TSource, TResult>(selector, bufferTime, scheduler, leaveOpen: false, triggers: triggers);
        }

        /// <summary>
        /// Create a <see cref="MappingView{TSource, TResult}"/> for <paramref name="source"/>
        /// </summary>
        /// <typeparam name="TSource">The type of the elements in the collection.</typeparam>
        /// <typeparam name="TResult">The type of the elements in the resulting collection.</typeparam>
        /// <param name="source">The source collection</param>
        /// <param name="selector">The function mapping an element of type <typeparamref name="TSource"/> to <typeparamref name="TResult"/>.</param>
        /// <param name="onRemove">An action to perform when an item is removed from the collection. Typically it will be x => x.Dispose()</param>
        /// <param name="scheduler">The scheduler to notify changes on.</param>
        /// <param name="triggers">Additional triggers for when mapping is updated.</param>
        /// <returns>A <see cref="MappingView{TSource, TResult}"/></returns>
        public static MappingView<TSource, TResult> AsMappingView<TSource, TResult>(
            this IObservable<Maybe<IEnumerable<TSource>?>> source,
            Func<TSource, TResult> selector,
            Action<TResult> onRemove,
            IScheduler? scheduler = null,
            params IObservable<object?>[] triggers)
            where TResult : class
        {
            if (source is null)
            {
                throw new ArgumentNullException(nameof(source));
            }

            if (selector is null)
            {
                throw new ArgumentNullException(nameof(selector));
            }

            if (onRemove is null)
            {
                 throw new ArgumentNullException(nameof(onRemove));
            }

            return source.AsReadOnlyView()
                         .AsMappingView<TSource, TResult>(selector, onRemove, TimeSpan.Zero, scheduler, leaveOpen: false, triggers: triggers);
        }

        /// <summary>
        /// Create a <see cref="MappingView{TSource, TResult}"/> for <paramref name="source"/>
        /// </summary>
        /// <typeparam name="TSource">The type of the elements in the collection.</typeparam>
        /// <typeparam name="TResult">The type of the elements in the resulting collection.</typeparam>
        /// <param name="source">The source collection</param>
        /// <param name="selector">The function mapping an element of type <typeparamref name="TSource"/> to <typeparamref name="TResult"/>.</param>
        /// <param name="onRemove">An action to perform when an item is removed from the collection. Typically it will be x => x.Dispose()</param>
        /// <param name="bufferTime">The time to buffer changes before updating and notifying.</param>
        /// <param name="scheduler">The scheduler to notify changes on.</param>
        /// <param name="triggers">Additional triggers for when mapping is updated.</param>
        /// <returns>A <see cref="MappingView{TSource, TResult}"/></returns>
        public static MappingView<TSource, TResult> AsMappingView<TSource, TResult>(
            this IObservable<Maybe<IEnumerable<TSource>?>> source,
            Func<TSource, TResult> selector,
            Action<TResult> onRemove,
            TimeSpan bufferTime,
            IScheduler? scheduler = null,
            params IObservable<object?>[] triggers)
            where TResult : class
        {
            if (source is null)
            {
                throw new ArgumentNullException(nameof(source));
            }

            if (selector is null)
            {
                throw new ArgumentNullException(nameof(selector));
            }

            if (onRemove is null)
            {
                 throw new ArgumentNullException(nameof(onRemove));
            }

            return source.AsReadOnlyView()
                         .AsMappingView<TSource, TResult>(selector, onRemove, bufferTime, scheduler, leaveOpen: false, triggers: triggers);
        }

        /// <summary>
        /// Create a <see cref="MappingView{TSource, TResult}"/> for <paramref name="source"/>
        /// </summary>
        /// <typeparam name="TSource">The type of the elements in the collection.</typeparam>
        /// <typeparam name="TResult">The type of the elements in the resulting collection.</typeparam>
        /// <param name="source">The source collection</param>
        /// <param name="selector">
        /// The function mapping an element of type <typeparamref name="TSource"/> to <typeparamref name="TResult"/>.
        /// The second parameter is the index of the element.
        /// </param>
        /// <param name="updater">
        /// The function updating an element for which the index changed.
        /// The second parameter is the index of the element.
        /// </param>
        /// <param name="scheduler">The scheduler to notify changes on.</param>
        /// <param name="triggers">Additional triggers for when mapping is updated.</param>
        /// <returns>A <see cref="MappingView{TSource, TResult}"/></returns>
        public static MappingView<TSource, TResult> AsMappingView<TSource, TResult>(
            this IObservable<Maybe<IEnumerable<TSource>?>> source,
            Func<TSource, int, TResult> selector,
            Func<TResult, int, TResult> updater,
            IScheduler? scheduler = null,
            params IObservable<object?>[] triggers)
        {
            if (source is null)
            {
                throw new ArgumentNullException(nameof(source));
            }

            if (selector is null)
            {
                throw new ArgumentNullException(nameof(selector));
            }

            if (updater is null)
            {
                 throw new ArgumentNullException(nameof(updater));
            }

            return source.AsReadOnlyView()
                         .AsMappingView<TSource, TResult>(selector, updater, TimeSpan.Zero, scheduler, leaveOpen: false, triggers: triggers);
        }

        /// <summary>
        /// Create a <see cref="MappingView{TSource, TResult}"/> for <paramref name="source"/>
        /// </summary>
        /// <typeparam name="TSource">The type of the elements in the collection.</typeparam>
        /// <typeparam name="TResult">The type of the elements in the resulting collection.</typeparam>
        /// <param name="source">The source collection</param>
        /// <param name="selector">
        /// The function mapping an element of type <typeparamref name="TSource"/> to <typeparamref name="TResult"/>.
        /// The second parameter is the index of the element.
        /// </param>
        /// <param name="updater">
        /// The function updating an element for which the index changed.
        /// The second parameter is the index of the element.
        /// </param>
        /// <param name="bufferTime">The time to buffer changes before updating and notifying.</param>
        /// <param name="scheduler">The scheduler to notify changes on.</param>
        /// <param name="triggers">Additional triggers for when mapping is updated.</param>
        /// <returns>A <see cref="MappingView{TSource, TResult}"/></returns>
        public static MappingView<TSource, TResult> AsMappingView<TSource, TResult>(
            this IObservable<Maybe<IEnumerable<TSource>?>> source,
            Func<TSource, int, TResult> selector,
            Func<TResult, int, TResult> updater,
            TimeSpan bufferTime,
            IScheduler? scheduler = null,
            params IObservable<object?>[] triggers)
        {
            if (source is null)
            {
                throw new ArgumentNullException(nameof(source));
            }

            if (selector is null)
            {
                throw new ArgumentNullException(nameof(selector));
            }

            if (updater is null)
            {
                 throw new ArgumentNullException(nameof(updater));
            }

            return source.AsReadOnlyView()
                         .AsMappingView<TSource, TResult>(selector, updater, bufferTime, scheduler, leaveOpen: false, triggers: triggers);
        }

        /// <summary>
        /// Create a <see cref="MappingView{TSource, TResult}"/> for <paramref name="source"/>
        /// </summary>
        /// <typeparam name="TSource">The type of the elements in the collection.</typeparam>
        /// <typeparam name="TResult">The type of the elements in the resulting collection.</typeparam>
        /// <param name="source">The source collection</param>
        /// <param name="selector">
        /// The function mapping an element of type <typeparamref name="TSource"/> to <typeparamref name="TResult"/>.
        /// The second parameter is the index of the element.
        /// </param>
        /// <param name="updater">
        /// The function updating an element for which the index changed.
        /// The second parameter is the index of the element.
        /// </param>
        /// <param name="onRemove">An action to perform when an item is removed from the collection. Typically it will be x => x.Dispose()</param>
        /// <param name="scheduler">The scheduler to notify changes on.</param>
        /// <param name="triggers">Additional triggers for when mapping is updated.</param>
        /// <returns>A <see cref="MappingView{TSource, TResult}"/></returns>
        public static MappingView<TSource, TResult> AsMappingView<TSource, TResult>(
            this IObservable<Maybe<IEnumerable<TSource>?>> source,
            Func<TSource, int, TResult> selector,
            Func<TResult, int, TResult> updater,
            Action<TResult> onRemove,
            IScheduler? scheduler = null,
            params IObservable<object?>[] triggers)
            where TResult : class
        {
            if (source is null)
            {
                throw new ArgumentNullException(nameof(source));
            }

            if (selector is null)
            {
                throw new ArgumentNullException(nameof(selector));
            }

            if (updater is null)
            {
                 throw new ArgumentNullException(nameof(updater));
            }

            return source.AsReadOnlyView()
                         .AsMappingView<TSource, TResult>(selector, updater, onRemove, TimeSpan.Zero, scheduler, leaveOpen: false, triggers: triggers);
        }

        /// <summary>
        /// Create a <see cref="MappingView{TSource, TResult}"/> for <paramref name="source"/>
        /// </summary>
        /// <typeparam name="TSource">The type of the elements in the collection.</typeparam>
        /// <typeparam name="TResult">The type of the elements in the resulting collection.</typeparam>
        /// <param name="source">The source collection</param>
        /// <param name="selector">
        /// The function mapping an element of type <typeparamref name="TSource"/> to <typeparamref name="TResult"/>.
        /// The second parameter is the index of the element.
        /// </param>
        /// <param name="updater">
        /// The function updating an element for which the index changed.
        /// The second parameter is the index of the element.
        /// </param>
        /// <param name="onRemove">An action to perform when an item is removed from the collection. Typically it will be x => x.Dispose()</param>
        /// <param name="bufferTime">The time to buffer changes before updating and notifying.</param>
        /// <param name="scheduler">The scheduler to notify changes on.</param>
        /// <param name="triggers">Additional triggers for when mapping is updated.</param>
        /// <returns>A <see cref="MappingView{TSource, TResult}"/></returns>
        public static MappingView<TSource, TResult> AsMappingView<TSource, TResult>(
            this IObservable<Maybe<IEnumerable<TSource>?>> source,
            Func<TSource, int, TResult> selector,
            Func<TResult, int, TResult> updater,
            Action<TResult> onRemove,
            TimeSpan bufferTime,
            IScheduler? scheduler = null,
            params IObservable<object?>[] triggers)
            where TResult : class
        {
            if (source is null)
            {
                throw new ArgumentNullException(nameof(source));
            }

            if (selector is null)
            {
                throw new ArgumentNullException(nameof(selector));
            }

            if (updater is null)
            {
                 throw new ArgumentNullException(nameof(updater));
            }

            return source.AsReadOnlyView()
                         .AsMappingView<TSource, TResult>(selector, updater, onRemove, bufferTime, scheduler, leaveOpen: false, triggers: triggers);
        }
    }
}