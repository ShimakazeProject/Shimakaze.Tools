
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

using Shimakaze.Models.Mix;

namespace Shimakaze.Tools.Mix
{
    public class MixBuilder
    {
        public MixBuilder(List<FileInfo> files, IdCalculater idCalculater, bool writeFlag = true)
        {
            Files = files;
            IdCalculater = idCalculater;
            WriteFlag = writeFlag;
        }
        public MixBuilder(IEnumerable<FileInfo> files, IdCalculater idCalculater, bool writeFlag = true)
        {
            Files = new(files);
            IdCalculater = idCalculater;
            WriteFlag = writeFlag;
        }

        public List<FileInfo> Files { get; init; } = new();

        public IdCalculater IdCalculater { get; set; }
        public bool WriteFlag { get; set; } = true;

        public virtual void Build(Stream stream, TextWriter writer)
        {
            using BinaryWriter sw = new(stream, Encoding.ASCII, true);
            uint flag = (uint)MixFileFlag.NONE;
            short fileCount = (short)Files.Count;
            uint fileSize;

            var map = Files.ToDictionary(x => IdCalculater(x.Name));
            fileSize = (uint)map.Values.Select(x => x.Length).Sum();

            if (WriteFlag)
                Console.WriteLine($"Flag: {flag}");
            Console.WriteLine($"File Count: {fileCount}");
            Console.WriteLine($"File Size: {fileSize}");

            if (WriteFlag)
                sw.Write(flag);
            sw.Write(fileCount);
            sw.Write(fileSize);
            using MemoryStream ms = new();

            Console.WriteLine("==============================================================");
            Console.WriteLine("    No    |     ID     |   Offset   |    Size    |    Name    ");

            int i = 0;
            foreach (var item in map)
            {
                i++;
                var (id, file) = (item.Key, item.Value);
                writer.WriteLine($"0x{id:X8} : {file.Name}");
                Console.WriteLine($" {i:D8} | 0x{id:X8} | 0x{ms.Position:X8} | 0x{file.Length:X8} | {file.Name}");

                sw.Write(id);
                sw.Write((int)ms.Position);
                sw.Write((int)file.Length);
                using var fs = file.OpenRead();
                fs.CopyTo(ms);
            }
            Console.WriteLine("==============================================================");

            ms.Seek(0, SeekOrigin.Begin);
            ms.CopyTo(stream);
            writer.Flush();
        }

    }
}