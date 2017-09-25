using TechTalk.SpecFlow;

namespace SBE._NUnit.Tests.Samples
{
    [Binding, Scope(Feature = "Implemented Feature")]
    sealed class ImplementedSteps
    {
        [Given(@"I have entered (.*) into the calculator")]
        private void GivenIHaveEnteredIntoTheCalculator(int p0)
        {
        }

        [When(@"I press add")]
        private void WhenIPressAdd()
        {
        }

        [Then(@"the result should be (.*) on the screen")]
        private void ThenTheResultShouldBeOnTheScreen(int p0)
        {
        }
    }
}
