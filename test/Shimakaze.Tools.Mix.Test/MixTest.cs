using System;
using System.IO;
using System.Runtime.CompilerServices;
using System.Text;

using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Shimakaze.Tools.Mix.Test
{
    [TestClass]
    public class MixTest
    {
        [TestMethod("Expand a TS mix file and the file made by the lasted XCC Mix Editor")]
        public void ExpandTSMixByXCCTest() => ExpandMixByXCCTest("TS");


        [TestMethod("Expand a TD mix file and the file made by the lasted XCC Mix Editor")]
        public void ExpandTDMixByXCCTest() => ExpandMixByXCCTest("TD", true);


        [TestMethod("Build a MIX file. ID is calc by TS. then Expand that.")]
        public void BuildTSIdMixTest() => BuildMixTest("TS", IdCalculaters.TSIdCalculater);


        [TestMethod("Build a MIX file. ID is calc by TD. then Expand that.")]
        public void BuildTDIdMixTest() => BuildMixTest("TD", IdCalculaters.OldIdCalculater, false);

        private static void ExpandMixByXCCTest(string name, bool noFlag = false, [CallerMemberName] string testName = null)
        {
            if (!Directory.Exists("Out"))
                Directory.CreateDirectory("Out");
            if (!Directory.Exists(Path.Combine("Out", name)))
                Directory.CreateDirectory(Path.Combine("Out", name));

            using var fs = File.OpenRead(Path.Combine("Assets", name + ".mix"));

            MixExpander expander = new(fs, Path.Combine("Out", name), new byte[1024], noFlag: noFlag);
            expander.Expand();

            if (File.ReadAllText(Path.Combine("Out", name, "File.txt")) != File.ReadAllText(Path.Combine("Assets", "File.txt")))
                throw new Exception(testName + " Test failed.");
        }

        private static void BuildMixTest(string name, IdCalculater idCalculater, bool writeFlag = true, [CallerMemberName] string callerName = null)
        {
            string path = Path.Combine("Assets", "File.txt");
            string outPath = Path.Combine("Out", name, "Build.mix");
            if (!Directory.Exists("Out"))
                Directory.CreateDirectory("Out");
            if (!Directory.Exists(Path.Combine("Out", name)))
                Directory.CreateDirectory(Path.Combine("Out", name));

            using FileStream fs = File.Create(outPath);
            using MemoryStream mapfs = new();
            using StreamWriter sw = new(mapfs, leaveOpen: true);
            using StreamReader sr = new(mapfs);

            MixBuilder builder = new(new[] { new FileInfo(path) }, idCalculater, writeFlag);
            builder.Build(fs, sw);

            fs.Seek(0, SeekOrigin.Begin);
            mapfs.Seek(0, SeekOrigin.Begin);

            MixExpander expander = new(fs, Path.Combine("Out", name), new byte[1024], sr);
            expander.Expand();

            if (File.ReadAllText(Path.Combine("Out", name, "File.txt")) != File.ReadAllText(Path.Combine("Assets", "File.txt")))
                throw new Exception(callerName + " Test failed.");
        }
    }
    [TestClass]
    public class IdCalculaterTest
    {
        [TestMethod]
        public void TDIdCalculaterTest() => IdCalcTest(IdCalculaters.OldIdCalculater, "TD", -4);
        [TestMethod]
        public void TSIdCalculaterTest() => IdCalcTest(IdCalculaters.TSIdCalculater, "TS");
        private static void IdCalcTest(IdCalculater idCalculater, string mixName, int offset = 0, [CallerMemberName] string callerName = null)
        {
            using var fs = File.OpenRead(Path.Combine("Assets", mixName + ".mix"));
            using BinaryReader br = new(fs);
            fs.Seek(10 + offset, SeekOrigin.Begin);
            var xccid = br.ReadUInt32();
            var id = idCalculater("File.txt");
            Console.WriteLine($"xccid: 0x{xccid:X8} |id: 0x{id:X8}");
            if (xccid != id)
                throw new Exception($"{callerName} Test failed. xccid: 0x{xccid:X8} |id: 0x{id:X8}");
        }
    }
}