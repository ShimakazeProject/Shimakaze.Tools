namespace Shimakaze.Tools.Csf.Translater.Exceptions;

[System.Serializable]
public class TranslateException : System.Exception
{
    public TranslateException() { }
    public TranslateException(string message) : base(message) { }
    public TranslateException(string message, System.Exception inner) : base(message, inner) { }
    protected TranslateException(
        System.Runtime.Serialization.SerializationInfo info,
        System.Runtime.Serialization.StreamingContext context) : base(info, context) { }
}