using System.Xml;

using Shimakaze.Models.Csf;
using Shimakaze.Tools.Csf.Translater.Apis;
using Shimakaze.Tools.InternalUtils;

namespace Shimakaze.Tools.Csf.Translater;

public class Generator
{
    public Generator(ITranslator? translator = default, int qps = 10)
    {
        Translator = translator;
        QPS = qps;
    }

    public ITranslator? Translator { get; set; }
    public int QPS { get; set; }

    public async Task<XmlDocument> GeneratI18nDocumentAsync(CsfLabel[] labels, Action<int>? progressCallback = default)
    {
        XmlDocument doc = new();
        // <I18n>
        XmlElement root = doc.CreateElement("I18n");

        int i = 0;
        await labels.Select(async label =>
        {
            progressCallback?.Invoke(i++);
            return await CreateLabelElementAsync(doc, label);
        }).EachAsync(async x => root.AppendChild(await x));

        doc.AppendChild(root);
        // </I18n>
        return doc;
    }

    public async Task AppendI18nDocumentAsync(CsfLabel[] labels, XmlDocument doc)
    {
        var root = doc.DocumentElement;

        if (root is null)
            throw new Exception("Invalid I18n document");

        Dictionary<string, XmlElement> elements = new();
        root.ChildNodes.Each((XmlElement element) =>
        {
            var name = element.Name switch
            {
                not "Label" => throw new Exception("Invalid XML"),
                _ => element.GetAttribute("name")
            };
            elements.Add(name, element);
        });

        await labels
            .Where(label => elements.ContainsKey(label.Label))
            .Where(label => elements[label.Label].ChildNodes.Count != label.Values.Length)
            .EachAsync(async label =>
            {
                var element = elements[label.Label];
                if (label.Values.Length > element.ChildNodes.Count)
                {
                    for (int i = element.ChildNodes.Count; i < label.Values.Length; i++)
                        element.AppendChild(await CreateValueElementAsync(doc, label.Values[i]));
                }
                else
                {
                    for (int i = label.Values.Length; i < element.ChildNodes.Count; i++)
                        element.RemoveChild(element.ChildNodes[i]!);
                }
            });

        await labels
            .Where(label => !elements.ContainsKey(label.Label))
            .Select(async label => await CreateLabelElementAsync(doc, label))
            .EachAsync(async x => root.AppendChild(await x));
    }

    public static CsfLabel[] GetAllTargetLabels(XmlDocument doc, bool failback = true)
    {
        var root = doc.DocumentElement;

        if (root is null)
            throw new Exception("Invalid I18n document");

        List<CsfLabel> labels = new();

        root.ChildNodes.Each((XmlElement element) =>
        {
            List<CsfValue> values = new();
            element.ChildNodes.Each((XmlElement value) =>
            {
                var target = value["Target"];

                switch (target?.NodeType)
                {
                    case null:
                    case XmlNodeType.None:
                    case XmlNodeType.Whitespace:
                    case XmlNodeType.SignificantWhitespace:
                        target = failback ? value["Source"] : throw new Exception("Invalid XML");
                        break;
                }

                if (target is null)
                    throw new Exception("Invalid XML");

                string? extra = default;
                if (value.HasAttribute("extra"))
                    extra = value.GetAttribute("extra");

                values.Add(string.IsNullOrEmpty(extra) ? new CsfValue(target.InnerText) : new CsfExtraValue(target.InnerText, extra));
            });

            labels.Add(new()
            {
                Label = element.GetAttribute("name"),
                Values = values.ToArray(),
            });
        });

        return labels.ToArray();
    }

    private async Task<XmlElement> CreateValueElementAsync(XmlDocument doc, CsfValue value)
    {
        // <Value>
        XmlElement valueElement = doc.CreateElement("Value");
        valueElement.SetAttribute("status", Translator is not null ? "MachineTranslated" : "Untranslated");

        if (value is CsfExtraValue extra)
            valueElement.SetAttribute("extra", extra.Extra);

        // <Source>
        XmlElement source = doc.CreateElement("Source");
        source.InnerText = value.Value;
        valueElement.AppendChild(source);
        // </Source>

        // <Target>
        XmlElement target = doc.CreateElement("Target");
        try
        {
            target.InnerText = Translator is not null && !string.IsNullOrWhiteSpace(value.Value) ? await Translator.TranslateAsync(value.Value) : string.Empty;
            if (Translator is not null)
                await Task.Delay(1000 / QPS);
        }
        catch
        {
            valueElement.SetAttribute("status", "Untranslated");
            target.InnerText = string.Empty;
        }
        valueElement.AppendChild(target);
        // </Target>


        return valueElement;
        // </Value>
    }

    private async Task<XmlElement> CreateLabelElementAsync(XmlDocument doc, CsfLabel label)
    {
        // <Label name="label">
        XmlElement element = doc.CreateElement("Label");
        element.SetAttribute("name", label.Label);

        await label.Values.EachAsync(async v => element.AppendChild(await CreateValueElementAsync(doc, v)));

        return element;
        // </Label>
    }
}