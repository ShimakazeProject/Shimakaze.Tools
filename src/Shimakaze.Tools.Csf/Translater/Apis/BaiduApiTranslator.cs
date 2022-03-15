using System.Diagnostics;
using System.Security.Cryptography;
using System.Text;
using System.Text.Json;

using Shimakaze.Tools.Csf.Translater.Exceptions;
using Shimakaze.Tools.Csf.Translater.Models;

namespace Shimakaze.Tools.Csf.Translater.Apis;

public class BaiduApiTranslator : TranslatorBase
{
    public BaiduApiTranslator(string sourceLang, string targetLang, string appId, string key)
    {
        SourceLang = sourceLang;
        TargetLang = targetLang;
        AppId = appId;
        Key = key;
    }

    protected override string Url => "https://fanyi-api.baidu.com/api/trans/vip/translate";

    public string SourceLang { get; set; }
    public string TargetLang { get; set; }
    public string AppId { get; set; }
    public string Key { get; set; }
    public static string Salt => GetRandomString(16);

    private static string GetRandomString(int v)
    {
        // GetRandomString
        var chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789";
        var stringChars = new char[v];
        for (var i = 0; i < stringChars.Length; i++)
            stringChars[i] = chars[RandomNumberGenerator.GetInt32(chars.Length)];
        return new string(stringChars);
    }

    public override async Task<string> TranslateAsync(string word)
    {
        var salt = Salt;
        Dictionary<string, string> query = new()
        {
            ["q"] = word,
            ["from"] = SourceLang,
            ["to"] = TargetLang,
            ["appid"] = AppId,
            ["salt"] = salt,
            ["sign"] = Sign(word, salt)
        };

        var response = await _httpClient.GetAsync(Url + GetQueryString(query));
        var content = await response.Content.ReadAsStringAsync();
        try
        {
            var json = JsonSerializer.Deserialize<BaiduApiResult>(content);
            return json switch
            {
                null => throw new TranslateException("Baidu Translate API response is null."),
                not null => json.TransResult[0].Dst,
            };
        }
        catch (Exception e)
        {
            OnError(new TranslateException(content, e));
            return string.Empty;
        }
    }

    private string Sign(string word, string salt)
    {
        using var md5 = MD5.Create();
        var str = AppId + word + salt + Key;
        var tmp1 = Encoding.UTF8.GetBytes(str);
        var tmp2 = md5.ComputeHash(tmp1);
        var tmp3 = BitConverter.ToString(tmp2);
        var sign = tmp3.Replace("-", "").ToLower();
        return sign;
    }
}