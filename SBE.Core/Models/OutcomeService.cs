using System.Collections.Generic;
using System.Linq;

namespace SBE.Core.Models
{
    static class OutcomeService
    {
        internal static Outcome CombineOutcomes(IEnumerable<Outcome> outcomes)
        {
            if (outcomes.AnyFailed())
            {
                return Outcome.Failed;
            }

            if (outcomes.AllPassed())
            {
                return Outcome.Passed;
            }

            if (outcomes.AnyPassed())
            {
                return Outcome.PartlyPassed;
            }

            return Outcome.Inconclusive;
        }

        private static bool AnyFailed(this IEnumerable<Outcome> outcomes)
        {
            return outcomes.Any(x => x == Outcome.Failed);
        }

        private static bool AllPassed(this IEnumerable<Outcome> outcomes)
        {
            return outcomes.All(x => x == Outcome.Passed);
        }

        private static bool AnyPassed(this IEnumerable<Outcome> outcomes)
        {
            return outcomes.Any(x => x == Outcome.Passed);
        }
    }
}
