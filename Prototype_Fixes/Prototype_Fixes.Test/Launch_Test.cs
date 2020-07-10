using Microsoft.CodeAnalysis.CSharp.Testing.MSTest;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Microsoft.CodeAnalysis.Diagnostics;
using Microsoft.CodeAnalysis.CodeFixes;
using Microsoft.CodeAnalysis.CodeActions;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis;
using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using Microsoft.CodeAnalysis.Text;
using System.Collections.Immutable;
using System.Threading;
using TestHelper;
using Prototype_Fixes;

namespace Prototype_Fixes.Test
{
    [TestClass]
    public class Launch_Test : TestHelper.CodeFixVerifier
    {
        // This contains code to analyze where no diagnostic should be reported

        // This contains code That should be fixed

        // 1. Regular using CodeFix
        private const string Launch_rep1 = @"
            using Windows.ApplicationModel.Activation;
            using Microsoft.UI.Xaml;
            namespace FakeNamespace
            {
                class Program
                {
                    protected override async void OnLaunched(LaunchActivatedEventArgs args){}
                }
            }";
        private const string Launch_fix1 = @"
            using Windows.ApplicationModel.Activation;
            using Microsoft.UI.Xaml;
            namespace FakeNamespace
            {
                class Program
                {
                    protected override async void OnLaunched(Microsoft.UI.Xaml.LaunchActivatedEventArgs args){}
                }
            }";



        //Denotes that method is a data test
        [DataTestMethod]
        [DataRow("")]
        // Test Method for valid code with no triggered diagnostics
        public void ValidWUXUsingNoDiagnostic(string testCode)
        {
            VerifyCSharpDiagnostic(testCode);
        }

        [DataTestMethod]
        [DataRow(Launch_rep1, Launch_fix1, 8, 62)]
        public void WhenDiagosticIsRaisedFixUpdatesCode(
           string test,
           string fixTest,
           int line,
           int column)
        {
            var expected = new DiagnosticResult
            {
                Id = WUX_Using_Analyzer.LAU_ID,
                Message = new LocalizableResourceString(nameof(Prototype_Fixes.Resources.WUX_Using_MessageFormat), Prototype_Fixes.Resources.ResourceManager, typeof(Prototype_Fixes.Resources)).ToString(),
                Severity = DiagnosticSeverity.Error,
                Locations =
                    new[] {
                            new DiagnosticResultLocation("Test0.cs", line, column)
                        }
            };

            VerifyCSharpDiagnostic(test, expected);
            //pass true to allow new errors
            VerifyCSharpFix(test, fixTest, null, true);
        }

        //Returns a WUX_Using_CodeFix
        protected override CodeFixProvider GetCSharpCodeFixProvider()
        {
            return new WUX_Using_CodeFix();
        }

        // Returns an analyzer
        protected override DiagnosticAnalyzer GetCSharpDiagnosticAnalyzer()
        {
            return new WUX_Using_Analyzer();
        }
    }
}
