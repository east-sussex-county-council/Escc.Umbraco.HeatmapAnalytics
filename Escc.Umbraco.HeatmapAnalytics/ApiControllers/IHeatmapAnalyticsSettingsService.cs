using Umbraco.Core.Models;

namespace Escc.Umbraco.HeatmapAnalytics.ApiControllers
{
    public interface IHeatmapAnalyticsSettingsService
    {
        HeatmapAnalyticsSettings ReadHeatmapAnalyticsSettings(IPublishedContent content);
    }
}