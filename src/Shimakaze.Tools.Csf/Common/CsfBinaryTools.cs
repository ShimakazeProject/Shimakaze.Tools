using Shimakaze.Models.Csf;

using Shimakaze.Tools.Csf.Serialization.Csf;

namespace Shimakaze.Tools.Csf.Common;
public static class CsfBinaryTools
{
    public static CsfStruct Load(Stream stream) => CsfStructSerializer.Deserialize(stream);
    public static async Task WriteAsync(Stream stream, CsfStruct csf)
    {
        CsfStructSerializer.Serialize(stream, csf);
        await stream.FlushAsync().ConfigureAwait(false);
    }
}
