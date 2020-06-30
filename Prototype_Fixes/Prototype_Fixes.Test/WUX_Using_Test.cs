﻿using Microsoft.CodeAnalysis.CSharp.Testing.MSTest;
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
    public class WUX_Using_Test : TestHelper.CodeFixVerifier
    {
        // This contains code to analyze where no diagnostic should be reported

        // <SnippetVariableAssigned>
        private const string NoWUX = @"
            using System;
            namespace FakeNamespace
            {
                class Program
                {
                    static void Main(string[] args)
                    {
                        int i = 0;
                        Console.WriteLine(i++);
                    }
                }
            }";

        // This contains code That should be fixed

        // 1. Regular using CodeFix
        private const string UsingWUX = @"
            using System;
            using Windows.UI.XAML;
            namespace FakeNamespace
            {
                class Program
                {
                }
            }";
        private const string UsingWUXFix = @"
            using System;
            using Microsoft.UI.XAML;
            namespace FakeNamespace
            {
                class Program
                {
                }
            }";

        // 2. Using Directive with Alias and CodeFix
        private const string UsingWUXAlias = @"
            using System;
            using alias = Windows.UI.XAML;
            namespace FakeNamespace
            {
                class Program
                {
                }
            }";
        private const string UsingWUXAliasFix = @"
            using System;
            using alias = Microsoft.UI.XAML;
            namespace FakeNamespace
            {
                class Program
                {
                }
            }";

        // 3. Static using CodeFix
        private const string UsingStaticWUX = @"
            using System;
            using static Windows.UI.Xaml.VisualStateManager;
            namespace FakeNamespace
            {
                class Program
                {
                }
            }";
        private const string UsingStaticWUXFix = @"
            using System;
            using static Microsoft.UI.Xaml.VisualStateManager;
            namespace FakeNamespace
            {
                class Program
                {
                }
            }";

        // 4. Inline Using CodeFix
        private const string UsingInlineWUX = @"
            using System;
            namespace FakeNamespace
            {
                class Program
                {
                    Windows.UI.Xaml.Controls.Frame rootFrame = Window.Current.Content as Windows.UI.Xaml.Controls.Frame; //Random Comment
                }
            }";
        private const string UsingInlineWUXFix = @"
            using System;
            namespace FakeNamespace
            {
                class Program
                {
                    Microsoft.UI.Xaml.Controls.Frame rootFrame = Window.Current.Content as Microsoft.UI.Xaml.Controls.Frame; //Random Comment
                }
            }";

        // 5. Inline static using
        private const string UsingInlineStaticWUX = @"
            using System;
            namespace FakeNamespace
            {
                class Program
                {
                    static Windows.UI.Xaml.VisualStateManager visual;
                }
            }";
        private const string UsingInlineStaticWUXFix = @"
            using System;
            namespace FakeNamespace
            {
                class Program
                {
                    static Microsoft.UI.Xaml.VisualStateManager visual;
                }
            }";


        //Denotes that method is a data test
        [DataTestMethod]
        [DataRow(""), DataRow(NoWUX)]
        // Test Method for valid code with no triggered diagnostics
        public void ValidWUXUsingNoDiagnostic(string testCode)
        {
            VerifyCSharpDiagnostic(testCode);
        }

        [DataTestMethod]
        [DataRow(UsingWUX, UsingWUXFix, 3, 13),
            DataRow(UsingWUXAlias, UsingWUXAliasFix, 3, 13),
            DataRow(UsingStaticWUX, UsingStaticWUXFix, 3, 13),
            DataRow(UsingInlineWUX, UsingInlineWUXFix, 7, 21),
            DataRow(UsingInlineStaticWUX, UsingInlineStaticWUXFix, 7, 21)]
        public void WhenDiagosticIsRaisedFixUpdatesCode(
           string test,
           string fixTest,
           int line,
           int column)
        {
            var expected = new DiagnosticResult
            {
                Id = WUX_Using_Analyzer.DiagnosticId,
                Message = new LocalizableResourceString(nameof(Prototype_Fixes.Resources.WUX_Using_MessageFormat), Prototype_Fixes.Resources.ResourceManager, typeof(Prototype_Fixes.Resources)).ToString(),
                Severity = DiagnosticSeverity.Warning,
                Locations =
                    new[] {
                            new DiagnosticResultLocation("Test0.cs", line, column)
                        }
            };

            VerifyCSharpDiagnostic(test, expected);

            VerifyCSharpFix(test, fixTest);
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