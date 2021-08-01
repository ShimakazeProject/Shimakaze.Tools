using System;
namespace Shimakaze.Tools.Csf.InternalUtils
{
    internal static class ByteUtils
    {
        public static byte[] GetLittleEndianBytes(int i)
        {
            byte[] result = BitConverter.GetBytes(i);
            if (!BitConverter.IsLittleEndian)
                Array.Reverse(result);
            return result;
        }
        public static int GetLittleEndianInt32(byte[] raw, int start = 0)
        {
            byte[] data = (byte[])raw.Clone();
            if (!BitConverter.IsLittleEndian)
                Array.Reverse(data);
            return BitConverter.ToInt32(data, start);
        }


        public static void ReverseEndianIfNotLittleEndian(byte[] data, int? length = null)
        {
            if (BitConverter.IsLittleEndian)
                return;

            if (length is null)
                length = data.Length;

            if (length % 4 > 0)
                throw new ArgumentException("length must be divided by 4");

            for (int i = 0; i < length; i += 4)
            {
                byte tmp = data[i + 0];
                data[i + 0] = data[i + 3];
                data[i + 3] = tmp;

                tmp = data[i + 1];
                data[i + 1] = data[i + 2];
                data[i + 2] = tmp;
            }
        }
    }
}