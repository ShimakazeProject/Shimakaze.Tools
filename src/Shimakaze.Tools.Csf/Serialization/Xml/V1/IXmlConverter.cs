using System.Xml;

namespace Shimakaze.Tools.Csf.Serialization.Xml.V1;

public interface IXmlConverter<T>
{
    T Deserialize(XmlReader reader);

    void Serialize(XmlWriter writer, T value);
}