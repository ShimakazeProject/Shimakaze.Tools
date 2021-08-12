using System.Xml;

using Shimakaze.Models.Csf;

namespace Shimakaze.Tools.Csf.Serialization.Xml.V1
{
    public class CsfValueXmlConverter : IXmlConverter<CsfValue>
    {
        public CsfValue Deserialize(XmlReader reader)
        {
            string? extra = null;
            string value = string.Empty;

            if (reader.NodeType is XmlNodeType.Text)
                value += reader.Value;
            else
            {
                if (reader.NodeType is XmlNodeType.Element)
                    extra = reader.GetAttribute("extra");
                while (reader.Read())
                {
                    if (reader.NodeType is XmlNodeType.Text)
                        value += reader.Value;
                    else if (reader.NodeType is XmlNodeType.EndElement && reader.Name is "Value" or "Label")
                        break;
                }
            }
            return string.IsNullOrWhiteSpace(extra) ? new CsfValue(value) : new CsfExtraValue(value, extra);
        }

        public void Serialize(XmlWriter writer, CsfValue value)
        {
            if (value is CsfExtraValue extra)
                writer.WriteAttributeString("extra", extra.Extra);
            writer.WriteString(value.Value);
        }
    }
}