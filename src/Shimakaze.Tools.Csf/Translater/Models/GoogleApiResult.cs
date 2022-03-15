// Love you https://json2csharp.com/

using System.Text.Json.Serialization;

namespace Shimakaze.Tools.Csf.Translater.Models;

#nullable disable

// Root myDeserializedClass = JsonSerializer.Deserialize<Root>(myJsonResponse);
public class ModelSpecification
{
}

public class ModelTracking
{
    [JsonPropertyName("checkpoint_md5")]
    public string CheckpointMd5 { get; set; }

    [JsonPropertyName("launch_doc")]
    public string LaunchDoc { get; set; }
}

public class TranslationEngineDebugInfo
{
    [JsonPropertyName("model_tracking")]
    public ModelTracking ModelTracking { get; set; }
}

public class Sentence
{
    [JsonPropertyName("trans")]
    public string Trans { get; set; }

    [JsonPropertyName("orig")]
    public string Orig { get; set; }

    [JsonPropertyName("backend")]
    public int Backend { get; set; }

    [JsonPropertyName("model_specification")]
    public List<ModelSpecification> ModelSpecification { get; set; }

    [JsonPropertyName("translation_engine_debug_info")]
    public List<TranslationEngineDebugInfo> TranslationEngineDebugInfo { get; set; }
}

public class Spell
{
}

public class LdResult
{
    [JsonPropertyName("srclangs")]
    public List<string> Srclangs { get; set; }

    [JsonPropertyName("srclangs_confidences")]
    public List<double> SrclangsConfidences { get; set; }

    [JsonPropertyName("extended_srclangs")]
    public List<string> ExtendedSrclangs { get; set; }
}

public class GoogleResult
{
    [JsonPropertyName("sentences")]
    public List<Sentence> Sentences { get; set; }

    [JsonPropertyName("src")]
    public string Src { get; set; }

    [JsonPropertyName("confidence")]
    public double Confidence { get; set; }

    [JsonPropertyName("spell")]
    public Spell Spell { get; set; }

    [JsonPropertyName("ld_result")]
    public LdResult LdResult { get; set; }
}

