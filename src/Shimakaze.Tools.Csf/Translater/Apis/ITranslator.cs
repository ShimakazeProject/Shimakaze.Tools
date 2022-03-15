using System.Text;

namespace Shimakaze.Tools.Csf.Translater.Apis;


public interface ITranslator
{
    Task<string> TranslateAsync(string word);
    event EventHandler<Exception> Error;
}