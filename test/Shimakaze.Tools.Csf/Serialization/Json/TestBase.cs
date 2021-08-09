using System;
using System.IO;
using System.Text.Json;
using System.Threading.Tasks;

using Microsoft.VisualStudio.TestTools.UnitTesting;

using Shimakaze.Models.Csf;

namespace Shimakaze.Tools.Csf.Serialization.Json.Test
{
    public abstract class TestBase
    {
        protected abstract JsonSerializerOptions Options { get; }
        protected abstract string JsonFilePath { get; }
        protected abstract string FaildJsonFileNameWithoutExtension { get; }

        [TestMethod]
        public void Test()
        {
            var file = File.ReadAllText(JsonFilePath);
            CsfStruct csf = JsonSerializer.Deserialize<CsfStruct>(file, Options)!;
            Console.WriteLine(csf.Head);
            var json = JsonSerializer.Serialize(csf, Options)!;

            if (!file.Equals(json))
            {
                if (!Directory.Exists("Out"))
                    Directory.CreateDirectory("Out");
                File.WriteAllText(Path.Combine("Out", FaildJsonFileNameWithoutExtension + ".json"), json);
                throw new Exception("Test failed.");
            }
        }

        [TestMethod]
        public async Task TestAsync()
        {
            using var fs = File.OpenRead(JsonFilePath);
            CsfStruct csf = (await JsonSerializer.DeserializeAsync<CsfStruct>(fs, Options))!;
            Console.WriteLine(csf.Head);
            using MemoryStream ms = new();
            await JsonSerializer.SerializeAsync(ms, csf, Options)!;

            fs.Seek(0, SeekOrigin.Begin);
            ms.Seek(0, SeekOrigin.Begin);
            using StreamReader fsr = new(fs);
            using StreamReader msr = new(ms);
            while (fsr.Peek() != -1 && msr.Peek() != -1)
            {
                if (fsr.Read() != msr.Read())
                {
                    var position = msr.BaseStream.Position;
                    if (!Directory.Exists("Out"))
                        Directory.CreateDirectory("Out");
                    using var ofs = File.Create(Path.Combine("Out", FaildJsonFileNameWithoutExtension + "Async.json"));
                    ms.Seek(0, SeekOrigin.Begin);
                    await ms.CopyToAsync(ofs);
                    throw new Exception("AsyncTest failed. difference at " + position);
                }
            }
        }

    }
}
