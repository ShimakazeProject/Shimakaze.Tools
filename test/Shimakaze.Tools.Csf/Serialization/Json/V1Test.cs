using System.IO;
using System.Text.Json;

using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Shimakaze.Tools.Csf.Serialization.Json.Test
{
    [TestClass]
    public class V1Test : TestBase
    {
        protected override JsonSerializerOptions Options => V1.CsfJsonConverterUtils.CsfJsonSerializerOptions;
        protected override string JsonFilePath => Path.Combine("Assets", "v1.json");
        protected override string FaildJsonFileNameWithoutExtension => "v1";
    }
}
