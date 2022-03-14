using System.Net;

namespace Shimakaze.Tools.Csf.Server.WebSystem;


/// <summary>
/// Servlet Delegate
/// </summary>
/// <param name="request">Web Request</param>
/// <param name="response">Web Response</param>
public delegate void ServletDelegate(HttpListenerRequest request, HttpListenerResponse response);

/// <summary>
/// Servlet Info
/// </summary>
/// <param name="Attribute">Attribute</param>
/// <param name="Servlet">Servlet</param>
public sealed record ServletInfo(ServletAttribute Attribute, ServletBase Servlet)
{
    internal ServletInfo((ServletBase Servlet, ServletAttribute Attribute) tuple) : this(tuple.Attribute, tuple.Servlet)
    { }

    internal void Invoke(HttpListenerRequest request, HttpListenerResponse response) => Servlet.OnRequest(request, response);
}