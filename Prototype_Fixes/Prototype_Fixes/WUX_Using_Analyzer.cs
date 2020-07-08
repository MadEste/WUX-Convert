using System;
using System.Collections.Generic;

using System.Collections.Immutable;
using System.Linq;
// Analyzer Namespaces
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.Diagnostics;

/**
 * Notes... Need to use only one rule and code fix so that user experience is that all conversion happens in one click.
 * Need to extend rule beyond namespaces to also fix other conversion conflicts : 
 * 1. event dispatch location changes: e.UWPLaunchActivatedEventArgs.exampleDispatch...
 * 2. Microsoft.UI.Xaml.LaunchActivatedEventArgs ...
 *      using Windows.ApplicationModel.Activation;
 *      using Microsoft.UI.Xaml;
 *  Both have confusing ambiguous LaunchActivatedEventArgs types shared
 * 
 */

namespace Prototype_Fixes
{
    [DiagnosticAnalyzer(LanguageNames.CSharp)]
    public class WUX_Using_Analyzer : DiagnosticAnalyzer
    {
        // Analyzer ID's
        public const string WUX_ID = "WUX_Update_1_2_1";

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
            context.RegisterSyntaxNodeAction(AnalyzeNode, SyntaxKind.QualifiedName);
        }

        /// <see cref="Windows.UI.Xaml.FrameworkElement.ArrangeOverride(Windows.Foundation.Size)" />  
        // Decides if node needs a diagnostic thrown
        private void AnalyzeNode(SyntaxNodeAnalysisContext context)
        {

           
            //try casting to Qualified Name, should always succede because of filter in initialize
            var node = (QualifiedNameSyntax)context.Node;
            //filter out qualified names that are not Windows, or are part of documentation
            if (!node.Left.ToString().Equals("Windows") || node.IsPartOfStructuredTrivia())
            {
                return;
            }

            //Get full Name
            String nodeRep = GetFullID(node);

            // TODO: Does this need to be a more explicit check?
            if (ValidNames.Contains(nodeRep) || nodeRep.StartsWith("Windows.UI.Xaml")) // for now check if .Xaml after the or instead of direct namespaces
            {
                // Create Diagnostic to report 
                // to update without changing Codefix, report at the location of its identifier
                var idNameNode = node.ChildNodes().OfType<IdentifierNameSyntax>().First();
                context.ReportDiagnostic(Diagnostic.Create(WUX_Rule, idNameNode.GetLocation()));
            }
        }

        //Navigate up the tree to the full ID name.
        private String GetFullID(QualifiedNameSyntax node)
        {
            //get to the top level parent
            while (node.Parent.IsKind(SyntaxKind.QualifiedName))
            {
                node = (QualifiedNameSyntax)node.Parent;
            }
            //Return a string rep of the full Type/Namespace
            return node.Left.ToString() + "." + node.Right.ToString();
        }

        //TODO: make this an external resource?
        private String[] ValidNames = new String[] 
            {   "Windows.UI.Colors",
                "Windows.UI.ColorHelper",
                "Windows.UI.Text.CaretType",
                "Windows.UI.Text.FindOptions",
                "Windows.UI.Text.FontStretch",
                "Windows.UI.Text.FontStyle",
                "Windows.UI.Text.FontWeight",
                "Windows.UI.Text.FormatEffect",
                "Windows.UI.Text.HorizontalCharacterAlignment",
                "Windows.UI.Text.IRichEditTextRange",
                "Windows.UI.Text.ITextCharacterFormat",
                "Windows.UI.Text.ITextDocument",
                "Windows.UI.Text.ITextParagraphFormat",
                "Windows.UI.Text.ITextRange",
                "Windows.UI.Text.ITextSelection",
                "Windows.UI.Text.LetterCase",
                "Windows.UI.Text.LineSpacingRule",
                "Windows.UI.Text.LinkType",
                "Windows.UI.Text.MarkerAlignment",
                "Windows.UI.Text.MarkerStyle",
                "Windows.UI.Text.MarkerType",
                "Windows.UI.Text.ParagraphAlignment",
                "Windows.UI.Text.ParagraphStyle",
                "Windows.UI.Text.PointOptions",
                "Windows.UI.Text.RangeGravity",
                "Windows.UI.Text.RichEditTextDocument",
                "Windows.UI.Text.RichEditTextRange",
                "Windows.UI.Text.SelectionOptions",
                "Windows.UI.Text.SelectionType",
                "Windows.UI.Text.TabAlignment",
                "Windows.UI.Text.TabLeader",
                "Windows.UI.Text.TextConstants",
                "Windows.UI.Text.TextDecorations",
                "Windows.UI.Text.TextGetOptions",
                "Windows.UI.Text.TextRangeUnit",
                "Windows.UI.Text.TextScript",
                "Windows.UI.Text.TextSetOptions",
                "Windows.UI.Text.UnderlineType",
                "Windows.UI.Text.VerticalCharacterAlignment",
                "Windows.UI.Xaml", // Replace with more namespaces?
                "Windows.System.DispatcherQueue",
                "Windows.System.DispatcherQueueController",
                "Windows.System.DispatcherQueueHandler",
                "Windows.System.DispatcherQueuePriority",
                "Windows.System.DispatcherQueueShutdownStartingEventArgs",
                "Windows.System.DispatcherQueueTimer"
            };
    }
}
