using System.Runtime.InteropServices;

using Shimakaze.Models.Csf;

namespace Shimakaze.Tools.Csf.Serialization.Csf;

public class CsfHeadSerializer : ICsfSerializer<CsfHead>
{
    private const int STRUCT_LENGTH = 20;

    public static CsfHead Deserialize(BinaryReader reader)
    {
        if (reader.ReadInt32() is not Constants.CSF_FLAG_RAW)
        {
            throw new ArgumentException("It's not CSF File Flag");
        }

        byte[] data = reader.ReadBytes(STRUCT_LENGTH);

        // Create Memory Space
        IntPtr ptr = Marshal.AllocHGlobal(STRUCT_LENGTH);

        // Copy data from Managed Byte Array to Memory
        Marshal.Copy(data, 0, ptr, STRUCT_LENGTH);

        // Copy data from Memory to Structure
        CsfHead result = Marshal.PtrToStructure<CsfHead>(ptr);

        // Free Memory
        Marshal.DestroyStructure<CsfHead>(ptr);

        return result;
    }

    public static void Serialize(BinaryWriter writer, CsfHead head)
    {

        // Create Memory Space
        IntPtr ptr = Marshal.AllocHGlobal(STRUCT_LENGTH);
        byte[] data = new byte[STRUCT_LENGTH];

        // Copy Structure Data to Memory
        Marshal.StructureToPtr(head, ptr, false);

        // Copy data from Memory to Managed Byte Array
        Marshal.Copy(ptr, data, 0, 20);

        // Free Memory
        Marshal.DestroyStructure<CsfHead>(ptr);

        // Write
        writer.Write(Constants.CSF_FLAG_RAW);
        writer.Write(data);
    }
}