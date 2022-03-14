namespace Shimakaze.Tools.Csf.Server;

/// <summary>
/// String Extensions
/// </summary>
public static class StringExtensins
{
    /// <summary>
    /// Get Int32
    /// </summary>
    /// <param name="this"></param>
    /// <param name="default"></param>
    /// <returns></returns>
    public static int GetInt32(this string? @this, int @default = default) => int.TryParse(@this, out var result) ? result : @default;
    /// <summary>
    /// Get Boolean
    /// </summary>
    /// <param name="this"></param>
    /// <param name="default"></param>
    /// <returns></returns>
    public static bool GetBoolean(this string? @this, bool @default = default) => bool.TryParse(@this, out var result) ? result : @default;
}