using SBE.Core.Models;
using SBE.Core.OutputGenerators;
using SBE.Core.Services;
using SBE.Core.Utils;

namespace SBE.Core
{
    public static class SBEHooks
    {
        public static void AfterScenario(ITestOutcomeEvent e)
        {
            var specflowContext = SpecflowContextWrapper.Create();
            var scenario = ScenarioService.GetScenario(e, specflowContext);
            FeatureService.RegisterScenario(scenario, specflowContext);
        }

        public static void AfterTestRun()
        {
            FileHelper.SourcePath = SBEConfiguration.SourcePath;
            FeatureService.SetFeatureTexts(SBEConfiguration.SourcePath);
            var assemblies = FeatureService.GetAllAssemblies();

            foreach (var generator in SBEConfiguration.Generators)
            {
                generator.Generate(assemblies);
            }
        }
    }
}
