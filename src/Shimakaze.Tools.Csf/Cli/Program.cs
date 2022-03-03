
using Shimakaze.Models.Csf;

namespace Shimakaze.Tools.Csf.Cli;

internal sealed record WorkerInfo(
    string InputFullPath,
    string FilenameWithoutExtension,
    string InputExtension,
    string BasePath,
    string OutputFullPath)
{
    public static WorkerInfo GetInfo(string[] args, string output_extension, int baseIndex = 1)
    {
        string filenameWithoutExtension = Path.GetFileNameWithoutExtension(args[baseIndex]);
        string extension = Path.GetExtension(args[baseIndex]).ToLower();
        string basepath = Path.GetDirectoryName(args[baseIndex]) ?? throw new InvalidOperationException("传入了null或根目录");
        string outputPath = args.Length > baseIndex + 1
            ? args[baseIndex + 1]
            : Path.Combine(basepath, filenameWithoutExtension + output_extension);

        return new(args[baseIndex], filenameWithoutExtension, extension, basepath, outputPath);
    }
}

internal class Program
{
    private static async Task Main(string[] args)
    {
        // 解析谓词
        if (args.Length < 1)
        {
            throw new Exception("参数过少");
        }

        switch (args[0])
        {
            case "json":
            case "jsonv2":
                WorkerInfo info = WorkerInfo.GetInfo(args, ".json");
                CsfStruct csf = await GetCsf(args, info);
                await CsfJsonTools.WriteAsync(File.Create(info.OutputFullPath), csf, 2).ConfigureAwait(false);
                break;
            case "jsonv1":
                info = WorkerInfo.GetInfo(args, ".json");
                csf = await GetCsf(args, info);
                await CsfJsonTools.WriteAsync(File.Create(info.OutputFullPath), csf, 1).ConfigureAwait(false);
                break;
            case "xml":
            case "xmlv1":
                info = WorkerInfo.GetInfo(args, ".xml");
                csf = await GetCsf(args, info);
                CsfXmlTools.Write(File.Create(info.OutputFullPath), csf, 1);
                break;
            case "csf":
                info = WorkerInfo.GetInfo(args, ".csf");
                csf = await GetCsf(args, info);
                CsfBinaryTools.Write(File.Create(info.OutputFullPath), csf);
                break;
            default:
                info = WorkerInfo.GetInfo(args, ".json");
                csf = await GetCsf(args, info);
                switch (info.InputExtension)
                {
                    case ".json":
                        CsfBinaryTools.Write(File.Create(info.OutputFullPath), csf);
                        break;
                    case ".csf":
                        await CsfJsonTools.WriteAsync(File.Create(info.OutputFullPath.Replace(".json", ".csf")), csf, 2).ConfigureAwait(false);
                        break;
                    case ".xml":
                        CsfBinaryTools.Write(File.Create(info.OutputFullPath), csf);
                        break;
                    default:
                        throw new NotSupportedException("不支持的文件扩展名");
                }
                break;
        }

        Console.WriteLine("结束");
    }

    private static int GetInt(string[] args, int @default, int baseIndex = 2)
    {
        return (args.Length > baseIndex + 1
            && int.TryParse(args[baseIndex + 1], out int result))
            || (args.Length > baseIndex
            && int.TryParse(args[baseIndex], out result))
            ? result
            : @default;
    }

    private static async Task<CsfStruct> GetCsf(string[] args, WorkerInfo info, int baseIndex = 1)
    {
        return info.InputExtension switch
        {
            ".json" => await CsfJsonTools.LoadAsync(File.OpenRead(info.InputFullPath), GetInt(args, 2, baseIndex + 1)).ConfigureAwait(false),
            ".csf" => CsfBinaryTools.Load(File.OpenRead(info.InputFullPath)),
            ".xml" => CsfXmlTools.Load(File.OpenRead(info.InputFullPath), GetInt(args, 1, baseIndex + 1)),
            _ => throw new NotSupportedException("不支持的文件扩展名"),
        };
    }

}