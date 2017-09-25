using NUnit.Framework;
using NUnit.Framework.Interfaces;
using SBE.Core.Models;

namespace SBE._NUnit
{
    internal partial class NUnitEventService
    {
        internal static ITestOutcomeEvent CreateAfterScenarioEvent()
        {
            return new AfterScenarioEvent
            {
                TestArguments = TestContext.CurrentContext.Test.Arguments,
                TestClassFullName = TestContext.CurrentContext.Test.ClassName,
                TestMethodName = TestContext.CurrentContext.Test.MethodName,
                Outcome = GetTestStatus()
            };
        }

        private static Outcome GetTestStatus()
        {
            var outcome = TestContext.CurrentContext.Result.Outcome.Status;
            switch (outcome)
            {
                case TestStatus.Passed:
                    return Outcome.Passed;
                case TestStatus.Failed:
                    return Outcome.Failed;
                default:
                    return Outcome.Inconclusive;
            }
        }
    }
}