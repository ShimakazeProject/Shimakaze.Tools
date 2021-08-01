using System.IO;
using System.Text.Json;

using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Shimakaze.Tools.Csf.Serialization.Json.Test
{
    [TestClass]
    public class V2Test : TestBase
    {
        protected override JsonSerializerOptions Options => V2.CsfJsonConverterUtils.CsfJsonSerializerOptions;
        protected override string JsonFilePath => Path.Combine("Assets", "v2.json");
        protected override string FaildJsonFileNameWithoutExtension => @"v2";
    }
}
