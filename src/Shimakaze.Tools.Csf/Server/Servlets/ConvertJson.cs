using System.Net;

using Shimakaze.Tools.Csf.Cli;
using Shimakaze.Tools.Csf.Server.WebSystem;

namespace Shimakaze.Tools.Csf.Server.Servlets;

/// <summary>
/// Convert Json To Csf
/// </summary>
[Servlet("/convert/json/csf", "Convert Json To Csf")]
public class ConvertJsonToCsfServlet : ConvertServletBase
{
    /// <inheritdoc/>
    protected override async Task ConvertAsync(Dictionary<string, string> queries, Stream outputStream, HttpListenerRequest request, HttpListenerResponse response)
    {
        var csf = await CsfJsonTools.LoadAsync(request.InputStream);
        CsfBinaryTools.Write(outputStream, csf);
    }
}

/// <summary>
/// Convert Json To Xml
/// </summary>
[Servlet("/convert/json/xml", "Convert Json To Xml")]
public class ConvertJsonToXmlServlet : ConvertServletBase
{
    /// <inheritdoc/>
    protected override async Task ConvertAsync(Dictionary<string, string> queries, Stream outputStream, HttpListenerRequest request, HttpListenerResponse response)
    {
        int version = queries.GetValueOrDefault("version").GetInt32(1);
        var csf = await CsfJsonTools.LoadAsync(request.InputStream);
        CsfXmlTools.Write(outputStream, csf, version);
    }
}
