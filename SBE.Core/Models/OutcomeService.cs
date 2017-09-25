using System.Collections.Generic;
using System.Linq;

namespace SBE.Core.Models
{
    static class OutcomeService
    {
        internal static Outcome CombineOutcomes(IEnumerable<Outcome> outcomes)
        {
            if (outcomes.Any(x => x == Outcome.Failed))
            {
                return Outcome.Failed;
            }
            else if (outcomes.All(x => x == Outcome.Passed))
            {
                return Outcome.Passed;
            }
            else if (outcomes.Any(x => x == Outcome.Passed))
            {
                return Outcome.PartlyPassed;
            }
            else
            {
                return Outcome.Inconclusive;
            }
        }
    }
}
