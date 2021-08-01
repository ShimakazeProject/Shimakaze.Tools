using System;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Threading.Tasks;
using Shimakaze.Models.Csf;
using Shimakaze.Tools.Csf.InternalUtils;

namespace Shimakaze.Tools.Csf.Serialization.Csf
{
    public sealed class CsfHeadSerializer : ICsfSerializer<CsfHead>, IAsyncCsfSerializer<CsfHead>
    {
        private const int STRUCT_LENGTH = 24;

        public byte[] Serialize(CsfHead t)
        {
            byte[] result = new byte[STRUCT_LENGTH];

            // Create Memory Space
            var ptr = Marshal.AllocHGlobal(STRUCT_LENGTH);

            // Copy Structure Data to Memory
            Marshal.StructureToPtr(t, ptr, false);

            // Copy Flag
            ByteUtils.GetLittleEndianBytes(Constants.CSF_FLAG_RAW).CopyTo(result.AsMemory(0, 4));

            // Copy data from Memory to Managed Byte Array
            Marshal.Copy(ptr, result, 4, STRUCT_LENGTH - 4);

            // Free Memory
            Marshal.DestroyStructure<CsfHead>(ptr);

            // Check Endian
            ByteUtils.ReverseEndianIfNotLittleEndian(result);

            return result;
        }

        public async Task SerializeAsync(CsfHead t, Stream stream)
        {
            await stream.WriteAsync(Serialize(t).AsMemory());
        }

        // Fixme: Flag Check
        public CsfHead Deserialize(byte[] raw)
        {
            if (raw.Length < STRUCT_LENGTH)
                throw new ArgumentException($"data is too short, Need 24 bytes, but is {raw.Length} bytes.");

            byte[] flag = ByteUtils.GetLittleEndianBytes(Constants.CSF_FLAG_RAW);

            if (!flag.SequenceEqual(raw.AsSpan(0, 4).ToArray()))
                throw new ArgumentException("It's not CSF File Flag");

            byte[] data = raw.AsSpan(0, STRUCT_LENGTH).ToArray();


            // Check Endian
            ByteUtils.ReverseEndianIfNotLittleEndian(data);

            // Create Memory Space
            var ptr = Marshal.AllocHGlobal(STRUCT_LENGTH - 4);

            // Copy data from Managed Byte Array to Memory
            Marshal.Copy(data, 4, ptr, STRUCT_LENGTH - 4);

            // Copy data from Memory to Structure
            var result = Marshal.PtrToStructure<CsfHead>(ptr);

            // Free Memory
            Marshal.DestroyStructure<CsfHead>(ptr);

            return result;
        }

        public async Task<CsfHead> DeserializeAsync(Stream stream, byte[] buffer)
        {
            await stream.ReadAsync(buffer.AsMemory());
            ByteUtils.ReverseEndianIfNotLittleEndian(buffer);
            return Deserialize(buffer);
        }

    }
}