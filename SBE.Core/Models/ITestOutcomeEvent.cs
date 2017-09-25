namespace SBE.Core.Models
{
    public interface ITestOutcomeEvent
    {
        object[] TestArguments { get; }
        string TestClassFullName { get; }
        string TestMethodName { get; }
        Outcome Outcome { get; }
    }
}
