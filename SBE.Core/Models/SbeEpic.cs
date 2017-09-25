using System.Collections.Generic;
using System.Linq;

namespace SBE.Core.Models
{
    public class SbeEpic
    {
        public string Name { get; internal set; }
        public Dictionary<string, SbeFeature> Features { get; private set; } = new Dictionary<string, SbeFeature>();

        public SbeEpic(string name = null, SbeFeature[] features = null)
        {
            Name = name;

            if (features != null)
            {
                Features = features.ToDictionary(key => key.Title);
            }
        }

        internal SbeFeature[] GetFeatures()
        {
            return Features.Values.OrderBy(feature =>
            {
                var sortTag = TagsService.SortTag(feature.Tags);
                return string.Concat(sortTag, feature.Title);
            }).ToArray();
        }
    }
}
