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
        // Analyzer ID's
        public const string WUX_ID = "WUX_Update";

        // Localized analyzer descriptions
        // See https://github.com/dotnet/roslyn/blob/master/docs/analyzers/Localizing%20Analyzers.md for more on localization
        private static readonly LocalizableString WUX_Using_Title = new LocalizableResourceString(nameof(Resources.WUX_Using_Title), Resources.ResourceManager, typeof(Resources));
        private static readonly LocalizableString WUX_Using_MessageFormat = new LocalizableResourceString(nameof(Resources.WUX_Using_MessageFormat), Resources.ResourceManager, typeof(Resources));
        private static readonly LocalizableString WUX_Using_Description = new LocalizableResourceString(nameof(Resources.WUX_Using_Description), Resources.ResourceManager, typeof(Resources));
        private const string Category = "Usage";

        // Creates a rule for WUX using Diagnostic
        // Creates a rule for WUX VAR Diagnostic TODO: update strings for this message?
        private static readonly DiagnosticDescriptor WUX_Rule = new DiagnosticDescriptor(WUX_ID, WUX_Using_Title, WUX_Using_MessageFormat, Category, DiagnosticSeverity.Error, isEnabledByDefault: true, description: WUX_Using_Description);

        //Returns a set of descriptors (Rules) that this analyzer is capable of reproducing
        public override ImmutableArray<DiagnosticDescriptor> SupportedDiagnostics { get { return ImmutableArray.Create(WUX_Rule); } }

        // Overide to implement DiagnosticAnalyzer Class
        public override void Initialize(AnalysisContext context)
        {
            //initialize context to analyze all the nodes
            context.ConfigureGeneratedCodeAnalysis(GeneratedCodeAnalysisFlags.Analyze | GeneratedCodeAnalysisFlags.None);
            context.EnableConcurrentExecution();
            // Call analyzer on all Identifier Name Nodes to see if need to implement diagnostic at that location
            context.RegisterSyntaxNodeAction(AnalyzeNode, SyntaxKind.IdentifierName);
        }

        // Decides if node needs a diagnostic thrown
        private void AnalyzeNode(SyntaxNodeAnalysisContext context)
        {
            //try casting to VariableDeclaration, should always succede because of filter in initialize
            var node = (IdentifierNameSyntax)context.Node;
            //Check if Identifier is Windows and Parent is Windows.UI
            // TODO: Does this need to be a more explicit check?
            if (node.Identifier.ToString().Equals("Windows") && node.Parent.ToString().Equals("Windows.UI"))
            {
                // Create Diagnostic to report 
                context.ReportDiagnostic(Diagnostic.Create(WUX_Rule, context.Node.GetLocation()));
            }
        }
    }
}
