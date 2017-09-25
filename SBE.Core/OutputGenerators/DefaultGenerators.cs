namespace SBE.Core.OutputGenerators
{
    public sealed class DefaultGenerators
    {
        public IOutputGenerator XmlDetail { get; private set; } = new XmlGenerator();
        public IOutputGenerator XmlSummary { get; private set; } = new XmlGenerator(showFeatureTexts: false, name: "summary");
        public IOutputGenerator JsonSummary { get; private set; } = new JsonSummaryGenerator();
        public IOutputGenerator PdfAll { get; private set; } = PdfGenerator.CreateAll();
        public IOutputGenerator PdfAllImplemented { get; private set; } = PdfGenerator.CreateAllImplemented();
        public IOutputGenerator PdfCurrent { get; private set; } = PdfGenerator.CreateCurrent();
    }
}
