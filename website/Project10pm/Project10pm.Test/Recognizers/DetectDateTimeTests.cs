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

            Assert.That(results.Count, Is.EqualTo(1));
            Assert.That(expectedResult.SnippetStartIndex, Is.GreaterThanOrEqualTo(0));
            Assert.That(expectedResult.SnippetEndIndex, Is.GreaterThanOrEqualTo(0));
            Assert.That(expectedResult.RecognitionTypeName, Is.Not.Null);
            Assert.That(expectedResult.SnippetText, Is.Not.Null);
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

            Assert.That(results.Count, Is.EqualTo(1));
            Assert.That(expectedResult.SnippetStartIndex, Is.GreaterThanOrEqualTo(0));
            Assert.That(expectedResult.SnippetEndIndex, Is.GreaterThanOrEqualTo(0));
            Assert.That(expectedResult.RecognitionTypeName, Is.Not.Null);
            Assert.That(expectedResult.SnippetText, Is.Not.Null);
        }


    }
}
