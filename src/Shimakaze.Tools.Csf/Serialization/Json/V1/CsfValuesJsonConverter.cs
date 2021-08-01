using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text.Json;
using System.Text.Json.Serialization;

using Shimakaze.Models.Csf;

using Shimakaze.Tools.Csf.InternalUtils;

namespace Shimakaze.Tools.Csf.Serialization.Json.V1
{
    public class CsfValuesJsonConverter : JsonConverter<CsfValue[]>
    {
        public override CsfValue[] Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            if (reader.TokenType != JsonTokenType.StartArray)
                throw new JsonException();

            var converter = options.GetConverter<CsfValue>();
            var result = new List<CsfValue>();
            while (reader.Read())
            {
                if (reader.TokenType is JsonTokenType.EndArray)
                    break;

                result.Add(converter!.Read(ref reader, options)!);
            }
            return result.ToArray();
        }
        public override void Write(Utf8JsonWriter writer, CsfValue[] value, JsonSerializerOptions options)
        {
            var converter = options.GetConverter<CsfValue>();
            if (value.Length > 1)
            {
                writer.WritePropertyName(nameof(CsfLabel.Values).ToLower());
                writer.WriteStartArray();
                value.Each(i => converter!.Write(writer, i, options));
                writer.WriteEndArray();
            }
            else if (value.Length == 1)
            {
                writer.WritePropertyName("value");
                converter!.Write(writer, value[0], options);
            }
            else
            {
                Debug.Assert(false);
            }
        }
    }
}
