namespace Shimakaze.Models.Csf;

public sealed record CsfLabel
{
    public string Label { get; set; } = string.Empty;

    public CsfValue[] Values { get; set; } = Array.Empty<CsfValue>();
}