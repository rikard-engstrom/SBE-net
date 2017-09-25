using SBE.Core.OutputGenerators;
using System.Collections.Generic;

namespace SBE.Core
{
    public static class SBEConfiguration
    {
        public static string SourcePath { get; set; }

        public static List<IOutputGenerator> Generators { get; private set; }

        static SBEConfiguration()
        {
            Generators = new List<IOutputGenerator>
            {
                DefaultGenerators.XmlDetail,
                DefaultGenerators.XmlSummary,
                DefaultGenerators.JsonSummary,
                DefaultGenerators.PdfAll,
                DefaultGenerators.PdfAllImplemented,
                DefaultGenerators.PdfCurrent
            };
        }

        public static DefaultGenerators DefaultGenerators { get; private set; } = new DefaultGenerators();
    }
}
