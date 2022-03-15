using System.Net;

using Shimakaze.WebServer;
using Shimakaze.WebServer.Locales;

namespace Shimakaze.Tools.Csf.Server.Servlets;

/// <summary>
/// Convert Base
/// </summary>
public abstract class ConvertServlet : HttpServlet
{
    /// <inheritdoc/>
    protected override async void OnPost(HttpListenerRequest request, HttpListenerResponse response)
    {
        try
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
        catch (Exception ex)
        {
            try
            {
                Logger.Warn(nameof(ConvertServlet), Locale.Log400);
                Logger.Warn(nameof(ConvertServlet), ex.ToString());
                response.StatusCode = 400;
                response.StatusDescription = Locale.Err400;
                using StreamWriter sw = new(response.OutputStream);
                sw.Write(ex.ToString());
                response.Close();
            }
            catch { }
        }
    }
    /// <summary>
    /// Convert
    /// </summary>
    /// <param name="queries"></param>
    /// <param name="outputStream"></param>
    /// <param name="request"></param>
    /// <param name="response"></param>
    /// <returns></returns>
    protected abstract Task ConvertAsync(Dictionary<string, string> queries, Stream outputStream, HttpListenerRequest request, HttpListenerResponse response);

    /// <inheritdoc/>
    protected override void OnGet(HttpListenerRequest request, HttpListenerResponse response)
    {
        using StreamWriter sw = new(response.OutputStream);
        sw.Write("GET Method cannot handle this request.");
        response.StatusCode = 400;
        response.StatusDescription = "GET Method cannot handle this request.";
        response.Close();
    }

}
