using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using Shimakaze.Models.Csf;
using Shimakaze.Tools.InternalUtils;

namespace Shimakaze.Tools.Csf.Serialization.Csf
{
    public sealed class CsfLabelSerializer : ICsfSerializer<CsfLabel>, IAsyncCsfSerializer<CsfLabel>
    {
        private CsfValueSerializer? _valueSerializer = null;
        private CsfValueSerializer CsfValueSerializer => _valueSerializer ??= new();

        public byte[] Serialize(CsfLabel t)
        {
            List<byte> result = new();
            // Copy Flag
            result.AddRange(ByteUtils.GetLittleEndianBytes(Constants.LBL_FLAG_RAW));

            result.AddRange(ByteUtils.GetLittleEndianBytes(t.Values.Length));

            result.AddRange(ByteUtils.GetLittleEndianBytes(t.Label.Length));

            result.AddRange(Encoding.ASCII.GetBytes(t.Label).Use(x => ByteUtils.ReverseEndianIfNotLittleEndian(x)));

            t.Values.Select(x => CsfValueSerializer.Serialize(x)).Each(result.AddRange);

            return result.ToArray();
        }

        public CsfLabel Deserialize(byte[] raw)
        {
            byte[] data = (byte[])raw.Clone();
            ByteUtils.ReverseEndianIfNotLittleEndian(data);
            if (Constants.LBL_FLAG_RAW != BitConverter.ToInt32(data))
                throw new ArgumentException("It's not CSF Label Flag");

            int count = BitConverter.ToInt32(data, 4);
            int lbllength = BitConverter.ToInt32(data, 8);
            string lableName = Encoding.ASCII.GetString(data, 12, lbllength);

            List<CsfValue> values = new();
            for (int i = 0; i < count; i++)
                values.Add(CsfValueSerializer.Deserialize(raw.Skip(12 + lbllength).ToArray()));

            return new()
            {
                Label = lableName,
                Values = values.ToArray()
            };
        }

        public async Task<CsfLabel> DeserializeAsync(Stream stream, byte[] buffer)
        {
            buffer.CheckLength(12);
            await stream.ReadAsync(buffer.AsMemory(0, 12)).ConfigureAwait(false);
            ByteUtils.ReverseEndianIfNotLittleEndian(buffer, 12);

            if (Constants.LBL_FLAG_RAW != BitConverter.ToInt32(buffer))
                throw new ArgumentException("It's not CSF Label Flag");

            int count = BitConverter.ToInt32(buffer, 4);
            int lbllength = BitConverter.ToInt32(buffer, 8);

            buffer.CheckLength(lbllength);
            await stream.ReadAsync(buffer.AsMemory(0, lbllength)).ConfigureAwait(false);
            ByteUtils.ReverseEndianIfNotLittleEndian(buffer, lbllength);

            string lableName = Encoding.ASCII.GetString(buffer);

            List<CsfValue> values = new();
            for (int i = 0; i < count; i++)
                values.Add(await CsfValueSerializer.DeserializeAsync(stream, buffer));

            return new()
            {
                Label = lableName,
                Values = values.ToArray()
            };
        }

        public async Task SerializeAsync(CsfLabel t, Stream stream)
        {
            // Copy Flag
            await stream.WriteAsync(ByteUtils.GetLittleEndianBytes(Constants.LBL_FLAG_RAW).AsMemory()).ConfigureAwait(false);

            await stream.WriteAsync(ByteUtils.GetLittleEndianBytes(t.Values.Length).AsMemory()).ConfigureAwait(false);

            await stream.WriteAsync(ByteUtils.GetLittleEndianBytes(t.Label.Length).AsMemory()).ConfigureAwait(false);

            await stream.WriteAsync(Encoding.ASCII.GetBytes(t.Label)
                .Use(x => ByteUtils.ReverseEndianIfNotLittleEndian(x)).AsMemory()).ConfigureAwait(false);

            await t.Values.Select(x => CsfValueSerializer.Serialize(x))
                          .EachAsync(async x => await stream.WriteAsync(x.AsMemory()).ConfigureAwait(false))
                          .ConfigureAwait(false);
        }
    }
}