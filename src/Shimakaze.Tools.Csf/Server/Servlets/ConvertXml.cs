using System.Net;

using Shimakaze.Tools.Csf.Cli;
using Shimakaze.Tools.Csf.Server.WebSystem;

namespace Shimakaze.Tools.Csf.Server.Servlets;

/// <summary>
/// Convert Xml To Csf
/// </summary>
[Servlet("/convert/xml/csf", "Convert Xml To Csf")]
public class ConvertXmlToCsfServlet : ConvertServletBase
{
    /// <inheritdoc/>
#pragma warning disable CS1998 // 异步方法缺少 "await" 运算符，将以同步方式运行
    protected override async Task ConvertAsync(Dictionary<string, string> queries, Stream outputStream, HttpListenerRequest request, HttpListenerResponse response)
    {
        var csf = CsfXmlTools.Load(request.InputStream);
        CsfBinaryTools.Write(outputStream, csf);
    }
#pragma warning restore CS1998 // 异步方法缺少 "await" 运算符，将以同步方式运行
}

/// <summary>
/// Convert Xml To Xml
/// </summary>
[Servlet("/convert/xml/json", "Convert Xml To Json")]
public class ConvertXmlToJsonServlet : ConvertServletBase
{
    /// <inheritdoc/>
    protected override async Task ConvertAsync(Dictionary<string, string> queries, Stream outputStream, HttpListenerRequest request, HttpListenerResponse response)
    {
        int version = queries.GetValueOrDefault("version").GetInt32(2);
        bool format = queries.GetValueOrDefault("format").GetBoolean(false);
        var csf = CsfXmlTools.Load(request.InputStream);
        await CsfJsonTools.WriteAsync(outputStream, csf, version, format);
    }
}
