﻿namespace Gu.Reactive.Analyzers
{
    using System;
    using System.Collections.Immutable;
    using System.Diagnostics.CodeAnalysis;
    using System.Linq;

    using Gu.Roslyn.AnalyzerExtensions;

    using Microsoft.CodeAnalysis;
    using Microsoft.CodeAnalysis.CSharp;
    using Microsoft.CodeAnalysis.CSharp.Syntax;
    using Microsoft.CodeAnalysis.Diagnostics;

    [DiagnosticAnalyzer(LanguageNames.CSharp)]
    internal class ConstructorAnalyzer : DiagnosticAnalyzer
    {
        public override ImmutableArray<DiagnosticDescriptor> SupportedDiagnostics { get; } = ImmutableArray.Create(
            Descriptors.GUREA02ObservableAndCriteriaMustMatch,
            Descriptors.GUREA06DoNotNewCondition,
            Descriptors.GUREA08InlineSingleLine,
            Descriptors.GUREA09ObservableBeforeCriteria,
            Descriptors.GUREA10DoNotMergeInObservable,
            Descriptors.GUREA13SyncParametersAndArgs);

        public override void Initialize(AnalysisContext context)
        {
            context.ConfigureGeneratedCodeAnalysis(GeneratedCodeAnalysisFlags.None);
            context.EnableConcurrentExecution();
            context.RegisterSyntaxNodeAction(Handle, SyntaxKind.BaseConstructorInitializer, SyntaxKind.ObjectCreationExpression);
        }

        private static void Handle(SyntaxNodeAnalysisContext context)
        {
            if (!context.IsExcludedFromAnalysis() &&
                context.Node is ConstructorInitializerSyntax { ArgumentList: { } } initializer &&
                context.SemanticModel.GetSymbolSafe(initializer, context.CancellationToken) is { } baseCtor)
            {
                if (baseCtor.ContainingType == KnownSymbol.Condition)
                {
                    if (TryGetObservableAndCriteriaMismatch(initializer.ArgumentList, baseCtor, context, out var observedText, out var criteriaText, out var missingText))
                    {
                        context.ReportDiagnostic(Diagnostic.Create(Descriptors.GUREA02ObservableAndCriteriaMustMatch, initializer.GetLocation(), observedText, criteriaText, missingText));
                    }

                    if (TryGetObservableArgument(initializer.ArgumentList, baseCtor, out var observableArgument) &&
                        FindInlineInvocation(observableArgument, context) is { })
                    {
                        context.ReportDiagnostic(Diagnostic.Create(Descriptors.GUREA08InlineSingleLine, observableArgument.GetLocation()));
                    }

                    if (TryGetCriteriaArgument(initializer.ArgumentList, baseCtor, out var criteriaArgument) &&
                        FindInlineInvocation(criteriaArgument, context) is { } inline)
                    {
                        context.ReportDiagnostic(Diagnostic.Create(Descriptors.GUREA08InlineSingleLine, inline.GetLocation()));
                    }

                    if (IsObservableBeforeCriteria(baseCtor))
                    {
                        context.ReportDiagnostic(Diagnostic.Create(Descriptors.GUREA09ObservableBeforeCriteria, initializer.ArgumentList.GetLocation()));
                    }

                    if (MergesObservable(initializer.ArgumentList, baseCtor, context, out var location))
                    {
                        context.ReportDiagnostic(Diagnostic.Create(Descriptors.GUREA10DoNotMergeInObservable, location.GetLocation()));
                    }
                }
                else if (baseCtor.ContainingType.IsEither(KnownSymbol.AndCondition, KnownSymbol.OrCondition) &&
                         HasMatchingArgumentAndParameterPositions(initializer, context) == false)
                {
                    context.ReportDiagnostic(Diagnostic.Create(Descriptors.GUREA13SyncParametersAndArgs, initializer.ArgumentList.GetLocation()));
                }
            }
            else if (context.Node is ObjectCreationExpressionSyntax objectCreation &&
                     context.SemanticModel.GetSymbolSafe(objectCreation, context.CancellationToken) is { } ctor)
            {
                if (ctor.ContainingType == KnownSymbol.Condition &&
                    objectCreation.ArgumentList is { })
                {
                    if (TryGetObservableAndCriteriaMismatch(objectCreation.ArgumentList, ctor, context, out var observedText, out var criteriaText, out var missingText))
                    {
                        context.ReportDiagnostic(Diagnostic.Create(Descriptors.GUREA02ObservableAndCriteriaMustMatch, objectCreation.GetLocation(), observedText, criteriaText, missingText));
                    }

                    if (IsObservableBeforeCriteria(ctor))
                    {
                        context.ReportDiagnostic(Diagnostic.Create(Descriptors.GUREA09ObservableBeforeCriteria, objectCreation.ArgumentList.GetLocation()));
                    }
                }

                if (ctor.ContainingType.IsAssignableTo(KnownSymbol.Condition, context.Compilation))
                {
                    context.ReportDiagnostic(Diagnostic.Create(Descriptors.GUREA06DoNotNewCondition, objectCreation.GetLocation()));
                }
            }
        }

        private static bool IsObservableBeforeCriteria(IMethodSymbol ctor)
        {
            return ctor.Parameters.TryElementAt(0, out var parameter0) &&
                   parameter0.Type == KnownSymbol.FuncOfT &&
                   ctor.Parameters.TryElementAt(1, out var parameter1) &&
                   parameter1.Type == KnownSymbol.IObservableOfT;
        }

        private static InvocationExpressionSyntax? FindInlineInvocation(ArgumentSyntax argument, SyntaxNodeAnalysisContext context)
        {
            return argument.Expression switch
            {
                InvocationExpressionSyntax argumentInvocation
                when CanBeInlined(argumentInvocation)
                => argumentInvocation,
                LambdaExpressionSyntax { Body: InvocationExpressionSyntax lambdaInvocation }
                when CanBeInlined(lambdaInvocation)
                => lambdaInvocation,
                _ => null,
            };

            bool CanBeInlined(InvocationExpressionSyntax invocation)
            {
                if (context.SemanticModel.GetSymbolSafe(invocation, context.CancellationToken) is { IsStatic: true } method &&
                    method.TrySingleDeclaration<MethodDeclarationSyntax>(context.CancellationToken, out var declaration))
                {
                    return declaration switch
                    {
                        { ExpressionBody: { } } => true,
                        { Body: { Statements: { Count: 1 } statements } }
                        when statements[0] is ReturnStatementSyntax _
                        => true,
                        _ => false,
                    };
                }

                return false;
            }
        }

        private static bool TryGetObservableAndCriteriaMismatch(ArgumentListSyntax argumentList, IMethodSymbol ctor, SyntaxNodeAnalysisContext context, [NotNullWhen(true)] out string? observedText, [NotNullWhen(true)] out string? criteriaText, [NotNullWhen(true)] out string? missingText)
        {
            if (TryGetObservableArgument(argumentList, ctor, out var observableArg) &&
                TryGetCriteriaArgument(argumentList, ctor, out var criteriaArg))
            {
                using var observableIdentifiers = IdentifierNameExecutionWalker.Create(observableArg, SearchScope.Recursive, context.SemanticModel, context.CancellationToken);
                using var criteriaIdentifiers = IdentifierNameExecutionWalker.Create(criteriaArg, SearchScope.Recursive, context.SemanticModel, context.CancellationToken);
                var observesInterval = false;
                using var observed = PooledSet<IPropertySymbol>.Borrow();
                foreach (var name in observableIdentifiers.IdentifierNames)
                {
                    if (context.SemanticModel.TryGetSymbol(name, context.CancellationToken, out var symbol))
                    {
                        if (symbol is IPropertySymbol property)
                        {
                            observed.Add(property);
                        }

                        if (symbol is IMethodSymbol method &&
                            method == KnownSymbol.Observable.Interval)
                        {
                            observesInterval = true;
                        }
                    }
                }

                using var usedInCriteria = PooledSet<IPropertySymbol>.Borrow();
                foreach (var name in criteriaIdentifiers.IdentifierNames)
                {
                    if (context.SemanticModel.TryGetSymbol(name, context.CancellationToken, out IPropertySymbol? property) &&
                        !property.ContainingType.IsValueType &&
                        !property.IsGetOnly() &&
                        !property.IsPrivateSetAssignedInCtorOnly(context.SemanticModel, context.CancellationToken))
                    {
                        usedInCriteria.Add(property);
                    }
                }

                using var missing = PooledSet<IPropertySymbol>.Borrow();
                missing.UnionWith(usedInCriteria);
                missing.ExceptWith(observed);
                if (observesInterval)
                {
                    missing.ExceptWith(missing.Where(x => x.Name == "Now").ToArray());
                }

                if (missing.Count != 0)
                {
                    observedText = string.Join(Environment.NewLine, observed.Select(p => $"  {p}"));
                    criteriaText = string.Join(Environment.NewLine, usedInCriteria.Select(p => $"  {p}"));
                    missingText = string.Join(Environment.NewLine, missing.Select(p => $"  {p}"));
                    return true;
                }
            }

            observedText = null;
            criteriaText = null;
            missingText = null;
            return false;
        }

        private static bool MergesObservable(ArgumentListSyntax argumentList, IMethodSymbol ctor, SyntaxNodeAnalysisContext context, [NotNullWhen(true)] out SyntaxNode? location)
        {
            if (TryGetObservableArgument(argumentList, ctor, out var argument))
            {
                using var pooled = InvocationExecutionWalker.Borrow(argument, SearchScope.Recursive, context.SemanticModel, context.CancellationToken);
                foreach (var invocation in pooled.Invocations)
                {
                    if (invocation.IsSymbol(KnownSymbol.Observable.Merge, context.SemanticModel, context.CancellationToken))
                    {
                        location = argument;
                        return true;
                    }
                }
            }

            location = null;
            return false;
        }

        private static bool? HasMatchingArgumentAndParameterPositions(ConstructorInitializerSyntax initializer, SyntaxNodeAnalysisContext context)
        {
            if (initializer?.ArgumentList is null)
            {
                return null;
            }

            if (initializer?.Parent is null)
            {
                return null;
            }

            if (context.SemanticModel.GetDeclaredSymbolSafe(initializer.Parent, context.CancellationToken) is IMethodSymbol ctor)
            {
                if (ctor.Parameters.Length != initializer.ArgumentList.Arguments.Count)
                {
                    return null;
                }

                for (var i = 0; i < initializer.ArgumentList.Arguments.Count; i++)
                {
                    var argument = initializer.ArgumentList.Arguments[i];
                    if (argument.Expression is IdentifierNameSyntax argName &&
                        argName.Identifier.ValueText != ctor.Parameters[i].Name)
                    {
                        return false;
                    }
                }
            }

            return true;
        }

        private static bool TryGetObservableArgument(ArgumentListSyntax argumentList, IMethodSymbol ctor, [NotNullWhen(true)] out ArgumentSyntax? argument)
        {
            if (ctor.Parameters[0].Type == KnownSymbol.IObservableOfT &&
                ctor.Parameters[1].Type == KnownSymbol.FuncOfT)
            {
                argument = argumentList.Arguments[0];
                return true;
            }

            if (ctor.Parameters[0].Type == KnownSymbol.FuncOfT &&
                ctor.Parameters[1].Type == KnownSymbol.IObservableOfT)
            {
                argument = argumentList.Arguments[1];
                return true;
            }

            argument = null;
            return false;
        }

        private static bool TryGetCriteriaArgument(ArgumentListSyntax argumentList, IMethodSymbol ctor, [NotNullWhen(true)] out ArgumentSyntax? argument)
        {
            if (ctor.Parameters[0].Type == KnownSymbol.IObservableOfT &&
                ctor.Parameters[1].Type == KnownSymbol.FuncOfT)
            {
                argument = argumentList.Arguments[1];
                return true;
            }

            if (ctor.Parameters[0].Type == KnownSymbol.FuncOfT &&
                ctor.Parameters[1].Type == KnownSymbol.IObservableOfT)
            {
                argument = argumentList.Arguments[0];
                return true;
            }

            argument = null;
            return false;
        }
    }
}
