using Escc.EastSussexGovUK.UmbracoDocumentTypes.RichTextPropertyEditor;
using Escc.Umbraco.PropertyEditors;
using Umbraco.Inception.Attributes;
using Umbraco.Inception.BL;

namespace Escc.Umbraco.HeatmapAnalytics.DocumentTypes
{
    /// <summary>
    /// A specification for the 'Heatmap analytics' document type in Umbraco
    /// </summary>
    [UmbracoContentType("Heatmap analytics", "HeatmapAnalytics", null, false, BuiltInUmbracoContentTypeIcons.IconFire, allowAtRoot: true,
        Description = "Manage settings for a heatmap analytics service")]
    public class HeatmapAnalyticsDocumentType : UmbracoGeneratedBase
    {
        [UmbracoTab("Content")]
        public HeatmapAnalyticsContentTab Content { get; set; }

        [UmbracoProperty("Author notes", "authorNotes", PropertyEditorAliases.RichTextPropertyEditor, RichTextAuthorNotesDataType.DataTypeName, sortOrder: 2)]
        public string AuthorNotes { get; set; }
    }
}
