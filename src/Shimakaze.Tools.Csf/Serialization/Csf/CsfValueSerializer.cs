using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;

using Shimakaze.Models.Csf;
using Shimakaze.Tools.Csf.InternalUtils;

namespace Shimakaze.Tools.Csf.Serialization.Csf
{
    public sealed class CsfValueSerializer : ICsfSerializer<CsfValue>, IAsyncCsfSerializer<CsfValue>
    {
        public CsfValue Deserialize(byte[] raw)
        {
            byte[] data = (byte[])raw.Clone();
            ByteUtils.ReverseEndianIfNotLittleEndian(data);
            bool isSTRW = BitConverter.ToInt32(data) switch
            {
                Constants.STR_FLAG_RAW => false,
                Constants.STRW_FLG_RAW => true,
                _ => throw new ArgumentException("It's not CSF String Flag")
            };

            int length = BitConverter.ToInt32(data, 4) << 1;

            CsfUtils.CodingValue(data, 8, length);

            string value = Encoding.Unicode.GetString(data, 8, length);

            if (isSTRW)
            {
                int offset = 8 + length;
                length = BitConverter.ToInt32(data, offset);
                string extra = Encoding.ASCII.GetString(data, offset + 4, length);
                return new CsfExtraValue(value, extra);
            }
            return new(value);
        }

        public async Task<CsfValue> DeserializeAsync(Stream stream, byte[] buffer)
        {
            buffer.CheckLength(8);
            await stream.ReadAsync(buffer.AsMemory(0, 8)).ConfigureAwait(false);
            ByteUtils.ReverseEndianIfNotLittleEndian(buffer);

            bool isSTRW = BitConverter.ToInt32(buffer) switch
            {
                Constants.STR_FLAG_RAW => false,
                Constants.STRW_FLG_RAW => true,
                _ => throw new ArgumentException("It's not CSF String Flag")
            };

            int length = BitConverter.ToInt32(buffer, 4) << 1;

            buffer.CheckLength(length);
            await stream.ReadAsync(buffer.AsMemory(0, length)).ConfigureAwait(false);
            ByteUtils.ReverseEndianIfNotLittleEndian(buffer);

            CsfUtils.CodingValue(buffer, 0, length);

            string value = Encoding.Unicode.GetString(buffer, 0, length);

            if (isSTRW)
            {
                await stream.ReadAsync(buffer.AsMemory(0, 4)).ConfigureAwait(false);

                length = ByteUtils.GetLittleEndianInt32(buffer);

                buffer.CheckLength(length);
                await stream.ReadAsync(buffer.AsMemory(0, length)).ConfigureAwait(false);
                ByteUtils.ReverseEndianIfNotLittleEndian(buffer);

                string extra = Encoding.ASCII.GetString(buffer, 0, length);
                return new CsfExtraValue(value, extra);
            }
            return new(value);
        }

        public byte[] Serialize(CsfValue t)
        {
            List<byte> result = new();
            CsfExtraValue? extra = t as CsfExtraValue;
            result.AddRange(ByteUtils.GetLittleEndianBytes(extra is null ? Constants.STR_FLAG_RAW : Constants.STRW_FLG_RAW));
            result.AddRange(ByteUtils.GetLittleEndianBytes(t.Value.Length));
            result.AddRange(Encoding.Unicode.GetBytes(t.Value)
                .Use(x => CsfUtils.CodingValue(x))
                .Use(x => ByteUtils.ReverseEndianIfNotLittleEndian(x)));
            if (extra is not null)
            {
                result.AddRange(ByteUtils.GetLittleEndianBytes(extra.Extra.Length));
                result.AddRange(Encoding.ASCII.GetBytes(extra.Extra)
                    .Use(x => ByteUtils.ReverseEndianIfNotLittleEndian(x)));
            }
            return result.ToArray();
        }

        public async Task SerializeAsync(CsfValue t, Stream stream)
        {
            CsfExtraValue? extra = t as CsfExtraValue;
            await stream.WriteAsync(ByteUtils.GetLittleEndianBytes(extra is null ? Constants.STR_FLAG_RAW : Constants.STRW_FLG_RAW).AsMemory()).ConfigureAwait(false);
            await stream.WriteAsync(ByteUtils.GetLittleEndianBytes(t.Value.Length).AsMemory()).ConfigureAwait(false);
            await stream.WriteAsync(Encoding.Unicode.GetBytes(t.Value)
                .Use(x => CsfUtils.CodingValue(x))
                .Use(x => ByteUtils.ReverseEndianIfNotLittleEndian(x)).AsMemory()).ConfigureAwait(false);
            if (extra is not null)
            {
                await stream.WriteAsync(ByteUtils.GetLittleEndianBytes(extra.Extra.Length).AsMemory()).ConfigureAwait(false);
                await stream.WriteAsync(Encoding.ASCII.GetBytes(extra.Extra)
                    .Use(x => ByteUtils.ReverseEndianIfNotLittleEndian(x)).AsMemory()).ConfigureAwait(false);
            }
        }
    }
}