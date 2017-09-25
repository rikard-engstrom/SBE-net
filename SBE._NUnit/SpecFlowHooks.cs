using SBE.Core;
using TechTalk.SpecFlow;

namespace SBE._NUnit
{
    [Binding]
    public class SpecFlowHooks
    {
        [AfterScenario]
        public void AfterScenario()
        {
            SBEHooks.AfterScenario(NUnitEventService.CreateAfterScenarioEvent());
        }

        [AfterTestRun]
        public static void AfterTestRun()
        {
            SBEHooks.AfterTestRun();
        }
    }
}
