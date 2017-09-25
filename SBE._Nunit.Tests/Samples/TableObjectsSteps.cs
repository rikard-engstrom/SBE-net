using TechTalk.SpecFlow;

namespace SBE._NUnit.Tests.Samples
{
    [Binding, Scope(Feature = "")]
    sealed class TableObjectsSteps
    {
        [Given(@"I have entered into the calculator")]
        private void GivenIHaveEnteredIntoTheCalculator(Table table)
        {
            ScenarioContext.Current.Pending();
        }

        [When(@"I press add")]
        private void WhenIPressAdd()
        {
            ScenarioContext.Current.Pending();
        }

        [Then(@"the result should be on the screen")]
        private void ThenTheResultShouldBeOnTheScreen(Table table)
        {
            ScenarioContext.Current.Pending();
        }
    }
}
