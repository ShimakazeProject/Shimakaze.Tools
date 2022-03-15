// Love you https://json2csharp.com/

using System.Text.Json.Serialization;

namespace Shimakaze.Tools.Csf.Translater.Models;

#nullable disable

// Root myDeserializedClass = JsonSerializer.Deserialize<Root>(myJsonResponse);
public class YouDaoApiResult
{
    [JsonPropertyName("type")]
    public string Type { get; set; }

    [JsonPropertyName("errorCode")]
    public int ErrorCode { get; set; }

    [JsonPropertyName("elapsedTime")]
    public int ElapsedTime { get; set; }

    [JsonPropertyName("translateResult")]
    public List<List<TranslateResult>> TranslateResult { get; set; }
}

public class TranslateResult
{
    [JsonPropertyName("src")]
    public string Src { get; set; }

    [JsonPropertyName("tgt")]
    public string Tgt { get; set; }
}

