using System;
using System.Collections.Generic;
using System.Text.Json;
using System.Text.Json.Serialization;

using Shimakaze.Models.Csf;

using Shimakaze.Tools.Csf.InternalUtils;

namespace Shimakaze.Tools.Csf.Serialization.Json.V1
{
    public class CsfLabelsJsonConverter : JsonConverter<CsfLabel[]>
    {
        public override CsfLabel[] Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            if (reader.TokenType != JsonTokenType.StartArray)
                throw new JsonException();

            var converter = options.GetConverter<CsfLabel>();
            var result = new List<CsfLabel>();
            while (reader.Read())
            {
                if (reader.TokenType is JsonTokenType.EndArray)
                    break;

                result.Add(converter!.Read(ref reader, options)!);
            }
            return result.ToArray();
        }
        public override void Write(Utf8JsonWriter writer, CsfLabel[] value, JsonSerializerOptions options)
        {
            var converter = options.GetConverter<CsfLabel>();
            writer.WriteStartArray();
            value.Each(i => converter!.Write(writer, i, options));
            writer.WriteEndArray();
        }
    }
}
