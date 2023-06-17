

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
                results.Add(new DateTimeRecognitionResult
                {
                    SnippetStartIndex = rawResult.Start,
                    SnippetEndIndex = rawResult.End,
                    SnippetText = rawResult.Text,
                    RecognitionTypeName = rawResult.TypeName,
                    Resolution = rawResult.Resolution,
                });

                //TODO: Temporary to learn the structure of these values
                Console.WriteLine(JsonConvert.SerializeObject(rawResult.Resolution, Formatting.Indented));
            }

            return results;
        }
    }

    public class DateTimeRecognitionResult
    {
        public int SnippetStartIndex { get; set; } = -1;
        public int SnippetEndIndex { get; set;} = -1;
        public string SnippetText { get; set; } = string.Empty;
        public string RecognitionTypeName { get; set; } = string.Empty;

        //TODO: Don't know yet what to do with this because there isn't much documentation. Will take a bit of time to discover patterns in how to use
        public SortedDictionary<string, object> Resolution { get; set; } = new SortedDictionary<string, object>();
    }

    public class DateTimeRecognitionParts
    {

    }
}
