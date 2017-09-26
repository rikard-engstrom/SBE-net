using NUnit.Framework;
using SBE.Core.Models;
using SBE.Core.OutputGenerators;
using SBE.Core.Services;
using System;
using System.Linq;

namespace SBE._NUnit.Tests.Core.OutputGenerators
{
    [TestFixture]
    public sealed class PdfGeneratorTest
    {
        [Test]
        public void CanGeneratePdf()
        {
            var scenario = new SbeScenario { Title = "Scenario Title", Outcome = Outcome.Passed };
            var feature = new SbeFeature(title: "Feature Title", scenarios: new[] { scenario });
            feature.FeatureText = @"@current
Feature: Implemented Feature
	Some description

Scenario: Add two numbers for implemented calculator
	Given I have entered 50 into the calculator
	And I have entered 70 into the calculator
	When I press add
	Then the result should be 120 on the screen";

            var collectedTests = new SbeAssembly[]
            {
                new SbeAssembly(
                    name: "some.strange.assembly",
                    epics: new[] { new SbeEpic(name: "Some Epic", features: new[] { feature }) })
            };

            string outputFileName = null;
            byte[] outputContent = null;

            var generator = new PdfGenerator("testing", OutputFilter.All, (file, bytes) => { outputFileName = file; outputContent = bytes; });
            generator.Generate(collectedTests);

            Assert.That(outputFileName, Is.EqualTo("\\some.strange.assembly.testing.sbe.pdf"), "FileName");

            var firstFourBytes = Convert.ToBase64String(outputContent.Take(4).ToArray());
            Assert.That(firstFourBytes, Is.EqualTo("JVBERg=="));
        }
    }
}
