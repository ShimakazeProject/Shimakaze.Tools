using System.Net;

using Shimakaze.Tools.Csf.Server.WebSystem;

namespace Shimakaze.Tools.Csf.Server.Servlets;

/// <summary>
/// Convert Base
/// </summary>
public abstract class ConvertServletBase : ServletBase
{
    /// <inheritdoc/>
    protected override async void OnPost(HttpListenerRequest request, HttpListenerResponse response)
    {
        var queries = request.GetQueries();

        using MemoryStream stream = new();
        await ConvertAsync(queries, stream, request, response);
        stream.Seek(0, SeekOrigin.Begin);
        response.ContentLength64 = stream.Length;
        response.ContentType = "application/json";
        await stream.CopyToAsync(response.OutputStream);
        response.Close();

    }
    /// <summary>
    /// Convert
    /// </summary>
    /// <param name="queries"></param>
    /// <param name="request"></param>
    /// <param name="response"></param>
    /// <returns></returns>
    protected abstract Task ConvertAsync(Dictionary<string, string> queries,Stream outputStream, HttpListenerRequest request, HttpListenerResponse response);

    /// <inheritdoc/>
    protected override void OnGet(HttpListenerRequest request, HttpListenerResponse response)
    {
        response.StatusCode = 400;
        response.StatusDescription = "GET Method cannot handle this request.";
        response.Close();
    }

}
