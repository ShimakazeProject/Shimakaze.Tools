
using Shimakaze.Models.Csf;

using Shimakaze.Tools.Csf.Serialization.Csf;

namespace Shimakaze.Tools.Csf.Cli;
public static class CsfBinaryTools
{
    public static CsfStruct Load(Stream stream) => CsfStructSerializer.Deserialize(stream);
    public static void Write(Stream stream, CsfStruct csf) => CsfStructSerializer.Serialize(stream, csf);
}
