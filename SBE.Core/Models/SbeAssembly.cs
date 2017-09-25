using System.Collections.Generic;
using System.Linq;

namespace SBE.Core.Models
{
    public class SbeAssembly
    {
        public string Name { get; internal set; }
        public Dictionary<string, SbeEpic> Epics { get; private set; } = new Dictionary<string, SbeEpic>();

        public SbeAssembly(string name = null, SbeEpic[] epics = null)
        {
            Name = name;

            if (epics?.Any() ?? false)
            {
                Epics = epics.ToDictionary(key => key.Name);
            }
        }

        public SbeEpic[] GetEpics()
        {
            return Epics.Values.OrderBy(x => x.Name).ToArray();
        }
    }
}
