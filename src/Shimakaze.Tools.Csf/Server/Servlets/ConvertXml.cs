using System.Net;

using Shimakaze.Tools.Csf.Common;
using Shimakaze.WebServer;

namespace Shimakaze.Tools.Csf.Server.Servlets;

/// <summary>
/// Convert Xml To Csf
/// </summary>
[WebServlet("/convert/xml/csf", "Convert Xml To Csf")]
public class ConvertXmlToCsfServlet : ConvertServlet
{
    /// <inheritdoc/>
    protected override async Task ConvertAsync(Dictionary<string, string> queries, Stream outputStream, HttpListenerRequest request, HttpListenerResponse response)
    {
        int xmlVersion = queries.GetValueOrDefault(CONSTANTS.QUERY_XML_VERSION).GetInt32(CONSTANTS.XML_VERSION);
        var csf = CsfXmlTools.Load(request.InputStream, xmlVersion);
        await CsfBinaryTools.WriteAsync(outputStream, csf).ConfigureAwait(false);
    }
}

/// <summary>
/// Convert Xml To Xml
/// </summary>
[WebServlet("/convert/xml/json", "Convert Xml To Json")]
public class ConvertXmlToJsonServlet : ConvertServlet
{
    /// <inheritdoc/>
    protected override async Task ConvertAsync(Dictionary<string, string> queries, Stream outputStream, HttpListenerRequest request, HttpListenerResponse response)
    {
        int jsonVersion = queries.GetValueOrDefault(CONSTANTS.QUERY_JSON_VERSION).GetInt32(CONSTANTS.JSON_VERSION);
        int xmlVersion = queries.GetValueOrDefault(CONSTANTS.QUERY_XML_VERSION).GetInt32(CONSTANTS.XML_VERSION);
        bool format = queries.GetValueOrDefault(CONSTANTS.QUERY_FORMAT).GetBoolean(false);
        var csf = CsfXmlTools.Load(request.InputStream, xmlVersion);
        await CsfJsonTools.WriteAsync(outputStream, csf, jsonVersion, format).ConfigureAwait(false);
    }
}
