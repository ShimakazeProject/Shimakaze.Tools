using System.Net;

using Shimakaze.Tools.InternalUtils;

namespace Shimakaze.Tools.Csf.Server.WebSystem;

internal sealed class WebServer : IDisposable
{
    private readonly HttpListener _listener = new();
    private readonly Dictionary<string, ServletInfo> _servlets = new();
    private bool _disposedValue;

    public WebServer(string host, ushort port, bool useSsl = false)
    {
        Initialize_Servlets();

        _listener.AuthenticationSchemes = AuthenticationSchemes.Anonymous;
        _listener.Prefixes.Add($"http{(useSsl ? "s" : string.Empty)}://{host}:{port}/");
    }

    void Initialize_Servlets() => typeof(Program).Assembly
            .GetExportedTypes()
            .Where(t => t.IsAssignableTo(typeof(ServletBase)))
            .Where(t => t.CustomAttributes.Any(attr => attr.AttributeType == typeof(ServletAttribute)))
            .Select(t => (Servlet: (ServletBase)Activator.CreateInstance(t, true)!, Attribute: (ServletAttribute)t.GetCustomAttributes(typeof(ServletAttribute), false)[0]))
            .Each(i => _servlets.Add(i.Attribute.FindServlet().Path, new(i)));

    public async Task Run()
    {
        try
        {
            _listener.Start();
            Logger.WriteLine($"[Server] Start");
            _listener.Prefixes.Each(i => Logger.WriteLine($"[Server] Listening at \"{i}\""));
            while (true)
                await Process();
        }
        catch (Exception e)
        {
            Logger.WriteLine("An Exception has been throw.");
            Logger.WriteLine(e.ToString());
            throw;
        }
    }

    private async Task Process()
    {
        HttpListenerContext context = await _listener.GetContextAsync();
        HttpListenerRequest request = context.Request;
        HttpListenerResponse response = context.Response;
        string? path = request.Url?.AbsolutePath;

        if (string.IsNullOrEmpty(path))
        {
            Logger.WriteLine($"[Server] Unknown Uri {request.HttpMethod}: \"{request.Url}\"");
            response.StatusCode = 400;
            response.StatusDescription = "Oops! We couldn't handle this request!";
            response.Close();
            return;
        }
        if (_servlets.TryGetValue(path, out var servletInfo))
        {
            Logger.WriteLine($"[Server] Success {request.HttpMethod}: \"{request.Url}\"");
            Thread thread = new(() => servletInfo.Invoke(request, response));
            thread.Name = servletInfo.Attribute.Name;
            thread.Start();
        }
        else
        {
            Logger.WriteLine($"[Server] 404 Not Found {request.HttpMethod}: \"{request.Url}\"");
            response.StatusCode = 404;
            response.StatusDescription = "Oops! We couldn't find a Servlet to handle this request!";
            response.Close();
        }
    }

    private void Dispose(bool disposing)
    {
        if (!_disposedValue)
        {
            if (disposing)
            {
                (_listener as IDisposable)?.Dispose();
            }

            _disposedValue = true;
        }
    }

    // ~HttpServer()
    // {
    //     Dispose(disposing: false);
    // }

    public void Dispose()
    {
        Dispose(disposing: true);
        GC.SuppressFinalize(this);
    }
}

internal static class LoggerExtensions
{
    public static ServletAttribute FindServlet(this ServletAttribute servletAttribute)
    {
        Logger.WriteLine($"[Initialize] Find Servlet: \"{servletAttribute.Name}\" Path: \"{servletAttribute.Path}\"");
        return servletAttribute;
    }
}