using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shimakaze.Tools.Csf.InternalUtils
{
    internal static class CsfUtils
    {
        /// <summary>
        /// 值字符串 编/解码<br/>
        /// CSF文档中的Unicode编码内容都是按位异或的<br/>
        /// 这个方法使用for循环实现
        /// </summary>
        /// <param name="ValueData">内容</param>
        /// <param name="ValueDataLength">内容长度</param>
        public static void CodingValue(byte[] ValueData, int start = 0, int? ValueDataLength = null)
        {
            if (ValueDataLength is null)
                ValueDataLength = ValueData.Length;
            for (var i = 0; i < ValueDataLength; i++)
                ValueData[start + i] = (byte)~ValueData[start + i];
        }
    }
}