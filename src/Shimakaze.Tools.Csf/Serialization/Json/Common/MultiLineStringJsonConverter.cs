using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Shimakaze.Tools.Csf.Serialization.Json.Common;

public class MultiLineStringJsonConverter : JsonConverter<string>
{
    public override string Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        switch (reader.TokenType)
        {
            case JsonTokenType.StartArray:
                StringBuilder? sb = null;
                while (reader.Read())
                {
                    switch (reader.TokenType)
                    {
                        case JsonTokenType.EndArray:
                            if (sb is null)
                            {
                                throw new JsonException();
                            }

                            return sb.ToString();

                        case JsonTokenType.String:
                            if (sb is null)
                            {
                                sb = new StringBuilder(reader.GetString());
                            }
                            else
                            {
                                sb.Append('\n' + reader.GetString());
                            }

                            break;

                        default:
                            throw new JsonException();
                    }
                }
                throw new JsonException();
            case JsonTokenType.String:
                return reader.GetString()!;

            default:
                throw new JsonException();
        }
    }

    public override void Write(Utf8JsonWriter writer, string value, JsonSerializerOptions options)
    {
        List<string>? lines = value.Split('\n').Select(i => i.TrimEnd('\r')).ToList();
        if (lines.Count == 1)
        {
            writer.WriteStringValue(lines[0]);
        }
        else
        {
            writer.WriteStartArray();
            lines.ForEach(writer.WriteStringValue);
            writer.WriteEndArray();
        }
    }
}