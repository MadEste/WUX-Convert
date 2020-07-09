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
    public class INC_Test : TestHelper.CodeFixVerifier
    {
        // This contains code to analyze where a diagnostic should be reported

        // 1. Microsoft.Xaml.Interactivity is unuseable for now
        private const string INC_Report_1 = @"
            using System;
            using Microsoft.Xaml.Interactivity;
            namespace FakeNamespace
            {
                public class ImageScrollBehavior : DependencyObject, IBehavior
                {
                    public DependencyObject AssociatedObject { get; private set; }
                    public void Attach(DependencyObject associatedObject)
                    {
                        AssociatedObject = associatedObject;
                        if (!GetScrollViewer())
                        {
                            ((ListViewBase)associatedObject).Loaded += ListGridView_Loaded;
                        }
                    }
                }
            }";

        // 2. Inking is no longer compatible
        private const string INC_Report_2 = @"
            using System;
            using Windows.UI.Input.Inking;
            namespace FakeNamespace
            {
                public class Aclass
                {
                }
            }";

        // 3. Inking not compat
        private const string INC_Report_3 = @"
            using System;
            using Windows.ApplicationModel.Preview.InkWorkspace;
            namespace FakeNamespace
            {
                public class Aclass
                {
                }
            }";

        // 4. Inking not compat
        private const string INC_Report_4 = @"
            using System;
            using Windows.UI.Input.Inking.Core;
            namespace FakeNamespace
            {
                public class Aclass
                {
                }
            }";

        [DataTestMethod]
        [DataRow(INC_Report_1, 3, 19),
            DataRow(INC_Report_2, 3, 19),
            DataRow(INC_Report_3, 3, 19),
            DataRow(INC_Report_4, 3, 19)]
        public void INCDiagosticIsRaised(
           string test,
           int line,
           int column)
        {
            var expected = new DiagnosticResult
            {
                Id = WUX_Using_Analyzer.INC_ID,
                Message = new LocalizableResourceString(nameof(Prototype_Fixes.Resources.WUX_Using_MessageFormat), Prototype_Fixes.Resources.ResourceManager, typeof(Prototype_Fixes.Resources)).ToString(),
                Severity = DiagnosticSeverity.Error,
                Locations =
                    new[] {
                            new DiagnosticResultLocation("Test0.cs", line, column)
                        }
            };
            VerifyCSharpDiagnostic(test, expected);
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
