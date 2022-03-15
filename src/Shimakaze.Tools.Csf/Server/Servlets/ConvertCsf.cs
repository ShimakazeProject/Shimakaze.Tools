using System.Net;

using Shimakaze.Tools.Csf.Common;
using Shimakaze.WebServer;

namespace Shimakaze.Tools.Csf.Server.Servlets;

/// <summary>
/// Convert Csf To Json
/// </summary>
[WebServlet("/convert/csf/json", "Convert Csf To Json")]
public class ConvertCsfToJsonServlet : ConvertServlet
{
    /// <inheritdoc/>
    protected override async Task ConvertAsync(Dictionary<string, string> queries, Stream outputStream, HttpListenerRequest request, HttpListenerResponse response)
    {
        int jsonVersion = queries.GetValueOrDefault(CONSTANTS.QUERY_JSON_VERSION).GetInt32(CONSTANTS.JSON_VERSION);
        bool format = queries.GetValueOrDefault(CONSTANTS.QUERY_FORMAT).GetBoolean(false);
        var csf = CsfBinaryTools.Load(request.InputStream);
        await CsfJsonTools.WriteAsync(outputStream, csf, jsonVersion, format).ConfigureAwait(false);
    }
}
/// <summary>
/// Convert Csf To Xml
/// </summary>
[WebServlet("/convert/csf/xml", "Convert Csf To Xml")]
public class ConvertCsfToXmlServlet : ConvertServlet
{
    /// <inheritdoc/>
    protected override async Task ConvertAsync(Dictionary<string, string> queries, Stream outputStream, HttpListenerRequest request, HttpListenerResponse response)
    {
        int xmlVersion = queries.GetValueOrDefault(CONSTANTS.QUERY_XML_VERSION).GetInt32(CONSTANTS.XML_VERSION);
        bool format = queries.GetValueOrDefault(CONSTANTS.QUERY_FORMAT).GetBoolean(false);
        var csf = CsfBinaryTools.Load(request.InputStream);
        await CsfXmlTools.WriteAsync(outputStream, csf, xmlVersion, format).ConfigureAwait(false);
    }
}
