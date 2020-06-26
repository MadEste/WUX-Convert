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
        public const string DiagnosticId = "WUX_Using";

        // Localized analyzer descriptions
        // See https://github.com/dotnet/roslyn/blob/master/docs/analyzers/Localizing%20Analyzers.md for more on localization
        private static readonly LocalizableString Title = new LocalizableResourceString(nameof(Resources.WUX_Using_Title), Resources.ResourceManager, typeof(Resources));
        private static readonly LocalizableString MessageFormat = new LocalizableResourceString(nameof(Resources.WUX_Using_MessageFormat), Resources.ResourceManager, typeof(Resources));
        private static readonly LocalizableString Description = new LocalizableResourceString(nameof(Resources.WUX_Using_Description), Resources.ResourceManager, typeof(Resources));
        private const string Category = "Usage";

        // Gives out this analyzer discription to VS
        private static DiagnosticDescriptor Rule = new DiagnosticDescriptor(DiagnosticId, Title, MessageFormat, Category, DiagnosticSeverity.Warning, isEnabledByDefault: true, description: Description);

        //Returns a set of descriptors (Rule) that this analyzer is capable of reproducing
        public override ImmutableArray<DiagnosticDescriptor> SupportedDiagnostics { get { return ImmutableArray.Create(Rule); } }

        // Overide to implement DiagnosticAnalyzer Class
        public override void Initialize(AnalysisContext context)
        {
            //initialize context to analyze all the nodes
            context.ConfigureGeneratedCodeAnalysis(GeneratedCodeAnalysisFlags.Analyze | GeneratedCodeAnalysisFlags.None);
            context.EnableConcurrentExecution();
            //TODO: Implement AnalyzeNode() changed to filter usingdirective leaves
            context.RegisterSyntaxNodeAction(AnalyzeNode, SyntaxKind.UsingDirective);
        }

        // Decides if node from Using Directive needs a warning or not.
        private void AnalyzeNode(SyntaxNodeAnalysisContext context)
        {
            //try casting to UsingSyntax
            var node = (UsingDirectiveSyntax)context.Node;
            //Skip usings that are not in windows namespace
            if (!node.Name.ToString().StartsWith("Windows"))
            {
                return;
            }
            context.ReportDiagnostic(Diagnostic.Create(Rule, context.Node.GetLocation()));
        }
    }
}
