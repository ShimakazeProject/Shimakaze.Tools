using System;
using System.Net;

using Shimakaze.Tools.Csf.Server.WebSystem;

namespace Shimakaze.Tools.Csf.Server;


static class Program
{
    /// <summary>
    /// Start Language Server
    /// </summary>
    /// <param name="host"></param>
    /// <param name="port">Http Port</param>
    /// <param name="tlsport">Https Port</param>
    static async Task Main(string host = "localhost", ushort port = 45082, bool useSsl = false) => await new WebServer(host, port, useSsl).Run();

}
public static class RequestExtensions
{
    public static Dictionary<string, string> GetQueries(this HttpListenerRequest request)
    {
        if (string.IsNullOrWhiteSpace(request.Url?.Query))
            return new();
        return request.Url!.Query
                   .Split('&')
                   .Select(x => x.Split('='))
                   .ToDictionary(x => x[0], x => x[1]);
    }
}
public static class StringExtensins
{
    public static int GetInt32(this string? @this, int @default = default) => int.TryParse(@this, out var result) ? result : @default;
    public static bool GetBoolean(this string? @this, bool @default = default) => bool.TryParse(@this, out var result) ? result : @default;
}