using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Threading.Tasks;
using Shimakaze.Models.Csf;
using Shimakaze.Tools.InternalUtils;

namespace Shimakaze.Tools.Csf.Serialization.Csf
{
    public sealed class CsfStructSerializer : ICsfSerializer<CsfStruct>, IAsyncCsfSerializer<CsfStruct>
    {
        private CsfHeadSerializer? _headSerializer = null;
        private CsfHeadSerializer CsfHeadSerializer => _headSerializer ??= new();
        private CsfLabelSerializer? _labelSerializer = null;
        private CsfLabelSerializer CsfLabelSerializer => _labelSerializer ??= new();
        public CsfStruct Deserialize(byte[] data)
        {
            CsfHead csfHead = CsfHeadSerializer.Deserialize(data);
            CsfLabel[] csfLabels = new CsfLabel[csfHead.LabelCount];
            for (int i = 0; i < csfHead.LabelCount; i++)
                csfLabels[i] = CsfLabelSerializer.Deserialize(data);

            return new()
            {
                Head = csfHead,
                Datas = csfLabels
            };
        }

        public async Task<CsfStruct> DeserializeAsync(Stream stream, byte[] buffer)
        {
            CsfHead csfHead = await CsfHeadSerializer.DeserializeAsync(stream, buffer);
            CsfLabel[] csfLabels = new CsfLabel[csfHead.LabelCount];
            for (int i = 0; i < csfHead.LabelCount; i++)
                csfLabels[i] = await CsfLabelSerializer.DeserializeAsync(stream, buffer);

            return new()
            {
                Head = csfHead,
                Datas = csfLabels
            };
        }

        public byte[] Serialize(CsfStruct t)
        {
            List<byte> bytes = new();
            bytes.AddRange(CsfHeadSerializer.Serialize(t.Head));
            t.Datas.Select(CsfLabelSerializer.Serialize).Each(bytes.AddRange);
            return bytes.ToArray();
        }

        public async Task SerializeAsync(CsfStruct t, Stream stream)
        {
            await CsfHeadSerializer.SerializeAsync(t.Head, stream);
            await t.Datas.EachAsync(x => CsfLabelSerializer.SerializeAsync(x, stream));
        }
    }
}