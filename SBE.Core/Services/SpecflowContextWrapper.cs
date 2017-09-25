using SBE.Core.Models;
using TechTalk.SpecFlow;

namespace SBE.Core.Services
{
    sealed class SpecflowContextWrapper
    {
        internal string FeatureTitle { get; set; }
        internal string[] FeatureTags { get; set; }
        internal string ScenarioTitle { get; set; }
        internal string[] ScenarioTags { get; set; }

        internal static SpecflowContextWrapper Create()
        {
            return new SpecflowContextWrapper
            {
                FeatureTitle = FeatureContext.Current.FeatureInfo.Title,
                FeatureTags = FeatureContext.Current.FeatureInfo.Tags,
                ScenarioTitle = ScenarioContext.Current.ScenarioInfo.Title,
                ScenarioTags = ScenarioContext.Current.ScenarioInfo.Tags
            };
        }

        internal string GetEpicName()
        {
            return TagsService.GetEpicName(FeatureTags);
        }
    }
}
