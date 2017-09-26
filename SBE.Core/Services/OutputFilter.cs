using SBE.Core.Models;

namespace SBE.Core.Services
{
    internal sealed class OutputFilter
    {
        enum Filter
        {
            All,
            AllImplemented,
            CurrentIteration
        }

        internal static readonly OutputFilter All = new OutputFilter(Filter.All);
        internal static readonly OutputFilter AllImplemented = new OutputFilter(Filter.AllImplemented);
        internal static readonly OutputFilter CurrentIteration = new OutputFilter(Filter.CurrentIteration);

        readonly Filter filter;

        private OutputFilter(Filter filter)
        {
            this.filter = filter;
        }

        internal bool IncludeFeature(SbeFeature feature)
        {
            return filter == Filter.All ||
                    (filter == Filter.CurrentIteration && feature.IsCurrentIteration()) ||
                    (filter == Filter.AllImplemented && feature.IsImplemented());
        }
    }
}
