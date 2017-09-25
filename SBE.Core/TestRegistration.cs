using SBE.Core.Models;
using SBE.Core.OutputGenerators;
using SBE.Core.Services;
using SBE.Core.Utils;

namespace SBE.Core
{
    public static class TestRegistration
    {
        public static void TestOutcome(ITestOutcomeEvent e)
        {
            var specflowContext = SpecflowContextWrapper.Create();
            var scenario = ScenarioService.GetScenario(e, specflowContext);
            FeatureService.RegisterScenario(scenario, specflowContext);
        }

        public static void AfterTestRun()
        {
            FileHelper.SourcePath = SbeConfiguration.SourcePath;
            FeatureService.SetFeatureTexts(SbeConfiguration.SourcePath);
            var assemblies = FeatureService.GetAllAssemblies();

            foreach (var generator in SbeConfiguration.Generators)
            {
                generator.Generate(assemblies);
            }
        }
    }
}
