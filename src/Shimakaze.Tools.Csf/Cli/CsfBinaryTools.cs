
using Shimakaze.Models.Csf;

using Shimakaze.Tools.Csf.Serialization.Csf;

namespace Shimakaze.Tools.Csf.Cli;
internal static class CsfBinaryTools
{
    public static CsfStruct Load(Stream stream)
    {
        return CsfStructSerializer.Deserialize(stream);
    }
    public static void Write(Stream stream, CsfStruct csf)
    {
        CsfStructSerializer.Serialize(stream, csf);

    }
}
