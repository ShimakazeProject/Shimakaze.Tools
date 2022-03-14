using System.Text;

using Shimakaze.Models.Csf;
using Shimakaze.Tools.InternalUtils;

namespace Shimakaze.Tools.Csf.Serialization.Csf;

public sealed class CsfStructSerializer : ICsfSerializer<CsfStruct>
{
    public static CsfStruct Deserialize(Stream stream)
    {
        using BinaryReader reader = new(stream, Encoding.Default, true);
        return Deserialize(reader);
    }
    public static void Serialize(Stream stream, CsfStruct t)
    {
        using BinaryWriter writer = new(stream, Encoding.Default, true);
        Serialize(writer, t);
    }
    public static CsfStruct Deserialize(BinaryReader reader)
    {
        CsfHead csfHead = CsfHeadSerializer.Deserialize(reader);
        CsfLabel[] csfLabels = new CsfLabel[csfHead.LabelCount];
        for (int i = 0; i < csfHead.LabelCount; i++)
        {
            csfLabels[i] = CsfLabelSerializer.Deserialize(reader);
        }

        return new()
        {
            Head = csfHead,
            Datas = csfLabels
        };
    }

    public static void Serialize(BinaryWriter writer, CsfStruct t)
    {
        CsfHeadSerializer.Serialize(writer, t.Head);
        t.Datas.Each(t => CsfLabelSerializer.Serialize(writer, t));
    }
}