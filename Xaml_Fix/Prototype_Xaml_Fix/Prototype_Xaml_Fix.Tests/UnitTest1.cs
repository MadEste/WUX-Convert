using Microsoft.VisualStudio.TestTools.UnitTesting;
using RapidXaml;
using RapidXaml.TestHelpers;

namespace Prototype_Xaml_Fix.Tests
{
    [TestClass]
    public class UnitTest1
    {
        [TestMethod]
        public void DoesReportWhenShould()
        {
            var analyzer = new MyElementAnalyzer();

            var testElement = CustomAnalysisTestHelper.StringToElement("<MyElement />");

            var analysisResult = analyzer.Analyze(testElement, new ExtraAnalysisDetails());

            Assert.AreEqual(1, analysisResult.Actions.Count);
            Assert.AreEqual(ActionType.AddAttribute, analysisResult.Actions[0].Action);
            Assert.AreEqual("IsEnabled", analysisResult.Actions[0].Name);
        }

        [TestMethod]
        public void DoesNotReportWhenShouldNot()
        {
            var analyzer = new MyElementAnalyzer();

            var testElement = CustomAnalysisTestHelper.StringToElement("<MyElement IsEnabled=\"False\" />");

            var analysisResult = analyzer.Analyze(testElement, new ExtraAnalysisDetails());

            Assert.IsTrue(analysisResult.IsNone);
        }
    }
}
