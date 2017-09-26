using NUnit.Framework;
using SBE.Core;
using System;
using System.IO;
using System.Reflection;

namespace SBE._NUnit.Tests
{
    [SetUpFixture]
    sealed class Startup
    {
        [OneTimeSetUp]
        public void OneTimeSetUp()
        {
            SBEConfiguration.SourcePath = GetSourcePath();
            SBEConfiguration.Generators.Clear();
            SBEConfiguration.Generators.Add(SBEConfiguration.DefaultGenerators.JsonSummary);
            SBEConfiguration.Generators.Add(SBEConfiguration.DefaultGenerators.PdfAll);
            SBEConfiguration.Generators.Add(SBEConfiguration.DefaultGenerators.PdfAllImplemented);
            SBEConfiguration.Generators.Add(SBEConfiguration.DefaultGenerators.PdfCurrent);
            SBEConfiguration.Generators.Add(SBEConfiguration.DefaultGenerators.XmlDetail);
            SBEConfiguration.Generators.Add(SBEConfiguration.DefaultGenerators.XmlSummary);
        }

        private static string GetSourcePath()
        {
            var outputDirectoryPathBuildResult = Environment.GetEnvironmentVariable("BUILD_SOURCESDIRECTORY");
            if (string.IsNullOrEmpty(outputDirectoryPathBuildResult))
            {
                var uriCurrentExecutingAssembly = Assembly.GetExecutingAssembly().GetName().CodeBase;
                var absolutePathCurrentExecutingAssembly = Path.GetDirectoryName(new Uri(uriCurrentExecutingAssembly).AbsolutePath);
                outputDirectoryPathBuildResult = GetPathParentDirectory(absolutePathCurrentExecutingAssembly, 3);
            }

            return outputDirectoryPathBuildResult;
        }

        private static string GetPathParentDirectory(string inputDirectoryPath, int hirerachyLevels)
        {
            var rootDirectoryPath = inputDirectoryPath;

            for (var index = 0; index < hirerachyLevels; index++)
            {
                rootDirectoryPath = Path.GetDirectoryName(rootDirectoryPath);
            }

            return rootDirectoryPath;
        }
    }
}