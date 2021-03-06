using System.Net;

using Shimakaze.Tools.Csf.Common;
using Shimakaze.WebServer;

namespace Shimakaze.Tools.Csf.Server.Servlets;

/// <summary>
/// Convert Json To Csf
/// </summary>
[WebServlet("/convert/json/csf", "Convert Json To Csf")]
public class ConvertJsonToCsfServlet : ConvertServlet
{
    /// <inheritdoc/>
    protected override async Task ConvertAsync(Dictionary<string, string> queries, Stream outputStream, HttpListenerRequest request, HttpListenerResponse response)
    {
        int jsonVersion = queries.GetValueOrDefault(CONSTANTS.QUERY_JSON_VERSION).GetInt32(CONSTANTS.JSON_VERSION);
        var csf = await CsfJsonTools.LoadAsync(request.InputStream, jsonVersion).ConfigureAwait(false);
        await CsfBinaryTools.WriteAsync(outputStream, csf).ConfigureAwait(false);
    }
}

/// <summary>
/// Convert Json To Json
/// </summary>
[WebServlet("/convert/json/json", "Convert Json To Csf")]
public class ConvertJsonToJsonServlet : ConvertServlet
{
    /// <inheritdoc/>
    protected override async Task ConvertAsync(Dictionary<string, string> queries, Stream outputStream, HttpListenerRequest request, HttpListenerResponse response)
    {
        int jsonVersion = queries.GetValueOrDefault(CONSTANTS.QUERY_JSON_VERSION).GetInt32(CONSTANTS.JSON_VERSION);
        int newVersion = queries.GetValueOrDefault(CONSTANTS.QUERY_NEW_VERSION).GetInt32(CONSTANTS.JSON_VERSION);
        bool format = queries.GetValueOrDefault(CONSTANTS.QUERY_FORMAT).GetBoolean(false);
        var csf = await CsfJsonTools.LoadAsync(request.InputStream, jsonVersion).ConfigureAwait(false);
        await CsfJsonTools.WriteAsync(outputStream, csf, newVersion, format).ConfigureAwait(false);
    }
}

/// <summary>
/// Convert Json To Xml
/// </summary>
[WebServlet("/convert/json/xml", "Convert Json To Xml")]
public class ConvertJsonToXmlServlet : ConvertServlet
{
    /// <inheritdoc/>
    protected override async Task ConvertAsync(Dictionary<string, string> queries, Stream outputStream, HttpListenerRequest request, HttpListenerResponse response)
    {
        int jsonVersion = queries.GetValueOrDefault(CONSTANTS.QUERY_JSON_VERSION).GetInt32(CONSTANTS.JSON_VERSION);
        int xmlVersion = queries.GetValueOrDefault(CONSTANTS.QUERY_XML_VERSION).GetInt32(CONSTANTS.XML_VERSION);
        bool format = queries.GetValueOrDefault(CONSTANTS.QUERY_FORMAT).GetBoolean(false);
        var csf = await CsfJsonTools.LoadAsync(request.InputStream, jsonVersion).ConfigureAwait(false);
        await CsfXamlTools.WriteAsync(outputStream, csf, xmlVersion, format).ConfigureAwait(false);
    }
}
