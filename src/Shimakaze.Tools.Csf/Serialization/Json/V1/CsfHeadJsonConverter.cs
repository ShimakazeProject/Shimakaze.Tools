using System.Text.Json;
using System.Text.Json.Serialization;

using Shimakaze.Models.Csf;

namespace Shimakaze.Tools.Csf.Serialization.Json.V1;

public class CsfHeadJsonConverter : JsonConverter<CsfHead>
{
    public override CsfHead Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        if (reader.TokenType != JsonTokenType.StartObject)
        {
            throw new JsonException();
        }

        CsfHead result = new();
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
                case "version":
                    reader.Read();
                    result.Version = reader.GetInt32();
                    break;

                case "language":
                    reader.Read();
                    if (reader.TokenType is JsonTokenType.Number)
                    {
                        result.Language = reader.GetInt32();
                    }
                    else if (reader.TokenType is JsonTokenType.String)
                    {
                        string? code = reader.GetString();
                        for (result.Language = 0; result.Language < Constants.LanguageList.Length; result.Language++)
                        {
                            if (Constants.LanguageList[result.Language].Equals(code))
                            {
                                break;
                            }
                        }
                    }
                    break;

                default:
                    throw new JsonException();
            }
        }
        return result;
    }

    public override void Write(Utf8JsonWriter writer, CsfHead value, JsonSerializerOptions options)
    {
        writer.WriteStartObject();
        writer.WriteNumber(nameof(value.Version).ToLower(), value.Version);
        if (value.Language < Constants.LanguageList.Length)
        {
            writer.WriteString(nameof(value.Language).ToLower(), Constants.LanguageList[value.Language]);
        }
        else
        {
            writer.WriteNumber(nameof(value.Language).ToLower(), value.Language);
        }

        writer.WriteEndObject();
    }
}