using SBE.Core.Models;
using SBE.Core.Services;
using SBE.Core.Utils;
using System;
using System.IO;

namespace SBE.Core.OutputGenerators
{
    sealed class PdfGenerator : IOutputGenerator
    {
        internal static IOutputGenerator CreateAll() => new PdfGenerator("all", OutputFilter.All);
        internal static IOutputGenerator CreateAllImplemented() => new PdfGenerator("all-implemented", OutputFilter.AllImplemented);
        internal static IOutputGenerator CreateCurrent() => new PdfGenerator("current", OutputFilter.CurrentIteration);

        readonly OutputFilter filter;
        readonly string name;
        readonly Action<string, byte[]> writer;

        internal PdfGenerator(string name, OutputFilter filter, Action<string, byte[]> writer = null)
        {
            this.name = name;
            this.writer = writer ?? File.WriteAllBytes;
            this.filter = filter;
        }

        public void Generate(SbeAssembly[] assemblies)
        {
            foreach (var assembly in assemblies)
            {
                var file = FileHelper.GetOutputFileName(name, "pdf", assembly.Name);
                var service = new GherkinPdfService(string.Empty, string.Empty, string.Empty, filter);
                var bytes = service.GeneratePdf(assembly.GetEpics());
                writer(file, bytes);
            }
        }
    }
}
