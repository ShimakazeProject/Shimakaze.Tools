using System;
using System.IO;
using System.Text.Json;
using System.Threading.Tasks;
using System.Xml;

using Microsoft.VisualStudio.TestTools.UnitTesting;

using Shimakaze.Models.Csf;
using Shimakaze.Models.Csf.Serialization;

namespace Shimakaze.Tools.Csf.Serialization.Xml.Test
{
    [TestClass]
    public class UnitTest
    {
        protected JsonSerializerOptions Options => Json.V2.CsfJsonConverterUtils.CsfJsonSerializerOptions;
        protected string JsonFilePath => Path.Combine("Assets", "v2.json");

        [TestMethod]
        public async Task SerializeTest()
        {
            if (!Directory.Exists("Out"))
                Directory.CreateDirectory("Out");

            using var fs = File.OpenRead(JsonFilePath);
            CsfStruct? csf = await JsonSerializer.DeserializeAsync<CsfStruct>(fs, Options);

            V1.CsfStructXmlConverter converter = new();

            XmlWriterSettings settings = new();
            settings.Indent = true;
            settings.ConformanceLevel = ConformanceLevel.Auto;
            settings.IndentChars = "\t";
            settings.OmitXmlDeclaration = false;

            using XmlWriter writer = XmlWriter.Create(File.Create(Path.Combine("Out", "v1.xml")), settings);

            converter.Serialize(writer, csf!);

        }
        [TestMethod]
        public async Task DeserializeTest()
        {
            if (!Directory.Exists("Out"))
                Directory.CreateDirectory("Out");

            using XmlReader reader = XmlReader.Create(File.OpenRead(Path.Combine("Assets", "v1.xml")));

            V1.CsfStructXmlConverter converter = new();

            var csf = converter.Deserialize(reader);

            using var fs = File.Create(Path.Combine("Out", "v2.json"));
            await JsonSerializer.SerializeAsync(fs, csf, Options);
        }
    }
}