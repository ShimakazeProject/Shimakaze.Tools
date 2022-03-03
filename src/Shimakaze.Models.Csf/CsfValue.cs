namespace Shimakaze.Models.Csf;

public record CsfValue(string Value);
public sealed record CsfExtraValue(string Value, string Extra) : CsfValue(Value);