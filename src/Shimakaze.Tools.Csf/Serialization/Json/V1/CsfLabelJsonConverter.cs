using System;
using System.Text.Json;
using System.Text.Json.Serialization;

using Shimakaze.Models.Csf;

namespace Shimakaze.Tools.Csf.Serialization.Json.V1
{
    public class CsfLabelJsonConverter : JsonConverter<CsfLabel>
    {
        public override CsfLabel Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            if (reader.TokenType != JsonTokenType.StartObject)
                throw new JsonException();

            var result = new CsfLabel();
            var (Value, Extra) = ("", "");
            var converter = options.GetConverter<CsfValue[]>();
            var converter2 = options.GetConverter<string>();
            while (reader.Read())
            {
                if (reader.TokenType is JsonTokenType.EndObject)
                    break;
                if (reader.TokenType != JsonTokenType.PropertyName)
                    throw new JsonException();
                switch (reader.GetString()?.ToLower())
                {
                    case "label":
                        reader.Read();
                        result.Label = reader.GetString()!;
                        break;
                    case "values":
                        reader.Read();
                        result.Values = converter!.Read(ref reader, options)!;
                        break;
                    case "value":
                        if (result.Values.Length > 0)
                            throw new JsonException();
                        reader.Read();

                        Value = converter2!.Read(ref reader, options)!;

                        break;
                    case "extra":
                        if (result.Values.Length > 0)
                            throw new JsonException();
                        reader.Read();
                        Extra = reader.TokenType is JsonTokenType.String ? reader.GetString()! : throw new JsonException();
                        break;
                    default:
                        throw new JsonException();
                }
            }
            
            if (result.Values.Length < 1)
                result.Values = new [] { string.IsNullOrEmpty(Extra) ? (new(Value)) : (CsfValue)new CsfExtraValue(Value, Extra) };
            return result;
        }
        public override void Write(Utf8JsonWriter writer, CsfLabel value, JsonSerializerOptions options)
        {
            var converter = options.GetConverter<CsfValue[]>();
            writer.WriteStartObject();
            writer.WriteString(nameof(value.Label).ToLower(), value.Label);
            converter!.Write(writer, value.Values, options);
            writer.WriteEndObject();
        }
    }
}
