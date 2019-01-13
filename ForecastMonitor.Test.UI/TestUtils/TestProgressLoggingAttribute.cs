using System;
using NUnit.Framework;
using NUnit.Framework.Interfaces;

namespace ForecastMonitor.Test.UI.TestUtils
{
    [AttributeUsage(AttributeTargets.Class)]
    public class TestProgressLoggingAttribute : Attribute, ITestAction
    {
        public void BeforeTest(ITest test)
        {
            WriteTestInfo("Executing", test);
        }

        public void AfterTest(ITest test)
        {
            WriteTestInfo("Finished", test);
            WriteTestOutCome();
        }

        public ActionTargets Targets => ActionTargets.Test;

        private void WriteTestInfo(string message, ITest details)
        {
            Console.WriteLine(@"{0} {1}: {2}.{3}",
                message,
                details.IsSuite ? "Test Suite" : "Test Case",
                details.ClassName ?? "{no fixture}",
                details.MethodName ?? "{no method}");
        }

        private void WriteTestOutCome()
        {
            Console.WriteLine(@"Test Outcome: Status: {0} Label: {1} Stage of Execution: {2}",
                TestContext.CurrentContext.Result.Outcome.Status,
                TestContext.CurrentContext.Result.Outcome.Label != ""
                    ? TestContext.CurrentContext.Result.Outcome.Label
                    : "{no label}",
                TestContext.CurrentContext.Result.Outcome.Site);
        }
    }
}
