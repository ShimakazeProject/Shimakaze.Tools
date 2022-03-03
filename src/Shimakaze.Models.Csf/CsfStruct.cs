namespace Shimakaze.Models.Csf;

public sealed class CsfStruct
{
    public CsfHead Head { get; set; }

    public CsfLabel[] Datas { get; set; } = Array.Empty<CsfLabel>();
}