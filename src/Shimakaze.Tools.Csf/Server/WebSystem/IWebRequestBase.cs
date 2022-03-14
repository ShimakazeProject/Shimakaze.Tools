// See https://aka.ms/new-console-template for more information
using System.Net;


namespace Shimakaze.Tools.Csf.Server.WebSystem;

/// <summary>
/// Http Request Parser
/// </summary>
public interface IWebRequestBase
{
    /// <summary>
    /// On Request
    /// </summary>
    /// <param name="request">Web Request</param>
    /// <param name="response">Web Response</param>
    void OnRequest(HttpListenerRequest request, HttpListenerResponse response);
}
