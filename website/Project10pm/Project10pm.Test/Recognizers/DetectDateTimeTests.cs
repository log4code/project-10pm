using Microsoft.Recognizers.Text;
using Newtonsoft.Json;
using Project10pm.Recognizers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Project10pm.Test.Recognizers
{
    [TestFixture]
    internal class DetectDateTimeTests
    {
        [TestCase(DateTimeRecognition.ENGLISH_CULTURE, "06-14-2023", "06-14-2023")]
        [TestCase(DateTimeRecognition.ENGLISH_CULTURE, "06/14/2023", "06/14/2023")]
        [TestCase(DateTimeRecognition.ENGLISH_CULTURE, "2023-06-14", "2023-06-14")]
        public void SingleExplicitInstance_FullDate_ReturnsValidResult(string culture, string content, string dateText)
        {
            var results = DateTimeRecognition.DetectDateTimeReferences(content, culture);
            var expectedResult = results.First(i => i.SnippetText == dateText);

            Assert.Multiple(() =>
            {
                Assert.That(results.Count, Is.EqualTo(1));
                Assert.That(expectedResult.SnippetStartIndex, Is.GreaterThanOrEqualTo(0));
                Assert.That(expectedResult.SnippetEndIndex, Is.GreaterThanOrEqualTo(0));
                Assert.That(expectedResult.RecognitionTypeName, Is.Not.Null);
                Assert.That(expectedResult.SnippetText, Is.Not.Null);
            });
        }

        [TestCase(DateTimeRecognition.ENGLISH_CULTURE, "06-14-2023 and 06-15-2023", 2)]
        public void MultipleExplicitInstance_FullDate_ReturnsValidResults(string culture, string content, int count)
        {
            var results = DateTimeRecognition.DetectDateTimeReferences(content, culture);

            Assert.That(results.Count, Is.EqualTo(2));
        }

        [TestCase(DateTimeRecognition.ENGLISH_CULTURE, "today", "today")]
        [TestCase(DateTimeRecognition.ENGLISH_CULTURE, "tomorrow", "tomorrow")]
        [TestCase(DateTimeRecognition.ENGLISH_CULTURE, "yesterday", "yesterday")]
        public void ImpliedInstance_FullDate_ReturnsValidResult(string culture, string content, string dateText)
        {
            var results = DateTimeRecognition.DetectDateTimeReferences(content, culture);
            var expectedResult = results.First(i => i.SnippetText == dateText);
            
            Assert.Multiple(() =>
            {
                Assert.That(results.Count, Is.EqualTo(1));
                Assert.That(expectedResult.SnippetStartIndex, Is.GreaterThanOrEqualTo(0));
                Assert.That(expectedResult.SnippetEndIndex, Is.GreaterThanOrEqualTo(0));
                Assert.That(expectedResult.RecognitionTypeName, Is.Not.Null);
                Assert.That(expectedResult.SnippetText, Is.Not.Null);
            });
        }

        [TestCase(DateTimeRecognition.ENGLISH_CULTURE, "06-14-2023 10:15 pm", "06-14-2023 10:15 pm")]
        [TestCase(DateTimeRecognition.ENGLISH_CULTURE, "06-14-2023 10:15:00", "06-14-2023 10:15:00")]
        public void SingleExplitInstance_FullDateTime_ReturnsValidResult(string culture, string content, string dateText)
        {
            var results = DateTimeRecognition.DetectDateTimeReferences(content, culture);
            var expectedResult = results.First(i => i.SnippetText == dateText);
            
            Assert.Multiple(() =>
            {
                Assert.That(results.Count, Is.EqualTo(1));
                Assert.That(expectedResult.SnippetStartIndex, Is.GreaterThanOrEqualTo(0));
                Assert.That(expectedResult.SnippetEndIndex, Is.GreaterThanOrEqualTo(0));
                Assert.That(expectedResult.RecognitionTypeName, Is.Not.Null);
                Assert.That(expectedResult.SnippetText, Is.Not.Null);
            });
        }

        [TestCase(DateTimeRecognition.ENGLISH_CULTURE, "06-14-2023", "America/New_York")]
        public void SingleExplicitInstance_FullDate_ReturnsCorrectOffset(string culture, string content, string timeZoneId)
        {
            var timeZoneInfo = TimeZoneInfo.FindSystemTimeZoneById(timeZoneId);
            var results = DateTimeRecognition.DetectDateTimeReferences(content, culture, timeZoneInfo);

            var expectedResult = results.First();
            DateTimeOffset? offset = expectedResult.Resolution.FirstOrDefault()?.LocalOffsetStart;

            DateTime expectedValue = DateTime.Parse(content);
            Assert.That(offset?.LocalDateTime, Is.EqualTo(expectedValue));
        }

        [Test]
        public void FreeFormText_MultipleComplexDateTimeRanges_ReturnsAllDetectedReferences()
        {
            var content = @"
End of School Celebration

You are invited to celebrate the end of our 3rd grade year with Mrs. Smith's’ class!

When: Tuesday, May 23rd @ 12:00 PM- 1:30 PM

Where: ROOM 25

The class will be having a brief song performance. This will give everyone a chance
to take a class picture in the classroom. After we have taken class pictures, we will pass
out their diplomas and awards and give everyone a chance to take individual and or family
photos. After we have watched our class slide show, we will go to the cafeteria to make an end of the year treat!

*This party is optional for parent attendance. We will be taking plenty of photos
and videos throughout the day. Please RSVP below or by email by Monday May 22nd.
"
            ;

            var timeZoneInfo = TimeZoneInfo.FindSystemTimeZoneById("America/New_York");
            var results = DateTimeRecognition.DetectDateTimeReferences(content, timeZone: timeZoneInfo);

            TestContext.WriteLine(JsonConvert.SerializeObject(results, Formatting.Indented));

            var dateTimeRanges = results.Where(i => i.Resolution.Any(y => y.Type == TimexType.DateTimeRange));
             

            Assert.Multiple(() =>
            {
                Assert.That(results.Count, Is.GreaterThan(0));
                Assert.That(dateTimeRanges.Count, Is.GreaterThan(0));
            });
        }
    }
}
