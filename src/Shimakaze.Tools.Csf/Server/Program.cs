using Shimakaze.Tools.Csf.Server.WebSystem;

namespace Shimakaze.Tools.Csf.Server;

static class Program
{
    /// <summary>
    /// Start Language Server
    /// </summary>
    /// <param name="host">Listen Host</param>
    /// <param name="port">Http Port</param>
    /// <param name="ssl">Use SSL/TLS</param>
    static async Task Main(string host = "localhost", ushort port = 45082, bool ssl = false) => await new WebServer(host, port, ssl).Run();
}
