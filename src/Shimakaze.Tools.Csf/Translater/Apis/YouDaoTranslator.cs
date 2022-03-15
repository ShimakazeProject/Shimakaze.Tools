using System.Diagnostics;
using System.Text.Json;

using Shimakaze.Tools.Csf.Translater.Exceptions;
using Shimakaze.Tools.Csf.Translater.Models;

namespace Shimakaze.Tools.Csf.Translater.Apis;

public class YouDaoTranslator : TranslatorBase
{
    public YouDaoTranslator(string type = TYPES.ZH_CN2EN)
    {
        Type = type;
    }

    public string Type { get; set; }

    protected override string Url => "https://fanyi.youdao.com/translate";

    public override async Task<string> TranslateAsync(string word)
    {
        Dictionary<string, string> query = new()
        {
            ["doctype"] = "json",
            ["type"] =Type,
            ["i"] = word
        };


        var response = await _httpClient.GetAsync(Url + GetQueryString(query));
        var content = await response.Content.ReadAsStringAsync();
        try
        {
            var json = JsonSerializer.Deserialize<YouDaoApiResult>(content);
            return json switch
            {
                null => throw new TranslateException("YouDao Translate API response is null."),
                not null => json.TranslateResult.First().First().Tgt,
            };

        }
        catch (Exception e)
        {
            OnError(new TranslateException(content, e));
            return string.Empty;
        }
    }

    // ZH_CN2EN 中文　»　英语
    // ZH_CN2JA 中文　»　日语
    // ZH_CN2KR 中文　»　韩语
    // ZH_CN2FR 中文　»　法语
    // ZH_CN2RU 中文　»　俄语
    // ZH_CN2SP 中文　»　西语
    // EN2ZH_CN 英语　»　中文
    // JA2ZH_CN 日语　»　中文
    // KR2ZH_CN 韩语　»　中文
    // FR2ZH_CN 法语　»　中文
    // RU2ZH_CN 俄语　»　中文
    // SP2ZH_CN 西语　»　中文
    public static class TYPES
    {
        public const string ZH_CN2EN = "ZH_CN2EN";
        public const string ZH_CN2JA = "ZH_CN2JA";
        public const string ZH_CN2KR = "ZH_CN2KR";
        public const string ZH_CN2FR = "ZH_CN2FR";
        public const string ZH_CN2RU = "ZH_CN2RU";
        public const string ZH_CN2SP = "ZH_CN2SP";
        public const string EN2ZH_CN = "EN2ZH_CN";
        public const string JA2ZH_CN = "JA2ZH_CN";
        public const string KR2ZH_CN = "KR2ZH_CN";
        public const string FR2ZH_CN = "FR2ZH_CN";
        public const string RU2ZH_CN = "RU2ZH_CN";
        public const string SP2ZH_CN = "SP2ZH_CN";
    }

}