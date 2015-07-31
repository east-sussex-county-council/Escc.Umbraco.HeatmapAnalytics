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
using Escc.EastSussexGovUK.UmbracoDocumentTypes.Css;
using Escc.EastSussexGovUK.UmbracoDocumentTypes.DataTypes;
using Escc.EastSussexGovUK.UmbracoDocumentTypes.RichTextPropertyEditor;
using Escc.Umbraco.HeatmapAnalytics.DocumentTypes;
using Escc.Umbraco.PropertyEditors.Stylesheets;
using Escc.Umbraco.PropertyTypes;
using Exceptionless;
using ExCSS;
using Umbraco.Inception.CodeFirst;
using Umbraco.Web.WebApi;

namespace Escc.Umbraco.HeatmapAnalytics.ApiControllers
{
    /// <summary>
    /// Create Umbraco document types needed for this project
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

                EnableCorsSupport(HttpContext.Current.Request, response);

                return response;
            }
            catch (Exception e)
            {
                e.ToExceptionless().Submit();
                return Request.CreateResponse(HttpStatusCode.InternalServerError);
            }
        }

        /// <summary>
        /// Enables CORS support.
        /// </summary>
        /// <remarks>
        /// This code is a temporary copy until Escc.Data.Web is moved to NuGet and improved to remove dependencies on WebForms and Escc.EastSussexGovUK
        /// </remarks>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Globalization", "CA1308:NormalizeStringsToUppercase")]
        private static void EnableCorsSupport(HttpRequest request, HttpResponseMessage response)
        {
            // Load config from elsewhere
            var config = ConfigurationManager.GetSection("EsccWebTeam.EastSussexGovUK/RemoteMasterPage") as NameValueCollection;
            if (config == null || String.IsNullOrEmpty(config["CorsAllowedOrigins"])) return;
            var allowedOrigins = new List<string>(config["CorsAllowedOrigins"].Split(new[] { ";" }, StringSplitOptions.RemoveEmptyEntries));
            
            // Not a CORS request - do nothing
            var requestOrigin = request.Headers["Origin"];
            if (String.IsNullOrEmpty(requestOrigin)) return;

            // Is the origin in the list of allowed origins?
            var allowedOrigin = new List<string>(allowedOrigins).Contains(requestOrigin.ToLowerInvariant());

            // If it is, echo back the origin as a CORS header
            if (allowedOrigin)
            {
                response.Content.Headers.Add("Access-Control-Allow-Origin", requestOrigin);
            }
        }



        /// <summary>
        /// Checks the authorisation token passed with the request is valid, so that this method cannot be called without knowing the token.
        /// </summary>
        /// <param name="token">The token.</param>
        /// <returns></returns>
        private static bool CheckAuthorisationToken(string token)
        {
            return token == ConfigurationManager.AppSettings["Escc.Umbraco.Inception.AuthToken"];
        } 
        
        /// <summary>
        /// Creates the supporting types (eg data types) needed for <see cref="CreateUmbracoDocumentTypes"/> to succeed.
        /// </summary>
        /// <returns></returns>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Maintainability", "CA1506:AvoidExcessiveClassCoupling"), System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1031:DoNotCatchGeneralExceptionTypes")]
        [AcceptVerbs("POST")]
        public HttpResponseMessage CreateUmbracoSupportingTypes([FromUri] string token)
        {
            if (!CheckAuthorisationToken(token)) return Request.CreateResponse(HttpStatusCode.Forbidden);

            try
            {
                // Create stylesheets for properties using rich text editor
                TinyMceStylesheets.CreateStylesheets(new StylesheetService(), new Parser());

                // Insert data types before the document types that use them, otherwise the relevant property is not created
                MultiNodeTreePickerDataType.CreateDataType();
                RichTextAuthorNotesDataType.CreateDataType();

                return Request.CreateResponse(HttpStatusCode.Created);
            }
            catch (Exception e)
            {
                e.ToExceptionless().Submit();
                return Request.CreateResponse(HttpStatusCode.InternalServerError);
            }
        }


        /// <summary>
        /// Creates the Umbraco document types defined by this project.
        /// </summary>
        /// <returns></returns>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1031:DoNotCatchGeneralExceptionTypes")]
        [AcceptVerbs("POST")]
        public HttpResponseMessage CreateUmbracoDocumentTypes([FromUri] string token)
        {
            if (!CheckAuthorisationToken(token)) return Request.CreateResponse(HttpStatusCode.Forbidden);

            try
            {
                UmbracoCodeFirstInitializer.CreateOrUpdateEntity(typeof(HeatmapAnalyticsDocumentType));
 
                return Request.CreateResponse(HttpStatusCode.Created);
            }
            catch (Exception e)
            {
                e.ToExceptionless().Submit();
                return Request.CreateResponse(HttpStatusCode.InternalServerError);
            }
        }
    }
}
