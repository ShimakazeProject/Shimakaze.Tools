using System.Net;


namespace Shimakaze.Tools.Csf.Server.WebSystem;

/// <summary>
/// Servlet Base
/// </summary>
public abstract class ServletBase : IWebRequestBase
{
    /// <summary>
    /// Servlet
    /// </summary>
    protected ServletBase() : base()
    {
    }

    /// <inheritdoc/>
    public virtual void OnRequest(HttpListenerRequest request, HttpListenerResponse response)
    {
        (request.HttpMethod switch
        {
            "POST" => OnPost,
            "GET" => OnGet,
            _ => (ServletDelegate)OnOther
        })(request, response);
    }
    /// <summary>
    /// On GET Request
    /// </summary>
    /// <param name="request">Web Request</param>
    /// <param name="response">Web Response</param>
    protected virtual void OnGet(HttpListenerRequest request, HttpListenerResponse response)
    { }
    /// <summary>
    /// On POST Request
    /// </summary>
    /// <param name="request">Web Request</param>
    /// <param name="response">Web Response</param>
    protected virtual void OnPost(HttpListenerRequest request, HttpListenerResponse response)
    { }
    /// <summary>
    /// On Other Request
    /// </summary>
    /// <param name="request">Web Request</param>
    /// <param name="response">Web Response</param>
    protected virtual void OnOther(HttpListenerRequest request, HttpListenerResponse response)
    { }
}