
using System;
using System.Linq;
using System.Xml;
using System.Xml.Serialization;

namespace Shimakaze.Tools.Csf.Serialization.Xml.V1
{
    public interface IXmlConverter<T>
    {
        T Deserialize(XmlReader reader);
        void Serialize(XmlWriter writer, T value);
    }
}