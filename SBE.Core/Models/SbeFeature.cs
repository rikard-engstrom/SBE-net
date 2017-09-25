using System.Collections.Generic;
using System.Linq;

namespace SBE.Core.Models
{
    public class SbeFeature
    {
        public string Title { get; set; }
        public ICollection<SbeScenario> Scenarios { get; private set; } = new List<SbeScenario>();
        public string FeatureText { get; set; }
        public string[] Tags { get; internal set; }

        public SbeFeature(string title = null, SbeScenario[] scenarios = null)
        {
            Title = title;

            if (scenarios != null)
            {
                Scenarios = scenarios.ToList();
            }
        }

        internal bool Success()
        {
            return Scenarios.All(x => x.Success());
        }

        public Outcome GetOutcome()
        {
            return OutcomeService.CombineOutcomes(Scenarios.Select(x => x.Outcome));
        }

        internal bool IsCurrentIteration()
        {
            return TagsService.IsCurrentIteration(Tags);
        }

        internal bool IsImplemented()
        {
            var outcome = GetOutcome();
            return outcome == Outcome.Passed || 
                                outcome == Outcome.PartlyPassed || 
                                outcome == Outcome.Failed;
        }
    }
}
