using Newtonsoft.Json.Linq;
using NUnit.Framework;
using SBE.Core.Models;
using SBE.Core.OutputGenerators;

namespace SBE._NUnit.Tests.Core.OutputGenerators
{
    [TestFixture]
    public sealed class JsonSummaryGeneratorTest
    {
        [Test]
        public void CanGenerateOutput()
        {
            var scenario = new SbeScenario { Title = "Scenario Title", Outcome = Outcome.Passed };
            var feature = new SbeFeature(title: "Feature Title", scenarios: new[] { scenario });

            var collectedTests = new SbeAssembly[]
            {
                new SbeAssembly(
                    name: "some.strange.assembly",
                    epics: new[] { new SbeEpic(name: "Some Epic", features: new[] { feature }) })
            };

            string outputFileName = null;
            string outputContent = null;

            var generator = new JsonSummaryGenerator((file, json) => { outputFileName = file; outputContent = json; });
            generator.Generate(collectedTests);

            Assert.That(outputFileName, Is.EqualTo("\\some.strange.assembly.summary.sbe.json"), "FileName");

            var output = JObject.Parse(outputContent);
            Assert.That(output["epics"].HasValues, Is.True, "Should contain epics");
        }
    }
}
