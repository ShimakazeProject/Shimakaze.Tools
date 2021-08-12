using System.Diagnostics;
using System.Xml;

using Shimakaze.Models.Csf;

namespace Shimakaze.Tools.Csf.Serialization.Xml.V1
{
    public class CsfLabelXmlConverter : IXmlConverter<CsfLabel>
    {
        protected CsfValuesXmlConverter csfValuesXmlConverter = new();
        public CsfLabel Deserialize(XmlReader reader)
        {
            CsfLabel label = new();
            if (reader.NodeType is XmlNodeType.Element && reader.Name is "Label")
            {
                var lbl = reader.GetAttribute("name");
                if (lbl is "GUI:Blank")
                    Debugger.Break();

                if (!string.IsNullOrWhiteSpace(lbl))
                    label.Label = lbl;
                label.Values = reader["extra"] switch
                {
                    not null => new[] { csfValuesXmlConverter.csfValueXmlConverter.Deserialize(reader) },
                    _ => csfValuesXmlConverter.Deserialize(reader),
                };
            }
            return label;
        }

        public void Serialize(XmlWriter writer, CsfLabel value)
        {
            // <Label name="label_name">
            writer.WriteStartElement("Label");
            writer.WriteAttributeString("name", value.Label);

            csfValuesXmlConverter.Serialize(writer, value.Values);

            // </Label>
            writer.WriteEndElement();
        }
    }
}