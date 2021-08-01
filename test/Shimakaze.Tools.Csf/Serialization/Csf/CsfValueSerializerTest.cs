
using Microsoft.VisualStudio.TestTools.UnitTesting;

using Shimakaze.Tools.Csf.Serialization.Csf;

namespace Shimakaze.Models.Csf.Serialization.Csf.Test
{

    [TestClass]
    public class CsfValueSerializerTest : SerializerTestBase<CsfValueSerializer, CsfValue>
    {
        protected override byte[] TestData => new byte[]
        {
            0x20, 0x52, 0x54, 0x53,
            0x05, 0x00, 0x00, 0x00,
            0xB5, 0xAA, 0x26, 0x70, 0xD1, 0xFF, 0xD1, 0xFF, 0xD1, 0xFF
        };

        protected override CsfValue TestObject => new("啊这...");


    }
}