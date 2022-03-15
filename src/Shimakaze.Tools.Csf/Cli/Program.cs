
using Shimakaze.Models.Csf;
using Shimakaze.Tools.Csf.Common;

namespace Shimakaze.Tools.Csf.Cli;

internal static class CONSTANTS
{
    public const int JSON_VERSION = 2;
    public const int XML_VERSION = 2;
}
/// <summary>
/// Support Format
/// </summary>
public enum SupportFormat
{
    /// <summary>
    /// Binary
    /// </summary>
    CSF,
    /// <summary>
    /// Json
    /// </summary>
    JSON,
    /// <summary>
    /// Xaml
    /// </summary>
    XAML,
}

/// <summary>
/// Csf Builder
/// </summary>
public static class Program
{
    /// <summary>
    /// Csf Builder
    /// </summary>
    /// <param name="input">Input File</param>
    /// <param name="output">Output File</param>
    /// <param name="jsonVersion">Json Version</param>
    /// <param name="xamlVersion">Xaml Version</param>
    /// <param name="newVersion">New Version</param>
    /// <param name="reverse">Reverse Build</param>
    /// <param name="format">Format Output</param>
    /// <param name="upgrade">Use New Version</param>
    /// <param name="inputType">Input Type</param>
    /// <param name="outputType">Output Type</param>
    /// <returns></returns>
    /// <exception cref="NotSupportedException"></exception>
    public static async Task Main(
        FileInfo input,
        FileInfo output,
        int jsonVersion = CONSTANTS.JSON_VERSION,
        int xamlVersion = CONSTANTS.XML_VERSION,
        int newVersion = 0,
        bool reverse = false,
        bool format = true,
        bool upgrade = true,
        SupportFormat inputType = SupportFormat.JSON,
        SupportFormat outputType = SupportFormat.CSF
        )
    {
        await using Stream istream = (reverse ? output : input).OpenRead();
        await using Stream ostream = (reverse ? input : output).Create();
        CsfStruct csf = (reverse ? outputType : inputType) switch
        {
            SupportFormat.JSON => await CsfJsonTools.LoadAsync(istream, jsonVersion).ConfigureAwait(false),
            SupportFormat.XAML => CsfXamlTools.Load(istream, xamlVersion),
            SupportFormat.CSF => CsfBinaryTools.Load(istream),
            _ => throw new NotSupportedException(),
        };
        switch (reverse ? inputType : outputType)
        {
            case SupportFormat.JSON:
                await CsfJsonTools.WriteAsync(ostream, csf, upgrade ? newVersion : jsonVersion, format).ConfigureAwait(false);
                break;
            case SupportFormat.XAML:
                await CsfXamlTools.WriteAsync(ostream, csf, upgrade ? newVersion : xamlVersion, format).ConfigureAwait(false);
                break;
            case SupportFormat.CSF:
                await CsfBinaryTools.WriteAsync(ostream, csf).ConfigureAwait(false);
                break;
            default:
                throw new NotSupportedException();
        }
        await ostream.FlushAsync().ConfigureAwait(false);
    }
}