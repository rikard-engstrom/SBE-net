using SBE.Core.Models;

namespace SBE.Core.Services
{
    static partial class ScenarioService
    {
        internal static SbeScenario GetScenario(ITestOutcomeEvent e, SpecflowContextWrapper specflowContext)
        {
            var testReflection = ReflectionService.GetTestMethodInfo(e);

            return new SbeScenario
            {
                Title = specflowContext.ScenarioTitle,
                Tags = specflowContext.ScenarioTags,
                Outcome = e.Outcome,
                AssemblyName = testReflection.AssemblyName,
                NamedArgumets = testReflection.NamedArgumets
            };
        }
    }
}
