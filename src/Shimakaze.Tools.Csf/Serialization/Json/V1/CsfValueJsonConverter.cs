using System.Text.Json;
using System.Text.Json.Serialization;

using Shimakaze.Models.Csf;

namespace Shimakaze.Tools.Csf.Serialization.Json.V1;

public class CsfValueJsonConverter : JsonConverter<CsfValue>
{
    public override CsfValue Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        (string Value, string Extra) = ("", "");
        JsonConverter<string>? converter = options.GetConverter<string>();
        switch (reader.TokenType)
        {
            case JsonTokenType.String:
            case JsonTokenType.StartArray:
                Value = converter!.Read(ref reader, options)!;
                break;

            case JsonTokenType.StartObject:
                while (reader.Read())
                {
                    if (reader.TokenType is JsonTokenType.EndObject)
                    {
                        break;
                    }

                    if (reader.TokenType != JsonTokenType.PropertyName)
                    {
                        throw new JsonException();
                    }

                    switch (reader.GetString()?.ToLower())
                    {
                        case "value":
                            reader.Read();
                            Value = converter!.Read(ref reader, options)!;
                            break;

                        case "extra":
                            reader.Read();
                            Extra = reader.TokenType is JsonTokenType.String ? reader.GetString()! : throw new JsonException();
                            break;

                        default:
                            throw new JsonException();
                    }
                }
                break;

            default:
                throw new JsonException();
        }
        return string.IsNullOrEmpty(Extra) ? (new(Value)) : (CsfValue)new CsfExtraValue(Value, Extra);
    }

    public override void Write(Utf8JsonWriter writer, CsfValue value, JsonSerializerOptions options)
    {
        options.GetConverter<string>()!.Write(writer, value.Value, options);

        if (value is CsfExtraValue extra)
        {
            writer.WriteString(nameof(extra.Extra).ToLower(), extra.Extra);
        }
    }
}