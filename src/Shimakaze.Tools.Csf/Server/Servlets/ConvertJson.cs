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
        var csf = await CsfJsonTools.LoadAsync(request.InputStream, jsonVersion);
        CsfBinaryTools.Write(outputStream, csf);
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
        var csf = await CsfJsonTools.LoadAsync(request.InputStream, jsonVersion);
        CsfXmlTools.Write(outputStream, csf, xmlVersion);
    }
}
