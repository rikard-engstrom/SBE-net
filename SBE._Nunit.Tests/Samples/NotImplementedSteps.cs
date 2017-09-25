using TechTalk.SpecFlow;

namespace SBE._NUnit.Samples
{
    [Binding, Scope(Feature = "Sample of a not implemented feature")]
    sealed class SampleSteps
    {
        [Given(@"I have entered (.*) into the calculator")]
        private void GivenIHaveEnteredIntoTheCalculator(int p0)
        {
            ScenarioContext.Current.Pending();
        }
        
        [When(@"I press add")]
        private void WhenIPressAdd()
        {
            ScenarioContext.Current.Pending();
        }
        
        [Then(@"the result should be (.*) on the screen")]
        private void ThenTheResultShouldBeOnTheScreen(int p0)
        {
            ScenarioContext.Current.Pending();
        }
    }
}
