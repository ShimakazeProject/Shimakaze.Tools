using System.Text;
using System.Xml;

using Shimakaze.Models.Csf;

using XmlV1 = Shimakaze.Tools.Csf.Serialization.Xml.V1.CsfStructXmlConverter;

namespace Shimakaze.Tools.Csf.Common;

public static class CsfXamlTools
{
    public static CsfStruct Load(Stream stream, int version = 1)
    {
        XmlTextReader reader = new(stream);
        CsfStruct csf;

        switch (version)
        {
            case 1:
                XmlV1 xmlConverter = new();
                csf = xmlConverter.Deserialize(reader);
                break;

            default:
                throw new("未知的Xml版本");
        }

        return csf;
    }
    public static async Task WriteAsync(Stream stream, CsfStruct csf, int version = 1, bool format = false)
    {
        XmlWriter writer = XmlWriter.Create(stream, new()
        {
            Encoding = Encoding.UTF8,
            Indent = format,
            Async = true
        });
        switch (version)
        {
            case 1:
                XmlV1 xmlConverter = new();
                xmlConverter.Serialize(writer, csf!);
                break;

            default:
                throw new("未知的Xml版本");
        }
        await writer.FlushAsync().ConfigureAwait(false);
        await stream.FlushAsync().ConfigureAwait(false);
    }
}
