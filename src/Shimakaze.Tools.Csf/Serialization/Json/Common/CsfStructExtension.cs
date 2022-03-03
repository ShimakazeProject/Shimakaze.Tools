using Shimakaze.Models.Csf;

namespace Shimakaze.Tools.Csf.Serialization.Json.Common;

internal static class CsfStructExtension
{
    public static void ReCount(this CsfStruct csf)
    {
        CsfHead head = csf.Head;
        head.LabelCount = csf.Datas.Length;
        head.StringCount = csf.Datas.Select(x => x.Values.Length).Sum();
        csf.Head = head;
    }
}