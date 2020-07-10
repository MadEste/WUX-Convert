using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.CodeAnalysis.Simplification;
// Code analysis usings
using System.Composition;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CodeFixes;
using System.Collections.Immutable;
using System.Threading.Tasks;
using System.Linq;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.CodeActions;
using System.Threading;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.Formatting;
using System.Diagnostics;

namespace Prototype_Fixes
{
    [ExportCodeFixProvider(LanguageNames.CSharp, Name = nameof(WUX_Using_CodeFix)), Shared]
    public class WUX_Using_CodeFix : CodeFixProvider
    {
        // title of codeFix
        private const string title = "Update Windows Namespace";

        // required to overide for CodeFixProvider, create an array of all fixable errors from the higlight analyzer
        public override ImmutableArray<string> FixableDiagnosticIds
        {
            //use the id of the associated analyzer to create array
            get { return ImmutableArray.Create(WUX_Using_Analyzer.WUX_ID, WUX_Using_Analyzer.LAU_ID); }
        }

        // an optional overide to fix all occurences instead of just one.
        public sealed override FixAllProvider GetFixAllProvider()
        {
            //default fix all provider
            return WellKnownFixAllProviders.BatchFixer;
        }

        // Required Overide to fix the occurance
        public override async Task RegisterCodeFixesAsync(CodeFixContext context)
        {
            //trees are immutable, have to get the root and make changes to a new
            // tree and update
            var root = await context.Document.GetSyntaxRootAsync(context.CancellationToken).ConfigureAwait(false);

            // TODO: Create analysis and generate code action for each fix

            var diagnostic = context.Diagnostics.First();
            var diagnosticSpanSrc = diagnostic.Location.SourceSpan;

            //get the diagnostic id
            var dID = diagnostic.Id;

            //TRY using ID
            //var diagnosticID = diagnostic.Location; Never used

            //var tokens = root.FindToken(diagnosticSpanSrc.Start).Parent.AncestorsAndSelf().OfType<IdentifierNameSyntax>();

            //new code
            var t = root.FindNode(diagnosticSpanSrc) as IdentifierNameSyntax;

            //find the token of associated diagnostic in the tree
            //var token = root.FindToken(diagnosticSpanSrc.Start).Parent.AncestorsAndSelf().OfType<UsingDirectiveSyntax>().First();

            //Register a different fix for launch args
            if (dID == WUX_Using_Analyzer.LAU_ID)
            {
                context.RegisterCodeFix(
                CodeAction.Create(
                    title: title,
                    createChangedDocument: c => ReplaceLaunchNameAsync(context.Document, t, c),
                    equivalenceKey: title),
                diagnostic);
            }
            else
            {

                // Register a code action that will invoke the fix.
                context.RegisterCodeFix(
                    CodeAction.Create(
                        title: title,
                        createChangedDocument: c => ReplaceNameAsync(context.Document, t, c),
                        equivalenceKey: title),
                    diagnostic);
            }
        }

        private async Task<Document> ReplaceLaunchNameAsync(Document doc, IdentifierNameSyntax launchNode, CancellationToken c)
        {
            //TODO GO through this code and remove unecessary bits

            //grab the Windows token to replace

            //create a Replacement Token ? HOW to make this prettier...
            var replaceNode = SyntaxFactory.QualifiedName(
                SyntaxFactory.QualifiedName(
                    SyntaxFactory.QualifiedName(
                        SyntaxFactory.IdentifierName("Microsoft"), SyntaxFactory.IdentifierName("UI")
                    ), SyntaxFactory.IdentifierName("Xaml")
                    ), 
                SyntaxFactory.IdentifierName("LaunchActivatedEventArgs"));

            /*
            var microsoft = SyntaxFactory.IdentifierName("Microsoft");
            var ui = SyntaxFactory.IdentifierName("UI");

            SyntaxFactory.ExpressionStatement(
                SyntaxFactory.MemberAccessExpression(
                    SyntaxKind.SimpleMemberAccessExpression,
                    SyntaxFactory.MemberAccessExpression(
                        SyntaxKind.SimpleMemberAccessExpression,
                        SyntaxFactory.MemberAccessExpression(
                            SyntaxKind.SimpleMemberAccessExpression,
                            SyntaxFactory.IdentifierName("Microsoft"),
                            SyntaxFactory.IdentifierName("UI"))
                        .WithOperatorToken(
                            SyntaxFactory.Token(SyntaxKind.DotToken)),
                        SyntaxFactory.IdentifierName("Xaml"))
                    .WithOperatorToken(
                        SyntaxFactory.Token(SyntaxKind.DotToken)),
                    SyntaxFactory.IdentifierName("LaunchActivatedEventArgs"))
                .WithOperatorToken(
                    SyntaxFactory.Token(SyntaxKind.DotToken)))
            .WithSemicolonToken(
                SyntaxFactory.MissingToken(SyntaxKind.SemicolonToken))
            .NormalizeWhitespace()
            



            var newToken = SyntaxFactory.Identifier(SyntaxTrivia., SyntaxKind.IdentifierToken, "Microsoft", "Microsoft", winTrailTrivia);

            //Replace node with new
            var newNode = usingNode.ReplaceToken(winToken, micToken);
            launchNode.

            */

            // add annotations to format the new token d
            //TODO: TEST IF NECESSARY! - Sometimes removes tabs etc, consider removing entirely
            var formattedNode = replaceNode.WithAdditionalAnnotations(Formatter.Annotation);

            // replace the old win token with new Microsoft
            var oldRoot = await doc.GetSyntaxRootAsync(c);
            //Try Not using formattedNode
            var newRoot = oldRoot.ReplaceNode(launchNode, replaceNode);
            // Return a new doc wit htransfromed tree
            return doc.WithSyntaxRoot(newRoot);

        }

        //Actual code to update tree node
        private async Task<Document> ReplaceNameAsync(Document doc, IdentifierNameSyntax usingNode, CancellationToken c)
        {
            //TODO GO through this code and remove unecessary bits

            //grab the Windows token to replace
            var winToken = usingNode.DescendantTokens().Single(n => n.Text == "Windows");

            //grab leading trivia for first token
            var winLeadTrivia = winToken.LeadingTrivia;
            //grab traling trivia
            var winTrailTrivia = winToken.TrailingTrivia;

            //create a Microsoft token with old trivia
            var micToken = SyntaxFactory.Identifier(winLeadTrivia, SyntaxKind.IdentifierToken, "Microsoft", "Microsoft", winTrailTrivia);

            //Replace node with new
            var newNode = usingNode.ReplaceToken(winToken, micToken);
           


            // add annotations to format the new token d
            //TODO: TEST IF NECESSARY! - Sometimes removes tabs etc, consider removing entirely
            var formattedNode = newNode.WithAdditionalAnnotations(Formatter.Annotation);

            // replace the old win token with new Microsoft
            var oldRoot = await doc.GetSyntaxRootAsync(c);
            Debug.WriteLine(usingNode.ToFullString());
            Debug.WriteLine(newNode.ToFullString());
            //Try Not using formattedNode
            var newRoot = oldRoot.ReplaceNode(usingNode, newNode);
            // Return a new doc wit htransfromed tree
            return doc.WithSyntaxRoot(newRoot);
        }
    }
}

