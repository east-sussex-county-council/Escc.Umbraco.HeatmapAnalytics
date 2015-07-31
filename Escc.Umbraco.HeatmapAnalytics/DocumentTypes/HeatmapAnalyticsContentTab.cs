using Umbraco.Inception.Attributes;
using Umbraco.Inception.BL;

namespace Escc.Umbraco.HeatmapAnalytics.DocumentTypes
{
    /// <summary>
    /// Content tab for the 'Heatmap analytics' document type in Umbraco
    /// </summary>
    public class HeatmapAnalyticsContentTab : TabBase
    {
        [UmbracoProperty("Where to enable it?", "whereToEnableIt", "Umbraco.MultiNodeTreePicker", "Multi-node tree picker", description: "Select the target pages on which to enable heatmap analytics.", sortOrder: 1)]
        public string WhereToDisplayIt { get; set; }

        [UmbracoProperty("Where else to enable it?", "whereElseToEnableIt", BuiltInUmbracoDataTypes.TextboxMultiple, description: "Paste any target page URLs, one per line. This is useful for targeting external applications like the online library catalogue.", sortOrder: 2)]
        public string WhereElseToDisplayIt { get; set; }
    }
}
