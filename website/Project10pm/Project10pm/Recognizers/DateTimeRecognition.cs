

using Microsoft.Recognizers.Text;
using Microsoft.Recognizers.Text.DateTime;
using Newtonsoft.Json;
using System.IO.Pipelines;
using static Project10pm.Controllers.ContentController;

namespace Project10pm.Recognizers
{
    public static class DateTimeRecognition
    {
        public const string DEFAULT_CULTURE = Culture.English;
        public const string ENGLISH_CULTURE = Culture.English;

        public static readonly TimeZoneInfo DEFAULT_TIME_ZONE = TimeZoneInfo.Utc;

        public static List<DateTimeRecognitionResult> DetectDateTimeReferences(string content, 
                                                                                string culture = DEFAULT_CULTURE, 
                                                                                TimeZoneInfo? timeZone = null)
        {
            timeZone ??= DEFAULT_TIME_ZONE;

            List<ModelResult> rawResults =
                    DateTimeRecognizer.RecognizeDateTime(content, culture);

            //TODO: Temporary to learn the structure of these values
            Console.WriteLine(JsonConvert.SerializeObject(rawResults, Formatting.Indented));

            var results = new List<DateTimeRecognitionResult>(rawResults.Count);
            foreach (var rawResult in rawResults)
            {
                IEnumerable<Dictionary<string, string>>? resolutionValues = rawResult.Resolution["values"] as IEnumerable<Dictionary<string, string>>;
                var resolutionParts = ConvertResolutionValues(resolutionValues, timeZone);

                results.Add(new DateTimeRecognitionResult
                {
                    SnippetStartIndex = rawResult.Start,
                    SnippetEndIndex = rawResult.End,
                    SnippetText = rawResult.Text,
                    RecognitionTypeName = rawResult.TypeName,
                    Resolution = resolutionParts
                });
            }

            return results;
        }

        private static List<DateTimeResolutionPart> ConvertResolutionValues(IEnumerable<Dictionary<string, string>>? resolutions, TimeZoneInfo localTimeZone)
        {
            var parts = new List<DateTimeResolutionPart>();
            if (resolutions == null)
            {
                return parts;
            }

            foreach (var item in resolutions)
            {
                //TODO: Keep an eye on this pattern. Probably a better breakout abstraction in the futre

                if (item["type"] == "datetimerange")
                {
                    parts.Add(ConvertResolutionDateTimeRange(item, localTimeZone));
                }

                if(false == item.ContainsKey("value"))
                {
                    continue;
                }

                DateTimeOffset? resolutionOffset = null;
                DateTime unboundDateTime;
                if(DateTime.TryParse(item["value"], out unboundDateTime))
                {
                    resolutionOffset = new TimeShift(unboundDateTime, localTimeZone).ToCurrentDateTimeOffset();
                }

                TimexType type;
                switch (item["type"])
                {
                    case "date":
                        type = TimexType.Date;
                        break;
                    case "datetime":
                        type = TimexType.DateTime;
                        break;
                    case "datetimerange":
                        type = TimexType.DateTimeRange;
                        break;
                    default:
                        type = TimexType.Unknown;
                        break;
                }

                var part = new DateTimeResolutionPart
                {
                    LocalOffsetStart = resolutionOffset,
                    Type = type,

                };

                parts.Add(part);
            }
            return parts;
        }

        private static DateTimeResolutionPart ConvertResolutionDateTimeRange(Dictionary<string, string> item, TimeZoneInfo localTimeZone)
        {

            DateTimeOffset? startResolutionOffset = null;
            DateTime unboundStartDateTime;
            if (DateTime.TryParse(item["start"], out unboundStartDateTime))
            {
                startResolutionOffset = new TimeShift(unboundStartDateTime, localTimeZone).ToCurrentDateTimeOffset();
            }

            DateTimeOffset? endResolutionOffset = null;
            DateTime unboundEndDateTime;
            if (DateTime.TryParse(item["end"], out unboundEndDateTime))
            {
                endResolutionOffset = new TimeShift(unboundEndDateTime, localTimeZone).ToCurrentDateTimeOffset();
            }


            TimexType type = TimexType.DateTimeRange;

            var part = new DateTimeResolutionPart
            {
                LocalOffsetStart = startResolutionOffset,
                LocalOffsetEnd = endResolutionOffset,
                Type = type,

            };

            return part;
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
        public DateTimeOffset? LocalOffsetStart { get; set; }
        public DateTimeOffset? LocalOffsetEnd {  get; set; }
        public TimexType Type { get; set; } = TimexType.Unknown;
    }

    public enum TimexType
    {
        Unknown = 0,
        Date = 1,
        DateTime = 2,
        DateTimeRange = 3
    }
}
