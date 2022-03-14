using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

using Shimakaze.Tools.Csf.Cli;
using Shimakaze.Tools.Csf.Server.WebSystem;

namespace Shimakaze.Tools.Csf.Server.Servlets;

/// <summary>
/// Convert Csf To Json
/// </summary>
[Servlet("/convert/csf/json", "Convert Csf To Json")]
public class ConvertCsfToJsonServlet : ConvertServletBase
{
    /// <inheritdoc/>
    protected override async Task ConvertAsync(Dictionary<string, string> queries, Stream outputStream, HttpListenerRequest request, HttpListenerResponse response)
    {
        int version = queries.GetValueOrDefault("version").GetInt32(2);
        bool format = queries.GetValueOrDefault("format").GetBoolean(false);
        var csf = CsfBinaryTools.Load(request.InputStream);
        await CsfJsonTools.WriteAsync(outputStream, csf, version, format);
    }
}
/// <summary>
/// Convert Csf To Xml
/// </summary>
[Servlet("/convert/csf/xml", "Convert Csf To Xml")]
public class ConvertCsfToXmlServlet : ConvertServletBase
{
    /// <inheritdoc/>
#pragma warning disable CS1998 // 异步方法缺少 "await" 运算符，将以同步方式运行
    protected override async Task ConvertAsync(Dictionary<string, string> queries, Stream outputStream, HttpListenerRequest request, HttpListenerResponse response)
    {
        int version = queries.GetValueOrDefault("version").GetInt32(1);
        var csf = CsfBinaryTools.Load(request.InputStream);
        CsfXmlTools.Write(outputStream, csf, version);
    }
#pragma warning restore CS1998 // 异步方法缺少 "await" 运算符，将以同步方式运行
}
