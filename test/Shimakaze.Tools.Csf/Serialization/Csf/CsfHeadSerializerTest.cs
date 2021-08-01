using System;
using System.IO;
using System.Threading.Tasks;

using Microsoft.VisualStudio.TestTools.UnitTesting;

using Shimakaze.Tools.Csf.Serialization.Csf;

namespace Shimakaze.Models.Csf.Serialization.Csf.Test
{
    [TestClass]
    public class CsfHeadSerializerTest : SerializerTestBase<CsfHeadSerializer, CsfHead>
    {
        protected override byte[] TestData => new byte[]
        {
            0x20, 0x46, 0x53, 0x43,
            0x03, 0x00, 0x00, 0x00,
            0x01, 0x00, 0x00, 0x00,
            0x02, 0x00, 0x00, 0x00,
            0xCE, 0x98, 0x9B, 0x5C,
            0x09, 0x00, 0x00, 0x00
        };
        protected override CsfHead TestObject => new()
        {
            Version = 3,
            LabelCount = 1,
            StringCount = 2,
            Language = 9,
            Unknown = 0x5c9b98ce,
        };

    }
}