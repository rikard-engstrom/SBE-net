using Gherkin;
using SBE.Core.Models;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace SBE.Core.Services
{
    static partial class FeatureService
    {
        static class FeatureFileService
        {
            sealed class ParsedFeature
            {
                internal string Title { get; set; }
                internal string Content { get; set; }
            }

            internal static void SetFeatureTexts(string sourcePath, SbeFeature[] features)
            {
                var featureFiles = Directory.GetFiles(sourcePath, "*.feature", SearchOption.AllDirectories);
                var parsedFeatures = featureFiles.Select(ParseFeatureFile).ToDictionary(key => key.Title);
                SetFeatureTexts(features, parsedFeatures);
            }

            private static void SetFeatureTexts(SbeFeature[] features, Dictionary<string, ParsedFeature> parsedFeatures)
            {
                foreach (var feature in features)
                {
                    if (parsedFeatures.TryGetValue(feature.Title, out ParsedFeature parsedFeature))
                    {
                        feature.FeatureText = parsedFeature.Content;
                    }
                }
            }

            private static ParsedFeature ParseFeatureFile(string file)
            {
                var content = File.ReadAllText(file);

                var parser = new Parser();
                using (var reader = new StringReader(content))
                {
                    var doc = parser.Parse(reader);
                    var f = doc.ToString();
                    return new ParsedFeature
                    {
                        Title = doc.Feature.Name,
                        Content = content
                    };
                }
            }
        }
    }
}
