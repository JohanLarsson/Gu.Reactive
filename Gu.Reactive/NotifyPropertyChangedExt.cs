﻿#pragma warning disable RS0026 // Do not add multiple public overloads with optional parameters
#pragma warning disable RS0027 // Public API with optional parameter(s) should have the most parameters amongst its public overloads
namespace Gu.Reactive
{
    using System;
    using System.ComponentModel;
    using System.Linq.Expressions;
    using System.Reactive;
    using System.Reactive.Disposables;
    using System.Reactive.Linq;
    using System.Reflection;

    using Gu.Reactive.Internals;

    /// <summary>
    /// Extension methods for subscribing to property changes.
    /// </summary>
    public static partial class NotifyPropertyChangedExt
    {
        /// <summary>
        /// Extension method for listening to property changes.
        /// Handles nested x => x.Level1.Level2.Level3
        /// Unsubscribes &amp; subscribes when each level changes.
        /// Handles nulls.
        /// </summary>
        /// <typeparam name="TNotifier">The source type.</typeparam>
        /// <typeparam name="TProperty">The property type.</typeparam>
        /// <param name="source">The source instance to track changes for. </param>
        /// <param name="property">
        /// An expression specifying the property path to track.
        /// Example x => x.Foo.Bar.Meh.
        /// </param>
        /// <param name="signalInitial">
        /// If true OnNext is called immediately on subscribe.
        /// </param>
        /// <returns>An <see cref="IObservable{T}"/>.</returns>
        public static IObservable<EventPattern<PropertyChangedEventArgs>> ObservePropertyChanged<TNotifier, TProperty>(
            this TNotifier source,
            Expression<Func<TNotifier, TProperty>> property,
            bool signalInitial = true)
            where TNotifier : class, INotifyPropertyChanged
        {
            if (source is null)
            {
                throw new ArgumentNullException(nameof(source));
            }

            if (property is null)
            {
                throw new ArgumentNullException(nameof(property));
            }

            var notifyingPath = NotifyingPath.GetOrCreate(property);
            return ObservePropertyChangedCore(
                source,
                notifyingPath,
                (sender, args) => new EventPattern<PropertyChangedEventArgs>(sender, args),
                signalInitial);
        }

        /// <summary>
        /// Prefer other overloads with x => x.PropertyName for refactor friendliness.
        /// This is faster though.
        /// </summary>
        /// <param name="source"> The source instance to track changes for. </param>
        /// <param name="propertyName"> The name of the property to track. Note that nested properties are not allowed. </param>
        /// <param name="signalInitial"> If true OnNext is called immediately on subscribe. </param>
        /// <returns>An <see cref="IObservable{T}"/>.</returns>
        public static IObservable<EventPattern<PropertyChangedEventArgs>> ObservePropertyChanged(
            this INotifyPropertyChanged source,
            string propertyName,
            bool signalInitial = true)
        {
            if (source is null)
            {
                throw new ArgumentNullException(nameof(source));
            }

            if (propertyName is null)
            {
                throw new ArgumentNullException(nameof(propertyName));
            }

            if (source.GetType().GetProperty(propertyName, BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic) is null)
            {
                throw new ArgumentException($"The type {source.GetType()} does not have a property named {propertyName}", propertyName);
            }

            return ObservePropertyChangedCore(
                source,
                propertyName,
                (sender, args) => new EventPattern<PropertyChangedEventArgs>(sender, args),
                signalInitial);
        }

        /// <summary>
        /// Observe property changes for <paramref name="source"/>.
        /// </summary>
        /// <param name="source">The source.</param>
        /// <returns>The <see cref="IObservable{T}"/>.</returns>
        public static IObservable<EventPattern<PropertyChangedEventArgs>> ObservePropertyChanged(this INotifyPropertyChanged source)
        {
            if (source is null)
            {
                throw new ArgumentNullException(nameof(source));
            }

            return Observable.FromEventPattern<PropertyChangedEventHandler, PropertyChangedEventArgs>(
                x => source.PropertyChanged += x,
                x => source.PropertyChanged -= x);
        }

        /// <summary>
        /// Extension method for listening to property changes.
        /// Handles nested x => x.Level1.Level2.Level3
        /// Unsubscribes &amp; subscribes when each level changes.
        /// Handles nulls.
        /// </summary>
        /// <typeparam name="TNotifier">The source type.</typeparam>
        /// <typeparam name="TProperty">The property type.</typeparam>
        /// <param name="source">The source instance to track changes for. </param>
        /// <param name="property">
        /// An expression specifying the property path to track.
        /// Example x => x.Foo.Bar.Meh.
        ///  </param>
        /// <param name="signalInitial">
        /// If true OnNext is called immediately on subscribe.
        /// </param>
        /// <returns>An <see cref="IObservable{T}"/>.</returns>
        public static IObservable<PropertyChangedEventArgs> ObservePropertyChangedSlim<TNotifier, TProperty>(
            this TNotifier source,
            Expression<Func<TNotifier, TProperty>> property,
            bool signalInitial = true)
            where TNotifier : class, INotifyPropertyChanged
        {
            if (source is null)
            {
                throw new ArgumentNullException(nameof(source));
            }

            if (property is null)
            {
                throw new ArgumentNullException(nameof(property));
            }

            var notifyingPath = NotifyingPath.GetOrCreate(property);
            return ObservePropertyChangedCore(source, notifyingPath, (_, e) => e, signalInitial);
        }

        /// <summary>
        /// This is a faster version of ObservePropertyChanged. It returns only the <see cref="PropertyChangedEventArgs"/> from source and not the EventPattern.
        /// </summary>
        /// <param name="source"> The source instance to track changes for. </param>
        /// <param name="propertyName"> The name of the property to track. Note that nested properties are not allowed. </param>
        /// <param name="signalInitial"> If true OnNext is called immediately on subscribe. </param>
        /// <returns>The <see cref="IObservable{T}"/>.</returns>
        public static IObservable<PropertyChangedEventArgs> ObservePropertyChangedSlim(this INotifyPropertyChanged source, string propertyName, bool signalInitial = true)
        {
            if (source is null)
            {
                throw new ArgumentNullException(nameof(source));
            }

            if (string.IsNullOrEmpty(propertyName))
            {
                throw new ArgumentException("message", nameof(propertyName));
            }

            if (source.GetType().GetProperty(propertyName, BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic) is null)
            {
                throw new ArgumentException($"The type {source.GetType()} does not have a property named {propertyName}", propertyName);
            }

            return ObservePropertyChangedCore(
                source,
                propertyName,
                (_, args) => args,
                signalInitial);
        }

        /// <summary>
        /// Observe property changes for <paramref name="source"/>.
        /// </summary>
        /// <param name="source">The source.</param>
        /// <returns>The <see cref="IObservable{T}"/>.</returns>
        public static IObservable<PropertyChangedEventArgs> ObservePropertyChangedSlim(this INotifyPropertyChanged source)
        {
            if (source is null)
            {
                throw new ArgumentNullException(nameof(source));
            }

            var observable = Observable.Create<PropertyChangedEventArgs>(
                o =>
                {
                    void Handler(object _, PropertyChangedEventArgs e) => o.OnNext(e);
                    source.PropertyChanged += Handler;
                    return Disposable.Create(() => source.PropertyChanged -= Handler);
                });
            return observable;
        }

        /// <summary>
        /// Observe property changes for the path <paramref name="source"/>.
        /// This signals when any of the items in tha path signals.
        /// </summary>
        /// <typeparam name="TNotifier">The source type.</typeparam>
        /// <typeparam name="TProperty">The property type.</typeparam>
        /// <param name="source">The source instance.</param>
        /// <param name="property">An expression specifying the property path.</param>
        /// <param name="signalInitial"> If true OnNext is called immediately on subscribe. </param>
        /// <returns>The <see cref="IObservable{T}"/>.</returns>
        public static IObservable<PropertyChangedEventArgs> ObserveFullPropertyPathSlim<TNotifier, TProperty>(this TNotifier source, Expression<Func<TNotifier, TProperty>> property, bool signalInitial = true)
            where TNotifier : class, INotifyPropertyChanged
        {
            if (source is null)
            {
                throw new ArgumentNullException(nameof(source));
            }

            if (property is null)
            {
                throw new ArgumentNullException(nameof(property));
            }

            var notifyingPath = NotifyingPath.GetOrCreate(property);
            if (notifyingPath.Count < 2)
            {
                var message = "Expected path to have more than one item.\r\n" +
                              $"The path was {property}\r\n" +
                              "Did you mean to call ObservePropertyChangedSlim?";
                throw new ArgumentException(message, nameof(property));
            }

            return ObserveFullPropertyPathCore(source, notifyingPath, (_, e) => e, signalInitial);
        }

        /// <summary>
        /// Extension method for listening to property changes.
        /// Handles nested x => x.Level1.Level2.Level3
        /// Unsubscribes &amp; subscribes when each level changes.
        /// Handles nulls.
        /// </summary>
        /// <typeparam name="TNotifier">The source type.</typeparam>
        /// <typeparam name="TProperty">The property type.</typeparam>
        /// <param name="source">The source instance to track changes for. </param>
        /// <param name="property">
        /// An expression specifying the property path to track.
        /// Example x => x.Foo.Bar.Meh.
        ///  </param>
        /// <param name="signalInitial">
        /// If true OnNext is called immediately on subscribe.
        /// </param>
        /// <returns>The <see cref="IObservable{T}"/>.</returns>
        public static IObservable<Maybe<TProperty>> ObserveValue<TNotifier, TProperty>(
            this TNotifier source,
            Expression<Func<TNotifier, TProperty>> property,
            bool signalInitial = true)
            where TNotifier : class, INotifyPropertyChanged
        {
            if (source is null)
            {
                throw new ArgumentNullException(nameof(source));
            }

            if (property is null)
            {
                throw new ArgumentNullException(nameof(property));
            }

            var notifyingPath = NotifyingPath.GetOrCreate(property);
            return source.ObserveValueCore(
                             notifyingPath,
                             (_, _, value) => value,
                             signalInitial)
                         .DistinctUntilChanged();
        }

        internal static bool IsMatch(this PropertyChangedEventArgs e, PropertyInfo property)
        {
            return IsMatch(e, property.Name);
        }

        internal static bool IsMatch(this PropertyChangedEventArgs e, string propertyName)
        {
            return string.IsNullOrEmpty(e.PropertyName) || e.PropertyName == propertyName;
        }

        /// <summary>
        /// Extension method for listening to property changes.
        /// Handles nested x => x.Level1.Level2.Level3
        /// Unsubscribes &amp; subscribes when each level changes.
        /// Handles nulls.
        /// </summary>
        /// <param name="source">The source instance to track changes for. </param>
        /// <param name="propertyPath">
        /// A cached property path. Creating the property path from Expression&lt;Func&lt;TNotifier, TProperty&gt;&gt; is a bit expensive so caching can make sense.
        ///  </param>
        /// <param name="signalInitial">
        /// If true OnNext is called immediately on subscribe.
        /// </param>
        internal static IObservable<EventPattern<PropertyChangedEventArgs>> ObservePropertyChanged<TNotifier, TProperty>(
            this TNotifier source,
            NotifyingPath<TNotifier, TProperty> propertyPath,
            bool signalInitial = true)
            where TNotifier : class, INotifyPropertyChanged
        {
            return ObservePropertyChangedCore(
                source,
                propertyPath,
                (sender, args) => new EventPattern<PropertyChangedEventArgs>(sender, args),
                signalInitial);
        }

        private static IObservable<T> ObserveValueCore<TNotifier, TProperty, T>(
            this TNotifier source,
            NotifyingPath<TNotifier, TProperty> notifyingPath,
            Func<object?, PropertyChangedEventArgs, Maybe<TProperty>, T> create,
            bool signalInitial = true)
            where TNotifier : class, INotifyPropertyChanged
        {
            if (signalInitial)
            {
                return Observable.Defer(
                                     () =>
                                         {
                                             var sourceAndValue = notifyingPath.SourceAndValue(source);
                                             return Observable.Return(
                                                 create(
                                                     sourceAndValue.Source,
                                                     CachedEventArgs.GetOrCreatePropertyChangedEventArgs(string.Empty),
                                                     sourceAndValue.Value));
                                         })
                                 .Concat(source.ObserveValueCore(notifyingPath, create, signalInitial: false));
            }

            if (notifyingPath.Count > 1)
            {
                return Observable.Create<T>(
                    o =>
                    {
                        var tracker = notifyingPath.CreateTracker(source);
                        tracker.TrackedPropertyChanged += Handler;
                        return new CompositeDisposable(2)
                        {
                            tracker,
                            Disposable.Create(() => tracker.TrackedPropertyChanged -= Handler),
                        };

                        void Handler(IPropertyTracker _, object sender, PropertyChangedEventArgs args, SourceAndValue<INotifyPropertyChanged?, TProperty> sourceAndValue)
                        {
                            o.OnNext(create(sender, args, sourceAndValue.Value));
                        }
                    });
            }

            return Observable.Create<T>(
                                 o =>
                                 {
                                     void Handler(object sender, PropertyChangedEventArgs e)
                                     {
                                         if (e.IsMatch(notifyingPath.Last.Property))
                                         {
                                             var value = notifyingPath.Last.GetMaybe(sender);
                                             o.OnNext(create(sender, e, value));
                                         }
                                     }

                                     source.PropertyChanged += Handler;
                                     return Disposable.Create(() => source.PropertyChanged -= Handler);
                                 });
        }

        private static IObservable<T> ObservePropertyChangedCore<TNotifier, TProperty, T>(
            this TNotifier source,
            NotifyingPath<TNotifier, TProperty> notifyingPath,
            Func<object?, PropertyChangedEventArgs, T> create,
            bool signalInitial = true)
            where TNotifier : class, INotifyPropertyChanged
        {
            if (signalInitial)
            {
                return Observable.Return(
                                     create(
                                         notifyingPath.SourceAndValue(source).Source,
                                         CachedEventArgs.StringEmpty))
                                 .Concat(source.ObservePropertyChangedCore(notifyingPath, create, signalInitial: false));
            }

            if (notifyingPath.Count > 1)
            {
                return Observable.Create<T>(o =>
                {
                    var tracker = notifyingPath.CreateTracker(source);
                    void Handler(IPropertyTracker _, object sender, PropertyChangedEventArgs e, SourceAndValue<INotifyPropertyChanged?, TProperty> __) => o.OnNext(create(sender, e));
                    tracker.TrackedPropertyChanged += Handler;
                    return new CompositeDisposable(2)
                    {
                        tracker,
                        Disposable.Create(() => tracker.TrackedPropertyChanged -= Handler),
                    };
                });
            }

            return ObservePropertyChangedCore(source, notifyingPath.Last.Property.Name, create, signalInitial: false);
        }

        private static IObservable<T> ObserveFullPropertyPathCore<TNotifier, TProperty, T>(this TNotifier source, NotifyingPath<TNotifier, TProperty> notifyingPath, Func<object?, PropertyChangedEventArgs, T> create, bool signalInitial = true)
            where TNotifier : class, INotifyPropertyChanged
        {
            if (signalInitial)
            {
                return Observable.Return(
                                     create(
                                         notifyingPath.SourceAndValue(source).Source,
                                         CachedEventArgs.GetOrCreatePropertyChangedEventArgs(string.Empty)))
                                 .Concat(source.ObserveFullPropertyPathCore(notifyingPath, create, signalInitial: false));
            }

            return Observable.Create<T>(
                o =>
                    {
                        var tracker = notifyingPath.CreateTracker(source);
                        void Handler(object sender, PropertyChangedEventArgs e) => o.OnNext(create(sender, e));
                        foreach (var propertyTracker in tracker)
                        {
                            propertyTracker.TrackedPropertyChanged += Handler;
                        }

                        return Disposable.Create(
                            () =>
                                {
                                    foreach (var propertyTracker in tracker)
                                    {
                                        propertyTracker.TrackedPropertyChanged -= Handler;
                                    }

                                    tracker.Dispose();
                                });
                    });
        }

        private static IObservable<T> ObservePropertyChangedCore<TNotifier, T>(
            this TNotifier source,
            string propertyName,
            Func<object, PropertyChangedEventArgs, T> create,
            bool signalInitial = true)
            where TNotifier : INotifyPropertyChanged
        {
            if (signalInitial)
            {
                return Observable.Return(
                                     create(
                                         source,
                                         CachedEventArgs.GetOrCreatePropertyChangedEventArgs(string.Empty)))
                                 .Concat(source.ObservePropertyChangedCore(propertyName, create, signalInitial: false));
            }

            return Observable.Create<T>(
                o =>
                    {
                        void Handler(object sender, PropertyChangedEventArgs e)
                        {
                            if (e.IsMatch(propertyName))
                            {
                                o.OnNext(create(sender, e));
                            }
                        }

                        source.PropertyChanged += Handler;
                        return Disposable.Create(() => source.PropertyChanged -= Handler);
                    });
        }
    }
}
