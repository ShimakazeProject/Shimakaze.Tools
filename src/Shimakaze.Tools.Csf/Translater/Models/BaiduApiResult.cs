// Love you https://json2csharp.com/

using System.Text.Json.Serialization;

namespace Shimakaze.Tools.Csf.Translater.Models;

#nullable disable

// Root myDeserializedClass = JsonSerializer.Deserialize<Root>(myJsonResponse);
public class TransResult
{
    [JsonPropertyName("src")]
    public string Src { get; set; }

    [JsonPropertyName("dst")]
    public string Dst { get; set; }
}

public class BaiduApiResult
{
    [JsonPropertyName("from")]
    public string From { get; set; }

    [JsonPropertyName("to")]
    public string To { get; set; }

    [JsonPropertyName("trans_result")]
    public List<TransResult> TransResult { get; set; }
}



