using System;
using System.Collections.Generic;
using System.Linq;
using Escc.Umbraco.PropertyTypes;
using Umbraco.Core.Models;
using Umbraco.Web;

namespace Escc.Umbraco.HeatmapAnalytics.ApiControllers
{
    /// <summary>
    /// Reads settings for heatmap analytics from an Umbraco page
    /// </summary>
    public class HeatmapAnalyticsSettingsService : IHeatmapAnalyticsSettingsService
    {
        private readonly IUrlListReader _targetUrlReader;

        /// <summary>
        /// Initializes a new instance of the <see cref="HeatmapAnalyticsSettingsService"/> class.
        /// </summary>
        /// <param name="targetUrlReader">The URL list reader.</param>
        public HeatmapAnalyticsSettingsService(IUrlListReader targetUrlReader)
        {
            if (targetUrlReader==null) throw new ArgumentNullException("targetUrlReader");
            _targetUrlReader = targetUrlReader;
        }

        /// <summary>
        /// Reads the heatmap analytics settings from fields on an <see cref="IPublishedContent"/> using the <c>Heatmap analytics</c> document type from <c>Escc.Umbraco.HeatmapAnalytics</c>.
        /// </summary>
        /// <param name="content">The content.</param>
        /// <returns></returns>
        /// <exception cref="System.ArgumentNullException">content</exception>
        public HeatmapAnalyticsSettings ReadHeatmapAnalyticsSettings(IPublishedContent content)
        {
            if (content == null) throw new ArgumentNullException("content");

            var model = new HeatmapAnalyticsSettings();

            var heatmapSettingsPage = content.AncestorOrSelf(1).Siblings().FirstOrDefault(sibling => sibling.DocumentTypeAlias == "HeatmapAnalytics");
            if (heatmapSettingsPage == null) return model;

            ((List<Uri>)model.HeatmapAnalyticsUrls).AddRange(_targetUrlReader.ReadUrls(heatmapSettingsPage, "whereToEnableIt_Content", "whereElseToEnableIt_Content"));
            return model;
        }
    }
}