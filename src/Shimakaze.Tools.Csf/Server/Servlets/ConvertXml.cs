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
#pragma warning disable CS1998 // 异步方法缺少 "await" 运算符，将以同步方式运行
    protected override async Task ConvertAsync(Dictionary<string, string> queries, Stream outputStream, HttpListenerRequest request, HttpListenerResponse response)
    {
        int xmlVersion = queries.GetValueOrDefault(CONSTANTS.QUERY_XML_VERSION).GetInt32(CONSTANTS.XML_VERSION);
        var csf = CsfXmlTools.Load(request.InputStream, xmlVersion);
        CsfBinaryTools.Write(outputStream, csf);
    }
#pragma warning restore CS1998 // 异步方法缺少 "await" 运算符，将以同步方式运行
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
        await CsfJsonTools.WriteAsync(outputStream, csf, jsonVersion, format);
    }
}
