namespace Shimakaze.Tools.Csf.Server;

/// <summary>
/// Logger
/// </summary>
public static class Logger
{
    /// <summary>
    /// Write a Log
    /// </summary>
    /// <param name="message">Message</param>
    public static void WriteLine(string message) => Console.WriteLine($"[\x1b[38;2;0;0;255m{DateTime.Now:O}\x1b[0m]   {message}");

    /// <summary>
    /// Write a Log
    /// </summary>
    /// <param name="format">format</param>
    /// <param name="args">arguments</param>
    public static void WriteLine(string format, params string[] args) => WriteLine(string.Format(format, args));
}
