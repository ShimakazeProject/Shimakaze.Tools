namespace Shimakaze.Tools.Csf.Serialization.Json
{
    internal class Constants
    {
        public static class SchemaUrls
        {
            public const string BASE_URL = "https://shimakazeproject.github.io";
            public const string V1 = BASE_URL + "/json/csf/v1/schema.json";
            public const string V2 = BASE_URL + "/json/csf/v2/schema.json";
        }
        public static readonly string[] LanguageList = new[] {
            "en_US",
            "en_UK",
            "de",
            "fr",
            "es",
            "it",
            "jp",
            "Jabberwockie",
            "kr",
            "zh"
        };
    }
}