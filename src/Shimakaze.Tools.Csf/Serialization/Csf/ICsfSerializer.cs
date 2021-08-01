using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;

namespace Shimakaze.Tools.Csf.Serialization.Csf
{
    public interface ICsfSerializer<T>
    {
        T Deserialize(byte[] data);
        byte[] Serialize(T t);
    }
}