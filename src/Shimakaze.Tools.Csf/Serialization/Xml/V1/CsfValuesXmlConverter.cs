using System.Collections.Generic;
using System.Xml;

using Shimakaze.Models.Csf;
using Shimakaze.Tools.InternalUtils;

namespace Shimakaze.Tools.Csf.Serialization.Xml.V1
{
    public class CsfValuesXmlConverter : IXmlConverter<CsfValue[]>
    {
        // TODO: Remove this property
        internal protected CsfValueXmlConverter csfValueXmlConverter = new();
        public CsfValue[] Deserialize(XmlReader reader)
        {
            List<CsfValue> values = new();
            while (reader.Read())
            {
                if (reader.NodeType is XmlNodeType.Whitespace)
                    continue;
                if (reader.NodeType is XmlNodeType.Element)
                {
                    switch (reader.Name)
                    {
                        case "Value":
                            values.Add(csfValueXmlConverter.Deserialize(reader));
                            break;
                        case "Values":
                            while (reader.Read())
                            {
                                if (reader.NodeType is XmlNodeType.Whitespace)
                                    continue;
                                if (reader.NodeType is XmlNodeType.Element && reader.Name is "Value")
                                    values.Add(csfValueXmlConverter.Deserialize(reader));
                                else if (reader.NodeType is XmlNodeType.EndElement && reader.Name is "Values")
                                    break;
                            }
                            break;

                    }
                }
                else if (reader.NodeType is XmlNodeType.Text)
                    values.Add(csfValueXmlConverter.Deserialize(reader));
                else if (reader.NodeType is XmlNodeType.EndElement && reader.Name is "Label")
                    break;
            }

            if (values.Count is 0)
                values.Add(new(string.Empty));

            return values.ToArray();
        }

        public void Serialize(XmlWriter writer, CsfValue[] value)
        {
            if (value.Length != 1)
            {
                // <Values>
                writer.WriteStartElement("Values");
                value.Each(y =>
                {
                    // <Value>
                    writer.WriteStartElement("Value");
                    csfValueXmlConverter.Serialize(writer, y);
                    // </Value>
                    writer.WriteEndElement();
                });
                // </Values>
                writer.WriteEndElement();
            }
            else
                csfValueXmlConverter.Serialize(writer, value[0]);
        }
    }
}