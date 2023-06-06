using Microsoft.Recognizers.Text;
using Microsoft.Recognizers.Text.DateTime;
using Newtonsoft.Json;

namespace POC.Microsoft.Recognizers.Text
{
    public class ValidDateParseTests : DefaultCultureFixture
    {
        public ValidDateParseTests(string culture) : base(culture) { }

        [TestCase("2023-06-05")]
        [TestCase("06-05-2023")]
        public void ISO8601Date_SingleInstanceFullDate_ReturnsDate(string date)
        {
            List<ModelResult> results = 
                    DateTimeRecognizer.RecognizeDateTime(date, _culture);

            foreach (ModelResult result in results)
            {
                TestContext.WriteLine(JsonConvert.SerializeObject(result, Formatting.Indented));
            }

            Assert.That(results, Is.Not.Null);
            Assert.That(results.Count, Is.EqualTo(1));
            Assert.That(results[0].Text, Is.EqualTo(date));
        }

        [TestCase("2023-06")]
        public void ISO8601Date_SingleInstanceYearMonth_ReturnsDate(string date)
        {
            List<ModelResult> results =
                    DateTimeRecognizer.RecognizeDateTime(date, _culture);

            Assert.That(results, Is.Not.Null);
            Assert.That(results.Count, Is.EqualTo(1));
            Assert.That(results[0].Text, Is.EqualTo(date));
        }

        [TestCase("2023")]
        public void ISO8601Date_SingleInstanceYear_ReturnsDate(string date)
        {
            List<ModelResult> results =
                    DateTimeRecognizer.RecognizeDateTime(date, _culture);

            Assert.That(results, Is.Not.Null);
            Assert.That(results.Count, Is.EqualTo(1));
            Assert.That(results[0].Text, Is.EqualTo(date));
        }

        [TestCase("Today's date is 2023-06-05.", 2)]
        [TestCase("The current date is 2023-06-05.", 2)]
        public void ISO8601DateSentence_SingleInstanceFullDate_ReturnsDate(string date, int resultCount)
        {
            List<ModelResult> results =
                    DateTimeRecognizer.RecognizeDateTime(date, _culture);

            foreach(ModelResult result in results)
            {
                TestContext.WriteLine(JsonConvert.SerializeObject(result, Formatting.Indented));
            }

            Assert.That(results, Is.Not.Null);
            Assert.That(results.Count, Is.EqualTo(resultCount));
        }

        [TestCase("Today's date is 2023-06-05, but tomorrow is 2023-06-06", 4)]
        public void ISO8601DateSentence_TwoInstanceFullDate_ReturnsDate(string date, int resultCount)
        {
            List<ModelResult> results =
                    DateTimeRecognizer.RecognizeDateTime(date, _culture);

            foreach (ModelResult result in results)
            {
                TestContext.WriteLine(JsonConvert.SerializeObject(result, Formatting.Indented));
            }

            Assert.That(results, Is.Not.Null);
            Assert.That(results.Count, Is.EqualTo(resultCount));
        }


        [TestCase("Today's date is June 5th, 2023, but tomorrow is June 6th, 2023.", 4)]
        [TestCase("Das heutige Datum ist der 5. Juni 2023, aber morgen ist der 6. Juni 2023.", 4)]
        public void WordDateSentence_TwoInstanceFullDate_ReturnsDate(string date, int resultCount)
        {
            List<ModelResult> results =
                    DateTimeRecognizer.RecognizeDateTime(date, _culture);

            foreach (ModelResult result in results)
            {
                TestContext.WriteLine(JsonConvert.SerializeObject(result, Formatting.Indented));
            }

            Assert.That(results, Is.Not.Null);
            Assert.That(results.Count, Is.EqualTo(resultCount));
        }

        [TestCase("The test is next Monday.", 1)]
        [TestCase("I'll go back 8pm today",1)]
        public void WordDateSentence_ReferencePointFullDate_ReturnsDate(string date, int resultCount)
        {
            List<ModelResult> results =
                    DateTimeRecognizer.RecognizeDateTime(date, _culture);

            foreach (ModelResult result in results)
            {
                TestContext.WriteLine(JsonConvert.SerializeObject(result, Formatting.Indented));
            }

            Assert.That(results, Is.Not.Null);
            Assert.That(results.Count, Is.EqualTo(resultCount));
        }
    }
}