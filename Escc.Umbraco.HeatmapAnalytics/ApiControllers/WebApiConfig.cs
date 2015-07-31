using System;
using System.Net.Http.Headers;
using System.Web.Http;
using Exceptionless;
using Umbraco.Core;

namespace Escc.Umbraco.HeatmapAnalytics.ApiControllers
{
    /// <summary>
    /// Configure the default behaviours of Web APIs
    /// </summary>
    public class WebApiConfig : ApplicationEventHandler
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="WebApiConfig"/> class.
        /// </summary>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1031:DoNotCatchGeneralExceptionTypes")]
        public WebApiConfig()
        {
            try
            {
                // When a browser requests the API, return JSON by default
                GlobalConfiguration.Configuration.Formatters.JsonFormatter.SupportedMediaTypes.Add(new MediaTypeHeaderValue("text/html"));
            }
            catch (Exception e)
            {
                e.ToExceptionless().Submit();
            }
        }
    }
}