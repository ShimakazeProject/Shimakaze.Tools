using System.Net;

namespace Shimakaze.WebServer;

/// <summary>
/// Servlet Interface
/// </summary>
public interface IServlet : IDisposable
{
    /// <summary>
    /// Initialized Servlet<br/>
    /// Invoked when Servlet has been Created.
    /// </summary>
    void Init();

    /// <summary>
    /// 
    /// </summary>
    /// <param name="request">Http Request</param>
    /// <param name="response">Http Response</param>
    void Service(HttpListenerRequest request, HttpListenerResponse response);
}
