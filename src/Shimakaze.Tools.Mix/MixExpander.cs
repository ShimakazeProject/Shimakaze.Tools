using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Shimakaze.Models.Mix;
using Shimakaze.Tools.InternalUtils;

namespace Shimakaze.Tools.Mix
{
    public class MixExpander
    {
        public Stream Input { get; init; }
        public string OutputPath { get; init; }
        public bool NoFlag { get; set; }
        private readonly byte[] buffer;
        private readonly TextReader? nameMapReader;

        public MixExpander(Stream input, string outputPath, byte[] buffer, TextReader? nameMapReader = default, bool noFlag = false)
        {
            Input = input;
            OutputPath = outputPath;
            NoFlag = noFlag;
            this.nameMapReader = nameMapReader;
            this.buffer = buffer;
        }

        public void Expand()
        {
            using BinaryReader mix = new(Input, Encoding.ASCII, true);

            var (mixHead, mixEntries) = HeaderParser(mix);

            var body_offset = mix.BaseStream.Position;

            var nameMap = NameMapParser(
                mix.BaseStream,
                mixEntries.Any(x => x.Id is Constants.LXD_TS_ID or Constants.LXD_TD_ID)
                        ? mixEntries.First(x => x.Id is Constants.LXD_TS_ID or Constants.LXD_TD_ID)
                        : null,
                body_offset);

            int num = 0;
            Console.WriteLine("Expanding...");
            Console.WriteLine("==============================================================");
            Console.WriteLine("    No    |     ID     |   Offset   |    Size    |    Name    ");
            foreach (var entry in mixEntries)
            {
                num++;
                var name = GetName(entry.Id);
                Console.WriteLine($" {num:D8} | 0x{entry.Id:X8} | 0x{entry.Offset:X8} | 0x{entry.Size:X8} | {name}");
                using var file = File.Create(Path.Combine(OutputPath, name));
                Input.Seek(entry.Offset + body_offset, SeekOrigin.Begin);
                buffer.CheckLength(entry.Size);
                Input.Read(buffer.AsSpan(0, entry.Size));
                file.Write(buffer.AsSpan(0, entry.Size));
            }
            Console.WriteLine("==============================================================");
            Console.WriteLine("All Done!");

            string GetName(uint id) => nameMap is not null && nameMap.TryGetValue(id, out var name) ? name : $"0x{id:X8}";
        }


        protected virtual (MixFileInfo mixHead, MixIndexEntry[] mixEntries) HeaderParser(BinaryReader br)
        {
            MixFileInfo mixHead;
            MixIndexEntry[] mixEntries;

            uint mixFlag = NoFlag ? (uint)MixFileFlag.NONE : br.ReadUInt32();
            if ((mixFlag & (uint)MixFileFlag.ENCRYPTED) != 0)
            {
                throw new NotImplementedException("This Mix File is Encrypted.");
            }
            else
            {
                mixHead = new()
                {
                    Flag = mixFlag,
                    Files = br.ReadInt16(),
                    Size = br.ReadInt32()
                };
                mixEntries = new MixIndexEntry[mixHead.Files];
                for (int i = 0; i < mixHead.Files; i++)
                {
                    mixEntries[i] = new()
                    {
                        Id = br.ReadUInt32(),
                        Offset = br.ReadInt32(),
                        Size = br.ReadInt32()
                    };
                }
            }

            return (mixHead, mixEntries!);
        }


        protected virtual Dictionary<uint, string> NameMapParser(Stream stream, MixIndexEntry? lxd, long body_offset)
        {
            Dictionary<uint, string> fileNameMap = new();
            Task<Dictionary<uint, string>>? t_lnmf = default;
            Task<Dictionary<uint, string>>? t_lxd = default;

            if (nameMapReader is not null)
            {
                t_lnmf = Task.Run(() =>
                {
                    Console.WriteLine("Loading FileMap List");
                    Dictionary<uint, string> fileNameMap = new();
                    while (nameMapReader.Peek() > 0)
                    {
                        var line = nameMapReader.ReadLine();

                        if (string.IsNullOrWhiteSpace(line) || line.StartsWith("#"))
                            continue;

                        var kvp = line.Split(":").Select(x => x.Trim()).ToArray();
                        fileNameMap.Add(Convert.ToUInt32(kvp[0], 16), kvp[1].Split("#")[0]);
                    }
                    return fileNameMap;
                });
            }
            if (lxd is not null)
            {
                t_lxd = Task.Run(() =>
                {
                    Dictionary<uint, string> fileNameMap = new();
                    try
                    {
                        fileNameMap.Add(lxd.Value.Id, "xcc local database.dat");

                        Func<string, uint> GetId = lxd.Value.Id is Constants.LXD_TS_ID
                            ? IdCalculaters.TSIdCalculater
                            : IdCalculaters.OldIdCalculater;

                        Console.WriteLine("Find local xcc database.dat File!");
                        Console.WriteLine("Trying Generat FileName Map.");

                        StringBuilder sb = new();
                        using BinaryReader br = new(stream, Encoding.ASCII, true);
                        stream.Seek(lxd.Value.Offset + body_offset, SeekOrigin.Begin);
                        stream.Seek(48, SeekOrigin.Current);

                        var count = br.ReadInt32();

                        byte ch;
                        for (int i = 0; i < count; i++)
                        {
                            if (stream.Position > lxd.Value.Offset + body_offset + lxd.Value.Size)
                                throw new FileLoadException("Cannot Load Filename from " + fileNameMap[lxd.Value.Id]);

                            sb.Clear();
                            while ((ch = br.ReadByte()) > 0)
                                sb.Append((char)ch);
                            var name = sb.ToString();
                            fileNameMap[GetId(name)] = name;
                        }
                    }
                    catch (Exception ex)
                    {
                        Console.Error.WriteLine(ex);
                    }
                    return fileNameMap;
                });


            }

            t_lxd?.Wait();
            t_lxd?.Result?.Each(x => fileNameMap[x.Key] = x.Value);
            t_lnmf?.Wait();
            t_lnmf?.Result?.Each(x => fileNameMap[x.Key] = x.Value);

            return fileNameMap;
        }
    }
}
