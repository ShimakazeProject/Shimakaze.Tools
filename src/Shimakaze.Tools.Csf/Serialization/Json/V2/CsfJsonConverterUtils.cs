using System.Text.Encodings.Web;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Shimakaze.Tools.Csf.Serialization.Json.V2;

public static class CsfJsonConverterUtils
{
    internal static JsonConverter<T>? GetConverter<T>(this JsonSerializerOptions options)
    {
        return options.GetConverter(typeof(T)) as JsonConverter<T>;
    }

    //=> options.Converters.Where(i => i is JsonConverter<T>).Select(i => i as JsonConverter<T>).First();

    internal static T? Read<T>(this JsonConverter<T> @this, ref Utf8JsonReader reader, JsonSerializerOptions options)
    {
        return @this.Read(ref reader, typeof(T), options);
    }


    /// <summary>
    /// Create New Instance Always
    /// </summary>
    public static JsonSerializerOptions CsfJsonSerializerOptions
    {
        get
        {
            JsonSerializerOptions? options = new()
            {
                Encoder = JavaScriptEncoder.UnsafeRelaxedJsonEscaping,

                //IncludeFields = true,
                ReadCommentHandling = JsonCommentHandling.Skip,
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
                AllowTrailingCommas = true,
            };
            options.Converters.Add(new CsfStructJsonConverter());
            options.Converters.Add(new CsfLabelsJsonConverter());
            options.Converters.Add(new CsfLabelJsonConverter());
            options.Converters.Add(new V1.CsfValuesJsonConverter());
            options.Converters.Add(new V1.CsfValueJsonConverter());
            options.Converters.Add(new Common.MultiLineStringJsonConverter());
            return options;
        }
    }
}