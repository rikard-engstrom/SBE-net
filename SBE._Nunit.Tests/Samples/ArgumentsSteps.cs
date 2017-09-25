using TechTalk.SpecFlow;

namespace SBE._NUnit.Tests.Samples
{
    sealed class ArgumentsSteps
    {
        [Given(@"I have entered (.*) into the calculator")]
        private void GivenIHaveEnteredIntoTheCalculator(int number)
        {
            ScenarioContext.Current.Pending();
        }

        [When(@"I press add")]
        private void WhenIPressAdd()
        {
            ScenarioContext.Current.Pending();
        }

        [Then(@"the result should be (.*) on the screen")]
        private void ThenTheResultShouldBeOnTheScreen(int total)
        {
            ScenarioContext.Current.Pending();
        }
    }
}
