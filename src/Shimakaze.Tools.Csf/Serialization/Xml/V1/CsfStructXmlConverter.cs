using System.Xml;

using Shimakaze.Models.Csf;

namespace Shimakaze.Tools.Csf.Serialization.Xml.V1
{
    public class CsfStructXmlConverter : IXmlConverter<CsfStruct>
    {
        protected CsfLabelsXmlConverter csfLabelsXmlConverter = new();
        public CsfStruct Deserialize(XmlReader reader)
        {
            CsfHead head = new();
            while (reader.Read())
            {
                if (reader.NodeType is XmlNodeType.Whitespace or XmlNodeType.XmlDeclaration or XmlNodeType.ProcessingInstruction)
                    continue;
                if (reader.NodeType is XmlNodeType.Element && reader.Name is "Resources")
                {
                    if (int.TryParse(reader.GetAttribute("version"), out var v))
                        head.Version = v;
                    if (int.TryParse(reader.GetAttribute("language"), out var l))
                        head.Language = l;
                    break;
                }
            }
            var labels = csfLabelsXmlConverter.Deserialize(reader);

            return new()
            {
                Head = head,
                Datas = labels
            };
        }

        public void Serialize(XmlWriter writer, CsfStruct value)
        {
            writer.WriteStartDocument();
            writer.WriteProcessingInstruction("xml-model", $"href=\"{Constants.SchemaUrls.V1}\" type=\"application/xml\" schematypens=\"http://www.w3.org/2001/XMLSchema\"");

            // <Resources protocol="1" version="3" language="0">
            writer.WriteStartElement("Resources");
            writer.WriteAttributeString("protocol", "1");
            writer.WriteAttributeString("version", value.Head.Version.ToString());
            writer.WriteAttributeString("language", value.Head.Version.ToString());

            csfLabelsXmlConverter.Serialize(writer, value.Datas);

            // </Resources>
            writer.WriteEndElement();
            writer.WriteEndDocument();
        }
    }
}