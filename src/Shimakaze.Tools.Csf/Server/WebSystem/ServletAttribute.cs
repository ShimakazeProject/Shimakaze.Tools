// See https://aka.ms/new-console-template for more information

namespace Shimakaze.Tools.Csf.Server.WebSystem;

/// <summary>
/// Servlet Attribute
/// </summary>
[AttributeUsage(AttributeTargets.Class, Inherited = false, AllowMultiple = false)]
public sealed class ServletAttribute : Attribute
{
    /// <summary>
    /// Servlet Attribute
    /// </summary>
    /// <param name="path"></param>
    /// <param name="name">Servlet Name</param>
    public ServletAttribute(string path, string name = "<Untitled Servlet>")
    {
        Path = path;
        Name = name;
    }

    /// <summary>
    /// Servlet Name
    /// </summary>
    public string Name { get; }
    /// <summary>
    /// Request Path
    /// </summary>
    public string Path { get; }
}
