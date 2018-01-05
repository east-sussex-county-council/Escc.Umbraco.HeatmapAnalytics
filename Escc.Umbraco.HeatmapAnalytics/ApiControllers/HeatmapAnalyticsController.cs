using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Configuration;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Web;
using System.Web.Http;
using Escc.Umbraco.PropertyTypes;
using Escc.Web;
using Exceptionless;
using Umbraco.Web.WebApi;

namespace Escc.Umbraco.HeatmapAnalytics.ApiControllers
{
    /// <summary>
    /// Publish the list of pages configured to use heatmap analytics
    /// </summary>
    public class HeatmapAnalyticsController : UmbracoApiController
    {
        /// <summary>
        /// Gets the urls where heatmap analytics should be enabled.
        /// </summary>
        /// <returns></returns>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Reliability", "CA2000:Dispose objects before losing scope"), System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1031:DoNotCatchGeneralExceptionTypes"), System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1024:UsePropertiesWhereAppropriate")]
        public HttpResponseMessage GetHeatmapAnalyticsUrls()
        {
            try
            {
                var service = new HeatmapAnalyticsSettingsService(new UrlListReader());
                var settings = service.ReadHeatmapAnalyticsSettings(UmbracoContext.ContentCache.GetAtRoot().FirstOrDefault());

                var response = Request.CreateResponse(HttpStatusCode.OK, settings);
                response.Headers.CacheControl = new CacheControlHeaderValue()
                {
                    Public = true,
                    MaxAge = TimeSpan.FromDays(1)
                };
                response.Content.Headers.Expires = DateTimeOffset.Now.Add(response.Headers.CacheControl.MaxAge.Value);

                var corsPolicy = new CorsPolicyFromConfig().CorsPolicy;
                new CorsHeaders(Request.Headers, response.Headers, corsPolicy).UpdateHeaders();

                return response;
            }
            catch (Exception e)
            {
                e.ToExceptionless().Submit();
                return Request.CreateResponse(HttpStatusCode.InternalServerError);
            }
        }
    }
}
