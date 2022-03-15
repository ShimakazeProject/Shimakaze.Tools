using System.Diagnostics;

using Shimakaze.Tools.InternalUtils;

namespace Shimakaze.Tools.Csf.Translater.Apis;

public abstract class TranslatorBase : ITranslator
{
    protected readonly HttpClient _httpClient = new();

    /// <summary>
    /// It will be called when an error occurs.
    /// </summary>
    public event EventHandler<Exception>? Error;

    /// <summary>
    /// Server Host.
    /// </summary>
    /// <remarks>
    /// It will be used to create a <see cref="TranslatorBase.Uri"/>. <br/>
    /// It can be an IP address or a domain.
    /// </remarks>
    public string Host { get; init; } = string.Empty;

    protected abstract string Url { get; }

    protected static string GetQueryString(IEnumerable<KeyValuePair<string, string>> query)
        => "?" + string.Join("&", query.Select(x => $"{x.Key}={x.Value}"));

    protected void OnError(Exception e)
    {
        Debug.WriteLine(e);
        Error?.Invoke(this, e);
    }

    public abstract Task<string> TranslateAsync(string word);

    public virtual string Translate(string word) => TranslateAsync(word).RunSync();
}

