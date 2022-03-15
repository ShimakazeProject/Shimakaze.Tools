using System.Diagnostics;
using System.Text.Json;

using Shimakaze.Tools.Csf.Translater.Exceptions;
using Shimakaze.Tools.Csf.Translater.Models;

namespace Shimakaze.Tools.Csf.Translater.Apis;

public class GoogleTranslator : TranslatorBase
{
    public string SourceLang { get; set; } = string.Empty;
    public string TargetLang { get; set; }

    public GoogleTranslator(string targetLang, string host = "translate.google.com")
    {
        TargetLang = targetLang;
        Host = host;
    }

    public GoogleTranslator(string sourceLang, string targetLang, string host = "translate.google.com") : this(targetLang, host) => SourceLang = sourceLang;

    public override async Task<string> TranslateAsync(string word)
    {
        Dictionary<string, string> query = new()
        {
            ["client"] = "gtx",
            ["dt"] = "t",
            ["dj"] = "1",
            ["ie"] = "UTF-8",
            ["sl"] = string.IsNullOrEmpty(SourceLang) ? "auto" : SourceLang,
            ["tl"] = TargetLang,
            ["q"] = word
        };

        var response = await _httpClient.GetAsync(Url + GetQueryString(query));
        var content = await response.Content.ReadAsStringAsync();
        try
        {
            var json = JsonSerializer.Deserialize<GoogleResult>(content);
            return json switch
            {
                null => throw new TranslateException("Google Translate API response is null."),
                not null => json.Sentences[0].Trans,
            };
        }
        catch (Exception e)
        {
            OnError(new TranslateException(content, e));
            return string.Empty;
        }
    }

    protected override string Url => $"https://{Host}/translate_a/single";
}

