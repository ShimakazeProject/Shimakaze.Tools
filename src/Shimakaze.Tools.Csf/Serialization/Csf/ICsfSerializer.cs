namespace Shimakaze.Tools.Csf.Serialization.Csf;

#if PREVIEW
[System.Runtime.Versioning.RequiresPreviewFeatures]
#endif
public interface ICsfSerializer<T>
{
#if PREVIEW
    static abstract void Serialize(BinaryWriter writer, T t);
    static abstract T Deserialize(BinaryReader reader);
#endif
}
