using SBE.Core.Models;

namespace SBE._NUnit
{
    sealed class AfterScenarioEvent : ITestOutcomeEvent
    {
        public object[] TestArguments { get; internal set; }
        public string TestClassFullName { get; internal set; }
        public string TestMethodName { get; internal set; }
        public Outcome Outcome { get; internal set; }
    }
}