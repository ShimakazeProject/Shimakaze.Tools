using System.Text.Json;
using System.Text.Json.Serialization;

using Shimakaze.Models.Csf;

namespace Shimakaze.Tools.Csf.Serialization.Json.V2;

public class CsfLabelJsonConverter : JsonConverter<CsfLabel>
{
    public override CsfLabel Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        if (reader.TokenType != JsonTokenType.PropertyName)
        {
            throw new JsonException();
        }

        CsfLabel? result = new()
        {
            Label = reader.GetString()!
        };

        (string Value, string Extra) = ("", "");
        JsonConverter<CsfValue[]>? converter = options.GetConverter<CsfValue[]>();
        JsonConverter<string>? converter2 = options.GetConverter<string>();

        while (reader.Read())
        {
        OUTER:
            if (reader.TokenType is JsonTokenType.EndObject)
            {
                break;
            }

            if (reader.TokenType is JsonTokenType.StartArray or JsonTokenType.String)
            {
                Value = converter2!.Read(ref reader, options)!;
                break;
            }
            else if (reader.TokenType is JsonTokenType.StartObject)
            {
                while (reader.Read())
                {
                    if (reader.TokenType is JsonTokenType.EndObject)
                    {
                        goto OUTER;
                    }

                    if (reader.TokenType != JsonTokenType.PropertyName)
                    {
                        throw new JsonException();
                    }

                    if ("values" == reader.GetString()?.ToLower())
                    {
                        reader.Read();
                        result.Values = converter!.Read(ref reader, options)!;
                        break;
                    }
                    switch (reader.GetString()?.ToLower())
                    {
                        case "value":
                            if (result.Values.Length > 0)
                            {
                                throw new JsonException();
                            }

                            reader.Read();
                            Value = converter2!.Read(ref reader, options)!;
                            continue;
                        case "extra":
                            if (result.Values.Length > 0)
                            {
                                throw new JsonException();
                            }

                            reader.Read();
                            Extra = reader.TokenType is JsonTokenType.String ? reader.GetString()! : throw new JsonException();
                            continue;
                        default:
                            throw new JsonException();
                    }
                }
                continue;
            }
            else
            {
                throw new JsonException();
            }
        }
        if (result.Values.Length < 1)
        {
            result.Values = new[] { string.IsNullOrEmpty(Extra) ? (new(Value)) : (CsfValue)new CsfExtraValue(Value, Extra) };
        }

        return result;
    }

    public override void Write(Utf8JsonWriter writer, CsfLabel value, JsonSerializerOptions options)
    {
        JsonConverter<CsfValue[]>? converter = options.GetConverter<CsfValue[]>();
        writer.WritePropertyName(value.Label);
        if (value.Values.Length == 1)
        {
            if (value.Values[0] is not CsfExtraValue)
            {
                options.GetConverter<CsfValue>()!.Write(writer, value.Values[0], options);
                return;
            }
        }
        writer.WriteStartObject();
        converter!.Write(writer, value.Values, options);
        writer.WriteEndObject();
    }
}