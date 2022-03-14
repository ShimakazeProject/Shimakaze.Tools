using Shimakaze.Models.Csf;

using YamlDotNet.Core;
using YamlDotNet.Serialization;

namespace Shimakaze.Tools.Csf.Serialization.Yaml.V1;
public sealed class CsfValueYamlConverter : IYamlTypeConverter
{
    public bool Accepts(Type type)
    {
        return typeof(CsfValue).IsAssignableFrom(type);
    }

    public object? ReadYaml(IParser parser, Type type)
    {
        throw new NotImplementedException();
    }

    public void WriteYaml(IEmitter emitter, object? value, Type type)
    {
        throw new NotImplementedException();
    }
}
