using System.Text.Json;
using System.Text.Json.Serialization;

using Shimakaze.Models.Csf;
using Shimakaze.Tools.Csf.Serialization.Json.Common;

namespace Shimakaze.Tools.Csf.Serialization.Json.V2;

public class CsfStructJsonConverter : JsonConverter<CsfStruct>
{
    public override CsfStruct Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        if (reader.TokenType != JsonTokenType.StartObject)
        {
            throw new JsonException();
        }

        CsfStruct result = new();
        CsfHead head = new();
        head.Version = 3;
        head.Language = 0;
        while (reader.Read())
        {
            if (reader.TokenType == JsonTokenType.EndObject)
            {
                break;
            }

            if (reader.TokenType != JsonTokenType.PropertyName)
            {
                throw new JsonException();
            }

            switch (reader.GetString()?.ToLower())
            {
                case "$schema":
                    reader.Skip();
                    break;

                case "protocol":
                    reader.Read();
                    if (reader.TokenType != JsonTokenType.Number)
                    {
                        throw new JsonException();
                    }

                    if (reader.GetInt32() != 2)
                    {
                        throw new NotSupportedException("Supported protocol Version is 2 but it is " + reader.GetInt32());
                    }

                    break;

                case "version":
                    reader.Read();
                    head.Version = reader.GetInt32();
                    break;

                case "language":
                    reader.Read();
                    if (reader.TokenType is JsonTokenType.Number)
                    {
                        head.Language = reader.GetInt32();
                    }
                    else if (reader.TokenType is JsonTokenType.String)
                    {
                        string? code = reader.GetString();
                        for (head.Language = 0; head.Language < Constants.LanguageList.Length; head.Language++)
                        {
                            if (Constants.LanguageList[head.Language].Equals(code))
                            {
                                break;
                            }
                        }
                    }
                    break;

                // case "version":
                //     reader.Read();
                //     head.Version = reader.GetInt32();
                //     break;
                // case "language":
                //     reader.Read();
                //     if (reader.TokenType is JsonTokenType.Number)
                //     {
                //         head.Language = reader.GetInt32();
                //     }
                //     else if (reader.TokenType is JsonTokenType.String)
                //     {
                //         var code = reader.GetString();
                //         for (head.Language = 0; head.Language < Constants.LanguageList.Length; head.Language++)
                //         {
                //             if (Constants.LanguageList[head.Language].Equals(code))
                //                 break;
                //         }
                //     }
                //     break;
                case "data":
                    reader.Read();
                    result.Datas = options!.GetConverter<CsfLabel[]>()!.Read(ref reader, options)!;
                    break;

                default:
                    break;
            }
        }
        result.Head = head;
        result.ReCount();
        return result;
    }

    public override void Write(Utf8JsonWriter writer, CsfStruct value, JsonSerializerOptions options)
    {
        writer.WriteStartObject();
        writer.WriteString("$schema", Constants.SchemaUrls.V2);
        writer.WriteNumber("protocol", 2);
        writer.WriteNumber(nameof(value.Head.Version).ToLower(), value.Head.Version);
        writer.WriteNumber(nameof(value.Head.Language).ToLower(), value.Head.Language);

        writer.WritePropertyName("data");
        options!.GetConverter<CsfLabel[]>()!.Write(writer, value.Datas, options);
        writer.WriteEndObject();
    }
}