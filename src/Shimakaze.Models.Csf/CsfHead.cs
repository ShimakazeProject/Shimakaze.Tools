using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace Shimakaze.Models.Csf;

[NativeCppClass]
[StructLayout(LayoutKind.Explicit, CharSet = CharSet.Ansi, Pack = 4, Size = 20)]
public struct CsfHead : IEquatable<CsfHead>
{
    [FieldOffset(00)] private int version;
    [FieldOffset(04)] private int labelCount;
    [FieldOffset(08)] private int stringCount;
    [FieldOffset(12)] private int unknown;
    [FieldOffset(16)] private int language;

    public int Version { get => version; set => version = value; }

    public int LabelCount { get => labelCount; set => labelCount = value; }

    public int StringCount { get => stringCount; set => stringCount = value; }

    public int Unknown { get => unknown; set => unknown = value; }

    public int Language { get => language; set => language = value; }

    public override bool Equals(object? obj)
    {
        return obj is CsfHead head && Equals(head);
    }

    public bool Equals(CsfHead other)
    {
        return version == other.version &&
                labelCount == other.labelCount &&
                stringCount == other.stringCount &&
                unknown == other.unknown &&
                language == other.language;
    }

    public override int GetHashCode()
    {
        return HashCode.Combine(version, labelCount, stringCount, unknown, language);
    }

    public override string ToString()
    {
        return $"Version = {Version}, LabelCount = {LabelCount}, StringCount = {StringCount}, Unknown = {Unknown}, Language = {Language}";
    }

    public static bool operator ==(CsfHead left, CsfHead right)
    {
        return left.Equals(right);
    }

    public static bool operator !=(CsfHead left, CsfHead right)
    {
        return !(left == right);
    }
}