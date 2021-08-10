
using Microsoft.VisualStudio.TestTools.UnitTesting;

using Shimakaze.Tools.Csf.Serialization.Csf;

namespace Shimakaze.Tools.Csf.Serialization.Csf.Test
{
    [TestClass]
    public class CsfLabelSerializerTest : SerializerTestBase<CsfLabelSerializer, CsfLabel>
    {
        protected override byte[] TestData => new byte[]
        {
            0x20, 0x4C, 0x42, 0x4C,
            0x01, 0x00, 0x00, 0x00,
            0x0B, 0x00, 0x00, 0x00,
            0x54, 0x48, 0x45, 0x4D, 0x45, 0x3A, 0x49, 0x6E, 0x74, 0x72, 0x6F,
            0x20, 0x52, 0x54, 0x53,
            0x02, 0x00, 0x00, 0x00,
            0xFF, 0xA0, 0xC5, 0xA8
        };

        protected override CsfLabel TestObject => new()
        {
            Label = "THEME:Intro",
            Values = new CsfValue[] { new("开场") }
        };

    }
}