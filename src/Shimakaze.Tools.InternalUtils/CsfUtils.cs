namespace Shimakaze.Tools.InternalUtils;

internal static class CsfUtils
{
    /// <summary>
    /// 值字符串 编/解码<br/>
    /// CSF文档中的Unicode编码内容都是按位异或的<br/>
    /// 这个方法使用for循环实现
    /// </summary>
    /// <param name="valueData">内容</param>
    /// <param name="valueDataLength">内容长度</param>
    public static byte[] CodingValue(byte[] valueData, int start = 0, int? valueDataLength = null)
    {
        if (valueDataLength is null)
        {
            valueDataLength = valueData.Length;
        }

        for (int i = 0; i < valueDataLength; i++)
        {
            valueData[start + i] = (byte)~valueData[start + i];
        }

        return valueData;
    }
}