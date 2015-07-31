using System;
using System.Collections.Generic;

namespace Escc.Umbraco.HeatmapAnalytics.ApiControllers
{
    /// <summary>
    /// Configuration settings for using heatmap analytics
    /// </summary>
    public class HeatmapAnalyticsSettings
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="HeatmapAnalyticsSettings"/> class.
        /// </summary>
        public HeatmapAnalyticsSettings()
        {
            HeatmapAnalyticsUrls = new List<Uri>();
        }

        /// <summary>
        /// Gets the URLs where heatmap analytics should be enabled.
        /// </summary>
        /// <value>
        /// The heatmap analytics URLs.
        /// </value>
        public IList<Uri> HeatmapAnalyticsUrls { get; private set; } 
    }
}