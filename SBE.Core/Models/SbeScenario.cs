using System.Collections.Generic;

namespace SBE.Core.Models
{
    public class SbeScenario
    {
        public string Title { get; set; }
        public Outcome Outcome { get; set; }
        public IEnumerable<KeyValuePair<string, string>> NamedArguments { get; set; }
        public string AssemblyName { get; set; }
        public KeyValuePair<string, string>[] NamedArgumets { get; set; }
        public string[] Tags { get; set; }

        internal bool Success()
        {
            return Outcome == Outcome.Passed;
        }
    }
}
