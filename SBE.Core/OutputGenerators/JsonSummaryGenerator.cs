using Newtonsoft.Json;
using SBE.Core.Models;
using SBE.Core.Utils;
using System;
using System.IO;
using System.Linq;

namespace SBE.Core.OutputGenerators
{
    internal sealed class JsonSummaryGenerator : IOutputGenerator
    {
        readonly Action<string, string> writer;

        internal JsonSummaryGenerator(Action<string, string> writer = null)
        {
            this.writer = writer ?? File.WriteAllText;
        }

        public void Generate(SbeAssembly[] assemblies)
        {
            foreach (var assembly in assemblies)
            {
                var outputModel = new
                {
                    epics = assembly.GetEpics()
                                .Select(e => new
                                {
                                    e.Name,
                                    Features = e.GetFeatures()
                                        .Select(x => new
                                        {
                                            x.Title,
                                            x.Tags,
                                            Success = x.Success(),
                                            Scenarios = x.Scenarios.Select(s => new { s.Title, Success = s.Success(), Outcome = s.Outcome.ToString(), s.Tags })
                                        })
                                })
                };

                var json = JsonConvert.SerializeObject(outputModel, Formatting.Indented);
                var file = FileHelper.GetOutputFileName("summary", "json", assembly.Name);
                writer(file, json);
            }
        }
    }
}
