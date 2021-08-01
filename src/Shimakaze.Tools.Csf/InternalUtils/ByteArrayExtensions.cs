using System;
namespace Shimakaze.Tools.Csf.InternalUtils
{
    internal static class ByteArrayExtensions
    {

        public static byte[] Use(this byte[] data, Action<byte[]> action)
        {
            action(data);
            return data;
        }
        public static void CheckLength(this byte[] buffer, int length)
        {
            if (buffer.Length < length)
                throw new ArgumentException($"Buffer too Short! need {length}bytes, but it's {buffer.Length}bytes");
        }

    }
}