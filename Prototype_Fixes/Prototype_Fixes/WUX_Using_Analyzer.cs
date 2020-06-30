using System;
using System.Collections.Generic;

using System.Collections.Immutable;
using System.Linq;
// Analyzer Namespaces
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.Diagnostics;

namespace Prototype_Fixes
{
    [DiagnosticAnalyzer(LanguageNames.CSharp)]
    public class WUX_Using_Analyzer : DiagnosticAnalyzer
    {
        public const string WUX_Using_ID = "WUX_Using";
        public const string WUX_Var_ID = "WUX_Var";

        // Localized analyzer descriptions
        // See https://github.com/dotnet/roslyn/blob/master/docs/analyzers/Localizing%20Analyzers.md for more on localization
        private static readonly LocalizableString WUX_Using_Title = new LocalizableResourceString(nameof(Resources.WUX_Using_Title), Resources.ResourceManager, typeof(Resources));
        private static readonly LocalizableString WUX_Using_MessageFormat = new LocalizableResourceString(nameof(Resources.WUX_Using_MessageFormat), Resources.ResourceManager, typeof(Resources));
        private static readonly LocalizableString WUX_Using_Description = new LocalizableResourceString(nameof(Resources.WUX_Using_Description), Resources.ResourceManager, typeof(Resources));
        private const string Category = "Usage";

        // Creates a rule for WUX using Diagnostic
        private static readonly DiagnosticDescriptor WUX_Using_Rule = new DiagnosticDescriptor(WUX_Using_ID, WUX_Using_Title, WUX_Using_MessageFormat, Category, DiagnosticSeverity.Warning, isEnabledByDefault: true, description: WUX_Using_Description);
        // Creates a rule for WUX VAR Diagnostic TODO: update strings for this message
        private static readonly DiagnosticDescriptor WUX_Var_Rule = new DiagnosticDescriptor(WUX_Var_ID, WUX_Using_Title, WUX_Using_MessageFormat, Category, DiagnosticSeverity.Error, isEnabledByDefault: true, description: WUX_Using_Description);

        //Returns a set of descriptors (Rule) that this analyzer is capable of reproducing
        public override ImmutableArray<DiagnosticDescriptor> SupportedDiagnostics { get { return ImmutableArray.Create(WUX_Using_Rule, WUX_Var_Rule); } }

        // Overide to implement DiagnosticAnalyzer Class
        public override void Initialize(AnalysisContext context)
        {
            //initialize context to analyze all the nodes
            context.ConfigureGeneratedCodeAnalysis(GeneratedCodeAnalysisFlags.Analyze | GeneratedCodeAnalysisFlags.None);
            context.EnableConcurrentExecution();
            context.RegisterSyntaxNodeAction(AnalyzeUsingNode, SyntaxKind.UsingDirective);
            context.RegisterSyntaxNodeAction(AnalyzeVarNode, SyntaxKind.VariableDeclaration);
        }

        // Decides if node from Using Directive needs a warning or not.
        private void AnalyzeUsingNode(SyntaxNodeAnalysisContext context)
        {
            //try casting to UsingSyntax, should always succede because of filter in initialize
            var node = (UsingDirectiveSyntax)context.Node;
            //Skip usings that are not in windows namespace
            if (!node.Name.ToString().StartsWith("Windows.UI"))
            {
                return;
            }
            context.ReportDiagnostic(Diagnostic.Create(WUX_Using_Rule, context.Node.GetLocation()));
        }

        // Decides if node from Variable Declaration needs a warning or not.
        private void AnalyzeVarNode(SyntaxNodeAnalysisContext context)
        {
            //TODO: OVerhaul this!
            //try casting to UsingSyntax, should always succede because of filter in initialize
            var node = (UsingDirectiveSyntax)context.Node;
            //Skip usings that are not in windows namespace
            if (!node.Name.ToString().StartsWith("Windows.UI"))
            {
                return;
            }
            context.ReportDiagnostic(Diagnostic.Create(WUX_Using_Rule, context.Node.GetLocation()));
        }
    }
}
