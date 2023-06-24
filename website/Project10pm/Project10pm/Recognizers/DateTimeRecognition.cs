

using Microsoft.Recognizers.Text;
using Microsoft.Recognizers.Text.DateTime;
using Newtonsoft.Json;
using static Project10pm.Controllers.ContentController;

namespace Project10pm.Recognizers
{
    public static class DateTimeRecognition
    {
        public const string DEFAULT_CULTURE = Culture.English;
        public const string ENGLISH_CULTURE = Culture.English;

        public static List<DateTimeRecognitionResult> DetectDateTimeReferences(string content, string culture = DEFAULT_CULTURE)
        {
            List<ModelResult> rawResults =
                    DateTimeRecognizer.RecognizeDateTime(content, culture);

            var results = new List<DateTimeRecognitionResult>(rawResults.Count);
            foreach (var rawResult in rawResults)
            {
                IEnumerable<Dictionary<string, string>>? resolutionValues = rawResult.Resolution["values"] as IEnumerable<Dictionary<string, string>>;
                var resolutionParts = ConvertResolutionValues(resolutionValues);

                results.Add(new DateTimeRecognitionResult
                {
                    SnippetStartIndex = rawResult.Start,
                    SnippetEndIndex = rawResult.End,
                    SnippetText = rawResult.Text,
                    RecognitionTypeName = rawResult.TypeName,
                    Resolution = resolutionParts
                });

                //TODO: Temporary to learn the structure of these values
                Console.WriteLine(JsonConvert.SerializeObject(rawResult.Resolution, Formatting.Indented));
            }

            return results;
        }

        private static List<DateTimeResolutionPart> ConvertResolutionValues(IEnumerable<Dictionary<string, string>>? objs)
        {
            var parts = new List<DateTimeResolutionPart>();
            if (objs == null)
            {
                return parts;
            }

            foreach (var value in objs)
            {
                DateTimeOffset timex;
                TimexType type = TimexType.Unknown;
                DateTimeOffset.TryParse(value["timex"], out timex);

                switch (value["type"])
                {
                    case "date":
                        type = TimexType.Date;
                        break;
                    case "datetime":
                        type = TimexType.DateTime;
                        break;
                    default:
                        type = TimexType.Unknown;
                        break;
                }

                var part = new DateTimeResolutionPart
                {
                    Timex = timex,
                    Type = type,
                    Value = value["timex"]
                };

                parts.Add(part);
            }
            return parts;
        }
    }

    public class DateTimeRecognitionResult
    {
        public int SnippetStartIndex { get; set; } = -1;
        public int SnippetEndIndex { get; set;} = -1;
        public string SnippetText { get; set; } = string.Empty;
        public string RecognitionTypeName { get; set; } = string.Empty;

        //TODO: Don't know yet what to do with this because there isn't much documentation. Will take a bit of time to discover patterns in how to use
        public List<DateTimeResolutionPart> Resolution { get; set; } = new List<DateTimeResolutionPart>();
    }

    public class DateTimeResolutionPart
    {
        public DateTimeOffset? Timex { get; set; }
        public TimexType Type { get; set; } = TimexType.Unknown;
        public string Value { get; set; } = string.Empty;
    }

    public enum TimexType
    {
        Unknown = 0,
        Date = 1,
        DateTime = 2
    }
}
