using System.Text.Json;

using Shimakaze.Models.Csf;

using JsonV1 = Shimakaze.Tools.Csf.Serialization.Json.V1.CsfJsonConverterUtils;
using JsonV2 = Shimakaze.Tools.Csf.Serialization.Json.V2.CsfJsonConverterUtils;

namespace Shimakaze.Tools.Csf.Common;

public static class CsfJsonTools
{
    public static async Task<CsfStruct> LoadAsync(Stream stream, int version = 2)
    {
        CsfStruct? csf = await JsonSerializer.DeserializeAsync<CsfStruct>(stream, version switch
        {
            1 => JsonV1.CsfJsonSerializerOptions,
            2 => JsonV2.CsfJsonSerializerOptions,
            _ => throw new("未知的Json版本")
        }).ConfigureAwait(false);

        return csf ?? throw new("Cannot Load Json. Becuse the result is null.");
    }
    public static async Task WriteAsync(Stream stream, CsfStruct csf, int version = 2, bool format = false)
    {
        var option = version switch
        {
            1 => JsonV1.CsfJsonSerializerOptions,
            2 => JsonV2.CsfJsonSerializerOptions,
            _ => throw new("未知的Json版本")
        };
        option.WriteIndented = format;
        await JsonSerializer.SerializeAsync(stream, csf, option).ConfigureAwait(false);
        await stream.FlushAsync().ConfigureAwait(false);
    }
}
