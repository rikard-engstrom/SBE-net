using SBE.Core.Models;
using System.Collections.Generic;
using System.Linq;

namespace SBE.Core.Services
{
    static partial class FeatureService
    {
        static readonly Dictionary<string, SbeAssembly> Assemblies = new Dictionary<string, SbeAssembly>();

        internal static void RegisterScenario(SbeScenario scenario, SpecflowContextWrapper specflowContext)
        {
            var assembly = GetCurrentAssembly(scenario.AssemblyName);
            var epic = GetCurrentEpic(assembly, specflowContext);
            var feature = GetCurrentFeature(epic, specflowContext);
            feature.Scenarios.Add(scenario);
        }

        private static SbeEpic GetCurrentEpic(SbeAssembly assembly, SpecflowContextWrapper specflowContext)
        {
            var epicName = specflowContext.GetEpicName();

            if (!assembly.Epics.TryGetValue(epicName, out SbeEpic epic))
            {
                epic = new SbeEpic
                {
                    Name = epicName,
                };
                assembly.Epics.Add(epicName, epic);
            }

            return epic;
        }

        internal static void SetFeatureTexts(string sourcePath)
        {
            var features = Assemblies.Values.SelectMany(a => a.Epics.Values.SelectMany(e => e.Features.Values)).ToArray();
            FeatureFileService.SetFeatureTexts(sourcePath, features);
        }

        private static SbeAssembly GetCurrentAssembly(string assemblyName)
        {
            if (!Assemblies.TryGetValue(assemblyName, out SbeAssembly assembly))
            {
                assembly = new SbeAssembly
                {
                    Name = assemblyName,
                };
                Assemblies.Add(assemblyName, assembly);
            }

            return assembly;
        }

        private static SbeFeature GetCurrentFeature(SbeEpic epic, SpecflowContextWrapper specflowContext)
        {
            var title = specflowContext.FeatureTitle;

            if (!epic.Features.TryGetValue(title, out SbeFeature feature))
            {
                feature = new SbeFeature
                {
                    Title = specflowContext.FeatureTitle,
                    Tags = specflowContext.FeatureTags,
                };
                epic.Features.Add(title, feature);
            }

            return feature;
        }

        internal static SbeAssembly[] GetAllAssemblies()
        {
            return Assemblies.Values.OrderBy(x => x.Name).ToArray();
        }
    }
}
