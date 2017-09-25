using SBE.Core.Models;
using SBE.Core.Utils;
using System;
using System.Linq;

namespace SBE.Core.OutputGenerators
{
    sealed class XmlGenerator : IOutputGenerator, IDisposable
    {
        readonly string assembly;
        readonly XmlHelper xml;
        readonly bool showFeatureTexts;
        readonly string name;

        private XmlGenerator(string assembly, string name, bool showFeatureTexts = true)
        {
            this.assembly = assembly;
            this.name = name;
            this.showFeatureTexts = showFeatureTexts;
            xml = new XmlHelper(FileHelper.GetOutputFileName(name, "xml", assembly));
        }

        internal XmlGenerator(bool showFeatureTexts = true, string name = "details")
        {
            this.showFeatureTexts = showFeatureTexts;
            this.name = name;
        }

        public void Generate(SbeAssembly[] assemblies)
        {
            foreach (var assembly in assemblies)
            {
                var generator = new XmlGenerator(assembly.Name, name, showFeatureTexts);
                generator.GenerateOutput(assembly.GetEpics());
            }
        }

        private void GenerateOutput(SbeEpic[] epics)
        {
            xml.StartDocument();
            xml.StartElement("epics");

            foreach (var epic in epics)
            {
                WriteEpic(epic);
            }

            xml.EndDocument();
        }

        private void WriteEpic(SbeEpic epic)
        {
            xml.StartElement("epic");
            xml.StartElement("features");

            foreach (var feature in epic.GetFeatures())
            {
                WriteFeature(feature);
            }

            xml.EndElement();
            xml.EndElement();
        }

        private void WriteFeature(SbeFeature feature)
        {
            xml.StartElement("feature");
            xml.AttributeString("scenarioCount", feature.Scenarios.Count.ToString());
            xml.AttributeString("scenarioSuccessCount", feature.Scenarios.Count(x => x.Success()).ToString());
            xml.AttributeString("success", feature.Success().ToString());
            xml.CDataElementString("title", feature.Title);

            if (showFeatureTexts)
            {
                xml.CDataElementString("featureText", feature.FeatureText);
            }

            WriteTags(feature.Tags);
            xml.StartElement("scenarios");

            foreach (var scenario in feature.Scenarios)
            {
                WriteScenario(scenario);
            }

            xml.EndElement();
            xml.EndElement();
        }

        private void WriteScenario(SbeScenario scenario)
        {
            xml.StartElement("scenario");
            xml.AttributeString("success", scenario.Success().ToString());
            xml.AttributeString("outcome", scenario.Outcome.ToString());
            xml.CDataElementString("title", scenario.Title);
            WriteTags(scenario.Tags);
            xml.EndElement();
        }

        private void WriteTags(string[] tags)
        {
            if (tags?.Any() ?? false)
            {
                xml.StartElement("tags");
                foreach (var tag in tags)
                {
                    xml.ElementString("tag", tag);
                }

                xml.EndElement();
            }
        }

        public void Dispose()
        {
            xml?.Dispose();
        }
    }
}
