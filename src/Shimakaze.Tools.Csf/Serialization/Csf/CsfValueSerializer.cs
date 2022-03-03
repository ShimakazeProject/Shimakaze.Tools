using System.Text;

using Shimakaze.Models.Csf;
using Shimakaze.Tools.InternalUtils;

namespace Shimakaze.Tools.Csf.Serialization.Csf;

public sealed class CsfValueSerializer : ICsfSerializer<CsfValue>
{
    public static CsfValue Deserialize(BinaryReader reader)
    {
        bool isSTRW = reader.ReadInt32() switch
        {
            Constants.STR_FLAG_RAW => false,
            Constants.STRW_FLG_RAW => true,
            _ => throw new ArgumentException("It's not CSF String Flag")
        };

        int length = reader.ReadInt32() << 1;

        byte[]? data = CsfUtils.CodingValue(reader.ReadBytes(length));

        string value = Encoding.Unicode.GetString(data);

        if (isSTRW)
        {
            length = reader.ReadInt32();
            string extra = Encoding.ASCII.GetString(reader.ReadBytes(length));
            return new CsfExtraValue(value, extra);
        }
        return new(value);
    }

    public static void Serialize(BinaryWriter writer, CsfValue t)
    {
        CsfExtraValue? extra = t as CsfExtraValue;
        writer.Write(extra is null ? Constants.STR_FLAG_RAW : Constants.STRW_FLG_RAW);
        writer.Write(t.Value.Length);
        writer.Write(CsfUtils.CodingValue(Encoding.Unicode.GetBytes(t.Value)));
        if (extra is not null)
        {
            writer.Write(extra.Extra.Length);
            writer.Write(Encoding.ASCII.GetBytes(extra.Extra));
        }
    }
}