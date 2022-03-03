using System.Xml;

using Shimakaze.Models.Csf;
using Shimakaze.Tools.InternalUtils;

namespace Shimakaze.Tools.Csf.Serialization.Xml.V1;

public class CsfLabelsXmlConverter : IXmlConverter<CsfLabel[]>
{
    protected CsfLabelXmlConverter csfLabelXmlConverter = new();

    public CsfLabel[] Deserialize(XmlReader reader)
    {
        List<CsfLabel> labels = new();
        while (reader.Read())
        {
            if (reader.NodeType is XmlNodeType.Whitespace)
            {
                continue;
            }

            if (reader.NodeType is XmlNodeType.Element && reader.Name is "Label")
            {
                labels.Add(csfLabelXmlConverter.Deserialize(reader));
            }
            else if (reader.NodeType is XmlNodeType.EndElement && reader.Name is "Resources")
            {
                break;
            }
        }
        return labels.ToArray();
    }

    public void Serialize(XmlWriter writer, CsfLabel[] value)
    {
        value.Each(x => csfLabelXmlConverter.Serialize(writer, x));
    }
}