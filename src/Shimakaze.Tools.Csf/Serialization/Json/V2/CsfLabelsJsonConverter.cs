using System.Text.Json;
using System.Text.Json.Serialization;

using Shimakaze.Models.Csf;
using Shimakaze.Tools.InternalUtils;

namespace Shimakaze.Tools.Csf.Serialization.Json.V2;

public class CsfLabelsJsonConverter : JsonConverter<CsfLabel[]>
{
    public override CsfLabel[] Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        if (reader.TokenType != JsonTokenType.StartObject)
        {
            throw new JsonException();
        }

        JsonConverter<CsfLabel>? converter = options.GetConverter<CsfLabel>();
        List<CsfLabel>? result = new();
        while (reader.Read())
        {
            if (reader.TokenType is JsonTokenType.EndObject)
            {
                break;
            }

            result.Add(converter!.Read(ref reader, options)!);
        }
        return result.ToArray();
    }

    public override void Write(Utf8JsonWriter writer, CsfLabel[] value, JsonSerializerOptions options)
    {
        JsonConverter<CsfLabel>? converter = options.GetConverter<CsfLabel>();
        writer.WriteStartObject();
        value.Each(i => converter!.Write(writer, i, options));
        writer.WriteEndObject();
    }
}