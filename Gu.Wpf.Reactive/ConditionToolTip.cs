﻿namespace Gu.Wpf.Reactive
{
    using System;
    using System.Windows;
    using System.Windows.Controls;
    using System.Windows.Controls.Primitives;
    using System.Windows.Data;
    using System.Windows.Input;

    using Gu.Reactive;

    /// <summary>
    /// Exposes AdornedElement and sets DataContext to the CommandProxy of the adorned element.
    /// </summary>
    public class ConditionToolTip : ToolTip
    {
#pragma warning disable SA1202 // Elements must be ordered by access

        /// <summary>Identifies the <see cref="Condition"/> dependency property.</summary>
        public static readonly DependencyProperty ConditionProperty = DependencyProperty.Register(
            nameof(Condition),
            typeof(ICondition),
            typeof(ConditionToolTip),
            new PropertyMetadata(default(ICondition)));

        /// <summary>Identifies the <see cref="InferConditionFromCommand"/> dependency property.</summary>
        public static readonly DependencyProperty InferConditionFromCommandProperty = DependencyProperty.Register(
            nameof(InferConditionFromCommand),
            typeof(bool),
            typeof(ConditionToolTip),
            new PropertyMetadata(
                defaultValue: true,
                propertyChangedCallback: (d, e) => ((ConditionToolTip)d).UpdateInferConditionFromCommand((bool)e.NewValue)));

        private static readonly DependencyPropertyKey CommandTypePropertyKey = DependencyProperty.RegisterReadOnly(
            nameof(CommandType),
            typeof(Type),
            typeof(ConditionToolTip),
            new PropertyMetadata(default(Type)));

        /// <summary>Identifies the <see cref="CommandType"/> dependency property.</summary>
        public static readonly DependencyProperty CommandTypeProperty = CommandTypePropertyKey.DependencyProperty;

        private static readonly DependencyProperty PlacementTargetProxyProperty = DependencyProperty.Register(
            "PlacementTargetProxy",
            typeof(UIElement),
            typeof(ConditionToolTip),
            new PropertyMetadata(
                default(UIElement),
                OnPlacementTargetProxyChanged));

        private static readonly DependencyProperty CommandProxyProperty = DependencyProperty.Register(
            "CommandProxy",
            typeof(ICommand),
            typeof(ConditionToolTip),
            new PropertyMetadata(default(ICommand), OnCommandProxyChanged));

#pragma warning restore SA1202 // Elements must be ordered by access

        static ConditionToolTip()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(ConditionToolTip), new FrameworkPropertyMetadata(typeof(ConditionToolTip)));
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ConditionToolTip"/> class.
        /// </summary>
        public ConditionToolTip()
        {
            this.UpdateInferConditionFromCommand(this.InferConditionFromCommand);
        }

        /// <summary>
        /// Gets or sets the command type of the PlacementTarget.
        /// </summary>
        public Type? CommandType
        {
            get => (Type?)this.GetValue(CommandTypeProperty);
            protected set => this.SetValue(CommandTypePropertyKey, value);
        }

        /// <summary>
        /// Gets or sets a value indicating whether <see cref="Condition"/> binds to the Condition of the command if any.
        /// </summary>
        public bool InferConditionFromCommand
        {
            get => (bool)this.GetValue(InferConditionFromCommandProperty);
            set => this.SetValue(InferConditionFromCommandProperty, value);
        }

        /// <summary>
        /// Gets or sets the condition if the command is a ConditionRelayCommand null otherwise.
        /// </summary>
        public ICondition? Condition
        {
            get => (ICondition?)this.GetValue(ConditionProperty);
            set => this.SetValue(ConditionProperty, value);
        }

        private static void OnPlacementTargetProxyChanged(DependencyObject o, DependencyPropertyChangedEventArgs e)
        {
            var commandToolTip = (ConditionToolTip)o;
            if (commandToolTip.PlacementTarget is ButtonBase target)
            {
                var command = target.GetValue(ButtonBase.CommandProperty) as IConditionRelayCommand;
                commandToolTip.SetCurrentValue(CommandProxyProperty, command);
            }
            else
            {
                commandToolTip.SetCurrentValue(CommandProxyProperty, null);
            }
        }

        private static void OnCommandProxyChanged(DependencyObject o, DependencyPropertyChangedEventArgs e)
        {
            var commandToolTip = (ConditionToolTip)o;
            if (e.NewValue is IConditionRelayCommand command)
            {
                commandToolTip.SetCurrentValue(ConditionProperty, command.Condition);
                commandToolTip.CommandType = command.GetType();
            }
            else
            {
                commandToolTip.SetCurrentValue(ConditionProperty, null);
                commandToolTip.CommandType = null;
            }
        }

        private void UpdateInferConditionFromCommand(bool infer)
        {
            if (infer)
            {
                _ = BindingOperations.SetBinding(
                    this,
                    PlacementTargetProxyProperty,
                    this.CreateOneWayBinding(PlacementTargetProperty));
            }
            else
            {
                BindingOperations.ClearBinding(this, PlacementTargetProxyProperty);
            }
        }
    }
}
