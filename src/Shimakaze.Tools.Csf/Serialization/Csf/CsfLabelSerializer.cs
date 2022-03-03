using System.Text;

using Shimakaze.Models.Csf;
using Shimakaze.Tools.InternalUtils;

namespace Shimakaze.Tools.Csf.Serialization.Csf;

public sealed class CsfLabelSerializer : ICsfSerializer<CsfLabel>
{

    public static void Serialize(BinaryWriter writer, CsfLabel t)
    {
        // Copy Flag
        writer.Write(Constants.LBL_FLAG_RAW);

        writer.Write(t.Values.Length);

        writer.Write(t.Label.Length);

        writer.Write(Encoding.ASCII.GetBytes(t.Label));

        t.Values.Each(x => CsfValueSerializer.Serialize(writer, x));
    }

    public static CsfLabel Deserialize(BinaryReader reader)
    {
        if (reader.ReadInt32() is not Constants.LBL_FLAG_RAW)
        {
            throw new ArgumentException("It's not CSF Label Flag");
        }

        int count = reader.ReadInt32();
        int lbllength = reader.ReadInt32();
        string lableName = Encoding.ASCII.GetString(reader.ReadBytes(lbllength));

        CsfValue[]? values = new CsfValue[count];
        for (int i = 0; i < count; i++)
        {
            values[i] = CsfValueSerializer.Deserialize(reader);
        }

        return new()
        {
            Label = lableName,
            Values = values
        };
    }

}